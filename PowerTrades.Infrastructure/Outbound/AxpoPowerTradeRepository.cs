using Microsoft.Extensions.Logging;
using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Power;

namespace PowerTrades.Infrastructure.Outbound
{
    public class AxpoPowerTradeRepository(Axpo.IPowerService powerService, ILogger<AxpoPowerTradeRepository> log) : IPowerTradeRepository
    {
        public async Task<List<PowerTrade>> GetPowerTrades(DateTime date)
        {
            var axpoPowerTrades = (await powerService.GetTradesAsync(date)).ToList();

            log.LogInformation($"Axpo Power Trades retrieved from library: {axpoPowerTrades.Count}");

            return axpoPowerTrades
                .Select(axpoPowerTrade => AxpoPowerTradeToDomain(axpoPowerTrade))
                .ToList();
        }

        private PowerTrade AxpoPowerTradeToDomain(Axpo.PowerTrade axpoPowerTrade)
        {
            log.LogInformation($"Converting Axpo Power Trade. TradeId: {axpoPowerTrade.TradeId}, Date: {axpoPowerTrade.Date}");
            return new PowerTrade
            {
                Periods = axpoPowerTrade.Periods.Select(axpoPowerPeriod => AxpoPowerPeriodToDomain(axpoPowerPeriod)).ToList()
            };
        }

        private PowerPeriod AxpoPowerPeriodToDomain(Axpo.PowerPeriod axpoPowerPeriod)
        {
            log.LogDebug($"Converting Axpo Power Period. Period: {axpoPowerPeriod.Period}. Volume: {axpoPowerPeriod.Volume}");

            return new PowerPeriod
            {
                HourOfTheDay = axpoPowerPeriod.Period,
                Volume = axpoPowerPeriod.Volume,
            };
        }
    }
}
