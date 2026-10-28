namespace TeraLinkaCareApi.Application.DTOs;

public class PatientEncounterListDto
{
    public Guid Puid { get; set; }
    public Guid? PatientPuid { get; set; }
    public Guid? PtEncounterPuid { get; set; }
    public Guid? ClinicalUnitPuid { get; set; }
    public string? ClinicalUnitLabel { get; set; }
    public string? BedLabel { get; set; }
    public DateTime? SysInTime { get; set; }
    public DateTime? SysOutTime { get; set; }
    public string? EncounterNumber { get; set; }
    public string LifeTimeNumber { get; set; } = null!;
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? AttendingPhysician { get; set; }
}
