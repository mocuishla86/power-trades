using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Power;

namespace PowerTrades.Infrastructure.Outbound
{
    public class FakePowerTradeRepository : IPowerTradeRepository
    {
        public List<PowerTrade> GetPowerTrades(DateTime date) => [
                PowerTrade.WithAllPeriodsWithVolume(100),
                PowerTrade.WithAllPeriodsWithVolume(50),
            ];
    }
}
