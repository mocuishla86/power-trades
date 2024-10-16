using PowerTrades.Domain.powertrade;

namespace PowerTrades.Application.outbound
{
    public interface IPowerTradeRepository
    {
        List<PowerTrade> GetPowerTrades(DateTime date);
    }
}
