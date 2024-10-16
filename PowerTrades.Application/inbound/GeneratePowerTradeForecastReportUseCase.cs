using PowerTrades.Application.outbound;
using PowerTrades.domain;

namespace PowerTrades.Application.inbound
{
    public class GeneratePowerTradeForecastReportUseCase(IPowerTradeRepository powerTradeRepository)
    {
        public PowerTradeForecastReport Generate() {
            return new PowerTradeForecastReport
            {
                Periods = Enumerable.Range(1, 24)
                            .Select(index => new ForecastedPowerPeriod { DateTime = DateTime.Now, AggregatedVolume = 33 })
                            .ToList()
            };
           }
    }
}
