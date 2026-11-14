using wt_lab6_6minimalapi_krivetskij.Models;

namespace wt_lab6_6minimalapi_krivetskij.Data;

/// <summary>
/// In-memory storage for patients.
/// The collection is shared by all patient API endpoints.
/// </summary>
public static class PatientStore
{
    public static List<Patient> Patients { get; } = new()
    {
        new Patient
        {
            Id = 1,
            FullName = "John Smith",
            Age = 25,
            DoctorId = 1,
            AppointmentDate = new DateTime(2026, 11, 12, 9, 30, 0),
            Diagnosis = "Seasonal influenza",
            IsActive = true
        },
        new Patient
        {
            Id = 2,
            FullName = "Olivia Johnson",
            Age = 34,
            DoctorId = 3,
            AppointmentDate = new DateTime(2026, 11, 14, 11, 0, 0),
            Diagnosis = "Gastritis",
            IsActive = true
        },
        new Patient
        {
            Id = 3,
            FullName = "Michael Brown",
            Age = 42,
            DoctorId = 4,
            AppointmentDate = new DateTime(2026, 11, 16, 14, 15, 0),
            Diagnosis = "Migraine",
            IsActive = true
        },
        new Patient
        {
            Id = 4,
            FullName = "Emily Davis",
            Age = 51,
            DoctorId = 2,
            AppointmentDate = new DateTime(2026, 11, 18, 10, 0, 0),
            Diagnosis = "Hypertension",
            IsActive = true
        },
        new Patient
        {
            Id = 5,
            FullName = "Daniel Wilson",
            Age = 29,
            DoctorId = 5,
            AppointmentDate = new DateTime(2026, 11, 20, 16, 45, 0),
            Diagnosis = "Allergic rhinitis",
            IsActive = false
        }
    };
}
