using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TeraLinkaCareApi.Core.Domain.Entities;

namespace TeraLinkaCareApi.Infrastructure.Persistence;

public partial class CRSDbContext : DbContext
{
    public CRSDbContext()
    {
    }

    public CRSDbContext(DbContextOptions<CRSDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<A_PtEncounter> A_PtEncounters { get; set; }

    public virtual DbSet<BasicWorklistMgtService> BasicWorklistMgtServices { get; set; }

    public virtual DbSet<CfgBodyLocation> CfgBodyLocations { get; set; }

    public virtual DbSet<CfgCaseType> CfgCaseTypes { get; set; }

    public virtual DbSet<CodeList> CodeLists { get; set; }

    public virtual DbSet<CountOfBasicWorklistMgtItem> CountOfBasicWorklistMgtItems { get; set; }

    public virtual DbSet<CountOfProcedureRegistrationView> CountOfProcedureRegistrationViews { get; set; }

    public virtual DbSet<DcmFindPatientLevelView> DcmFindPatientLevelViews { get; set; }

    public virtual DbSet<DcmFindSeriesLevelView> DcmFindSeriesLevelViews { get; set; }

    public virtual DbSet<DcmFindStudyLevelView> DcmFindStudyLevelViews { get; set; }

    public virtual DbSet<DicomDestinationNode> DicomDestinationNodes { get; set; }

    public virtual DbSet<DicomDocument> DicomDocuments { get; set; }

    public virtual DbSet<DicomImage> DicomImages { get; set; }

    public virtual DbSet<DicomNode> DicomNodes { get; set; }

    public virtual DbSet<DicomOperationNode> DicomOperationNodes { get; set; }

    public virtual DbSet<DicomPDF> DicomPDFs { get; set; }

    public virtual DbSet<DicomPatient> DicomPatients { get; set; }

    public virtual DbSet<DicomSeries> DicomSeries { get; set; }

    public virtual DbSet<DicomSeriesMap> DicomSeriesMaps { get; set; }

    public virtual DbSet<DicomServiceProvider> DicomServiceProviders { get; set; }

    public virtual DbSet<DicomStudy> DicomStudies { get; set; }

    public virtual DbSet<DicomTag> DicomTags { get; set; }

    public virtual DbSet<DicomTagFilter> DicomTagFilters { get; set; }

    public virtual DbSet<DicomTagFilterDetail> DicomTagFilterDetails { get; set; }

    public virtual DbSet<DicomVideo> DicomVideos { get; set; }

    public virtual DbSet<EnvironmentConfig> EnvironmentConfigs { get; set; }

    public virtual DbSet<FileOperationRecord> FileOperationRecords { get; set; }

    public virtual DbSet<FunctionRoleGroup> FunctionRoleGroups { get; set; }

    public virtual DbSet<HangingProtocolConfig> HangingProtocolConfigs { get; set; }

    public virtual DbSet<ImageMarkToolSet> ImageMarkToolSets { get; set; }

    public virtual DbSet<ImageOverlayInfoSet> ImageOverlayInfoSets { get; set; }

    public virtual DbSet<InstitutionInfo> InstitutionInfos { get; set; }

    public virtual DbSet<JobOptResultLog> JobOptResultLogs { get; set; }

    public virtual DbSet<LoginUserDatum> LoginUserData { get; set; }

    public virtual DbSet<ModalityLookupTable> ModalityLookupTables { get; set; }

    public virtual DbSet<OperationRecord> OperationRecords { get; set; }

    public virtual DbSet<PtCase> PtCases { get; set; }

    public virtual DbSet<PtCaseRecord> PtCaseRecords { get; set; }

    public virtual DbSet<PtPatient> PtPatients { get; set; }

    public virtual DbSet<QCOperationRecordView> QCOperationRecordViews { get; set; }

    public virtual DbSet<QC_AutoMappingConfig> QC_AutoMappingConfigs { get; set; }

    public virtual DbSet<QC_ResendDcmAEDatum> QC_ResendDcmAEData { get; set; }

    public virtual DbSet<ReportDefine> ReportDefines { get; set; }

    public virtual DbSet<RoleFunction> RoleFunctions { get; set; }

    public virtual DbSet<RoleFunctionView> RoleFunctionViews { get; set; }

    public virtual DbSet<RoleGroup> RoleGroups { get; set; }

    public virtual DbSet<SearchDocumentPathView> SearchDocumentPathViews { get; set; }

    public virtual DbSet<SearchImagePathView> SearchImagePathViews { get; set; }

    public virtual DbSet<SearchPatientStudyView> SearchPatientStudyViews { get; set; }

    public virtual DbSet<SearchSeriesAndGroupImageCountView> SearchSeriesAndGroupImageCountViews { get; set; }

    public virtual DbSet<SearchVideoFullPathView> SearchVideoFullPathViews { get; set; }

    public virtual DbSet<SearchVideoFullProfileView> SearchVideoFullProfileViews { get; set; }

    public virtual DbSet<StorageDevice> StorageDevices { get; set; }

    public virtual DbSet<StudyListWithVideo> StudyListWithVideos { get; set; }

    public virtual DbSet<SysBed> SysBeds { get; set; }

    public virtual DbSet<SysClinicalUnit> SysClinicalUnits { get; set; }

    public virtual DbSet<SysClinicalUnitShift> SysClinicalUnitShifts { get; set; }

    public virtual DbSet<SysRoleFunction> SysRoleFunctions { get; set; }

    public virtual DbSet<SystemConfig> SystemConfigs { get; set; }

    public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }

    public virtual DbSet<VM_PtProductionCapImgRegion> VM_PtProductionCapImgRegions { get; set; }

    public virtual DbSet<VideoBroadcasting> VideoBroadcastings { get; set; }

    public virtual DbSet<vwPatientBedLocationCurrent> vwPatientBedLocationCurrents { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer("Server=DESKTOP-DSL3QH8\\MSSQLSERVER01;Database=CRS;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<A_PtEncounter>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("A_PtEncounter");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.AttendingPhysician).HasMaxLength(64);
            entity.Property(e => e.BedLabel).HasMaxLength(32);
            entity.Property(e => e.ClinicalService).HasMaxLength(50);
            entity.Property(e => e.ClinicalUnitLabel).HasMaxLength(32);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.EncounterNumber).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LifeTimeNumber).HasMaxLength(50);
            entity.Property(e => e.NationalId).HasMaxLength(50);
            entity.Property(e => e.SysInTime).HasColumnType("datetime");
            entity.Property(e => e.SysOutTime).HasColumnType("datetime");
            entity.Property(e => e.TransferInTime).HasColumnType("datetime");
            entity.Property(e => e.TransferOutStatus).HasMaxLength(32);
            entity.Property(e => e.TransferOutTime).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<BasicWorklistMgtService>(entity =>
        {
            entity.HasKey(e => new { e.AccessionNumber, e.PatientID, e.StudyInstanceUID, e.Modality, e.StudyDate });

            entity.ToTable("BasicWorklistMgtService", tb => tb.HasTrigger("TR_BasicWorklistMgtService_Insert"));

            entity.Property(e => e.AccessionNumber).HasMaxLength(64);
            entity.Property(e => e.PatientID).HasMaxLength(128);
            entity.Property(e => e.StudyInstanceUID).HasMaxLength(128);
            entity.Property(e => e.Modality).HasMaxLength(16);
            entity.Property(e => e.StudyDate).HasMaxLength(16);
            entity.Property(e => e.Anesthesiologist).HasMaxLength(128);
            entity.Property(e => e.AssistantPhysician).HasMaxLength(128);
            entity.Property(e => e.Birthdate).HasMaxLength(16);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Dept).HasMaxLength(50);
            entity.Property(e => e.DocumentNumber).HasMaxLength(64);
            entity.Property(e => e.HISProcedureID).HasMaxLength(128);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.NameOfPhysiciansReadingStudy).HasMaxLength(128);
            entity.Property(e => e.PatientName).HasMaxLength(128);
            entity.Property(e => e.PatientOtherID).HasMaxLength(128);
            entity.Property(e => e.PatientOtherName).HasMaxLength(128);
            entity.Property(e => e.PerformingPhysician).HasMaxLength(128);
            entity.Property(e => e.ProcedureCode).HasMaxLength(32);
            entity.Property(e => e.ReferringPhysician).HasMaxLength(128);
            entity.Property(e => e.Sex).HasMaxLength(4);
            entity.Property(e => e.StudyDescription).HasMaxLength(512);
        });

        modelBuilder.Entity<CfgBodyLocation>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("CfgBodyLocation");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.LongLabel).HasMaxLength(64);
            entity.Property(e => e.NISCategory).HasMaxLength(32);
            entity.Property(e => e.NISLocationId).HasMaxLength(32);
            entity.Property(e => e.NISLocationLabel).HasMaxLength(32);
            entity.Property(e => e.SVGGraphicId).HasMaxLength(64);
            entity.Property(e => e.ShortLabel).HasMaxLength(32);
        });

        modelBuilder.Entity<CfgCaseType>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("CfgCaseType");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.CaseTypeCategory).HasMaxLength(32);
            entity.Property(e => e.CaseTypeLongLabel).HasMaxLength(32);
            entity.Property(e => e.CaseTypeShortLabel).HasMaxLength(32);
        });

        modelBuilder.Entity<CodeList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CodeOption");

            entity.ToTable("CodeList");

            entity.Property(e => e.CodeName).HasMaxLength(32);
            entity.Property(e => e.Label).HasMaxLength(32);
            entity.Property(e => e.ParentCodeValue).HasMaxLength(32);
            entity.Property(e => e.Value).HasMaxLength(32);
        });

        modelBuilder.Entity<CountOfBasicWorklistMgtItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CountOfBasicWorklistMgtItem");
        });

        modelBuilder.Entity<CountOfProcedureRegistrationView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CountOfProcedureRegistrationView");
        });

        modelBuilder.Entity<DcmFindPatientLevelView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DcmFindPatientLevelView");

            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.OtherPatientId).HasMaxLength(50);
            entity.Property(e => e.OtherPatientNames).HasMaxLength(64);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PatientsBirthDate).HasMaxLength(50);
            entity.Property(e => e.PatientsName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.PatientsSex).HasMaxLength(50);
        });

        modelBuilder.Entity<DcmFindSeriesLevelView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DcmFindSeriesLevelView");

            entity.Property(e => e.AccessionNumber).HasMaxLength(50);
            entity.Property(e => e.BodyPartExamined).HasMaxLength(64);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.InstitutionName).HasMaxLength(64);
            entity.Property(e => e.Modality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.NameofPhysiciansReading).HasMaxLength(64);
            entity.Property(e => e.OtherPatientId).HasMaxLength(50);
            entity.Property(e => e.OtherPatientNames).HasMaxLength(64);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PatientPosition).HasMaxLength(64);
            entity.Property(e => e.PatientsBirthDate).HasMaxLength(50);
            entity.Property(e => e.PatientsName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.PatientsSex).HasMaxLength(50);
            entity.Property(e => e.PerformingPhysiciansName).HasMaxLength(64);
            entity.Property(e => e.ReferringPhysiciansName).HasMaxLength(64);
            entity.Property(e => e.SeriesDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SeriesDescription).HasMaxLength(128);
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesModality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.SeriesTime)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.StudyDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StudyDescription).HasMaxLength(128);
            entity.Property(e => e.StudyID).HasMaxLength(50);
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.StudyTime)
                .HasMaxLength(32)
                .IsFixedLength();
        });

        modelBuilder.Entity<DcmFindStudyLevelView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DcmFindStudyLevelView");

            entity.Property(e => e.AccessionNumber).HasMaxLength(50);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.InstitutionName).HasMaxLength(64);
            entity.Property(e => e.Modality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.NameofPhysiciansReading).HasMaxLength(64);
            entity.Property(e => e.OtherPatientId).HasMaxLength(50);
            entity.Property(e => e.OtherPatientNames).HasMaxLength(64);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.PatientsBirthDate).HasMaxLength(50);
            entity.Property(e => e.PatientsName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.PatientsSex).HasMaxLength(50);
            entity.Property(e => e.PerformingPhysiciansName).HasMaxLength(64);
            entity.Property(e => e.ReferringPhysiciansName).HasMaxLength(64);
            entity.Property(e => e.StudyDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StudyDescription).HasMaxLength(128);
            entity.Property(e => e.StudyID).HasMaxLength(50);
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.StudyTime)
                .HasMaxLength(32)
                .IsFixedLength();
        });

        modelBuilder.Entity<DicomDestinationNode>(entity =>
        {
            entity.HasKey(e => e.LogicalName);

            entity.Property(e => e.LogicalName)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.AETitle)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.HostName).HasMaxLength(50);
            entity.Property(e => e.IPAddress)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.RoutingRulePattern).HasMaxLength(256);
            entity.Property(e => e.SendingAETitle)
                .HasMaxLength(64)
                .IsFixedLength();
        });

        modelBuilder.Entity<DicomDocument>(entity =>
        {
            entity.HasKey(e => e.SOPInstanceUID);

            entity.ToTable("DicomDocument");

            entity.Property(e => e.SOPInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FilePath).HasMaxLength(1024);
            entity.Property(e => e.HaveSendToRemote)
                .HasMaxLength(10)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.ImageDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageStatus).HasMaxLength(50);
            entity.Property(e => e.ImageTime)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.KeyImage)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.NumberOfFrames).HasDefaultValue(0);
            entity.Property(e => e.ReferencedSOPInstanceUID).HasMaxLength(128);
            entity.Property(e => e.ReferencedSeriesInstanceUID).HasMaxLength(128);
            entity.Property(e => e.SOPClassUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.StorageDeviceID)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.UnmappedDcmTags).HasMaxLength(1024);
        });

        modelBuilder.Entity<DicomImage>(entity =>
        {
            entity.HasKey(e => e.SOPInstanceUID);

            entity.ToTable("DicomImage");

            entity.Property(e => e.SOPInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FilePath).HasMaxLength(1024);
            entity.Property(e => e.HaveSendToRemote)
                .HasMaxLength(10)
                .HasDefaultValueSql("((0))")
                .IsFixedLength();
            entity.Property(e => e.ImageDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageStatus).HasMaxLength(50);
            entity.Property(e => e.ImageTime)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.KeyImage)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.NumberOfFrames).HasDefaultValue(0);
            entity.Property(e => e.ReferencedSOPInstanceUID).HasMaxLength(128);
            entity.Property(e => e.ReferencedSeriesInstanceUID).HasMaxLength(128);
            entity.Property(e => e.SOPClassUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.StorageDeviceID)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.UnmappedDcmTags).HasMaxLength(1024);
            entity.Property(e => e.ImageMarkerUrl).HasMaxLength(256);
        });

        modelBuilder.Entity<DicomNode>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.AETitle)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.AcceptedTransferSyntaxesCustomize)
                .HasMaxLength(10)
                .HasDefaultValue("False");
            entity.Property(e => e.AuotRoutingDestination)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.EnabledAutoRouting)
                .HasMaxLength(10)
                .HasDefaultValue("False")
                .IsFixedLength();
            entity.Property(e => e.FilterRulePattern).HasMaxLength(50);
            entity.Property(e => e.IPAddress)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.NeedConfirmIPAddress).HasMaxLength(10);
            entity.Property(e => e.RemoteAETitle)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.ServiceJobTypes)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.TransferSyntaxesCustomize).HasMaxLength(512);
            entity.Property(e => e.WorklistQueryPattern).HasDefaultValue(0);
        });

        modelBuilder.Entity<DicomOperationNode>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AETitle)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.IPAddress)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.MoveAETitle)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.OperationType)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.RemoteAETitle)
                .HasMaxLength(64)
                .IsFixedLength();
        });

        modelBuilder.Entity<DicomPDF>(entity =>
        {
            entity.HasKey(e => e.SOPInstanceUID);

            entity.ToTable("DicomPDF");

            entity.Property(e => e.SOPInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FilePath).HasMaxLength(1024);
            entity.Property(e => e.HaveSendToRemote)
                .HasMaxLength(10)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.ImageDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageStatus).HasMaxLength(50);
            entity.Property(e => e.ImageTime)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.KeyImage)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.NumberOfFrames).HasDefaultValue(0);
            entity.Property(e => e.ReferencedSOPInstanceUID).HasMaxLength(128);
            entity.Property(e => e.ReferencedSeriesInstanceUID).HasMaxLength(128);
            entity.Property(e => e.SOPClassUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.StorageDeviceID)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.UnmappedDcmTags).HasMaxLength(1024);
        });

        modelBuilder.Entity<DicomPatient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK_Paitent");

            entity.ToTable("DicomPatient");

            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.EthnicGroup).HasMaxLength(50);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.OtherPatientId).HasMaxLength(50);
            entity.Property(e => e.OtherPatientNames).HasMaxLength(64);
            entity.Property(e => e.PatientComments).HasMaxLength(512);
            entity.Property(e => e.PatientsBirthDate).HasMaxLength(50);
            entity.Property(e => e.PatientsBirthTime).HasMaxLength(50);
            entity.Property(e => e.PatientsName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.PatientsSex).HasMaxLength(50);
        });

        modelBuilder.Entity<DicomSeries>(entity =>
        {
            entity.HasKey(e => e.SeriesInstanceUID).HasName("Series");

            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.BodyPartExamined).HasMaxLength(64);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.PatientPosition).HasMaxLength(64);
            entity.Property(e => e.ReferencedSeriesInstanceUID).HasMaxLength(128);
            entity.Property(e => e.ReferencedStudyInstanceUID).HasMaxLength(128);
            entity.Property(e => e.SeriesDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SeriesDescription).HasMaxLength(128);
            entity.Property(e => e.SeriesModality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.SeriesNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SeriesTime)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
        });

        modelBuilder.Entity<DicomSeriesMap>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("DicomSeriesMap");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.CreateDateTime).HasMaxLength(16);
            entity.Property(e => e.CreateUser).HasMaxLength(32);
            entity.Property(e => e.DicomSeriesDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DicomSeriesShiftDate).HasColumnType("datetime");
            entity.Property(e => e.DicomSeriesTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DicomSeriesUid)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.StoreTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<DicomServiceProvider>(entity =>
        {
            entity.HasKey(e => e.Port).HasName("PK_DicomServiceProvider_1");

            entity.ToTable("DicomServiceProvider");

            entity.Property(e => e.Port).HasDefaultValue(104);
            entity.Property(e => e.AETitle)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DicomServiceType).HasDefaultValue(1);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsFixedLength();
        });

        modelBuilder.Entity<DicomStudy>(entity =>
        {
            entity.HasKey(e => e.StudyInstanceUID).HasName("PK_Study");

            entity.ToTable("DicomStudy");

            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.AccessionNumber).HasMaxLength(50);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.InstitutionName).HasMaxLength(64);
            entity.Property(e => e.Modality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.NameofPhysiciansReading).HasMaxLength(64);
            entity.Property(e => e.NumMappingFailures).HasDefaultValue(0);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.PerformingPhysiciansName).HasMaxLength(64);
            entity.Property(e => e.ProcedureID).HasMaxLength(40);
            entity.Property(e => e.QCGuid).HasMaxLength(128);
            entity.Property(e => e.ReferencedStudyInstanceUID).HasMaxLength(128);
            entity.Property(e => e.ReferringPhysiciansName).HasMaxLength(64);
            entity.Property(e => e.StationName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.StudyDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StudyDescription).HasMaxLength(128);
            entity.Property(e => e.StudyID).HasMaxLength(50);
            entity.Property(e => e.StudyStatus).HasMaxLength(50);
            entity.Property(e => e.StudyTime)
                .HasMaxLength(32)
                .IsFixedLength();
        });

        modelBuilder.Entity<DicomTag>(entity =>
        {
            entity.HasKey(e => e.IdentifyName);

            entity.Property(e => e.IdentifyName)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime).HasMaxLength(16);
            entity.Property(e => e.CreateUser).HasMaxLength(32);
            entity.Property(e => e.DicomElem).HasMaxLength(10);
            entity.Property(e => e.DicomGroup).HasMaxLength(10);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.TagName).HasMaxLength(256);
        });

        modelBuilder.Entity<DicomTagFilter>(entity =>
        {
            entity.HasKey(e => e.TagFilterName);

            entity.Property(e => e.TagFilterName)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime).HasMaxLength(16);
            entity.Property(e => e.CreateUser).HasMaxLength(32);
            entity.Property(e => e.Description).HasMaxLength(256);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
        });

        modelBuilder.Entity<DicomTagFilterDetail>(entity =>
        {
            entity.HasKey(e => new { e.TagFilterName, e.TagIdentifyName }).HasName("PK_DicomTagFileDetail");

            entity.ToTable("DicomTagFilterDetail");

            entity.Property(e => e.TagFilterName)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.TagIdentifyName)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.AndAll)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime).HasMaxLength(16);
            entity.Property(e => e.CreateUser).HasMaxLength(32);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.TagRule).HasMaxLength(20);
            entity.Property(e => e.Value).HasMaxLength(256);
        });

        modelBuilder.Entity<DicomVideo>(entity =>
        {
            entity.HasKey(e => e.SOPInstanceUID);

            entity.ToTable("DicomVideo");

            entity.Property(e => e.SOPInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FilePath).HasMaxLength(1024);
            entity.Property(e => e.HaveSendToRemote)
                .HasMaxLength(10)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.ImageDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageStatus).HasMaxLength(50);
            entity.Property(e => e.ImageTime)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.KeyImage)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.NumberOfFrames).HasDefaultValue(0);
            entity.Property(e => e.ReferencedSOPInstanceUID).HasMaxLength(128);
            entity.Property(e => e.ReferencedSeriesInstanceUID).HasMaxLength(128);
            entity.Property(e => e.SOPClassUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.StorageDeviceID)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.UnmappedDcmTags).HasMaxLength(1024);
        });

        modelBuilder.Entity<EnvironmentConfig>(entity =>
        {
            entity.HasKey(e => new { e.Item, e.Name });

            entity.ToTable("EnvironmentConfig");

            entity.Property(e => e.Item)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.Value).HasMaxLength(1024);
        });

        modelBuilder.Entity<FileOperationRecord>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.OperationDate, e.OperationTime, e.Type }).HasName("PK_VM_DownloadFileLogger");

            entity.ToTable("FileOperationRecord");

            entity.Property(e => e.UserId)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.OperationDate).HasMaxLength(10);
            entity.Property(e => e.OperationTime).HasMaxLength(10);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.PatientsName).HasMaxLength(64);
            entity.Property(e => e.StudyDate).HasMaxLength(10);
            entity.Property(e => e.StudyDescription).HasMaxLength(128);
            entity.Property(e => e.StudyInstanceUID).HasMaxLength(128);
        });

        modelBuilder.Entity<FunctionRoleGroup>(entity =>
        {
            entity.ToTable("FunctionRoleGroup");

            entity.Property(e => e.FunctionName).HasMaxLength(96);
            entity.Property(e => e.RoleName).HasMaxLength(96);
        });

        modelBuilder.Entity<HangingProtocolConfig>(entity =>
        {
            entity.HasKey(e => e.DisplayIndex).HasName("PK_HangingProtocolConfigIndex");

            entity.ToTable("HangingProtocolConfig");

            entity.Property(e => e.DisplayIndex).ValueGeneratedNever();
            entity.Property(e => e.BottomMargin).HasDefaultValue(20);
            entity.Property(e => e.BoxMargin).HasDefaultValue(5);
            entity.Property(e => e.Caption)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.ColumnCounts).HasDefaultValue(3);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.LeftMargin).HasDefaultValue(20);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.RightMargin).HasDefaultValue(20);
            entity.Property(e => e.RowCounts).HasDefaultValue(3);
            entity.Property(e => e.TopMargin).HasDefaultValue(20);
        });

        modelBuilder.Entity<ImageMarkToolSet>(entity =>
        {
            entity.HasKey(e => e.ToolName).HasName("PK_ToolName");

            entity.ToTable("ImageMarkToolSet");

            entity.Property(e => e.ToolName)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ColorRGB)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FontName)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FontSize).HasDefaultValue(7);
            entity.Property(e => e.LineWidth)
                .HasDefaultValue(0.7m)
                .HasColumnType("decimal(18, 1)");
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.PointDiameter)
                .HasDefaultValue(6m)
                .HasColumnType("decimal(18, 1)");
            entity.Property(e => e.ValueDecimalPlace).HasDefaultValue(2);
            entity.Property(e => e.ValueLeftOffset).HasDefaultValue(7);
        });

        modelBuilder.Entity<ImageOverlayInfoSet>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK_InfoConfigName");

            entity.ToTable("ImageOverlayInfoSet");

            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Active)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Description).HasMaxLength(64);
            entity.Property(e => e.Modality)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
        });

        modelBuilder.Entity<InstitutionInfo>(entity =>
        {
            entity.HasKey(e => e.Institution);

            entity.ToTable("InstitutionInfo");

            entity.Property(e => e.Institution)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Description).HasMaxLength(512);
            entity.Property(e => e.ModifiedDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedUser)
                .HasMaxLength(32)
                .IsFixedLength();
        });

        modelBuilder.Entity<JobOptResultLog>(entity =>
        {
            entity.HasKey(e => new { e.PatientID, e.Date, e.StudyInstanceUID });

            entity.ToTable("JobOptResultLog");

            entity.Property(e => e.PatientID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.Date)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.CallingAETitle).HasMaxLength(100);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.OptContent).IsUnicode(false);
        });

        modelBuilder.Entity<LoginUserDatum>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("PK_UserID");

            entity.Property(e => e.UserID).HasMaxLength(32);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.DoctorCName).HasMaxLength(128);
            entity.Property(e => e.DoctorCode).HasMaxLength(20);
            entity.Property(e => e.DoctorEName).HasMaxLength(128);
            entity.Property(e => e.IsSupervisor)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.JobTitle).HasMaxLength(32);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.RoleList).HasMaxLength(1024);
            entity.Property(e => e.Title).HasMaxLength(64);
            entity.Property(e => e.UserGroupList).HasMaxLength(1024);
            entity.Property(e => e.UserPassword).HasMaxLength(32);
        });

        modelBuilder.Entity<ModalityLookupTable>(entity =>
        {
            entity.HasKey(e => e.DisplayIndex).HasName("PK_ModalityLookupTableIndex");

            entity.ToTable("ModalityLookupTable");

            entity.Property(e => e.DisplayIndex).ValueGeneratedNever();
            entity.Property(e => e.Caption)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.WindowCenter).HasDefaultValue(287);
            entity.Property(e => e.WindowWidth).HasDefaultValue(106);
        });

        modelBuilder.Entity<OperationRecord>(entity =>
        {
            entity.ToTable("OperationRecord");

            entity.Property(e => e.DateTime).HasMaxLength(128);
            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.Operation).HasMaxLength(128);
            entity.Property(e => e.OperationName).HasMaxLength(128);
            entity.Property(e => e.Operator).HasMaxLength(50);
            entity.Property(e => e.QCGuid).HasMaxLength(128);
            entity.Property(e => e.Reason).HasMaxLength(128);
        });

        modelBuilder.Entity<PtCase>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("PtCase");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.CareProviderId).HasMaxLength(32);
            entity.Property(e => e.CaseBeginTime).HasColumnType("datetime");
            entity.Property(e => e.CaseCloseCareProviderName).HasMaxLength(32);
            entity.Property(e => e.CaseCloseStatus).HasMaxLength(64);
            entity.Property(e => e.CaseCloseTime).HasColumnType("datetime");
            entity.Property(e => e.CaseEntityId).HasMaxLength(32);
            entity.Property(e => e.CaseLocation).HasMaxLength(64);
            entity.Property(e => e.CreateDateTime).HasMaxLength(16);
            entity.Property(e => e.CreateUser).HasMaxLength(32);
            entity.Property(e => e.EncounterNumber).HasMaxLength(32);
            entity.Property(e => e.LIfeTimeNumber).HasMaxLength(32);
            entity.Property(e => e.LoadTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.StoreTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<PtCaseRecord>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("PtCaseRecord");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.CareProviderId).HasMaxLength(32);
            entity.Property(e => e.Comment).HasMaxLength(2000);
            entity.Property(e => e.CreateDateTime).HasMaxLength(16);
            entity.Property(e => e.CreateUser).HasMaxLength(32);
            entity.Property(e => e.FormData).HasMaxLength(4000);
            entity.Property(e => e.LoadTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.ObservationDateTime).HasColumnType("datetime");
            entity.Property(e => e.ObservationShiftDate).HasColumnType("datetime");
            entity.Property(e => e.StoreTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<PtPatient>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("PtPatient");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LifeTimeNumber).HasMaxLength(50);
            entity.Property(e => e.NationalId).HasMaxLength(50);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<QCOperationRecordView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("QCOperationRecordView");

            entity.Property(e => e.AccessionNumber).HasMaxLength(50);
            entity.Property(e => e.Modality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.Operator).HasMaxLength(50);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.QCGuid).HasMaxLength(128);
            entity.Property(e => e.StudyDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StudyDescription).HasMaxLength(128);
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(256)
                .IsFixedLength();
        });

        modelBuilder.Entity<QC_AutoMappingConfig>(entity =>
        {
            entity.HasKey(e => e.StationName);

            entity.ToTable("QC_AutoMappingConfig");

            entity.Property(e => e.StationName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.EnvSetup).HasMaxLength(1024);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.StoreSCP).HasMaxLength(1024);
            entity.Property(e => e.WkSCP).HasMaxLength(1024);
        });

        modelBuilder.Entity<QC_ResendDcmAEDatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("QC_ResendDcmAEData");

            entity.Property(e => e.AETitle)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.RemoteAETitle)
                .HasMaxLength(64)
                .IsFixedLength();
        });

        modelBuilder.Entity<ReportDefine>(entity =>
        {
            entity.HasKey(e => e.Puid).HasName("Puid");

            entity.ToTable("ReportDefine");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.CreateDateTime).HasColumnType("datetime");
            entity.Property(e => e.FormDefineFile).HasMaxLength(256);
            entity.Property(e => e.ModifyDateTime).HasColumnType("datetime");
            entity.Property(e => e.ReportName).HasMaxLength(64);
        });

        modelBuilder.Entity<RoleFunction>(entity =>
        {
            entity.HasKey(e => e.FunctionName).HasName("PK_QCFunction");

            entity.ToTable("RoleFunction");

            entity.Property(e => e.FunctionName).HasMaxLength(96);
            entity.Property(e => e.CorrespondElementId).HasMaxLength(128);
            entity.Property(e => e.Description).HasMaxLength(128);
        });

        modelBuilder.Entity<RoleFunctionView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RoleFunctionView");

            entity.Property(e => e.CorrespondElementId).HasMaxLength(128);
            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.FunctionName).HasMaxLength(96);
            entity.Property(e => e.RoleName).HasMaxLength(96);
        });

        modelBuilder.Entity<RoleGroup>(entity =>
        {
            entity.HasKey(e => e.RoleName);

            entity.ToTable("RoleGroup");

            entity.Property(e => e.RoleName).HasMaxLength(96);
            entity.Property(e => e.Description).HasMaxLength(128);
        });

        modelBuilder.Entity<SearchDocumentPathView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SearchDocumentPathView");

            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FilePath).HasMaxLength(1024);
            entity.Property(e => e.ImageDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageFullPath).HasMaxLength(1279);
            entity.Property(e => e.ImageNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageStatus).HasMaxLength(50);
            entity.Property(e => e.ImageTime)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.SOPClassUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SOPInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesModality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.StorageDescription).HasMaxLength(128);
            entity.Property(e => e.StorageDeviceID)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.StoragePath).HasMaxLength(255);
        });

        modelBuilder.Entity<SearchImagePathView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SearchImagePathView");

            entity.Property(e => e.AccessionNumber).HasMaxLength(50);
            entity.Property(e => e.BodyPartExamined).HasMaxLength(64);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FilePath).HasMaxLength(1024);
            entity.Property(e => e.HttpFilePath).HasMaxLength(1024);
            entity.Property(e => e.ImageDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageFullPath).HasMaxLength(1279);
            entity.Property(e => e.ImageStatus).HasMaxLength(50);
            entity.Property(e => e.ImageTime)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PatientPosition).HasMaxLength(64);
            entity.Property(e => e.PatientsName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.SOPClassUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SOPInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesModality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.StorageDescription).HasMaxLength(128);
            entity.Property(e => e.StorageDeviceID)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.StoragePath).HasMaxLength(255);
            entity.Property(e => e.StudyDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StudyDescription).HasMaxLength(128);
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.StudyTime)
                .HasMaxLength(32)
                .IsFixedLength();
        });

        modelBuilder.Entity<SearchPatientStudyView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SearchPatientStudyView");

            entity.Property(e => e.AccessionNumber).HasMaxLength(50);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.InstitutionName).HasMaxLength(64);
            entity.Property(e => e.Modality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.NameofPhysiciansReading).HasMaxLength(64);
            entity.Property(e => e.OtherPatientId).HasMaxLength(50);
            entity.Property(e => e.OtherPatientNames).HasMaxLength(64);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.PatientsBirthDate).HasMaxLength(50);
            entity.Property(e => e.PatientsName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.PatientsSex).HasMaxLength(50);
            entity.Property(e => e.PerformingPhysiciansName).HasMaxLength(64);
            entity.Property(e => e.QCGuid).HasMaxLength(128);
            entity.Property(e => e.ReferringPhysiciansName).HasMaxLength(64);
            entity.Property(e => e.StudyDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StudyDescription).HasMaxLength(128);
            entity.Property(e => e.StudyID).HasMaxLength(50);
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.StudyTime)
                .HasMaxLength(32)
                .IsFixedLength();
        });

        modelBuilder.Entity<SearchSeriesAndGroupImageCountView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SearchSeriesAndGroupImageCountView");

            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.Modality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.ReceiveTime).HasMaxLength(6);
            entity.Property(e => e.SeriesDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SeriesTime)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
        });

        modelBuilder.Entity<SearchVideoFullPathView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SearchVideoFullPathView");

            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FileFullPath).HasMaxLength(1279);
            entity.Property(e => e.FilePath).HasMaxLength(1024);
            entity.Property(e => e.HaveSendToRemote)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageNumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageStatus).HasMaxLength(50);
            entity.Property(e => e.ImageTime)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.KeyImage)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.ReferencedSOPInstanceUID).HasMaxLength(128);
            entity.Property(e => e.ReferencedSeriesInstanceUID).HasMaxLength(128);
            entity.Property(e => e.SOPClassUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SOPInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.StorageDeviceID)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.UnmappedDcmTags).HasMaxLength(1024);
        });

        modelBuilder.Entity<SearchVideoFullProfileView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SearchVideoFullProfileView");

            entity.Property(e => e.AccessionNumber).HasMaxLength(50);
            entity.Property(e => e.BodyPartExamined).HasMaxLength(64);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.FilePath).HasMaxLength(1024);
            entity.Property(e => e.HttpFilePath).HasMaxLength(1024);
            entity.Property(e => e.ImageDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ImageFullPath).HasMaxLength(1279);
            entity.Property(e => e.ImageStatus).HasMaxLength(50);
            entity.Property(e => e.ImageTime)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PatientPosition).HasMaxLength(64);
            entity.Property(e => e.PatientsName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.SOPClassUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SOPInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesInstanceUID)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.SeriesModality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.StorageDescription).HasMaxLength(128);
            entity.Property(e => e.StorageDeviceID)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.StoragePath).HasMaxLength(255);
            entity.Property(e => e.StudyDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StudyDescription).HasMaxLength(128);
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.StudyTime)
                .HasMaxLength(32)
                .IsFixedLength();
        });

        modelBuilder.Entity<StorageDevice>(entity =>
        {
            entity.ToTable("StorageDevice");

            entity.Property(e => e.StorageDeviceID)
                .HasMaxLength(24)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.DicomFilePathRule)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.IPAddress).HasMaxLength(16);
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.StorageDescription).HasMaxLength(128);
            entity.Property(e => e.StorageLevel).HasMaxLength(2);
            entity.Property(e => e.StoragePath).HasMaxLength(255);
            entity.Property(e => e.UserID).HasMaxLength(16);
            entity.Property(e => e.UserPassword).HasMaxLength(16);
        });

        modelBuilder.Entity<StudyListWithVideo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("StudyListWithVideo");

            entity.Property(e => e.AccessionNumber).HasMaxLength(50);
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.Modality)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(50);
            entity.Property(e => e.ModifiedUser).HasMaxLength(50);
            entity.Property(e => e.PatientId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PatientsBirthDate).HasMaxLength(50);
            entity.Property(e => e.PatientsName)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.Physician).HasMaxLength(64);
            entity.Property(e => e.Sex).HasMaxLength(50);
            entity.Property(e => e.StudyDate)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.StudyID).HasMaxLength(50);
            entity.Property(e => e.StudyInstanceUID)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.StudyTime)
                .HasMaxLength(32)
                .IsFixedLength();
        });

        modelBuilder.Entity<SysBed>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("SysBed");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.BedLabel).HasMaxLength(32);
            entity.Property(e => e.BedLabelHIS).HasMaxLength(32);
            entity.Property(e => e.BedType).HasMaxLength(32);
            entity.Property(e => e.RoomLabel).HasMaxLength(32);
        });

        modelBuilder.Entity<SysClinicalUnit>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("SysClinicalUnit");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.BedZoneList).HasMaxLength(128);
            entity.Property(e => e.DisplayLabel).HasMaxLength(32);
            entity.Property(e => e.HISCode).HasMaxLength(32);
            entity.Property(e => e.Location).HasMaxLength(32);
        });

        modelBuilder.Entity<SysClinicalUnitShift>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("SysClinicalUnitShift");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.ShiftLongLabel).HasMaxLength(32);
            entity.Property(e => e.ShiftShortLabel).HasMaxLength(32);
        });

        modelBuilder.Entity<SysRoleFunction>(entity =>
        {
            entity.HasKey(e => e.Puid);

            entity.ToTable("SysRoleFunction");

            entity.Property(e => e.Puid).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<SystemConfig>(entity =>
        {
            entity.HasKey(e => e.SysConfigName).HasName("PK_SystemConfigName");

            entity.ToTable("SystemConfig");

            entity.Property(e => e.SysConfigName)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.Value).HasMaxLength(512);
        });

        modelBuilder.Entity<SystemConfiguration>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SystemConfiguration");

            entity.Property(e => e.CStoreBackupFilePath)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.CStroeTmpFilesPath).HasMaxLength(256);
            entity.Property(e => e.CreateDateTime).HasMaxLength(16);
            entity.Property(e => e.CreateUser).HasMaxLength(32);
            entity.Property(e => e.DailyTimerInterval).HasDefaultValue(1000);
            entity.Property(e => e.Description)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.ErrorImagesPath)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.JobProcessTimerInterval).HasDefaultValue(1000);
            entity.Property(e => e.LogLevel).HasDefaultValue(1);
            entity.Property(e => e.LogRootPath)
                .HasMaxLength(256)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.PACSMessageWriteToLog).HasMaxLength(4);
            entity.Property(e => e.ScheduleMessageWriteToLog).HasMaxLength(4);
            entity.Property(e => e.WorklistMessageWriteToLog).HasMaxLength(4);
        });

        modelBuilder.Entity<VM_PtProductionCapImgRegion>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("VM_PtProductionCapImgRegion");

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.Type).HasDefaultValue(0);
        });

        modelBuilder.Entity<VideoBroadcasting>(entity =>
        {
            entity.HasKey(e => new { e.Name, e.HostName, e.Port }).HasName("PK_VideoBroadcasting_1");

            entity.ToTable("VideoBroadcasting");

            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.HostName)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.CreateDateTime)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.CreateUser)
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsFixedLength();
            entity.Property(e => e.ModifiedDateTime).HasMaxLength(16);
            entity.Property(e => e.ModifiedUser).HasMaxLength(32);
            entity.Property(e => e.UserID)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<vwPatientBedLocationCurrent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwPatientBedLocationCurrent");

            entity.Property(e => e.BedLabel).HasMaxLength(32);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.EncounterNumber).HasMaxLength(32);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LifeTimeNumber).HasMaxLength(50);
            entity.Property(e => e.NationalId).HasMaxLength(50);
            entity.Property(e => e.SysInTime).HasColumnType("datetime");
            entity.Property(e => e.SysOutTime).HasColumnType("datetime");
            entity.Property(e => e.TransferInTime).HasColumnType("datetime");
            entity.Property(e => e.TransferOutStatus).HasMaxLength(32);
            entity.Property(e => e.TransferOutTime).HasColumnType("datetime");
            entity.Property(e => e.WardLabel).HasMaxLength(32);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
