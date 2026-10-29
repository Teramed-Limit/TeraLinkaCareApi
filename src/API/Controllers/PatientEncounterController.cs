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
    /// 依條件搜尋住院病人（病房、病歷號、姓名、住院日期區間）
    /// </summary>
    /// <remarks>全部條件未填時列出目前在院病人；最多回傳 500 筆；無結果回傳空陣列。</remarks>
    /// <param name="request">搜尋條件</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>病患就醫紀錄列表</returns>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<PatientEncounterListDto>>> Search(
        [FromQuery] PatientEncounterSearchRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Guid? clinicalUnitGuid = null;
            if (!string.IsNullOrWhiteSpace(request.ClinicalUnitId))
            {
                if (!Guid.TryParse(request.ClinicalUnitId, out var parsed))
                {
                    _logger.LogWarning("無效的臨床單位 ID 格式: {ClinicalUnitId}", request.ClinicalUnitId);
                    return BadRequest("臨床單位 ID 格式不正確");
                }
                clinicalUnitGuid = parsed;
            }

            if (
                request.SysInFrom.HasValue
                && request.SysInTo.HasValue
                && request.SysInFrom.Value.Date > request.SysInTo.Value.Date
            )
            {
                return BadRequest("住院日期起不可晚於住院日期迄");
            }

            var encounters = await _patientEncounterService.SearchPatientEncounters(
                clinicalUnitGuid,
                request.LifeTimeNumber,
                request.Name,
                request.SysInFrom,
                request.SysInTo,
                cancellationToken
            );

            return Ok(encounters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "搜尋病患就醫紀錄時發生錯誤");
            return StatusCode(500, "搜尋病患就醫紀錄時發生錯誤");
        }
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
