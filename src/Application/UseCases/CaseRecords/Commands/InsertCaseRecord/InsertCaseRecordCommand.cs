using System.Globalization;
using System.Text.Json;
using MediatR;
using TeraLinkaCareApi.Application.Common.Results;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Application.Services;
using TeraLinkaCareApi.Core.Domain.Entities;
using TeraLinkaCareApi.Core.Domain.Interfaces;
using TeraLinkaCareApi.Infrastructure.Persistence;
using TeraLinkaCareApi.Infrastructure.Persistence.UnitOfWork.Interfaces;

namespace TeraLinkaCareApi.Application.UseCases.CaseRecords.Commands.InsertCaseRecord;

public record InsertCaseRecordCommand(CaseFormDataDto FormData, string UserId)
    : IRequest<Result<string>>;

public class InsertCaseRecordCommandHandler
    : IRequestHandler<InsertCaseRecordCommand, Result<string>>
{
    private readonly IRepository<PtCaseRecord, CRSDbContext> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ShiftTimeService _shiftTimeService;
    private readonly UnitShiftService _unitShiftService;
    private readonly CRSDbContext _context;

    public InsertCaseRecordCommandHandler(
        IRepository<PtCaseRecord, CRSDbContext> repository,
        IUnitOfWork unitOfWork,
        ShiftTimeService shiftTimeService,
        UnitShiftService unitShiftService,
        CRSDbContext context
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _shiftTimeService = shiftTimeService;
        _unitShiftService = unitShiftService;
        _context = context;
    }

    public async Task<Result<string>> Handle(
        InsertCaseRecordCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            // 解析日期時間並獲取觀察時間
            var (observationDateTime, observationShiftDate, clinicalUnitShiftPuid) =
                await ProcessDateTimeAndShiftInfoAsync(request.FormData);

            // 創建報告記錄
            var newReport = new PtCaseRecord()
            {
                Puid = Guid.NewGuid(),
                PtCasePuid = request.FormData.CaseId,
                FormData = JsonSerializer.Serialize(request.FormData.FormData),
                FormDefinePuid = request.FormData.FormDefinePuid,
                ObservationDateTime = observationDateTime,
                ObservationShiftDate = observationShiftDate,
                ClinicalUnitShiftPuid = clinicalUnitShiftPuid,
                StoreTime = DateTime.Now,
                CareProviderId = request.UserId,
            };

            // 保存報告
            await _repository.AddAsync(newReport);
            await _unitOfWork.SaveAsync();

            // 返回新建的報告
            // var query =
            //     from caseRecord in _context.CaseRecords
            //     join caseRecordFormDefine in _context.ReportDefines
            //         on caseRecord.FormDefinePuid equals caseRecordFormDefine.Puid
            //     join clinicalUnitShift in _context.SysClinicalUnitShifts
            //         on caseRecord.ClinicalUnitShiftPuid equals clinicalUnitShift.Puid
            //         into clinicalShiftGroup
            //     from clinicalUnitShift in clinicalShiftGroup.DefaultIfEmpty()
            //     where caseRecord.Puid == newReport.Puid
            //     select new CaseRecordDto()
            //     {
            //         Puid = caseRecord.Puid,
            //         ObservationDateTime = caseRecord.ObservationDateTime,
            //         ObservationShiftDate = caseRecord.ObservationShiftDate,
            //         ShiftLongLabel = clinicalUnitShift.ShiftLongLabel,
            //         ShiftShortLabel = clinicalUnitShift.ShiftShortLabel,
            //         FormDefinePuid = caseRecord.FormDefinePuid,
            //         FormDefine = null,
            //         FormData = null,
            //         RawFormDefine = caseRecordFormDefine.FormDefine,
            //         RawFormData = caseRecord.FormData,
            //     };

            // var results = await query
            //     .OrderBy(x => x.ObservationDateTime)
            //     .ToListAsync(cancellationToken);
            // var record = results.First();

            // if (!string.IsNullOrEmpty(record.RawFormDefine))
            // {
            //     record.FormDefine = JsonSerializer.Deserialize<Dictionary<string, object>>(
            //         record.RawFormDefine
            //     );
            // }

            // if (!string.IsNullOrEmpty(record.RawFormData))
            // {
            //     record.FormData = JsonSerializer.Deserialize<Dictionary<string, object>>(
            //         record.RawFormData
            //     );
            // }

            return Result<string>.Success(newReport.Puid.ToString());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }

    private async Task<(
        DateTime ObservationDateTime,
        DateTime? ObservationShiftDate,
        Guid? ClinicalUnitShiftPuid
    )> ProcessDateTimeAndShiftInfoAsync(CaseFormDataDto formDataDto)
    {
        // 解析案例日期時間
        if (
            !DateTime.TryParseExact(
                formDataDto.CaseDateTime,
                "yyyyMMddHHmmss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime caseDateTime
            )
        )
        {
            throw new ArgumentException("Invalid CaseDateTime format");
        }

        // 默認使用CaseDateTime作為observationDateTime
        DateTime observationDateTime = caseDateTime;
        DateTime? observationShiftDate = null;


        // 需將 Nullable<Guid> 轉為 Guid，避免型別不符
        if (!formDataDto.ClinicalUnitPuid.HasValue || formDataDto.ClinicalUnitPuid.Value == Guid.Empty)
        {
            return (observationDateTime, observationShiftDate, null);
        }

        var clinicalUnitPuid = formDataDto.ClinicalUnitPuid.Value;

        // 獲取指定的臨床單位
        var clinicalUnits = await _unitShiftService.GetUnitByPuid(clinicalUnitPuid);

        // 獲取與該臨床單位相關的班別
        var shifts = await _unitShiftService.GetUnitShiftsByPuid(clinicalUnitPuid);

        // 計算班別時間資訊，指定臨床單位PUID
        var shiftInfo = _shiftTimeService.DetermineShiftAndTime(
            observationDateTime,
            clinicalUnits.ToList(),
            shifts.ToList(),
            formDataDto.ClinicalUnitPuid
        );

        if (shiftInfo == null)
        {
            throw new InvalidOperationException("無法根據提供的時間和臨床單位計算班別資訊");
        }

        Guid? clinicalUnitShiftPuid = shiftInfo.ClinicalUnitShiftPuid;

        // 設置臨床日期作為ObservationShiftDate
        if (!string.IsNullOrEmpty(shiftInfo.ClinicalDate))
        {
            observationShiftDate = DateTime.Parse(shiftInfo.ClinicalDate);
        }

        return (observationDateTime, observationShiftDate, clinicalUnitShiftPuid);
    }
}
