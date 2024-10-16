using PowerTrades.Application.outbound;
using PowerTrades.Domain.date;
using PowerTrades.Domain.Power;

namespace PowerTrades.Application.inbound
{
    public class GeneratePowerTradeForecastReportUseCase(IPowerTradeRepository powerTradeRepository, IDateTimeService dateTimeService)
    {
        private const int HOURS_IN_DAY = 24;

        public PowerTradeForecastReport Generate() {
            DateTime forecastedDay = dateTimeService.GetCurrentDateTime().Date.AddDays(1);
            List<PowerTrade> powerTrades = powerTradeRepository.GetPowerTrades(forecastedDay); 
            var aggregatedPowerTrade = powerTrades.Aggregate((a, b) => a + b);

            return new PowerTradeForecastReport
            {
                Periods = Enumerable.Range(1, HOURS_IN_DAY)
                            .Select(hourOfTheDay => new ForecastedPowerPeriod
                            {
                                DateTime = DateTime.Now, //TODO: Include right date
                                AggregatedVolume = aggregatedPowerTrade.GetPeriod(hourOfTheDay).Volume })
                            .ToList(),
                ForecastedDay = forecastedDay
                //TODO: ExecutionTimestamp
            };
           }
    }
}
