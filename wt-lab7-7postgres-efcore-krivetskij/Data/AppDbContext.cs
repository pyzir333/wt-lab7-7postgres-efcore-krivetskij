using Microsoft.EntityFrameworkCore;
using wt_lab6_6minimalapi_krivetskij.Models;

namespace wt_lab6_6minimalapi_krivetskij.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<Doctor> Doctors => Set<Doctor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>().HasData(
            new Doctor
            {
                Id = 1001,
                FullName = "Иванов Сергей Петрович",
                Specialty = "Терапевт",
                ExperienceYears = 12,
                Office = 101,
                IsActive = true
            },
            new Doctor
            {
                Id = 1002,
                FullName = "Петрова Анна Викторовна",
                Specialty = "Кардиолог",
                ExperienceYears = 8,
                Office = 205,
                IsActive = true
            }
        );

        modelBuilder.Entity<Patient>().HasData(
            new Patient
            {
                Id = 1001,
                FullName = "Сидоров Алексей Николаевич",
                Age = 35,
                DoctorId = 1001,
                AppointmentDate = new DateTime(2026, 11, 20, 10, 0, 0, DateTimeKind.Utc),
                Diagnosis = "Плановый осмотр",
                IsActive = true,
                CreatedAt = new DateTime(2026, 11, 14, 9, 0, 0, DateTimeKind.Utc)
            },
            new Patient
            {
                Id = 1002,
                FullName = "Козлова Мария Андреевна",
                Age = 42,
                DoctorId = 1002,
                AppointmentDate = new DateTime(2026, 11, 21, 11, 0, 0, DateTimeKind.Utc),
                Diagnosis = "Контрольное обследование",
                IsActive = true,
                CreatedAt = new DateTime(2026, 11, 14, 9, 0, 0, DateTimeKind.Utc)
            },
            new Patient
            {
                Id = 1003,
                FullName = "Смирнов Дмитрий Олегович",
                Age = 28,
                DoctorId = 1001,
                AppointmentDate = new DateTime(2026, 11, 22, 12, 0, 0, DateTimeKind.Utc),
                Diagnosis = "Консультация терапевта",
                IsActive = true,
                CreatedAt = new DateTime(2026, 11, 14, 9, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
