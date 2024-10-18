using NodaTime;

namespace PowerTrades.Domain.Date
{
    public class LisbonTimeZoneDateTimeService : IDateTimeService
    {
        public DateTime GetCurrentLocalDateTime() => DateTime.Now;

        public DateTimeZone GetLocalDateTimeZone() => DateTimeZoneProviders.Tzdb["Europe/Lisbon"];
    }
}
