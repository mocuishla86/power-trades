using PowerTrades.Domain.Power;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrades.Application.Outbound
{
    public interface IPowerTradeForecastReportRepository
    {
        void SaveReport(PowerTradeForecastReport report, String destination);
    }
}
