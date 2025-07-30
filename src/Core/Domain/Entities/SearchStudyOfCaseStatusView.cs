using System;
using System.Collections.Generic;

namespace TeraLinkaCareApi.Core.Domain.Entities;

public partial class SearchStudyOfCaseStatusView
{
    public string PatientId { get; set; } = null!;

    public string? PatientsName { get; set; }

    public string? PatientsSex { get; set; }

    public string? PatientsBirthDate { get; set; }

    public string? OtherPatientNames { get; set; }

    public string? OtherPatientId { get; set; }

    public string StudyInstanceUID { get; set; } = null!;

    public string StudyDate { get; set; } = null!;

    public string StudyTime { get; set; } = null!;

    public string ReferringPhysiciansName { get; set; } = null!;

    public string? AccessionNumber { get; set; }

    public string? StudyDescription { get; set; }

    public string Modality { get; set; } = null!;

    public string? PerformingPhysiciansName { get; set; }

    public string? NameofPhysiciansReading { get; set; }

    public int? TotalCases { get; set; }

    public int? OpenCases { get; set; }

    public string? AllCaseStatuses { get; set; }
}
