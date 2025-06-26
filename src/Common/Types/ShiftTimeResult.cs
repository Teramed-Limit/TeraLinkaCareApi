namespace TeraLinkaCareApi.Common.Types;

/// <summary>
/// 班別時間結果
/// </summary>
public class ShiftTimeResult
{
    public Guid ClinicalUnitShiftPuid { get; set; }
    public ShiftWithTime? CurrentShift { get; set; }
    public string? ClinicalDate { get; set; }
    public string? NaturalDate { get; set; }
    public DateTime? OriginalDate { get; set; }
}
