namespace PowerTrades.Domain.powertrade
{
    public class PowerPeriod
    {
        public int HourOfTheDay { get; set; }

        public double Volume { get; set; }

        public static PowerPeriod operator +(PowerPeriod a, PowerPeriod b)
        {
            if (a.HourOfTheDay != b.HourOfTheDay)
            {
                throw new ArgumentException("Only power periods with same hour of the day can be summed");
            }

            return new PowerPeriod()
            {
                HourOfTheDay = a.HourOfTheDay,
                Volume = a.Volume + b.Volume
            };
        }
    }

}
