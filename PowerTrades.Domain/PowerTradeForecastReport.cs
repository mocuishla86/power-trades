namespace PowerTrades.domain
{
    public class PowerTradeForecastReport
    {
        public DateTime ForecastedDay { get; set; }
        public DateTime ExecutionTimestamp { get; set; }
        public List<ForecastedPowerPeriod> Periods { get; set; }
    }
}
