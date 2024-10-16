using PowerTrades.Application.outbound;
using PowerTrades.domain;

namespace PowerTrades.Application.inbound
{
    public class GeneratePowerTradeForecastReportUseCase(IPowerTradeRepository powerTradeRepository)
    {
        private const int HOURS_IN_DAY = 24;

        public PowerTradeForecastReport Generate() {
            return new PowerTradeForecastReport
            {
                Periods = Enumerable.Range(1, HOURS_IN_DAY)
                            .Select(index => new ForecastedPowerPeriod { DateTime = DateTime.Now, AggregatedVolume = 33 })
                            .ToList()
            };
           }
    }
}
