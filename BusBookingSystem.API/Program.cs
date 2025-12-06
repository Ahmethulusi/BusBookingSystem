// BusBookingSystem.API/Program.cs

using BusBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. POSTGRESQL AYARI (BU KISIM EKLENECEK)
// -------------------------------------------------------------------------
// Postgres'in tarih formatı hatasını engellemek için:
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// DbContext'i sisteme tanıtıyoruz (Dependency Injection)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// -------------------------------------------------------------------------
builder.Services.AddScoped<BusBookingSystem.Application.Services.IBusService, BusBookingSystem.Application.Services.BusService>();
builder.Services.AddScoped<BusBookingSystem.Application.Services.ITripService, BusBookingSystem.Application.Services.TripService>();
builder.Services.AddScoped<BusBookingSystem.Application.Services.IPassengerService, BusBookingSystem.Application.Services.PassengerService>();
builder.Services.AddScoped<BusBookingSystem.Application.Services.ILocationService, BusBookingSystem.Application.Services.LocationService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline (Middleware) Ayarları
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seed Data - Uygulama başlarken şehirleri ekle
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    await BusBookingSystem.Infrastructure.Data.DbSeeder.SeedCitiesAsync(context);
}

app.Run();