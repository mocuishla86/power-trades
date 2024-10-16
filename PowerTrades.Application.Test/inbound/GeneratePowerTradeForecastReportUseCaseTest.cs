using FluentAssertions;
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
            DateTime now = new DateTime(2007, 3, 2, 0, 23, 12);
            IPowerTradeRepository powerTradeRepository = Substitute.For<IPowerTradeRepository>();
            IDateTimeService dateTimeService = Substitute.For<IDateTimeService>();
            dateTimeService.GetCurrentDateTime().Returns(now);
            var sut = new GeneratePowerTradeForecastReportUseCase(powerTradeRepository, dateTimeService);
            powerTradeRepository.GetPowerTrades(new DateTime(2007,3,3)).Returns(
            [
                PowerTrade.WithAllPeriodsWithVolume(100),
                PowerTrade.WithAllPeriodsWithVolume(50),
            ]);

            PowerTradeForecastReport report = sut.Generate();

            report.Periods.Should().HaveCount(24);
            report.Periods.Should().AllSatisfy(period => period.AggregatedVolume.Should().Be(150));
        }
    }
}