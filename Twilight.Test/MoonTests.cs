using System;
using System.Text.RegularExpressions;
using Xunit;
using Twilight;
using static System.Math;

namespace Twilight.Test
{
    public class MoonTests
    {

      

        [Theory]
        [InlineData("2016-08-09 10:10:10", "GMT Standard Time", "51.5", "-0.13", "2016-08-09 12:44:45", "2016-08-09 23:25:59", false, false)] //London
        //[InlineData("2016-08-09 10:10:10", "India Standard Time", "28.6", "77.2", "2016-08-09 05:47:00", "2016-08-09 19:06:00")] //NewDelhi
        //[InlineData("2016-08-09 10:10:10", "New Zealand Standard Time", "-36.847", "174.77", "2016-08-09 07:11:00", "2016-08-09 17:42:00")] //Auckland
        public void MoonRiseTest(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime rise, DateTime set, bool isAlwaysUp, bool isAlwaysDown)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(strtimeZoneInfo);

            var retval = Moon.MoonTimes(dateTime, tzi, lat, lng);

            Assert.Equal(rise, retval.Rise?.AddTicks(-((retval.Rise?.Ticks ?? 0) % TimeSpan.TicksPerSecond)));
            Assert.Equal(set, retval.Set?.AddTicks(-((retval.Set?.Ticks ?? 0) % TimeSpan.TicksPerSecond)));
            Assert.Equal(isAlwaysUp, retval.IsAlwaysUp);
            Assert.Equal(isAlwaysDown, retval.IsAlwaysDown);


        }

    }
}
