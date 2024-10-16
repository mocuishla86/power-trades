using FluentAssertions;
using NSubstitute;
using PowerTrades.Application.inbound;
using PowerTrades.Application.outbound;
using PowerTrades.domain;

namespace PowerTrades.Application.Test.inbound
{
    public class GeneratePowerTradeForecastReportUseCaseTest
    {
        [Fact]
        public void shoud_create_report_from_power_trade_list()
        {
            IPowerTradeRepository powerTradeRepository = Substitute.For<IPowerTradeRepository>();
            var sut = new GeneratePowerTradeForecastReportUseCase(powerTradeRepository);
            powerTradeRepository.GetPowerTrades(Arg.Any<DateTime>()).Returns(
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