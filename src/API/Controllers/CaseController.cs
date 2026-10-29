using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Application.UseCases.Cases.Commands.SetCaseStatus;
using TeraLinkaCareApi.Application.UseCases.Cases.Queries.GetCaseHistory;
using TeraLinkaCareApi.Application.UseCases.Cases.Queries.GetCaseImage;
using TeraLinkaCareApi.Application.UseCases.Cases.Queries.GetCasePreview;
using TeraLinkaCareApi.Application.UseCases.Cases.Queries.GetCaseRecordDate;
using TeraLinkaCareApi.Application.UseCases.Cases.Queries.GetCasesByPatient;

namespace TeraLinkaCareApi.API.Controllers;

/// <summary>
/// 傷口案例管理控制器
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CaseController : ControllerBase
{
    private readonly ILogger<CaseController> _logger;
    private readonly IMediator _mediator;

    public CaseController(ILogger<CaseController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// 根據病人ID和就醫ID獲取案例列表
    /// </summary>
    /// <param name="patientId">病人ID</param>
    /// <param name="encounterId">就醫ID</param>
    /// <returns>按身體部位分類的案例字典</returns>
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
    [HttpGet("patientId/{patientId}/encounterId/{encounterId}/cases")]
    public async Task<ActionResult<Dictionary<string, List<CaseDto>>>> GetByPatientIdAndEncounterId(
        string patientId,
        string encounterId
    )
    {
        try
        {
            if (string.IsNullOrEmpty(patientId) || string.IsNullOrEmpty(encounterId))
            {
                return BadRequest("病人ID和就醫ID不能為空");
            }

            var query = new GetCasesByPatientQuery(patientId, encounterId);
            var result = await _mediator.Send(query);

            if (!result.Succeeded)
            {
                return StatusCode(500, result.Error);
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "獲取案例列表時發生錯誤: PatientId={PatientId}, EncounterId={EncounterId}",
                patientId,
                encounterId
            );
            return StatusCode(500, "獲取案例列表時發生錯誤");
        }
    }

    /// <summary>
    /// 取得傷照預覽頁資料（依部位分組，每個案例的 series 最新在前、影像依 ImageNumber 排序）
    /// </summary>
    /// <param name="patientId">病人ID</param>
    /// <param name="encounterId">就醫ID</param>
    /// <param name="cancellationToken">取消權杖</param>
    /// <returns>預覽資料</returns>
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
    [HttpGet("patientId/{patientId}/encounterId/{encounterId}/preview")]
    public async Task<ActionResult<CasePreviewDto>> GetPreview(
        string patientId,
        string encounterId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            if (string.IsNullOrEmpty(patientId) || string.IsNullOrEmpty(encounterId))
            {
                return BadRequest("病人ID和就醫ID不能為空");
            }

            var result = await _mediator.Send(
                new GetCasePreviewQuery(patientId, encounterId),
                cancellationToken
            );

            if (!result.Succeeded)
            {
                // 例外細節已由 handler 記錄於 log，回應只給固定訊息
                return StatusCode(500, "獲取案例預覽失敗");
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "獲取案例預覽時發生錯誤: PatientId={PatientId}, EncounterId={EncounterId}",
                patientId,
                encounterId
            );
            return StatusCode(500, "獲取案例預覽失敗");
        }
    }

    /// <summary>
    /// 獲取案例歷史記錄
    /// </summary>
    /// <param name="caseId">案例ID</param>
    /// <returns>案例歷史記錄</returns>
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
    [HttpGet("caseId/{caseId}/caseHistory")]
    public async Task<ActionResult<CaseHistoryDto>> GetCaseHistory(string caseId)
    {
        try
        {
            if (!Guid.TryParse(caseId, out _))
            {
                return BadRequest("無效的案例ID格式");
            }

            var res = await _mediator.Send(new GetCaseHistoryQuery(caseId));
            return Ok(res.Data);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "找不到案例: CaseId={CaseId}", caseId);
            return NotFound($"找不到案例: {caseId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "獲取案例歷史記錄時發生錯誤: CaseId={CaseId}", caseId);
            return StatusCode(500, "獲取案例歷史記錄時發生錯誤");
        }
    }

    /// <summary>
    /// 獲取案例記錄
    /// </summary>
    /// <param name="caseId">案例ID</param>
    /// <param name="date">日期 (格式: yyyyMMdd)</param>
    /// <param name="isUsingShiftDate">是否使用班別日期</param>
    /// <returns>案例記錄列表</returns>
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
    [HttpGet("caseId/{caseId}/date/{date}/caseRecords")]
    public async Task<ActionResult<IEnumerable<CaseRecordDto>>> GetCaseRecord(
        string caseId,
        string date,
        [FromQuery] bool isUsingShiftDate = false
    )
    {
        try
        {
            if (!Guid.TryParse(caseId, out _))
            {
                return BadRequest("無效的案例ID格式");
            }

            if (
                !DateTime.TryParseExact(
                    date,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _
                )
            )
            {
                return BadRequest("無效的日期格式，請使用yyyyMMdd格式");
            }

            var res = await _mediator.Send(
                new GetCaseRecordDateQuery(caseId, date, isUsingShiftDate)
            );
            return Ok(res.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "獲取案例記錄時發生錯誤: CaseId={CaseId}, Date={Date}", caseId, date);
            return StatusCode(500, "獲取案例記錄時發生錯誤");
        }
    }

    /// <summary>
    /// 獲取案例圖片
    /// </summary>
    /// <param name="caseId">案例ID</param>
    /// <param name="date">日期 (格式: yyyyMMdd)</param>
    /// <param name="isUsingShiftDate">是否使用班別日期</param>
    /// <returns>案例圖片列表</returns>
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
    [HttpGet("caseId/{caseId}/date/{date}/images")]
    public async Task<ActionResult<IEnumerable<CaseImageDto>>> GetCaseImage(
        string caseId,
        string date,
        [FromQuery] bool isUsingShiftDate = false
    )
    {
        try
        {
            if (!Guid.TryParse(caseId, out _))
            {
                return BadRequest("無效的案例ID格式");
            }

            if (
                !DateTime.TryParseExact(
                    date,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _
                )
            )
            {
                return BadRequest("無效的日期格式，請使用yyyyMMdd格式");
            }

            var res = await _mediator.Send(new GetCaseImageQuery(caseId, date, isUsingShiftDate));
            return Ok(res.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "獲取案例圖片時發生錯誤: CaseId={CaseId}, Date={Date}", caseId, date);
            return StatusCode(500, "獲取案例圖片時發生錯誤");
        }
    }

    /// <summary>
    /// 將案例標記為已關閉
    /// </summary>
    /// <param name="caseId">案例ID</param>
    [HttpPost("caseId/{caseId}/setAsClosed")]
    public async Task<ActionResult> SetCaseAsClosed(string caseId)
    {
        try
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                _logger.LogError("用戶ID為空");
                return Unauthorized("未授權的用戶");
            }

            if (!Guid.TryParse(caseId, out Guid caseGuid))
            {
                return BadRequest("無效的案例ID格式");
            }

            var command = new SetCaseStatusCommand(caseGuid, true, userId);
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return NotFound(result.Error);
            }

            return Ok(new { message = "案例已成功關閉" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "關閉案例時發生錯誤: CaseId={CaseId}", caseId);
            return StatusCode(500, "關閉案例時發生錯誤");
        }
    }

    /// <summary>
    /// 將案例標記為開啟
    /// </summary>
    /// <param name="caseId">案例ID</param>
    [HttpPost("caseId/{caseId}/setAsOpen")]
    public async Task<ActionResult> SetCaseAsOpen(string caseId)
    {
        try
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                _logger.LogError("用戶ID為空");
                return Unauthorized("未授權的用戶");
            }

            if (!Guid.TryParse(caseId, out Guid caseGuid))
            {
                return BadRequest("無效的案例ID格式");
            }

            var command = new SetCaseStatusCommand(caseGuid, false, userId);
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
            {
                return NotFound(result.Error);
            }

            return Ok(new { message = "案例已成功開啟" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "開啟案例時發生錯誤: CaseId={CaseId}", caseId);
            return StatusCode(500, "開啟案例時發生錯誤");
        }
    }
}
