using Microsoft.EntityFrameworkCore;
using wt_lab6_6minimalapi_krivetskij.Data;
using wt_lab6_6minimalapi_krivetskij.Models;

var builder = WebApplication.CreateBuilder(args);

// Register API endpoint discovery and Swagger/OpenAPI documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

var databaseUrl = builder.Configuration["DATABASE_URL"];

if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);

    var dbPort = uri.Port > 0 ? uri.Port : 5432;

    connectionString =
        $"Host={uri.Host};" +
        $"Port={dbPort};" +
        $"Database={uri.AbsolutePath.TrimStart('/')};" +
        $"Username={Uri.UnescapeDataString(userInfo[0])};" +
        $"Password={Uri.UnescapeDataString(userInfo[1])};" +
        "SSL Mode=Disable";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Keep API documentation available in both Development and Production.
app.UseSwagger();
app.UseSwaggerUI();

// Root endpoint required by the laboratory assignment.
app.MapGet("/", () =>
        "Medical Center REST API is running. Open /swagger to explore the API.")
    .WithName("GetHome")
    .Produces<string>(StatusCodes.Status200OK);

// Return general information about the application as JSON.
app.MapGet("/api/info", () => Results.Ok(new
    {
        Application = "Medical Center",
        Technology = "ASP.NET Core Minimal API",
        Version = "1.0"
    }))
    .WithName("GetApiInfo")
    .Produces(StatusCodes.Status200OK);

// Patient endpoints. All operations use the shared PatientStore collection.
var patientsApi = app.MapGroup("/api/patients");

patientsApi.MapGet("/sorted", async (AppDbContext db) =>
{
    var patients = await db.Patients
        .AsNoTracking()
        .OrderBy(patient => patient.FullName)
        .ToListAsync();

    return Results.Ok(patients);
})
.WithName("GetPatientsSorted")
.Produces<List<Patient>>(StatusCodes.Status200OK);

patientsApi.MapGet("/active", async (AppDbContext db) =>
{
    var patients = await db.Patients
        .AsNoTracking()
        .Where(patient => patient.IsActive)
        .OrderBy(patient => patient.AppointmentDate)
        .ToListAsync();

    return Results.Ok(patients);
})
.WithName("GetActivePatients")
.Produces<List<Patient>>(StatusCodes.Status200OK);

patientsApi.MapGet("", async (AppDbContext db, string? search) =>
{
    IQueryable<Patient> query = db.Patients.AsNoTracking();

    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(patient =>
            patient.FullName.Contains(search) ||
            patient.Diagnosis.Contains(search));
    }

    var result = await query.ToListAsync();

    return Results.Ok(result);
})
.WithName("GetPatients")
.Produces<List<Patient>>(StatusCodes.Status200OK);

patientsApi.MapGet("/{id:int}", async (int id, AppDbContext db) =>
{
    var patient = await db.Patients
        .AsNoTracking()
        .FirstOrDefaultAsync(item => item.Id == id);

    if (patient is null)
    {
        return Results.NotFound(new { message = "Patient not found." });
    }

    return Results.Ok(patient);
})
.WithName("GetPatientById")
.Produces<Patient>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

patientsApi.MapPut("/{id:int}", async (int id, Patient input, AppDbContext db) =>
{
    var patient = await db.Patients.FindAsync(id);

    if (patient is null)
    {
        return Results.NotFound(new { message = "Patient not found." });
    }

    var validationError = ValidatePatient(input, db);
    if (validationError is not null)
    {
        return Results.BadRequest(new { message = validationError });
    }

    patient.FullName = input.FullName.Trim();
    patient.Age = input.Age;
    patient.DoctorId = input.DoctorId;
    patient.AppointmentDate = input.AppointmentDate;
    patient.Diagnosis = input.Diagnosis.Trim();
    patient.IsActive = input.IsActive;

    await db.SaveChangesAsync();

    return Results.Ok(patient);
})
.WithName("UpdatePatient")
.Produces<Patient>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);



patientsApi.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
{
    var patient = await db.Patients.FindAsync(id);

    if (patient is null)
    {
        return Results.NotFound(new { message = "Patient not found." });
    }

    db.Patients.Remove(patient);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeletePatient")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

// Doctor endpoints are retained as a second resource in the medical-center domain.
var doctorsApi = app.MapGroup("/api/doctors");

doctorsApi.MapGet("", async (AppDbContext db, string? search) =>
{
    IQueryable<Doctor> query = db.Doctors.AsNoTracking();

    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(doctor =>
            doctor.FullName.Contains(search) ||
            doctor.Specialty.Contains(search));
    }

    var result = await query.ToListAsync();

    return Results.Ok(result);
})
.WithName("GetDoctors")
.Produces<List<Doctor>>(StatusCodes.Status200OK);

doctorsApi.MapGet("/{id:int}", async (int id, AppDbContext db) =>
{
    var doctor = await db.Doctors
        .AsNoTracking()
        .FirstOrDefaultAsync(item => item.Id == id);

    if (doctor is null)
    {
        return Results.NotFound(new { message = "Doctor not found." });
    }

    return Results.Ok(doctor);
})
.WithName("GetDoctorById")
.Produces<Doctor>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

doctorsApi.MapPost("", async (Doctor input, AppDbContext db) =>
{
    var validationError = ValidateDoctor(input);
    if (validationError is not null)
    {
        return Results.BadRequest(new { message = validationError });
    }

    var newDoctor = new Doctor
    {
        FullName = input.FullName.Trim(),
        Specialty = input.Specialty.Trim(),
        ExperienceYears = input.ExperienceYears,
        Office = input.Office,
        IsActive = input.IsActive
    };

    db.Doctors.Add(newDoctor);
    await db.SaveChangesAsync();

    return Results.Created($"/api/doctors/{newDoctor.Id}", newDoctor);
})
.WithName("CreateDoctor")
.Produces<Doctor>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

doctorsApi.MapPut("/{id:int}", async (int id, Doctor input, AppDbContext db) =>
{
    var doctor = await db.Doctors.FindAsync(id);

    if (doctor is null)
    {
        return Results.NotFound(new { message = "Doctor not found." });
    }

    var validationError = ValidateDoctor(input);
    if (validationError is not null)
    {
        return Results.BadRequest(new { message = validationError });
    }

    doctor.FullName = input.FullName.Trim();
    doctor.Specialty = input.Specialty.Trim();
    doctor.ExperienceYears = input.ExperienceYears;
    doctor.Office = input.Office;
    doctor.IsActive = input.IsActive;

    await db.SaveChangesAsync();

    return Results.Ok(doctor);
})
.WithName("UpdateDoctor")
.Produces<Doctor>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

doctorsApi.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
{
    var doctor = await db.Doctors.FindAsync(id);

    if (doctor is null)
    {
        return Results.NotFound(new { message = "Doctor not found." });
    }

    if (await db.Patients.AnyAsync(patient => patient.DoctorId == id))
    {
        return Results.BadRequest(new
        {
            message = "The doctor cannot be deleted while patients are assigned to this doctor."
        });
    }

    db.Doctors.Remove(doctor);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteDoctor")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

// Use the Visual Studio launch profile locally; use Render's PORT in deployed environments.
var port = Environment.GetEnvironmentVariable("PORT");

if (string.IsNullOrWhiteSpace(port) && app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    port ??= "10000";
    app.Run($"http://0.0.0.0:{port}");
}

static string? ValidatePatient(Patient patient, AppDbContext db)
{
    if (string.IsNullOrWhiteSpace(patient.FullName))
    {
        return "FullName is required.";
    }

    if (patient.Age < 1 || patient.Age > 120)
    {
        return "Age must be between 1 and 120.";
    }

    if (!db.Doctors.Any(doctor => doctor.Id == patient.DoctorId))
    {
        return "DoctorId must refer to an existing doctor.";
    }

    if (patient.AppointmentDate == default)
    {
        return "AppointmentDate is required.";
    }

    if (string.IsNullOrWhiteSpace(patient.Diagnosis))
    {
        return "Diagnosis is required.";
    }

    return null;
}

static string? ValidateDoctor(Doctor doctor)
{
    if (string.IsNullOrWhiteSpace(doctor.FullName))
    {
        return "FullName is required.";
    }

    if (string.IsNullOrWhiteSpace(doctor.Specialty))
    {
        return "Specialty is required.";
    }

    if (doctor.ExperienceYears < 0 || doctor.ExperienceYears > 70)
    {
        return "ExperienceYears must be between 0 and 70.";
    }

    if (doctor.Office <= 0)
    {
        return "Office must be a positive number.";
    }

    return null;
}
