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
public class DicomStudyController : ControllerBase
{
    private readonly ILogger<DicomStudyController> _logger;
    private readonly IRepository<SearchStudyOfCaseStatusView, CRSDbContext> _repository;

    public DicomStudyController(
        ILogger<DicomStudyController> logger,
        IRepository<SearchStudyOfCaseStatusView, CRSDbContext> repository
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 根據病患 ID、檢查單號 取得檢查資料
    /// </summary>
    /// <returns>檢查資料</returns>
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
    [HttpGet("patientId/{patientId}/accessionNumber/{accessionNumber}")]
    public async Task<ActionResult<A_PtEncounter>> GetStudyLevelView(string patientId, string accessionNumber)
    {
        try
        {
            var studies = await _repository
            .GetByConditionAsync(x => x.PatientId == patientId && x.AccessionNumber == accessionNumber);


            if (!studies.Any())
            {
                _logger.LogWarning("找不到檢查資料，PatientId: {PatientId}，AccessionNumber: {AccessionNumber}", patientId, accessionNumber);
                return NotFound("找不到檢查資料");
            }

            return Ok(studies.Select((x) => new A_PtEncounter()
            {
                Puid = Guid.NewGuid(),
                LifeTimeNumber = x.PatientId.Trim(),
                EncounterNumber = x.AccessionNumber.Trim(),
                LastName = x.PatientsName?.Trim(),
                FirstName = "",
                Gender = x.PatientsSex,
                DateOfBirth = !string.IsNullOrEmpty(x.PatientsBirthDate) ?
                            DateTime.TryParseExact(x.PatientsBirthDate, "yyyyMMdd", null,
                                System.Globalization.DateTimeStyles.None, out var birthDate) ?
                                birthDate : (DateTime?)null : null,
                NationalId = x.OtherPatientId,
            }).First());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving study data");
            return StatusCode(500, "取得病患資料時發生錯誤");
        }
    }

    /// <summary>
    /// 取得檢查資料
    /// </summary>
    /// <returns>檢查資料</returns>
    [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
    [HttpGet]
    public async Task<ActionResult<SearchStudyOfCaseStatusView>> GetStudyLevelView()
    {
        try
        {
            var studies = await _repository.GetAllAsync();

            if (!studies.Any())
            {
                _logger.LogWarning("No studies found");
                return NotFound("找不到任何資料");
            }

            return Ok(studies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving study data");
            return StatusCode(500, "取得病患資料時發生錯誤");
        }
    }
}