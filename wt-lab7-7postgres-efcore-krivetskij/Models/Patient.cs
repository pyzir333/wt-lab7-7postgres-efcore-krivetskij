
namespace wt_lab6_6minimalapi_krivetskij.Models;

public class Patient
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public int Age { get; set; }

    public int DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string Diagnosis { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
}
