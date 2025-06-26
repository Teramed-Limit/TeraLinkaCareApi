using Serilog;
using TeraLinkaCareApi.Common.Types;
using TeraLinkaCareApi.Common.Utils;
using TeraLinkaCareApi.Core.Domain.Entities;

namespace TeraLinkaCareApi.Application.Services
{
    /// <summary>
    /// 班別時間服務，用於計算與臨床班別相關的時間資訊
    /// </summary>
    public class ShiftTimeService
    {
        /// <summary>
        /// 判斷指定時間屬於哪個班別，並計算相關日期時間資訊
        /// </summary>
        /// <param name="dateTime">要判斷的時間</param>
        /// <param name="clinicalUnits">臨床單位資料</param>
        /// <param name="shifts">班別資料</param>
        /// <param name="clinicalUnitPuid">臨床單位PUID，如果提供，將只計算該臨床單位的班別</param>
        /// <returns>返回班別和時間資訊或null</returns>
        public ShiftTimeResult? DetermineShiftAndTime(
            DateTime dateTime,
            List<SysClinicalUnit> clinicalUnits,
            List<SysClinicalUnitShift> shifts,
            Guid? clinicalUnitPuid = null
        )
        {
            try
            {
                if (clinicalUnits == null || !clinicalUnits.Any() || shifts == null || !shifts.Any())
                    return null;

                // 獲取小時和分鐘
                int hours = dateTime.Hour;
                int minutes = dateTime.Minute;

                // 將時間轉換為分鐘表示（從午夜開始）
                int timeInMinutes = hours * 60 + minutes;

                // 臨床日期計算（考慮到臨床日可能與自然日不同）
                // 如果提供了特定的臨床單位PUID，則使用該臨床單位，否則使用第一個
                SysClinicalUnit clinicalUnit;
                if (clinicalUnitPuid.HasValue)
                {
                    clinicalUnit = clinicalUnits.FirstOrDefault(u => u.Puid == clinicalUnitPuid.Value);
                    if (clinicalUnit == null)
                        return null; // 如果找不到指定的臨床單位，則返回null
                }
                else
                {
                    clinicalUnit = clinicalUnits[0]; // 如果沒有指定，則使用第一個臨床單位
                }

                int dayStartInMinutes =
                    clinicalUnit.DayBeginHour.GetValueOrDefault() * 60
                    + clinicalUnit.DayBeginOffsetMinutes.GetValueOrDefault();

                // 計算臨床日期
                DateTime clinicalDate = dateTime;
                if (timeInMinutes < dayStartInMinutes)
                {
                    // 如果當前時間早於日開始時間，則臨床日期為前一天
                    clinicalDate = clinicalDate.AddDays(-1);
                }

                // 計算前一天的臨床日期（用於處理可能是前一天大夜班的情況）
                DateTime prevClinicalDate = clinicalDate.AddDays(-1);

                // 計算前一天的臨床日期（用於處理可能是前一天大夜班的情況）
                DateTime nextClinicalDate = clinicalDate.AddDays(1);

                // 計算各臨床日期的班別時間
                List<ShiftWithTime> CalculateShiftTimesForDate(DateTime baseDate)
                {
                    var shiftsWithTimes = new List<ShiftWithTime>();

                    foreach (var shift in shifts)
                    {
                        if (shift.ClinicalUnitPuid == clinicalUnit.Puid)
                        {
                            // 班別開始時間（分鐘表示）
                            int shiftStartInMinutes =
                                shift.ShiftBeginHour.GetValueOrDefault() * 60
                                + shift.ShiftOffsetMinutes.GetValueOrDefault();
                            // 班別結束時間（分鐘表示）
                            int shiftEndInMinutes =
                                shiftStartInMinutes + shift.ShiftDuration.GetValueOrDefault();

                            // 建立該班別的開始和結束日期時間
                            DateTime shiftStartDate = new DateTime(
                                baseDate.Year,
                                baseDate.Month,
                                baseDate.Day,
                                shift.ShiftBeginHour.GetValueOrDefault(),
                                shift.ShiftOffsetMinutes.GetValueOrDefault(),
                                0
                            );

                            DateTime shiftEndDate = shiftStartDate
                                .AddMinutes(shift.ShiftDuration.GetValueOrDefault())
                                .AddSeconds(59);

                            shiftsWithTimes.Add(
                                new ShiftWithTime
                                {
                                    PuId = shift.Puid,
                                    ShiftBeginHour = shift.ShiftBeginHour.GetValueOrDefault(),
                                    ShiftOffsetMinutes = shift.ShiftOffsetMinutes.GetValueOrDefault(),
                                    ShiftDuration = shift.ShiftDuration.GetValueOrDefault(),
                                    ShiftShortLabel = shift.ShiftShortLabel,
                                    ShiftLongLabel = shift.ShiftLongLabel,
                                    ShiftStartDate = shiftStartDate,
                                    ShiftEndDate = shiftEndDate,
                                }
                            );
                        }
                    }

                    return shiftsWithTimes;
                }

                // 計算當前臨床日期的班別
                var currentDateShifts = CalculateShiftTimesForDate(clinicalDate);
                // 計算前一天臨床日期的班別（用於處理可能是前一天大夜班的情況）
                var prevDateShifts = CalculateShiftTimesForDate(prevClinicalDate);
                // 計算下一天臨床日期的班別
                var nextDateShifts = CalculateShiftTimesForDate(nextClinicalDate);

                // 所有可能的班別（當前日期和前一天的）
                var allPossibleShifts = currentDateShifts.Concat(prevDateShifts).Concat(nextDateShifts).ToList();

                // 找出當前班別
                ShiftWithTime currentShift = null;

                Console.WriteLine($"Target: {dateTime}");
                foreach (var shift in allPossibleShifts)
                {
                    Console.WriteLine($"{shift.ShiftStartDate} ~ {shift.ShiftEndDate}");
                    if (dateTime >= shift.ShiftStartDate && dateTime <= shift.ShiftEndDate)
                    {
                        currentShift = shift;
                        break;
                    }
                }

                if (currentShift != null)
                {
                    return new ShiftTimeResult
                    {
                        // 確保使用班別的PUID而非臨床單位班別的PUID
                        ClinicalUnitShiftPuid = currentShift.PuId,
                        CurrentShift = currentShift,
                        ClinicalDate = DateUtils.FormatDate(clinicalDate),
                        NaturalDate = DateUtils.FormatDate(dateTime),
                        OriginalDate = dateTime,
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error determining shift and time: {Message}", ex.Message);
                throw;
            }
        }
    }
}