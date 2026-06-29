using System;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data.Persistence;

public class PersistenceEf : IPersistence
{
    private readonly PersistenceDbContext _context;

    public PersistenceEf(PersistenceDbContext context)
    {
        _context = context;
    }

    public List<Speciality> GetAllSpecialities()
    {
        return _context.Specialities.ToList();
    }

    public Speciality? GetSpecialityById(Guid specialityId)
    {
        return _context.Specialities.SingleOrDefault(s => s.Id == specialityId);
    }

    public Doctor AddDoctor(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        _context.SaveChanges();
        return doctor;
    }

    public List<Doctor> GetActiveDoctors()
    {
        return _context.Doctors
            .Include(d => d.Speciality)
            .Where(d => d.IsActive)
            .ToList();
    }

    public Doctor? GetActiveDoctorById(Guid doctorId)
    {
        return _context.Doctors
            .Include(d => d.Speciality)
            .SingleOrDefault(d => d.Id == doctorId && d.IsActive);
    }

    public void DeactivateDoctor(Guid doctorId)
    {
        Doctor? doctor = _context.Doctors
            .SingleOrDefault(d => d.Id == doctorId && d.IsActive);

        if (doctor is null)
        {
            throw new ValidationException("El médico no existe o no está activo.");
        }

        doctor.IsActive = false;
        _context.SaveChanges();
    }
}
