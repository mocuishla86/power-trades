using FluentAssertions;
using Microsoft.Extensions.Logging;
using NodaTime;
using NSubstitute;
using PowerTrades.Application.Inbound;
using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Date;
using PowerTrades.Domain.Power;

namespace PowerTrades.Application.Test.Inbound
{
    public class GeneratePowerTradeForecastReportUseCaseTest
    {
        private IPowerTradeRepository powerTradeRepository;
        private IPowerTradeForecastReportRepository reportRepository;
        private IDateTimeService dateTimeService;
        private GeneratePowerTradeForecastReportUseCase sut;

        public GeneratePowerTradeForecastReportUseCaseTest()
        {
            powerTradeRepository = Substitute.For<IPowerTradeRepository>();
            reportRepository = Substitute.For<IPowerTradeForecastReportRepository>();
            dateTimeService = Substitute.For<IDateTimeService>();
            sut = new GeneratePowerTradeForecastReportUseCase(powerTradeRepository, reportRepository, dateTimeService, Substitute.For<ILogger<GeneratePowerTradeForecastReportUseCase>>());
        }

        [Fact]
        public void shoud_create_report_from_power_trade_list()
        {
            dateTimeService.GetCurrentLocalDateTime().Returns(DateTime.Now);
            dateTimeService.GetLocalDateTimeZone().Returns(DateTimeZoneProviders.Tzdb["Europe/Madrid"]);
            powerTradeRepository.GetPowerTrades(Arg.Any<DateTime>()).Returns(
            [
                PowerTrade.WithAllPeriodsWithVolume(100),
                PowerTrade.WithAllPeriodsWithVolume(50),
            ]);

            var report = sut.GenerateForecastReport("destination");

            report.Periods.Should().HaveCount(24);
            report.Periods.Should().AllSatisfy(period => period.AggregatedVolume.Should().Be(150));
            reportRepository.Received().SaveReport(report, "destination");
        }

        [Theory]
        [MemberData(nameof(DateTimeScenarios))]
        public void date_time_conversion_is_correctly_managed(DateTime localDateTime, string localTimeZone, DateTime expectedForecastedDay, DateTime expectedTimestamp, DateTime expectedFirstPeriodDateTime)
        {
            dateTimeService.GetCurrentLocalDateTime().Returns(localDateTime);
            dateTimeService.GetLocalDateTimeZone().Returns(DateTimeZoneProviders.Tzdb[localTimeZone]);
            powerTradeRepository.GetPowerTrades(expectedForecastedDay).Returns([PowerTrade.WithAllPeriodsWithVolume(100)]);

            PowerTradeForecastReport report = sut.GenerateForecastReport("destination");

            report.ForecastedDay.Should().Be(expectedForecastedDay);
            report.ExecutionTimestampInUtc.Should().Be(expectedTimestamp);
            report.Periods[0].DateTimeInUtc.Should().Be(expectedFirstPeriodDateTime);
            reportRepository.Received().SaveReport(report, "destination");
        }

        public static TheoryData<DateTime, string, DateTime, DateTime, DateTime> DateTimeScenarios => new TheoryData<DateTime, string, DateTime, DateTime, DateTime>
        {
            { new DateTime(2007,1,5,15,0,0), "Europe/Lisbon", new DateTime(2007,1,6), new DateTime(2007,1,5,15,0,0), new DateTime(2007,1,6,0,0,0) },
            { new DateTime(2007,7,5,15,0,0), "Europe/Lisbon", new DateTime(2007,7,6), new DateTime(2007,7,5,14,0,0), new DateTime(2007,7,5,23,0,0) },
            { new DateTime(2007,7,5,0,20,0), "Europe/Lisbon", new DateTime(2007,7,6), new DateTime(2007,7,4,23,20,0), new DateTime(2007,7,5,23,0,0) },
        };

    }
}