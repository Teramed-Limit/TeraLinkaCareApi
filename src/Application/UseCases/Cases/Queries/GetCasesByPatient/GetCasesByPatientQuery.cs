using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeraLinkaCareApi.Application.Common.Results;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Infrastructure.Persistence;

namespace TeraLinkaCareApi.Application.UseCases.Cases.Queries.GetCasesByPatient;

public record GetCasesByPatientQuery(string PatientId, string EncounterId)
    : IRequest<Result<Dictionary<string, List<CaseDto>>>>;

public class GetCasesByPatientQueryHandler
    : IRequestHandler<GetCasesByPatientQuery, Result<Dictionary<string, List<CaseDto>>>>
{
    private readonly CRSDbContext _context;
    private readonly IMapper _mapper;

    public GetCasesByPatientQueryHandler(CRSDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<Dictionary<string, List<CaseDto>>>> Handle(
        GetCasesByPatientQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var query =
                from caseItem in _context.PtCases
                join caseType in _context.CfgCaseTypes on caseItem.CaseTypePuid equals caseType.Puid
                join bodyLocation in _context.CfgBodyLocations on caseItem.CaseLocation equals bodyLocation
                    .NISLocationLabel into bodyLocationGroup
                from bodyLocation in bodyLocationGroup.DefaultIfEmpty()
                where
                    caseItem.LIfeTimeNumber == request.PatientId
                    && caseItem.EncounterNumber == request.EncounterId
                select new
                {
                    Case = caseItem,
                    LocationLabel = caseItem.CaseLocation,
                    LocationSVGId = bodyLocation != null ? bodyLocation.SVGGraphicId : null,
                    CasetypeShortLabel = caseType.CaseTypeShortLabel,
                };

            var results = await query.ToListAsync();

            var caseDtos = results.Select(r =>
            {
                var dto = _mapper.Map<CaseDto>(r.Case);
                dto.LocationLabel = r.LocationLabel;
                dto.LocationSVGId = r.LocationSVGId ?? "Unknown";
                dto.CaseTypeShortLabel = r.CasetypeShortLabel;
                dto.IsCaseClosed = r.Case.CaseCloseTime.HasValue;
                return dto;
            });

            var groupedCases = caseDtos
                .GroupBy(c => c.LocationLabel ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.ToList());

            return Result<Dictionary<string, List<CaseDto>>>.Success(groupedCases);
        }
        catch (Exception ex)
        {
            return Result<Dictionary<string, List<CaseDto>>>.Failure($"獲取案例失敗: {ex.Message}");
        }
    }
}