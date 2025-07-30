using System;
using System.Collections.Generic;

namespace TeraLinkaCareApi.Core.Domain.Entities;

public partial class A_PtEncounter
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

    public string? AttendingPhysicianDomainName { get; set; }

    public string? DedicatedPhysician { get; set; }

    public DateTime? SysInTime { get; set; }

    public DateTime? SysOutTime { get; set; }

    public DateTime? TransferInTime { get; set; }

    public DateTime? TransferOutTime { get; set; }

    public string? TransferOutStatus { get; set; }

    public string? Diagnosis { get; set; }

    public decimal? GCS { get; set; }

    public decimal? APACHEII { get; set; }

    public decimal? LOS { get; set; }

    public decimal? BT { get; set; }

    public decimal? HR { get; set; }

    public decimal? RR { get; set; }

    public decimal? ABPs { get; set; }

    public decimal? ABPd { get; set; }

    public decimal? ABPm { get; set; }

    public decimal? NBPs { get; set; }

    public decimal? NBPd { get; set; }

    public decimal? NBPm { get; set; }

    public decimal? SpO2 { get; set; }

    public string? VentilatorMode { get; set; }

    public int? DurationEndo { get; set; }

    public int? DurationFoley { get; set; }

    public int? DurationCVC { get; set; }

    public int? Isolation { get; set; }

    public int? CPR { get; set; }

    public int? OP { get; set; }

    public int? WithDraw { get; set; }

    public string? TTAS { get; set; }

    public string? WHEREABOUTS { get; set; }

    public string? CI { get; set; }

    public string? MAJOR { get; set; }

    public string? CRITICAL { get; set; }

    public string? LOS_ER { get; set; }

    public decimal? BodyHeight { get; set; }

    public decimal? BodyWeight { get; set; }

    public DateTime? LOOKDT { get; set; }

    public int? SEQ { get; set; }

    public string? Respiratory { get; set; }

    public string? Cardiovascular { get; set; }

    public string? Renal { get; set; }

    public string? Hemotology { get; set; }

    public string? GI { get; set; }

    public string? Metabolic { get; set; }

    public string? CONTENT { get; set; }

    public string? OtherNotification { get; set; }

    public string? CareProviderName { get; set; }

    public string? PtIdInfo { get; set; }

    public string? LabNotification { get; set; }

    public string? OrderNotification { get; set; }

    public string? ServicePhysician { get; set; }

    public DateTime? UpdateTime { get; set; }
}
