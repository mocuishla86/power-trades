using PowerTrades.Domain.Power;

namespace PowerTrades.Application.Outbound
{
    public interface IPowerTradeRepository
    {
        Task<List<PowerTrade>> GetPowerTrades(DateTime date);
    }
}
