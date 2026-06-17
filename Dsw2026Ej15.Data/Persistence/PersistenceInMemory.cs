using System;
using System.Text.Json;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;

namespace Dsw2026Ej15.Data.Persistence;

public class PersistenceInMemory : IPersistence
{
    private readonly List<Doctor> _doctors;
    private readonly List<Speciality> _specialities;

    public PersistenceInMemory()
    {
        _doctors = new List<Doctor>();
        _specialities = LoadSpecialities();
    }

    public List<Speciality> GetAllSpecialities()
    {
        return _specialities;
    }

    public Speciality? GetSpecialityById(Guid specialityId)
    {
        return _specialities.FirstOrDefault(s => s.Id == specialityId);
    }

    public Doctor AddDoctor(Doctor doctor)
    {
        _doctors.Add(doctor);
        return doctor;
    }

    public List<Doctor> GetActiveDoctors()
    {
        return _doctors.Where(d => d.IsActive).ToList();
    }

    public Doctor? GetActiveDoctorById(Guid doctorId)
    {
        return _doctors.FirstOrDefault(d => d.Id == doctorId && d.IsActive);
    }

    public void DeactivateDoctor(Guid doctorId)
    {
        Doctor? doctor = _doctors.FirstOrDefault(d => d.Id == doctorId && d.IsActive);

        if (doctor is null)
        {
            throw new ValidationException("El médico no existe o no está activo.");
        }

        doctor.IsActive = false;
    }

    private List<Speciality> LoadSpecialities()
    {
        string filePath = Path.Combine(AppContext.BaseDirectory, "specialities.json");

        if (!File.Exists(filePath))
        {
            return new List<Speciality>();
        }

        string json = File.ReadAllText(filePath);

        List<Speciality>? specialities = JsonSerializer.Deserialize<List<Speciality>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return specialities ?? new List<Speciality>();
    }
}