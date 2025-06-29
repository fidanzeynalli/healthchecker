using System.Text;
using HealthTracker.API.Data;
using HealthTracker.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext ve Identity Servisleri
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>(options => { /* password etc. */ })
    .AddSignInManager()
    .AddEntityFrameworkStores<AppDbContext>();

// 2) JWT-only authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? jwtSettings["Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

// CORS for React frontend
builder.Services.AddCors(o => o.AddPolicy("AllowReact",
  p => p.WithOrigins("http://localhost:3001")
        .AllowAnyHeader()
        .AllowAnyMethod()));

builder.Services.AddCors(o =>
  o.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// 5) Controllerları ekle, Swagger
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});
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

app.UseStaticFiles(); // For wwwroot/uploads

app.UseRouting();

app.UseCors("AllowReact");
app.UseCors("AllowAll");
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// **SADECE GELİŞTİRME ORTAMINDA** Swaggerı aç
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>       // UIı "/swagger" yoluna başlar
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HealthTracker.API v1");
        // İstersen rota prefix'ini değiştir
        // c.RoutePrefix = string.Empty; // UI'ı root ("/") altına taşır
    });
}

app.Run();