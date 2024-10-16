using PowerTrades.Application.outbound;
using PowerTrades.domain;

namespace PowerTrades.Application.inbound
{
    public class GeneratePowerTradeForecastReportUseCase(IPowerTradeRepository powerTradeRepository)
    {
        private const int HOURS_IN_DAY = 24;

        public PowerTradeForecastReport Generate() {
            List<PowerTrade> powerTrades = powerTradeRepository.GetPowerTrades(DateTime.Now);
            var aggregatedPowerTrade = powerTrades.Aggregate((a, b) => a + b);

            return new PowerTradeForecastReport
            {
                Periods = Enumerable.Range(1, HOURS_IN_DAY)
                            .Select(hourOfTheDay => new ForecastedPowerPeriod {
                                DateTime = DateTime.Now,
                                AggregatedVolume = aggregatedPowerTrade.GetPeriod(hourOfTheDay).Volume })
                            .ToList()
            };
           }
    }
}
