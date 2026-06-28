using System.Text.Json;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data.Persistence;

public class PersistenceEf : IPersistence
{
    private readonly Dsw2026Ej15DbContext _context;

    public PersistenceEf(Dsw2026Ej15DbContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
        LoadSpecialitiesIfEmpty();
    }

    public List<Speciality> GetAllSpecialities()
    {
        return _context.Specialities
            .AsNoTracking()
            .ToList();
    }

    public Speciality? GetSpecialityById(Guid specialityId)
    {
        return _context.Specialities
            .AsNoTracking()
            .FirstOrDefault(s => s.Id == specialityId);
    }

    public Doctor AddDoctor(Doctor doctor)
    {
        if (doctor.Speciality != null)
        {
            Speciality? speciality = _context.Specialities
                .FirstOrDefault(s => s.Id == doctor.Speciality.Id);

            if (speciality != null)
            {
                doctor.Speciality = speciality;
            }
        }

        _context.Doctors.Add(doctor);
        _context.SaveChanges();

        return doctor;
    }

    public List<Doctor> GetActiveDoctors()
    {
        return _context.Doctors
            .Include(d => d.Speciality)
            .Where(d => d.IsActive)
            .AsNoTracking()
            .ToList();
    }

    public Doctor? GetActiveDoctorById(Guid doctorId)
    {
        return _context.Doctors
            .Include(d => d.Speciality)
            .AsNoTracking()
            .FirstOrDefault(d => d.Id == doctorId && d.IsActive);
    }

    public void DeactivateDoctor(Guid doctorId)
    {
        Doctor? doctor = _context.Doctors
            .FirstOrDefault(d => d.Id == doctorId && d.IsActive);

        if (doctor is null)
        {
            throw new ValidationException("El médico no existe o no está activo.");
        }

        doctor.IsActive = false;
        _context.SaveChanges();
    }

    private void LoadSpecialitiesIfEmpty()
    {
        if (_context.Specialities.Any())
        {
            return;
        }

        string filePath = Path.Combine(AppContext.BaseDirectory, "specialities.json");

        if (!File.Exists(filePath))
        {
            return;
        }

        string json = File.ReadAllText(filePath);

        List<Speciality>? specialities = JsonSerializer.Deserialize<List<Speciality>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (specialities == null)
        {
            return;
        }

        _context.Specialities.AddRange(specialities);
        _context.SaveChanges();
    }
}
