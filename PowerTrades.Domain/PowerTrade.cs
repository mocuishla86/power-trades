
namespace PowerTrades.domain
{
    public class PowerTrade
    {
        private const int HOURS_IN_DAY = 24;

        public List<PowerPeriod> Periods { get; set; }

        public static PowerTrade WithAllPeriodsWithVolume(int volume)
        {
            var periods = new List<PowerPeriod>();
            for (int i = 1; i <= HOURS_IN_DAY; i++) {
                periods.Add(new PowerPeriod { HourOfTheDay = i, Volume = volume });
            }
            return new PowerTrade {  Periods = periods };
        }
    }

}
