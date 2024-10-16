using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrades.Domain.date
{
    public interface IDateTimeService
    {
        DateTime GetCurrentDateTime();
    }
}
