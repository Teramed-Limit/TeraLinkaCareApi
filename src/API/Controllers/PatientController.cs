using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Application.Services;

namespace TeraLinkaCareApi.API.Controllers;

/// <summary>
/// 病患資料控制器
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;
    private readonly PatientEncounterService _patientEncounterService;

    public PatientController(
        ILogger<PatientController> logger,
        PatientEncounterService patientEncounterService
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _patientEncounterService = patientEncounterService;
    }

    /// <summary>
    /// 根據病患 ID 取得病患資料
    /// </summary>
    /// <param name="patientId">病患 ID</param>
    /// <returns>病患資料</returns>
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
    [HttpGet("patientId/{patientId}/encounterId/{encounterId}")]
    public async Task<ActionResult<PatientInfoDto>> GetByPatientId(string patientId, string encounterId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(patientId))
            {
                _logger.LogWarning("Invalid patient ID provided: {PatientId}", patientId);
                return BadRequest("病患 ID 不能為空");
            }

            var patient = await _patientEncounterService.GetPatientInfo(patientId, encounterId);

            if (patient == null)
            {
                _logger.LogWarning("Patient not found with ID: {PatientId}", patientId);
                return NotFound($"找不到病患 ID 為 {patientId} 的資料");
            }

            return Ok(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient data for ID: {PatientId}", patientId);
            return StatusCode(500, "取得病患資料時發生錯誤");
        }
    }
}
