using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Application.Services;
using TeraLinkaCareApi.Core.Domain.Entities;
using TeraLinkaCareApi.Core.Domain.Interfaces;
using TeraLinkaCareApi.Infrastructure.Persistence;

namespace TeraLinkaCareApi.API.Controllers;

/// <summary>
/// 病患就醫紀錄控制器
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PatientEncounterController : ControllerBase
{
    private readonly ILogger<PatientEncounterController> _logger;
    private readonly IRepository<A_PtEncounter, CRSDbContext> _repository;
    private readonly PatientEncounterService _patientEncounterService;

    public PatientEncounterController(
        ILogger<PatientEncounterController> logger,
        IRepository<A_PtEncounter, CRSDbContext> repository,
        PatientEncounterService patientEncounterService
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _patientEncounterService = patientEncounterService;
    }

    /// <summary>
    /// 根據臨床單位 ID 取得病患就醫紀錄列表
    /// </summary>
    /// <param name="clinicalUnitId">臨床單位 ID (GUID 格式)</param>
    /// <returns>病患就醫紀錄列表</returns>
    [HttpGet("clinicalUnitId/{clinicalUnitId}")]
    public async Task<ActionResult<IEnumerable<PatientEncounterListDto>>> GetByClinicalUnitId(
        string clinicalUnitId
    )
    {
        try
        {
            if (!Guid.TryParse(clinicalUnitId, out Guid clinicalUnitGuid))
            {
                _logger.LogWarning("無效的臨床單位 ID 格式: {ClinicalUnitId}", clinicalUnitId);
                return BadRequest("臨床單位 ID 格式不正確");
            }

            var encounters = await _patientEncounterService.GetPatientEncounterList(
                clinicalUnitGuid
            );

            if (!encounters.Any())
            {
                return NotFound($"找不到臨床單位 ID {clinicalUnitId} 的病患就醫紀錄");
            }

            return Ok(encounters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "取得臨床單位 ID {ClinicalUnitId} 的病患就醫紀錄時發生錯誤", clinicalUnitId);
            return StatusCode(500, "取得病患就醫紀錄時發生錯誤");
        }
    }
}
