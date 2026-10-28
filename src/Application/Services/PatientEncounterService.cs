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
