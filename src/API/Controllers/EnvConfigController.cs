using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Common.Utils;
using TeraLinkaCareApi.Core.Domain.Entities;
using TeraLinkaCareApi.Core.Domain.Interfaces;
using TeraLinkaCareApi.Infrastructure.Persistence;
using TeraLinkaCareApi.Infrastructure.Persistence.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace TeraLinkaCareApi.API.Controllers;

[ApiController]
[AllowAnonymous]
[EnableCors("AllowEverything")]
[Route("api/[controller]")]
public class EnvConfigController : ControllerBase
{
    private readonly ILogger<EnvConfigController> _logger;
    private readonly IRepository<CfgBodyLocation, CRSDbContext> _bodyLocationRepository;
    private readonly IRepository<CfgCaseType, CRSDbContext> _caseTypeRepository;
    private readonly IRepository<SysClinicalUnit, CRSDbContext> _clinicalUnitRepository;
    private readonly IRepository<SysClinicalUnitShift, CRSDbContext> _clinicalUnitShiftRepository;

    public EnvConfigController(
        ILogger<EnvConfigController> logger,
        IRepository<CfgBodyLocation, CRSDbContext> bodyLocationRepository,
        IRepository<CfgCaseType, CRSDbContext> caseTypeRepository,
        IRepository<SysClinicalUnit, CRSDbContext> clinicalUnitRepository,
        IRepository<SysClinicalUnitShift, CRSDbContext> clinicalUnitShiftRepository
    )
    {
        _logger = logger;
        _bodyLocationRepository = bodyLocationRepository;
        _caseTypeRepository = caseTypeRepository;
        _clinicalUnitRepository = clinicalUnitRepository;
        _clinicalUnitShiftRepository = clinicalUnitShiftRepository;
    }


    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        try
        {
            var bodyLocatioData =  await _bodyLocationRepository.GetAllAsync();
            var clinicalUnitData = await _clinicalUnitRepository.GetAllAsync();
            var clinicalUnitShiftData = await _clinicalUnitShiftRepository.GetAllAsync();
            var caseTypeData = await _caseTypeRepository.GetAllAsync();
            return Ok(new
            {
                cfgBodyLocatin = bodyLocatioData,
                cfgCaseType = caseTypeData,
                sysClinicalUnit = clinicalUnitData,
                sysClinicalUnitShift = clinicalUnitShiftData,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "獲取參數錯誤");
            return StatusCode(500, "獲取參數錯誤");
        }
    }
}