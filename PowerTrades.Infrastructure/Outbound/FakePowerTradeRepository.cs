using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Power;

namespace PowerTrades.Infrastructure.Outbound
{
    public class FakePowerTradeRepository : IPowerTradeRepository
    {
        public Task<List<PowerTrade>> GetPowerTrades(DateTime date) => Task.FromResult<List<PowerTrade>>([
                PowerTrade.WithAllPeriodsWithVolume(100),
                PowerTrade.WithAllPeriodsWithVolume(50),
            ]);
    }
}
