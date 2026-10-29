using Microsoft.EntityFrameworkCore;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Infrastructure.Persistence;

namespace TeraLinkaCareApi.Application.Services;

public class PatientEncounterService
{
    private readonly CRSDbContext _context;

    public PatientEncounterService(CRSDbContext context)
    {
        _context = context;
    }

    public async Task<List<PatientEncounterListDto>> GetPatientEncounterList(Guid clinicalUnitId)
    {
        var patientEncounterList =
            from pe in _context.A_PtEncounters.AsNoTracking()
            join bed in _context.SysBeds on pe.BedLabel equals bed.BedLabel
            where pe.ClinicalUnitPuid == clinicalUnitId
            orderby bed.Sequence
            select new PatientEncounterListDto
            {
                Puid = pe.Puid,
                PatientPuid = pe.PatientPuid,
                PtEncounterPuid = pe.PtEncounterPuid,
                ClinicalUnitPuid = pe.ClinicalUnitPuid,
                ClinicalUnitLabel = pe.ClinicalUnitLabel,
                BedLabel = pe.BedLabel,
                SysInTime = pe.SysInTime,
                SysOutTime = pe.SysOutTime,
                EncounterNumber = pe.EncounterNumber,
                LifeTimeNumber = pe.LifeTimeNumber,
                LastName = pe.LastName,
                FirstName = pe.FirstName,
                Gender = pe.Gender,
                DateOfBirth = pe.DateOfBirth,
                AttendingPhysician = pe.AttendingPhysician,
            };

        return await patientEncounterList.ToListAsync();
    }

    public const int SearchMaxRows = 500;

    /// <summary>
    /// 依條件搜尋住院病人；未給任何條件時列出目前在院病人（SysOutTime 為 null）
    /// </summary>
    public async Task<List<PatientEncounterListDto>> SearchPatientEncounters(
        Guid? clinicalUnitId,
        string? lifeTimeNumber,
        string? name,
        DateTime? sysInFrom,
        DateTime? sysInTo,
        CancellationToken cancellationToken = default
    )
    {
        var encounters = _context.A_PtEncounters.AsNoTracking().AsQueryable();

        var hasCondition =
            clinicalUnitId.HasValue
            || !string.IsNullOrWhiteSpace(lifeTimeNumber)
            || !string.IsNullOrWhiteSpace(name)
            || sysInFrom.HasValue
            || sysInTo.HasValue;

        if (!hasCondition)
        {
            encounters = encounters.Where(pe => pe.SysOutTime == null);
        }

        if (clinicalUnitId.HasValue)
        {
            encounters = encounters.Where(pe => pe.ClinicalUnitPuid == clinicalUnitId.Value);
        }

        if (!string.IsNullOrWhiteSpace(lifeTimeNumber))
        {
            var lifeTime = lifeTimeNumber.Trim();
            encounters = encounters.Where(pe => pe.LifeTimeNumber == lifeTime);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            var keyword = name.Trim();
            encounters = encounters.Where(pe =>
                ((pe.LastName ?? "") + (pe.FirstName ?? "")).Contains(keyword)
            );
        }

        if (sysInFrom.HasValue)
        {
            var from = sysInFrom.Value.Date;
            encounters = encounters.Where(pe => pe.SysInTime >= from);
        }

        if (sysInTo.HasValue)
        {
            var toExclusive = sysInTo.Value.Date.AddDays(1);
            encounters = encounters.Where(pe => pe.SysInTime < toExclusive);
        }

        var query =
            from pe in encounters
            join bed in _context.SysBeds.AsNoTracking()
                on new { pe.BedLabel, ClinicalUnitPuid = pe.ClinicalUnitPuid }
                equals new { bed.BedLabel, ClinicalUnitPuid = (Guid?)bed.ClinicalUnitPuid }
                into bedGroup
            from bed in bedGroup.DefaultIfEmpty()
            // 在院病人優先，避免 Take(500) 截斷時切掉在院病人
            orderby (pe.SysOutTime == null ? 0 : 1),
                pe.ClinicalUnitLabel,
                bed.Sequence,
                pe.SysInTime descending
            select new PatientEncounterListDto
            {
                Puid = pe.Puid,
                PatientPuid = pe.PatientPuid,
                PtEncounterPuid = pe.PtEncounterPuid,
                ClinicalUnitPuid = pe.ClinicalUnitPuid,
                ClinicalUnitLabel = pe.ClinicalUnitLabel,
                BedLabel = pe.BedLabel,
                SysInTime = pe.SysInTime,
                SysOutTime = pe.SysOutTime,
                EncounterNumber = pe.EncounterNumber,
                LifeTimeNumber = pe.LifeTimeNumber,
                LastName = pe.LastName,
                FirstName = pe.FirstName,
                Gender = pe.Gender,
                DateOfBirth = pe.DateOfBirth,
                AttendingPhysician = pe.AttendingPhysician,
            };

        return await query.Take(SearchMaxRows).ToListAsync(cancellationToken);
    }

    public async Task<PatientInfoDto?> GetPatientInfo(string patientId, string encounterId)
    {
        return await _context
            .A_PtEncounters.AsNoTracking()
            .Where(pe => pe.LifeTimeNumber == patientId && pe.EncounterNumber == encounterId)
            .Select(pe => new PatientInfoDto
            {
                Puid = pe.Puid,
                PatientPuid = pe.PatientPuid,
                PtEncounterPuid = pe.PtEncounterPuid,
                ClinicalUnitPuid = pe.ClinicalUnitPuid,
                ClinicalUnitLabel = pe.ClinicalUnitLabel,
                BedLabel = pe.BedLabel,
                LifeTimeNumber = pe.LifeTimeNumber,
                EncounterNumber = pe.EncounterNumber,
                LastName = pe.LastName,
                FirstName = pe.FirstName,
                Gender = pe.Gender,
                DateOfBirth = pe.DateOfBirth,
                NationalId = pe.NationalId,
                ClinicalService = pe.ClinicalService,
                AttendingPhysician = pe.AttendingPhysician,
                SysInTime = pe.SysInTime,
                SysOutTime = pe.SysOutTime,
                TransferInTime = pe.TransferInTime,
                TransferOutTime = pe.TransferOutTime,
                TransferOutStatus = pe.TransferOutStatus,
                UpdateTime = pe.UpdateTime,
                BodyHeight = pe.BodyHeight,
                BodyWeight = pe.BodyWeight,
            })
            .FirstOrDefaultAsync();
    }
}
