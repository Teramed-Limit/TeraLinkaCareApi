using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using TeraLinkaCareApi.Application.Common.Results;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Infrastructure.Persistence;

namespace TeraLinkaCareApi.Application.UseCases.Cases.Queries.GetCaseImage;

public record GetCaseImageQuery(string CaseId, string Date, bool IsUsingShiftDate)
    : IRequest<Result<IEnumerable<CaseImageDto>>>;

public class GetCaseImageQueryHandler
    : IRequestHandler<GetCaseImageQuery, Result<IEnumerable<CaseImageDto>>>
{
    private readonly CRSDbContext _context;
    private readonly IConfiguration _configuration;

    public GetCaseImageQueryHandler(CRSDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<Result<IEnumerable<CaseImageDto>>> Handle(
        GetCaseImageQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var date = DateTime.ParseExact(request.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
            var nextDate = date.AddDays(1);
            var dateString = date.ToString("yyyyMMdd");
            var nextDateString = nextDate.ToString("yyyyMMdd");

            var imagePath = _configuration.GetSection("ImageVirtualPath").Value;
            var imageMarkerPath = _configuration.GetSection("ImageMarkerVirtualPath").Value;
            var query =
                from image in _context.DicomImages
                join caseSeriesMap in _context.DicomSeriesMaps
                    on image.SeriesInstanceUID equals caseSeriesMap.DicomSeriesUid
                join clinicalUnitShift in _context.SysClinicalUnitShifts
                    on caseSeriesMap.DicomSeriesClinicalUnitShiftPuid equals clinicalUnitShift.Puid
                    into clinicalShiftGroup
                from clinicalUnitShift in clinicalShiftGroup.DefaultIfEmpty()
                where
                    caseSeriesMap.PtCasePuid == Guid.Parse(request.CaseId)
                    && (
                        !request.IsUsingShiftDate
                            ? (
                                caseSeriesMap.DicomSeriesDate.CompareTo(dateString) >= 0
                                && caseSeriesMap.DicomSeriesDate.CompareTo(nextDateString) < 0
                            )
                            : (
                                caseSeriesMap.DicomSeriesShiftDate >= date
                                && caseSeriesMap.DicomSeriesShiftDate < nextDate
                            )
                    )
                select new CaseImageDto()
                {
                    CaseId = caseSeriesMap.PtCasePuid.ToString(),
                    SopInstanceUID = image.SOPInstanceUID.Trim(),
                    ImageNumber = image.ImageNumber.Trim(),
                    ImageDate = image.ImageDate.Trim(),
                    ImageTime = image.ImageTime.Trim(),
                    FilePath = imagePath + Path.ChangeExtension(image.FilePath.Trim(), ".jpg"),
                    ImageUrl = imagePath + Path.ChangeExtension(image.FilePath.Trim(), ".jpg"),
                    imageMarkerUrl = !string.IsNullOrEmpty(image.ImageMarkerUrl)
                        ? Path.Combine(imageMarkerPath, Path.GetFileName(image.ImageMarkerUrl))
                        : null,
                    SeriesInstanceUID = image.SeriesInstanceUID.Trim(),
                    ImageMarker = image.ImageMarker ?? "",
                    ImageComment = image.ImageComment ?? "",
                    ShiftDate = caseSeriesMap.DicomSeriesShiftDate ?? new DateTime(),
                    ShiftLongLabel = clinicalUnitShift.ShiftLongLabel ?? "",
                    ShiftShortLabel = clinicalUnitShift.ShiftShortLabel ?? "",
                };

            var images = await query.ToListAsync(cancellationToken);

            return Result<IEnumerable<CaseImageDto>>.Success(images);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "獲取案例圖片時發生錯誤: CaseId={CaseId}, Date={Date}", request.CaseId, request.Date);
            return Result<IEnumerable<CaseImageDto>>.Failure($"獲取案例圖片失敗: {ex.Message}");
        }
    }
}