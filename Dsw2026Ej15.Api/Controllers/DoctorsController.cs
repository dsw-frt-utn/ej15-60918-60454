using Dsw2026Ej15.Api.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateDoctorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationException("Name es requerido.");
        }

        if (string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            throw new ValidationException("LicenseNumber es requerido.");
        }

        Speciality? speciality = _persistence.GetSpecialityById(request.SpecialityId);

        if (speciality is null)
        {
            throw new ValidationException("SpecialityId no existe.");
        }

        Doctor doctor = new Doctor
        {
            Name = request.Name.Trim(),
            LicenseNumber = request.LicenseNumber.Trim(),
            IsActive = true,
            Speciality = speciality
        };

        Doctor createdDoctor = _persistence.AddDoctor(doctor);

        DoctorResponse response = new DoctorResponse
        {
            Id = createdDoctor.Id,
            Name = createdDoctor.Name,
            LicenseNumber = createdDoctor.LicenseNumber,
            SpecialityName = createdDoctor.Speciality!.Name
        };

        return CreatedAtAction(nameof(GetById), new { id = createdDoctor.Id }, response);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        List<DoctorListItemResponse> response = _persistence
            .GetActiveDoctors()
            .Select(d => new DoctorListItemResponse
            {
                Id = d.Id,
                Name = d.Name,
                LicenseNumber = d.LicenseNumber,
                SpecialityName = d.Speciality!.Name
            })
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        Doctor? doctor = _persistence.GetActiveDoctorById(id);

        if (doctor is null)
        {
            return NotFound();
        }

        DoctorResponse response = new DoctorResponse
        {
            Id = doctor.Id,
            Name = doctor.Name,
            LicenseNumber = doctor.LicenseNumber,
            SpecialityName = doctor.Speciality!.Name
        };

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        Doctor? doctor = _persistence.GetActiveDoctorById(id);

        if (doctor is null)
        {
            return NotFound();
        }

        _persistence.DeactivateDoctor(id);

        return NoContent();
    }
}