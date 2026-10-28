namespace TeraLinkaCareApi.Application.DTOs;

public class PatientInfoDto
{
    public Guid Puid { get; set; }
    public Guid? PatientPuid { get; set; }
    public Guid? PtEncounterPuid { get; set; }
    public Guid? ClinicalUnitPuid { get; set; }
    public string? ClinicalUnitLabel { get; set; }
    public string? BedLabel { get; set; }
    public string LifeTimeNumber { get; set; } = null!;
    public string? EncounterNumber { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalId { get; set; }
    public string? ClinicalService { get; set; }
    public string? AttendingPhysician { get; set; }
    public DateTime? SysInTime { get; set; }
    public DateTime? SysOutTime { get; set; }
    public DateTime? TransferInTime { get; set; }
    public DateTime? TransferOutTime { get; set; }
    public string? TransferOutStatus { get; set; }
    public DateTime? UpdateTime { get; set; }
    public decimal? BodyHeight { get; set; }
    public decimal? BodyWeight { get; set; }
}
