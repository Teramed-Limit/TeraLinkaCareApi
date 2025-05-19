namespace TeraLinkaCareApi.Application.DTOs;

public class CaseImageDto
{
    public string CaseId { get; set; }
    public string SeriesInstanceUID { get; set; }
    public string SopInstanceUID { get; set; }
    public string ImageNumber { get; set; }
    public string ImageDate { get; set; }
    public string ImageTime { get; set; }
    public string FilePath { get; set; }
    public string ImageUrl { get; set; }
    public string imageMarkerUrl { get; set; }
    public string ImageMarker { get; set; }
    public string ImageComment { get; set; }
    public DateTime? ShiftDate { get; set; }
    public string? ShiftLongLabel { get; set; }
    public string? ShiftShortLabel { get; set; }
}