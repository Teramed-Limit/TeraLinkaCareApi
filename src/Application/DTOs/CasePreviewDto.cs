namespace TeraLinkaCareApi.Application.DTOs;

/// <summary>
/// 傷照預覽頁資料：依部位分組的案例與其 DICOM series
/// </summary>
public class CasePreviewDto
{
    public List<PreviewBodyPartDto> BodyParts { get; set; } = new();
}

/// <summary>
/// 一個部位底下的所有案例
/// </summary>
public class PreviewBodyPartDto
{
    public string LocationLabel { get; set; } = "Unknown";
    public string LocationSVGId { get; set; } = "Unknown";
    public List<PreviewCaseDto> Cases { get; set; } = new();
}

/// <summary>
/// 案例（完整 CaseDto 欄位）與其 series，series 依日期時間遞減（最新在前）
/// </summary>
public class PreviewCaseDto : CaseDto
{
    public List<PreviewSeriesDto> Series { get; set; } = new();
}

/// <summary>
/// 一次檢查（DICOM series）與其影像，影像依 ImageNumber 遞增
/// </summary>
public class PreviewSeriesDto
{
    public string SeriesInstanceUID { get; set; } = "";

    /// <summary>yyyyMMdd</summary>
    public string SeriesDate { get; set; } = "";

    /// <summary>HHmmss（可能帶小數秒）</summary>
    public string SeriesTime { get; set; } = "";
    public DateTime? ShiftDate { get; set; }
    public string? ShiftShortLabel { get; set; }
    public string? ShiftLongLabel { get; set; }
    public List<CaseImageDto> Images { get; set; } = new();
}
