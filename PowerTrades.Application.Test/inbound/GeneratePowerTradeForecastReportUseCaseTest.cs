using FluentAssertions;
using NodaTime;
using NSubstitute;
using PowerTrades.Application.inbound;
using PowerTrades.Application.outbound;
using PowerTrades.Domain.date;
using PowerTrades.Domain.Power;

namespace PowerTrades.Application.Test.inbound
{
    public class GeneratePowerTradeForecastReportUseCaseTest
    {
        [Fact]
        public void shoud_create_report_from_power_trade_list()
        {
            IPowerTradeRepository powerTradeRepository = Substitute.For<IPowerTradeRepository>();
            IDateTimeService dateTimeService = Substitute.For<IDateTimeService>();
            dateTimeService.GetCurrentLocalDateTime().Returns(DateTime.Now);
            dateTimeService.GetLocalDateTimeZone().Returns(DateTimeZoneProviders.Tzdb["Europe/Madrid"]);
            var sut = new GeneratePowerTradeForecastReportUseCase(powerTradeRepository, dateTimeService);
            powerTradeRepository.GetPowerTrades(Arg.Any<DateTime>()).Returns(
            [
                PowerTrade.WithAllPeriodsWithVolume(100),
                PowerTrade.WithAllPeriodsWithVolume(50),
            ]);

            PowerTradeForecastReport report = sut.GenerateForecastReport();

            report.Periods.Should().HaveCount(24);
            report.Periods.Should().AllSatisfy(period => period.AggregatedVolume.Should().Be(150));
        }

        [Theory]
        [MemberData(nameof(DateTimeScenarios))]
        public void date_time_conversion_is_correctly_managed(DateTime localDateTime, String localTimeZone, DateTime expectedForecastedDay, DateTime expectedTimestamp, DateTime expectedFirstPeriodDateTime)
        {
            IPowerTradeRepository powerTradeRepository = Substitute.For<IPowerTradeRepository>();
            IDateTimeService dateTimeService = Substitute.For<IDateTimeService>();
            dateTimeService.GetCurrentLocalDateTime().Returns(localDateTime);
            dateTimeService.GetLocalDateTimeZone().Returns(DateTimeZoneProviders.Tzdb[localTimeZone]);
            var sut = new GeneratePowerTradeForecastReportUseCase(powerTradeRepository, dateTimeService);
            powerTradeRepository.GetPowerTrades(expectedForecastedDay).Returns([PowerTrade.WithAllPeriodsWithVolume(100)]);

            PowerTradeForecastReport report = sut.GenerateForecastReport();

            report.ForecastedDay.Should().Be(expectedForecastedDay);
            report.ExecutionTimestamp.Should().Be(expectedTimestamp);
            report.Periods[0].DateTime.Should().Be(expectedFirstPeriodDateTime);
        }

        public static TheoryData<DateTime, String, DateTime, DateTime, DateTime> DateTimeScenarios => new TheoryData<DateTime, String, DateTime, DateTime, DateTime>
        {
            { new DateTime(2007,1,5,15,0,0), "Europe/Lisbon", new DateTime(2007,1,6), new DateTime(2007,1,5,15,0,0), new DateTime(2007,1,6,0,0,0) },
            { new DateTime(2007,7,5,15,0,0), "Europe/Lisbon", new DateTime(2007,7,6), new DateTime(2007,7,5,14,0,0), new DateTime(2007,7,5,23,0,0) },
            { new DateTime(2007,7,5,0,20,0), "Europe/Lisbon", new DateTime(2007,7,6), new DateTime(2007,7,4,23,20,0), new DateTime(2007,7,5,23,0,0) },
        };

    }
}