using FluentAssertions;
using PowerTrades.Domain.Power;

namespace PowerTrades.Domain.Test.Power
{
    public class PowerPeriodTest
    {
        [Fact]
        public void two_power_periods_with_same_hour_of_the_day_can_summed()
        {
            var powerPeriod1 = new PowerPeriod { HourOfTheDay = 3, Volume = 50 };
            var powerPeriod2 = new PowerPeriod { HourOfTheDay = 3, Volume = 80 };

            var sum = powerPeriod1 + powerPeriod2;

            sum.Should().BeEquivalentTo(new PowerPeriod { HourOfTheDay = 3, Volume = 130 });
        }

        [Fact]
        public void two_power_periods_with_different_cannot_be_summed()
        {
            var powerPeriod1 = new PowerPeriod { HourOfTheDay = 3, Volume = 50 };
            var powerPeriod2 = new PowerPeriod { HourOfTheDay = 5, Volume = 80 };

            Action action = () => { var sum = powerPeriod1 + powerPeriod2; };

            action.Should().Throw<ArgumentException>();
        }
    }
}