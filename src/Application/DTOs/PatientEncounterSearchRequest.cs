namespace TeraLinkaCareApi.Application.DTOs;

/// <summary>
/// 住院病人搜尋條件（全部為選填；全部未填時列出目前在院病人）
/// </summary>
public class PatientEncounterSearchRequest
{
    /// <summary>臨床單位（病房）ID，GUID 格式</summary>
    public string? ClinicalUnitId { get; set; }

    /// <summary>病歷號（精確比對）</summary>
    public string? LifeTimeNumber { get; set; }

    /// <summary>姓名（包含比對）</summary>
    public string? Name { get; set; }

    /// <summary>住院日期起（含）</summary>
    public DateTime? SysInFrom { get; set; }

    /// <summary>住院日期迄（含當日）</summary>
    public DateTime? SysInTo { get; set; }
}
