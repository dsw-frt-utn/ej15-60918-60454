using System;
using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Domain.Interfaces;

public interface IPersistence
{
    List<Speciality> GetAllSpecialities();
    Speciality? GetSpecialityById(Guid specialityId);

    Doctor AddDoctor(Doctor doctor);
    List<Doctor> GetActiveDoctors();
    Doctor? GetActiveDoctorById(Guid doctorId);
    void DeactivateDoctor(Guid doctorId);
}