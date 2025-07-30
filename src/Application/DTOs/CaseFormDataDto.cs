namespace TeraLinkaCareApi.Application.DTOs;

public class CaseFormDataDto
{
    public Guid CaseId { get; set; }
    public required string CaseDateTime { get; set; }
    public Guid FormDefinePuid { get; set; }
    public required Dictionary<string, object> FormData { get; set; }
    public Guid? ClinicalUnitPuid { get; set; } = Guid.Empty;
}
