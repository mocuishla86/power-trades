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

        [Fact(Timeout = 2_000)] //https://stackoverflow.com/a/20283707
        public async Task it_retries_and_uses_the_fastest_successful_call()
        {
            DateTime now = DateTime.Now;
            IEnumerable<Axpo.PowerTrade> axpoPowerTrades = [Axpo.PowerTrade.Create(now, 1)];
            powerService.GetTradesAsync(now).Returns(
                x => Task.Delay(1_000_000).ContinueWith(_ => axpoPowerTrades),
                x => Task.Delay(1_000_000).ContinueWith(_ => axpoPowerTrades),
                x => throw new Exception("Boom!"),
                x => throw new Exception("Boom!"),
                x => Task.Delay(1_000_000).ContinueWith(_ => axpoPowerTrades),
                x => Task.Delay(1_000_000).ContinueWith(_ => axpoPowerTrades),
                x => Task.FromResult(axpoPowerTrades)
            );

            var powerTrades = await sut.GetPowerTrades(now);

            powerTrades.Should().NotBeNull();
        }

    }
}
