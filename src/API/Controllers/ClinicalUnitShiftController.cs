using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraLinkaCareApi.API.Controllers.Base;
using TeraLinkaCareApi.Application.Services;
using TeraLinkaCareApi.Common.Types;
using TeraLinkaCareApi.Core.Domain.Entities;
using TeraLinkaCareApi.Core.Domain.Interfaces;
using TeraLinkaCareApi.Infrastructure.Persistence;
using TeraLinkaCareApi.Infrastructure.Persistence.UnitOfWork.Interfaces;

namespace TeraLinkaCareApi.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ClinicalUnitShiftController : BaseApiController<SysClinicalUnitShift, CRSDbContext>
{
    private readonly ILogger<ClinicalUnitShiftController> _logger;
    private readonly IRepository<SysClinicalUnit, CRSDbContext> _clinicalUnitRepository;
    private readonly ShiftTimeService _shiftTimeService;

    public ClinicalUnitShiftController(
        ILogger<ClinicalUnitShiftController> logger,
        IRepository<SysClinicalUnitShift, CRSDbContext> repository,
        IRepository<SysClinicalUnit, CRSDbContext> clinicalUnitRepository,
        IUnitOfWork unitOfWork,
        ShiftTimeService shiftTimeService
    )
        : base(repository, unitOfWork, logger)
    {
        _logger = logger;
        _clinicalUnitRepository = clinicalUnitRepository;
        _shiftTimeService = shiftTimeService;
    }

    // 獲取特定輪班對應的臨床單位資訊
    [HttpGet("{id}/clinicalunit")]
    public async Task<ActionResult<SysClinicalUnit>> GetClinicalUnitByShiftId(Guid id)
    {
        var shift = await Repository.GetByIdAsync(id);
        if (shift == null)
        {
            return NotFound($"未找到ID為 {id} 的輪班資訊");
        }

        var clinicalUnit = await _clinicalUnitRepository.GetByIdAsync(shift.ClinicalUnitPuid);
        if (clinicalUnit == null)
        {
            return NotFound($"未找到與輪班ID {id} 關聯的臨床單位");
        }

        return Ok(clinicalUnit);
    }

    // 根據時間範圍獲取輪班資訊
    [AllowAnonymous]
    [HttpGet("TestShiftsByTime/{clinicalUnitPuid}")]
    public async Task<ActionResult> TestShiftsByTime(Guid clinicalUnitPuid)
    {
        // 獲取指定的臨床單位
        var clinicalUnits = (
            await _clinicalUnitRepository.GetByConditionAsync(cu => cu.Puid == clinicalUnitPuid)
        ).ToList();

        if (!clinicalUnits.Any())
        {
            return BadRequest($"找不到PUID為 {clinicalUnitPuid} 的臨床單位");
        }

        // 獲取與該臨床單位相關的班別
        var shifts = (
            await Repository.GetByConditionAsync(s => s.ClinicalUnitPuid == clinicalUnitPuid)
        ).ToList();

        if (!shifts.Any())
        {
            return BadRequest($"找不到與臨床單位 {clinicalUnitPuid} 相關的班別資料");
        }

        var testTimes = new List<DateTime>
        {
            // new DateTime(2025, 6, 11, 16, 0, 0),
            // new DateTime(2025, 6, 11, 16, 1, 0),
            // new DateTime(2025, 6, 11, 8, 0, 0),
            // new DateTime(2025, 6, 11, 8, 1, 0),
            // new DateTime(2025, 6, 11, 0, 0, 0),
            // new DateTime(2025, 6, 11, 0, 1, 0),
            new DateTime(2025, 6, 11, 7, 0, 0)
        };

        var shiftTimeResults = new List<ShiftTimeResult>();

        testTimes.ForEach(async testTime =>
        {
            // 默認使用當前時間作為observationDateTime
            DateTime observationDateTime = testTime;
            DateTime? observationShiftDate = null;

            // 計算班別時間資訊，指定臨床單位PUID
            var shiftTimeResult = _shiftTimeService.DetermineShiftAndTime(
                observationDateTime,
                clinicalUnits,
                shifts,
                clinicalUnitPuid
            );

            Guid? clinicalUnitShiftPuid = null;
            if (shiftTimeResult != null)
            {
                // 設置班別相關資訊
                clinicalUnitShiftPuid = shiftTimeResult.ClinicalUnitShiftPuid;

                // 設置臨床日期作為ObservationShiftDate
                if (!string.IsNullOrEmpty(shiftTimeResult.ClinicalDate))
                {
                    observationShiftDate = DateTime.Parse(shiftTimeResult.ClinicalDate);
                }

                shiftTimeResults.Add(shiftTimeResult);
            }
            else
            {
                shiftTimeResults.Add(null);
            }
        });

        return Ok(shiftTimeResults);
    }
}