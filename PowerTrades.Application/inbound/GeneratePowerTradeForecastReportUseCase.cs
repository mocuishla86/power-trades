using NodaTime;
using PowerTrades.Application.outbound;
using PowerTrades.Domain.date;
using PowerTrades.Domain.Power;
using NodaTime.TimeZones;
using NodaTime.Text;
using System.Globalization;

namespace PowerTrades.Application.inbound
{
    public class GeneratePowerTradeForecastReportUseCase(IPowerTradeRepository powerTradeRepository, IDateTimeService dateTimeService)
    {
        private const int HOURS_IN_DAY = 24;

        public PowerTradeForecastReport Generate() {
            NodaTime.DateTimeZone zone = dateTimeService.GetLocalDateTimeZone();
            DateTime executionTimeStampInLocalTime = dateTimeService.GetCurrentLocalDateTime();
            DateTime forecastedDay = executionTimeStampInLocalTime.Date.AddDays(1);
            DateTime firstRowDateTimeInUtc = LocalTimeToUTC(zone, forecastedDay);
            List<PowerTrade> powerTrades = powerTradeRepository.GetPowerTrades(forecastedDay); 
            var aggregatedPowerTrade = powerTrades.Aggregate((a, b) => a + b);

            return new PowerTradeForecastReport
            {
                Periods = Enumerable.Range(1, HOURS_IN_DAY)
                            .Select(hourOfTheDay => new ForecastedPowerPeriod
                            {
                                DateTime = firstRowDateTimeInUtc.AddHours(hourOfTheDay - 1),
                                AggregatedVolume = aggregatedPowerTrade.GetPeriod(hourOfTheDay).Volume })
                            .ToList(),
                ForecastedDay = forecastedDay,
                ExecutionTimestamp = LocalTimeToUTC(zone, executionTimeStampInLocalTime)
            };
           }

        static DateTime LocalTimeToUTC(DateTimeZone timeZone, DateTime dateTime)
        {
            LocalDateTime ldt = LocalDateTime.FromDateTime(dateTime);
            ZonedDateTime zdt = ldt.InZoneLeniently(timeZone);
            return zdt.ToDateTimeUtc();
        }
    }
}
