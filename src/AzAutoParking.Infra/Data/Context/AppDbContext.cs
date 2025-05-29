using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.FluentApi;
using Microsoft.EntityFrameworkCore;

namespace AzAutoParking.Infra.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<User> Users {get; set;}
    public DbSet<Parking> Parkings {get; set;}
    public DbSet<Automobile> Automobiles {get; set;}
    public DbSet<ParkingSession> Reservations {get; set;}
    public DbSet<PriceParkingMinute> PriceParkingMinutes {get; set;}
    public DbSet<Log> Logs {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserFluentConfiguration());
        modelBuilder.ApplyConfiguration(new ParkingFluentConfiguration());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "azautoparking.db");
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }
    }
}