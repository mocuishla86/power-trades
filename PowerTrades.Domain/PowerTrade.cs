
namespace PowerTrades.domain
{
    public class PowerTrade
    {
        private const int HOURS_IN_DAY = 24;

        public List<PowerPeriod> Periods { get; set; }

        public static PowerTrade WithAllPeriodsWithVolume(int volume)
        {
            var periods = Enumerable.Range(1, HOURS_IN_DAY)
                .Select(hourOfTheDay => new PowerPeriod { HourOfTheDay = hourOfTheDay, Volume = volume })
                .ToList();
            return new PowerTrade {  Periods = periods };
        }
    }

}
