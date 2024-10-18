using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PowerTrades.Domain.Power;
using PowerTrades.Infrastructure.Outbound;

namespace PowerTrades.Infrastructure.Test.Outbound
{
    public class CsvFilePowerTradeForecastReportRepositoryTest
    {
        [Fact]
        public void the_report_should_be_saved_as_csv()
        {
            String tempPath = Path.GetTempPath();
            String randomFolder = tempPath + Guid.NewGuid().ToString();
            Directory.CreateDirectory(randomFolder);
            String expectedFileName = "PowerPosition_20050707_200507062312.csv";
            String expectedFileFullPath = Path.Combine(randomFolder, expectedFileName);
            var sut = new CsvFilePowerTradeForecastReportRepository(Substitute.For<ILogger<CsvFilePowerTradeForecastReportRepository>>());
            var report = new PowerTradeForecastReport
            {
                ExecutionTimestampInUtc = new DateTime(2005, 7, 6, 23, 12, 13),
                ForecastedDay = new DateTime(2005, 7, 7),
                Periods = [
                    new ForecastedPowerPeriod { DateTimeInUtc = new DateTime(2005, 7, 7, 0, 0, 0), AggregatedVolume = 100.45},
                    new ForecastedPowerPeriod { DateTimeInUtc = new DateTime(2005, 7, 7, 1, 0, 0), AggregatedVolume = 50.2}
                    ]
            };

            sut.SaveReport(report, randomFolder);

            File.Exists(expectedFileFullPath).Should().BeTrue();
            File.ReadAllText(expectedFileFullPath).Should().Be("datetime;Volume\r\n2005-07-07T00:00:00Z;100.45\r\n2005-07-07T01:00:00Z;50.2\r\n");
        }
    }
}

