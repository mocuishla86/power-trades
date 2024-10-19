using Microsoft.Extensions.Logging;
using PowerTrades.Application.Outbound;
using PowerTrades.Domain.Power;


namespace PowerTrades.Infrastructure.Outbound
{
    public class AxpoPowerTradeRepository(Axpo.IPowerService powerService, ILogger<AxpoPowerTradeRepository> log) : IPowerTradeRepository
    {
        private const int TASKS_IN_PARALLEL = 5;

        public async Task<List<PowerTrade>> GetPowerTrades(DateTime date)
        {
            List<Axpo.PowerTrade> axpoPowerTrades; 

            var cancellationTokenSource = new CancellationTokenSource();

            log.LogInformation($"Executing {TASKS_IN_PARALLEL} tasks in parallel to get trades from library and waiting for the fastest");
            List<Task<IEnumerable<Axpo.PowerTrade>>> tasks = Enumerable.Range(1, TASKS_IN_PARALLEL)
                .Select(taskindex => CallLibraryWithRetry(taskindex, date, cancellationTokenSource))
                .ToList();

            // Get first finished task
            var fastestTask = await Task.WhenAny(tasks);
            // Get its result
            axpoPowerTrades = (await fastestTask).ToList();
            //Cancel rest of tasks
            cancellationTokenSource.Cancel();

            log.LogInformation($"Axpo Power Trades retrieved from library: {axpoPowerTrades.Count}");

            return axpoPowerTrades
                .Select(axpoPowerTrade => AxpoPowerTradeToDomain(axpoPowerTrade))
                .ToList();
        }


        async Task<IEnumerable<Axpo.PowerTrade>> CallLibraryWithRetry(int taskIndex, DateTime date, CancellationTokenSource cancellationTokenSource)
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var result = await powerService.GetTradesAsync(date);
                    log.LogDebug($"Task {taskIndex}: finished successfully");
                    return result;
                }
                catch (Exception ex)
                {
                    log.LogWarning($"Task {taskIndex}: Error getting power trades from library. {ex.Message}");
                }
                Thread.Sleep(200);
            }
            log.LogDebug($"Task {taskIndex}: cancelled");
            return null;
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
