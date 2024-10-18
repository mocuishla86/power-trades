using NodaTime;
using PowerTrades.Domain.Power;
using NodaTime.TimeZones;
using NodaTime.Text;
using System.Globalization;
using Microsoft.Extensions.Logging;
using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Date;

namespace PowerTrades.Application.Inbound
{
    public class GeneratePowerTradeForecastReportUseCase(
        IPowerTradeRepository powerTradeRepository,
        IPowerTradeForecastReportRepository reportRepository,
        IDateTimeService dateTimeService,
        ILogger<GeneratePowerTradeForecastReportUseCase> log
        )
    {
        private const int HOURS_IN_DAY = 24;

        public PowerTradeForecastReport GenerateForecastReport(String destination)
        {
            log.LogInformation("Generating forecast report");
            DateTimeZone zone = dateTimeService.GetLocalDateTimeZone();
            log.LogInformation($"Current time zone: {zone.Id}");
            DateTime executionTimeStampInLocalTime = dateTimeService.GetCurrentLocalDateTime();
            DateTime forecastedDay = executionTimeStampInLocalTime.Date.AddDays(1);
            DateTime firstRowDateTimeInUtc = LocalTimeToUTC(zone, forecastedDay);

            List<PowerTrade> powerTrades = powerTradeRepository.GetPowerTrades(forecastedDay);
            var aggregatedPowerTrade = powerTrades.Aggregate((a, b) => a + b);

            var report = new PowerTradeForecastReport
            {
                Periods = Enumerable.Range(1, HOURS_IN_DAY)
                            .Select(hourOfTheDay => new ForecastedPowerPeriod
                            {
                                DateTimeInUtc = firstRowDateTimeInUtc.AddHours(hourOfTheDay - 1),
                                AggregatedVolume = aggregatedPowerTrade.GetPeriod(hourOfTheDay).Volume
                            })
                            .ToList(),
                ForecastedDay = forecastedDay,
                ExecutionTimestamp = LocalTimeToUTC(zone, executionTimeStampInLocalTime)
            };
            reportRepository.SaveReport(report, destination);
            return report;
        }

        static DateTime LocalTimeToUTC(DateTimeZone timeZone, DateTime dateTime)
        {
            LocalDateTime ldt = LocalDateTime.FromDateTime(dateTime);
            ZonedDateTime zdt = ldt.InZoneLeniently(timeZone);
            return zdt.ToDateTimeUtc();
        }
    }
}
