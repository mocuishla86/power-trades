using FluentAssertions;
using PowerTrades.Domain.powertrade;

namespace PowerTrades.Domain.Test
{
    public class PowerTradeTest
    {
        [Fact]
        public void two_power_trades_can_be_summed()
        {
            var powerTrade1 = new PowerTrade
            {
                Periods = [
                    new PowerPeriod { HourOfTheDay = 1, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 2, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 3, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 4, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 5, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 6, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 7, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 8, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 9, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 10, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 11, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 12, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 13, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 14, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 15, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 16, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 17, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 18, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 19, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 20, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 21, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 22, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 23, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 24, Volume = 10 },
                ]
            };
            var powerTrade2 = new PowerTrade
            {
                Periods = [
                    new PowerPeriod { HourOfTheDay = 1, Volume = 40 },
                    new PowerPeriod { HourOfTheDay = 2, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 3, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 4, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 5, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 6, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 7, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 8, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 9, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 10, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 11, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 12, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 13, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 14, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 15, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 16, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 17, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 18, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 19, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 20, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 21, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 22, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 23, Volume = 10 },
                    new PowerPeriod { HourOfTheDay = 24, Volume = 10 },
                ]
            };

            PowerTrade sum = powerTrade1 + powerTrade2;
            
            sum.Should().BeEquivalentTo(new PowerTrade
            {
                Periods = [
                    new PowerPeriod { HourOfTheDay = 1, Volume = 60 },
                    new PowerPeriod { HourOfTheDay = 2, Volume = 30 },
                    new PowerPeriod { HourOfTheDay = 3, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 4, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 5, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 6, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 7, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 8, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 9, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 10, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 11, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 12, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 13, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 14, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 15, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 16, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 17, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 18, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 19, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 20, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 21, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 22, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 23, Volume = 20 },
                    new PowerPeriod { HourOfTheDay = 24, Volume = 20 },
                ]
            });
        }
    }
}