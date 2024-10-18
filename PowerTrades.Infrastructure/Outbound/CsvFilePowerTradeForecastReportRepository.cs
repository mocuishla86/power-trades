using Microsoft.Extensions.Logging;
using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Power;
using System.Globalization;

namespace PowerTrades.Infrastructure.Outbound
{
    public class CsvFilePowerTradeForecastReportRepository(ILogger<CsvFilePowerTradeForecastReportRepository> log) : IPowerTradeForecastReportRepository
    {
        public void SaveReport(PowerTradeForecastReport report, string destination)
        {
            string fileName = $"PowerPosition_{report.ForecastedDay:yyyyMMdd}_{report.ExecutionTimestampInUtc:yyyyMMddHHmm}.csv";
            string fullPath = Path.Combine(destination, fileName);
            log.LogInformation($"Writing CSV file to: {fullPath}");
            using (StreamWriter outputFile = new StreamWriter(fullPath))
            {
                 outputFile.WriteLine("datetime;Volume");
                 report.Periods.ForEach(period => outputFile.WriteLine($"{period.DateTimeInUtc:s}Z;{FormatDecimal(period.AggregatedVolume)}"));
            }
        }

        private string FormatDecimal(double value)
        {
           return value.ToString("G", CultureInfo.InvariantCulture);
        }
    }
}
