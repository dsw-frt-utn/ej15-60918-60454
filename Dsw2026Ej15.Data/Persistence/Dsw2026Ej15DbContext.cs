using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data.Persistence;

public class Dsw2026Ej15DbContext : DbContext
{
    public Dsw2026Ej15DbContext(DbContextOptions<Dsw2026Ej15DbContext> options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Speciality> Specialities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Description).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            entity.Property(d => d.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.Property(d => d.IsActive).IsRequired();

            entity.HasOne(d => d.Speciality)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
