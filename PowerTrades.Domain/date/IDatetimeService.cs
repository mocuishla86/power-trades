using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrades.Domain.Date
{
    public interface IDateTimeService
    {
        DateTime GetCurrentLocalDateTime();
        DateTimeZone GetLocalDateTimeZone();
    }
}
