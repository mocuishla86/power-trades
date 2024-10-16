
namespace PowerTrades.domain
{
    public class PowerTrade
    {
        private const int HOURS_IN_DAY = 24;

        public List<PowerPeriod> Periods { get; set; }

        public PowerPeriod GetPeriod(int hourOfTheDay) => Periods.Single(period => period.HourOfTheDay == hourOfTheDay);

        public static PowerTrade operator +(PowerTrade a, PowerTrade b)
        {
            var periods = Enumerable.Range(1, HOURS_IN_DAY)
                .Select(hourOfTheDay => a.GetPeriod(hourOfTheDay) + b.GetPeriod(hourOfTheDay))
                .ToList();
            return new PowerTrade { Periods = periods };
        }

        public static PowerTrade WithAllPeriodsWithVolume(int volume)
        {
            var periods = Enumerable.Range(1, HOURS_IN_DAY)
                .Select(hourOfTheDay => new PowerPeriod { HourOfTheDay = hourOfTheDay, Volume = volume })
                .ToList();
            return new PowerTrade {  Periods = periods };
        }
    }

}
