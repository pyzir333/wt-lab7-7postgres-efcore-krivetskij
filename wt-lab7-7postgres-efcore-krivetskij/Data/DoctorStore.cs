using wt_lab6_6minimalapi_krivetskij.Models;

namespace wt_lab6_6minimalapi_krivetskij.Data;

/// <summary>
/// In-memory storage for doctors.
/// The collection is shared by all doctor API endpoints.
/// </summary>
public static class DoctorStore
{
    public static List<Doctor> Doctors { get; } = new()
    {
        new Doctor
        {
            Id = 1,
            FullName = "Dr. James Taylor",
            Specialty = "General Practitioner",
            ExperienceYears = 12,
            Office = 101,
            IsActive = true
        },
        new Doctor
        {
            Id = 2,
            FullName = "Dr. Sophia Clark",
            Specialty = "Cardiologist",
            ExperienceYears = 9,
            Office = 205,
            IsActive = true
        },
        new Doctor
        {
            Id = 3,
            FullName = "Dr. William Moore",
            Specialty = "Gastroenterologist",
            ExperienceYears = 15,
            Office = 310,
            IsActive = true
        },
        new Doctor
        {
            Id = 4,
            FullName = "Dr. Emma White",
            Specialty = "Neurologist",
            ExperienceYears = 7,
            Office = 214,
            IsActive = true
        },
        new Doctor
        {
            Id = 5,
            FullName = "Dr. Benjamin Harris",
            Specialty = "Allergist",
            ExperienceYears = 11,
            Office = 118,
            IsActive = false
        }
    };
}
