using AzAutoParking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzAutoParking.Infra.Data.FluentApi;

public class ParkingFluentConfiguration : IEntityTypeConfiguration<Parking>
{
    public void Configure(EntityTypeBuilder<Parking> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(p => p.ParkingNumber)
            .IsRequired();

        builder.HasIndex(p => p.ParkingNumber)
            .IsUnique();
        
        builder.Property(p => p.Available)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.HasMany(p => p.ParkingSessions)
            .WithOne(ps => ps.Parking)
            .HasForeignKey(ps => ps.ParkingId);
    }
    
}