
namespace wt_lab6_6minimalapi_krivetskij.Models;

public class Doctor
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    public int Office { get; set; }

    public bool IsActive { get; set; } = true;
}
