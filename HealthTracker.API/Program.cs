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
    // Diğer identity ayarları...
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 2) JWT Ayarlarını Oku
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
Console.WriteLine("JWT SecretKey: " + jwtSettings["SecretKey"]);
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

// 4) CORS (isteğe bağlı, frontend'in erişimi için)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

// 5) Controllerları ekle, Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HealthTracker.API", Version = "v1" });

    // 1. JWT Bearer güvenlik tanımı
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Lütfen 'Bearer {token}' formatında JWT girin."
    });

    // 2. Global güvenlik gereksinimi
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

builder.Services.AddScoped<HealthTracker.API.Services.IFoodService, HealthTracker.API.Services.FoodService>();
builder.Services.AddScoped<HealthTracker.API.Services.IMealService, HealthTracker.API.Services.MealService>();
builder.Services.AddScoped<HealthTracker.API.Services.IWaterService, HealthTracker.API.Services.WaterService>();

var app = builder.Build();
// **SADECE GELİŞTİRME ORTAMINDA** Swaggerı aç
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();           // JSON endpointını etkinleştirir
    app.UseSwaggerUI(c =>       // UIı "/swagger" yoluna başlar
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HealthTracker.API v1");
        // İstersen rota prefix'ini değiştir
        // c.RoutePrefix = string.Empty; // UI'ı root ("/") altına taşır
    });
}

// Pipeline
app.UseHttpsRedirection();
app.UseCors("AllowAll");

// **ÖNCE** Authenticate
app.UseAuthentication();
// **SONRA** Authorize
app.UseAuthorization();

app.MapControllers();

app.Run();