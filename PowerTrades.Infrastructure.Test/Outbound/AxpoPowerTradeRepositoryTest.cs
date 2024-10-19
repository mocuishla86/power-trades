using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PowerTrades.Domain.Power;
using PowerTrades.Infrastructure.Outbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTrades.Infrastructure.Test.Outbound
{
    public class AxpoPowerTradeRepositoryTest
    {
        private AxpoPowerTradeRepository sut;

        private Axpo.IPowerService powerService;

        public AxpoPowerTradeRepositoryTest()
        {
            powerService = Substitute.For<Axpo.IPowerService>();
            sut = new AxpoPowerTradeRepository(powerService, Substitute.For<ILogger<AxpoPowerTradeRepository>>());
        }

        [Fact]
        public async Task happy_case_values_retrieved_from_library_are_correctly_mapped()
        {
            DateTime now = DateTime.Now;
            var trade1 = Axpo.PowerTrade.Create(now, 1);
            trade1.Periods[0].SetVolume(33);
            var trade2 = Axpo.PowerTrade.Create(now, 1);
            trade2.Periods[0].SetVolume(66);
            powerService.GetTradesAsync(now).Returns(new List<Axpo.PowerTrade>
            {
                trade1, trade2
            });

            var domainTrades = await sut.GetPowerTrades(now);

            domainTrades.Should().HaveCount(2);
            domainTrades[0].Should().BeEquivalentTo(new PowerTrade {  Periods = [new PowerPeriod {  HourOfTheDay = 1, Volume = 33}] });
            domainTrades[1].Should().BeEquivalentTo(new PowerTrade {  Periods = [new PowerPeriod {  HourOfTheDay = 1, Volume = 66}] });
        }

    }
}
