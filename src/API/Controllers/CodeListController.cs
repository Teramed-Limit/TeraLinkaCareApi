﻿using Microsoft.AspNetCore.Mvc;
using WoundCareApi.API.Controllers.Base;
using WoundCareApi.Core.Domain.Entities;
using WoundCareApi.Core.Repository;
using WoundCareApi.Infrastructure.Persistence;
using WoundCareApi.Infrastructure.Persistence.UnitOfWork.Interfaces;

namespace WoundCareApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodeListController : BaseApiController<CodeList, CRSDbContext>
    {
        private new readonly CodeListRepository _repository;
        private readonly ILogger<CodeListController> _logger;

        public CodeListController(
            ILogger<CodeListController> logger,
            CodeListRepository repository,
            IUnitOfWork unitOfWork
        )
            : base(repository, unitOfWork, logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("codeNames")]
        public async Task<ActionResult<List<string>>> GetCodeListGroupByCodeName()
        {
            var item = await _repository.GetCodeNameList();

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpGet("all")]
        public async Task<
            ActionResult<Dictionary<string, List<CodeList>>>
        > GetCodeListOfEachCodeName()
        {
            var item = await _repository.GetCodeListOfEachCodeName();

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpGet("codeName/{codeName}")]
        public async Task<ActionResult<IEnumerable<CodeList>>> GetCodeListByCodeName(
            string codeName
        )
        {
            var item = await _repository.GetCodeListByCodeName(codeName);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item.OrderBy(x => x.ParentCodeValue).ThenBy(x => x.OrderingIndex));
        }

        [HttpPost("codeName/{codeName}/addCodeName")]
        public async Task<ActionResult> AddCodeName(string codeName)
        {
            await _repository.AddCodeName(codeName);
            return Ok();
        }

        [HttpDelete("codeName/{codeName}")]
        public async Task<IActionResult> DeleteCodeListCategory(string codeName)
        {
            await _repository.DeleteCodeName(codeName);
            return Ok();
        }
    }
}