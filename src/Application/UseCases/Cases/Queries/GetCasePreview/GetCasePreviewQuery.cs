using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TeraLinkaCareApi.Application.Common.Results;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Infrastructure.Persistence;

namespace TeraLinkaCareApi.Application.UseCases.Cases.Queries.GetCasePreview;

/// <summary>
/// 取得傷照預覽頁資料：部位 → 案例 → series（最新在前）→ 影像（依 ImageNumber）
/// </summary>
public record GetCasePreviewQuery(string PatientId, string EncounterId)
    : IRequest<Result<CasePreviewDto>>;

public class GetCasePreviewQueryHandler
    : IRequestHandler<GetCasePreviewQuery, Result<CasePreviewDto>>
{
    private readonly CRSDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public GetCasePreviewQueryHandler(
        CRSDbContext context,
        IMapper mapper,
        IConfiguration configuration
    )
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<Result<CasePreviewDto>> Handle(
        GetCasePreviewQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var imagePath = _configuration.GetSection("ImageVirtualPath").Value;
            var imageMarkerPath = _configuration.GetSection("ImageMarkerVirtualPath").Value;

            var query =
                from caseItem in _context.PtCases.AsNoTracking()
                join caseType in _context.CfgCaseTypes.AsNoTracking()
                    on caseItem.CaseTypePuid equals caseType.Puid
                    into caseTypeGroup
                from caseType in caseTypeGroup.DefaultIfEmpty()
                join bodyLocation in _context.CfgBodyLocations.AsNoTracking()
                    on caseItem.CaseLocation equals bodyLocation.NISLocationLabel
                    into bodyLocationGroup
                from bodyLocation in bodyLocationGroup.DefaultIfEmpty()
                join seriesMap in _context.DicomSeriesMaps.AsNoTracking()
                    on (Guid?)caseItem.Puid equals seriesMap.PtCasePuid
                    into seriesMapGroup
                from seriesMap in seriesMapGroup.DefaultIfEmpty()
                join clinicalUnitShift in _context.SysClinicalUnitShifts.AsNoTracking()
                    on seriesMap.DicomSeriesClinicalUnitShiftPuid equals clinicalUnitShift.Puid
                    into clinicalShiftGroup
                from clinicalUnitShift in clinicalShiftGroup.DefaultIfEmpty()
                join image in _context.DicomImages.AsNoTracking()
                    on seriesMap.DicomSeriesUid equals image.SeriesInstanceUID
                    into imageGroup
                from image in imageGroup.DefaultIfEmpty()
                where
                    caseItem.LIfeTimeNumber == request.PatientId
                    && caseItem.EncounterNumber == request.EncounterId
                select new
                {
                    Case = caseItem,
                    CaseTypeShortLabel = caseType != null ? caseType.CaseTypeShortLabel : null,
                    LocationSVGId = bodyLocation != null ? bodyLocation.SVGGraphicId : null,
                    HasSeries = seriesMap != null,
                    SeriesUid = seriesMap != null ? seriesMap.DicomSeriesUid : null,
                    SeriesDate = seriesMap != null ? seriesMap.DicomSeriesDate : null,
                    SeriesTime = seriesMap != null ? seriesMap.DicomSeriesTime : null,
                    ShiftDate = seriesMap != null ? seriesMap.DicomSeriesShiftDate : null,
                    ShiftShortLabel = clinicalUnitShift != null
                        ? clinicalUnitShift.ShiftShortLabel
                        : null,
                    ShiftLongLabel = clinicalUnitShift != null
                        ? clinicalUnitShift.ShiftLongLabel
                        : null,
                    HasImage = image != null,
                    SopInstanceUID = image != null ? image.SOPInstanceUID : null,
                    ImageSeriesInstanceUID = image != null ? image.SeriesInstanceUID : null,
                    ImageNumber = image != null ? image.ImageNumber : null,
                    ImageDate = image != null ? image.ImageDate : null,
                    ImageTime = image != null ? image.ImageTime : null,
                    FilePath = image != null ? image.FilePath : null,
                    ImageMarkerUrl = image != null ? image.ImageMarkerUrl : null,
                    ImageMarker = image != null ? image.ImageMarker : null,
                    ImageComment = image != null ? image.ImageComment : null,
                };

            var rows = await query.ToListAsync(cancellationToken);

            var cases = rows.GroupBy(r => r.Case.Puid)
                .Select(caseRows =>
                {
                    var first = caseRows.First();
                    var dto = _mapper.Map<PreviewCaseDto>(first.Case);
                    dto.LocationLabel = first.Case.CaseLocation;
                    dto.LocationSVGId = first.LocationSVGId ?? "Unknown";
                    dto.CaseTypeShortLabel = first.CaseTypeShortLabel;
                    dto.IsCaseClosed = first.Case.CaseCloseTime.HasValue;

                    dto.Series = caseRows
                        .Where(r => r.HasSeries && !string.IsNullOrWhiteSpace(r.SeriesUid))
                        .GroupBy(r => r.SeriesUid!.Trim())
                        .Select(seriesRows =>
                        {
                            var s = seriesRows.First();
                            var series = new PreviewSeriesDto
                            {
                                SeriesInstanceUID = seriesRows.Key,
                                SeriesDate = s.SeriesDate?.Trim() ?? "",
                                SeriesTime = s.SeriesTime?.Trim() ?? "",
                                ShiftDate = s.ShiftDate,
                                ShiftShortLabel = s.ShiftShortLabel,
                                ShiftLongLabel = s.ShiftLongLabel,
                            };

                            // 圖片欄位組法照抄 GetCaseImageQuery 的投影
                            series.Images = seriesRows
                                .Where(r => r.HasImage)
                                .GroupBy(r => r.SopInstanceUID!.Trim())
                                .Select(g => g.First())
                                .Select(r => new CaseImageDto()
                                {
                                    CaseId = first.Case.Puid.ToString(),
                                    SopInstanceUID = r.SopInstanceUID!.Trim(),
                                    ImageNumber = r.ImageNumber!.Trim(),
                                    ImageDate = r.ImageDate!.Trim(),
                                    ImageTime = r.ImageTime!.Trim(),
                                    FilePath =
                                        imagePath
                                        + Path.ChangeExtension(r.FilePath!.Trim(), ".jpg"),
                                    ImageUrl =
                                        imagePath
                                        + Path.ChangeExtension(r.FilePath!.Trim(), ".jpg"),
                                    imageMarkerUrl = !string.IsNullOrEmpty(r.ImageMarkerUrl)
                                        ? Path.Combine(
                                            imageMarkerPath!,
                                            Path.GetFileName(r.ImageMarkerUrl)
                                        )
                                        : null!,
                                    SeriesInstanceUID = r.ImageSeriesInstanceUID!.Trim(),
                                    ImageMarker = r.ImageMarker ?? "",
                                    ImageComment = r.ImageComment ?? "",
                                    ShiftDate = s.ShiftDate ?? new DateTime(),
                                    ShiftLongLabel = s.ShiftLongLabel ?? "",
                                    ShiftShortLabel = s.ShiftShortLabel ?? "",
                                })
                                .OrderBy(i => ParseImageNumber(i.ImageNumber))
                                .ThenBy(i => i.ImageNumber, StringComparer.Ordinal)
                                .ToList();

                            return series;
                        })
                        .OrderByDescending(s => SeriesSortKey(s), StringComparer.Ordinal)
                        .ToList();

                    return dto;
                })
                .ToList();

            var bodyParts = cases
                .GroupBy(c => c.LocationLabel ?? "Unknown")
                .Select(g => new PreviewBodyPartDto
                {
                    LocationLabel = g.Key,
                    LocationSVGId = g.First().LocationSVGId ?? "Unknown",
                    Cases = g.OrderByDescending(c => LatestSeriesKey(c), StringComparer.Ordinal)
                        .ThenByDescending(c => c.CaseBeginTime)
                        .ToList(),
                })
                // 部位依其最新一次檢查遞減排列；同為無檢查者依部位名稱
                .OrderByDescending(
                    p => p.Cases.Select(LatestSeriesKey).Max(StringComparer.Ordinal),
                    StringComparer.Ordinal
                )
                .ThenBy(p => p.LocationLabel, StringComparer.Ordinal)
                .ToList();

            return Result<CasePreviewDto>.Success(new CasePreviewDto { BodyParts = bodyParts });
        }
        catch (Exception ex)
        {
            Log.Error(
                ex,
                "獲取案例預覽時發生錯誤: PatientId={PatientId}, EncounterId={EncounterId}",
                request.PatientId,
                request.EncounterId
            );
            return Result<CasePreviewDto>.Failure("獲取案例預覽失敗");
        }
    }

    private static string SeriesSortKey(PreviewSeriesDto series) =>
        series.SeriesDate + series.SeriesTime;

    private static string LatestSeriesKey(PreviewCaseDto caseDto) =>
        caseDto.Series.Count > 0 ? SeriesSortKey(caseDto.Series[0]) : "";

    private static long ParseImageNumber(string? imageNumber) =>
        long.TryParse(imageNumber, out var number) ? number : long.MaxValue;
}
