// BusBookingSystem.API/Program.cs

using System.Text;
using BusBookingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. POSTGRESQL AYARI
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. JWT AUTHENTICATION AYARI
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddAuthorization();

// 3. SERVICES
builder.Services.AddScoped<BusBookingSystem.Application.Services.IBusService, BusBookingSystem.Application.Services.BusService>();
builder.Services.AddScoped<BusBookingSystem.Application.Services.ITripService, BusBookingSystem.Application.Services.TripService>();
builder.Services.AddScoped<BusBookingSystem.Application.Services.IPassengerService, BusBookingSystem.Application.Services.PassengerService>();
builder.Services.AddScoped<BusBookingSystem.Application.Services.ILocationService, BusBookingSystem.Application.Services.LocationService>();
builder.Services.AddScoped<BusBookingSystem.Application.Services.IAuthService, BusBookingSystem.Application.Services.AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 4. SWAGGER CONFIGURATION WITH JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bus Booking System API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// 5. Pipeline (Middleware) Ayarları
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // JWT Authentication middleware - UseAuthorization'dan önce olmalı!
app.UseAuthorization();
app.MapControllers();

// 6. SEED DATA - Uygulama başlarken şehirleri ekle
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    await BusBookingSystem.Infrastructure.Data.DbSeeder.SeedCitiesAsync(context);
}

app.Run();