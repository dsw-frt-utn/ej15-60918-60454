using System;
using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data.Persistence;

public class PersistenceDbContext : DbContext
{
    public PersistenceDbContext(DbContextOptions<PersistenceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Speciality> Specialities => Set<Speciality>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>().HasKey(d => d.Id);
        modelBuilder.Entity<Speciality>().HasKey(s => s.Id);

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.Speciality)
            .WithMany()
            .HasForeignKey(d => d.SpecialityId);

        modelBuilder.Entity<Speciality>().HasData(
            new Speciality
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Cardiología",
                Description = "Especialidad médica del corazón"
            },
            new Speciality
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Pediatría",
                Description = "Especialidad médica de niños"
            },
            new Speciality
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Name = "Dermatología",
                Description = "Especialidad médica de la piel"
            }
        );
    }
}