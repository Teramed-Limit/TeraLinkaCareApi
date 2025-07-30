using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraLinkaCareApi.Core.Domain.Entities;
using TeraLinkaCareApi.Core.Domain.Interfaces;
using TeraLinkaCareApi.Infrastructure.Persistence;

namespace TeraLinkaCareApi.API.Controllers;

/// <summary>
/// 病患資料控制器
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DicomPatientController : ControllerBase
{
    private readonly ILogger<DicomPatientController> _logger;
    private readonly IRepository<DicomPatient, CRSDbContext> _repository;

    public DicomPatientController(
        ILogger<DicomPatientController> logger,
        IRepository<DicomPatient, CRSDbContext> repository
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 根據病患 ID 取得病患資料
    /// </summary>
    /// <param name="patientId">病患 ID</param>
    /// <returns>病患資料</returns>
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
    [HttpGet("patientId/{patientId}")]
    public async Task<ActionResult<A_PtEncounter>> GetByPatientId(string patientId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(patientId))
            {
                _logger.LogWarning("Invalid patient ID provided: {PatientId}", patientId);
                return BadRequest("病患 ID 不能為空");
            }

            var patients = await _repository.GetByConditionAsync(x => x.PatientId == patientId
            );

            if (!patients.Any())
            {
                _logger.LogWarning("Patient not found with ID: {PatientId}", patientId);
                return NotFound($"找不到病患 ID 為 {patientId} 的資料");
            }

            var patient =
                patients.Select((x) => new A_PtEncounter()
                    {
                        Puid = Guid.NewGuid(),
                        LifeTimeNumber = x.PatientId.Trim(),
                        // EncounterNumber = x
                        LastName = x.PatientsName?.Trim(),
                        FirstName = "",
                        Gender = x.PatientsSex,
                        DateOfBirth = !string.IsNullOrEmpty(x.PatientsBirthDate) ? 
                            DateTime.TryParseExact(x.PatientsBirthDate, "yyyyMMdd", null, 
                                System.Globalization.DateTimeStyles.None, out var birthDate) ? 
                                birthDate : (DateTime?)null : null,
                        NationalId = x.OtherPatientId,
                        UpdateTime = !string.IsNullOrEmpty(x.ModifiedDateTime) ? 
                            DateTime.TryParse(x.ModifiedDateTime, out var modifiedDate) ? 
                                modifiedDate : DateTime.TryParse(x.CreateDateTime, out var createDate) ? 
                                    createDate : (DateTime?)null : 
                            DateTime.TryParse(x.CreateDateTime, out var createDate2) ? 
                                createDate2 : (DateTime?)null
                    })
                    .First();

            return Ok(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient data for ID: {PatientId}", patientId);
            return StatusCode(500, "取得病患資料時發生錯誤");
        }
    }
}