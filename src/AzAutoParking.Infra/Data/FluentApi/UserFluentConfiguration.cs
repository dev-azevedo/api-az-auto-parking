using AzAutoParking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzAutoParking.Infra.Data.FluentApi;

public class UserFluentConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.ConfirmedAccount)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(u => u.ConfirmationCode)
            .HasMaxLength(6);
        
        builder.Property(u => u.IsAdmin)
            .HasDefaultValue(false);

        builder.HasMany(u => u.ParkingSessions)
            .WithOne(ps => ps.User)
            .HasForeignKey(ps => ps.UserId);
        
        builder.HasMany(u => u.Automobile)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);
        
        builder.HasMany(u => u.Log)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);
    }
}