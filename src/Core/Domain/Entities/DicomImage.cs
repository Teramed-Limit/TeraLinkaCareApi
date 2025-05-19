namespace TeraLinkaCareApi.Core.Domain.Entities;

public partial class DicomImage
{
    public string SOPInstanceUID { get; set; } = null!;

    public string SeriesInstanceUID { get; set; } = null!;

    public string? SOPClassUID { get; set; }

    public string ImageNumber { get; set; } = null!;

    public string ImageDate { get; set; } = null!;

    public string ImageTime { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string StorageDeviceID { get; set; } = null!;

    public string? ImageStatus { get; set; }

    public int? WindowWidth { get; set; }

    public int? WindowCenter { get; set; }

    public string? KeyImage { get; set; }

    public string CreateDateTime { get; set; } = null!;

    public string CreateUser { get; set; } = null!;

    public string? ModifiedDateTime { get; set; }

    public string? ModifiedUser { get; set; }

    public string HaveSendToRemote { get; set; } = null!;

    public string? ReferencedSOPInstanceUID { get; set; }

    public string? ReferencedSeriesInstanceUID { get; set; }

    public string? UnmappedDcmTags { get; set; }

    public int? NumberOfFrames { get; set; }

    public string? ImageMarker { get; set; }

    public string? ImageComment { get; set; }

    public string? ImageMarkerUrl { get; set; }
}
