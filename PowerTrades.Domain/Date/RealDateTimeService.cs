using NodaTime;

namespace PowerTrades.Domain.Date
{
    public class RealDateTimeService : IDateTimeService
    {
        public DateTime GetCurrentLocalDateTime() => DateTime.Now;

        public DateTimeZone GetLocalDateTimeZone() => DateTimeZoneProviders.Bcl.GetSystemDefault();
    }
}
