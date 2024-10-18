using PowerTrades.Domain.Power;

namespace PowerTrades.Application.Outbound
{
    public interface IPowerTradeRepository
    {
        List<PowerTrade> GetPowerTrades(DateTime date);
    }
}
