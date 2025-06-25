using System.Text;
using HealthTracker.API.Data;
using HealthTracker.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;




var builder = WebApplication.CreateBuilder(args);

// 1) DbContext ve Identity Servisleri
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    // Di�er identity ayarlar�...
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 2) JWT Ayarlar�n� Oku
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

// 3) Authentication & JWT Bearer
    builder.Services.AddAuthentication(options =>
    {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true
        };
    });

// 4) CORS (iste�e ba�l�, frontend�in eri�imi i�in)
builder.Services.AddCors(opt =>
    opt.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader() .AllowAnyMethod() ));

// 5) Controller�lar� ekle, Swagger�
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HealthTracker.API", Version = "v1" });

    // 1. JWT Bearer g�venlik tan�m�
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "L�tfen 'Bearer {token}' format�nda JWT girin."
    });

    // 2. Global g�venlik gereksinimi
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();
// **SADECE GEL��T�RME ORTAMINDA** Swagger�� a�
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();           // JSON endpoint�ini etkinle�tirir
    app.UseSwaggerUI(c =>       // UI�� "/swagger" yoluna ba�lar
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HealthTracker.API v1");
        // �stersen rota prefix'ini de�i�tir
        // c.RoutePrefix = string.Empty; // UI'� root ("/") alt�na ta��r
    });
}

// Pipeline
app.UseHttpsRedirection();
app.UseCors("AllowAll");

// **�NCE** Authenticate
app.UseAuthentication();
// **SONRA** Authorize
app.UseAuthorization();

app.MapControllers();

app.Run();