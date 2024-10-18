using Microsoft.Extensions.Logging;
using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Power;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrades.Infrastructure.Outbound
{
    public class ConsolePowerTradeForecastReportRepository(ILogger<ConsolePowerTradeForecastReportRepository> log) : IPowerTradeForecastReportRepository
    {
        public void SaveReport(PowerTradeForecastReport report, String destination)
        {
            log.LogInformation($"Saving forecast report. Timestamp: {report.ExecutionTimestamp}. Forecasted day: {report.ForecastedDay}. ");
            report.Periods.ForEach(period => log.LogDebug($"Date in UTC: {period.DateTimeInUtc}, Volume: {period.AggregatedVolume}"));
        }
    }
}
