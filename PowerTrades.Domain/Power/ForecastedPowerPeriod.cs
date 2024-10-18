namespace PowerTrades.Domain.Power
{
    public class ForecastedPowerPeriod
    {
        public DateTime DateTimeInUtc { get; set; }

        public double AggregatedVolume { get; set; }
    }
}
