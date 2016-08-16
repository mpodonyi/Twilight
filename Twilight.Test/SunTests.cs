using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Twilight.Test
{
    public class SunTests
    {
        private void Test(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime sunrise, DateTime sunset, Func<DateTime, TimeZoneInfo, double, double, SunPeriod> fun)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(strtimeZoneInfo);

            SunPeriod sp = fun(dateTime, tzi, lat, lng);

            sp.Rise.Should().Be(sunrise);
            sp.Set.Should().Be(sunset);
        }


        [Theory]
        [InlineData("2016-08-09 10:10:10", "GMT Standard Time", "51.5", "-0.13", "2016-08-09 05:37:00", "2016-08-09 20:34:00")] //London
        [InlineData("2016-08-09 10:10:10", "India Standard Time", "28.6", "77.2", "2016-08-09 05:47:00", "2016-08-09 19:06:00")] //NewDelhi
        [InlineData("2016-08-09 10:10:10", "New Zealand Standard Time", "-36.847", "174.77", "2016-08-09 07:11:00", "2016-08-09 17:42:00")] //Auckland
        [InlineData("2016-08-09 10:10:10", "Atlantic Standard Time", "82.4508", "-62.5056", "2016-04-06 00:00:00", "2016-09-06 00:00:00")] //Auckland
        public void SunRiseSetTest(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime sunrise, DateTime sunset)
        {
            Test(dateTime, strtimeZoneInfo, lat, lng, sunrise, sunset, Sun.SunPeriod);
        }


        [Theory]
        [InlineData("2016-08-09 10:10:10", "GMT Standard Time", "51.5", "-0.13", "2016-08-09 04:58:00", "2016-08-09 21:12:00")] //London
        //[InlineData("2016-08-09 10:10:10", "India Standard Time", "28.6", "77.2", "2016-08-09 05:47:00", "2016-08-09 19:06:00")] //NewDelhi
        //[InlineData("2016-08-09 10:10:10", "New Zealand Standard Time", "-36.847", "174.77", "2016-08-09 07:11:00", "2016-08-09 17:42:00")] //Auckland
        public void SunRiseSetCivilTest(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime sunrise, DateTime sunset)
        {
            Test(dateTime, strtimeZoneInfo, lat, lng, sunrise, sunset, Sun.CivilPeriod);
        }

        [Theory]
        [InlineData("2016-08-09 10:10:10", "GMT Standard Time", "51.5", "-0.13", "2016-08-09 04:08:00", "2016-08-09 22:02:00")] //London
        //[InlineData("2016-08-09 10:10:10", "India Standard Time", "28.6", "77.2", "2016-08-09 05:47:00", "2016-08-09 19:06:00")] //NewDelhi
        //[InlineData("2016-08-09 10:10:10", "New Zealand Standard Time", "-36.847", "174.77", "2016-08-09 07:11:00", "2016-08-09 17:42:00")] //Auckland
        public void SunRiseSetNauticalTest(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime sunrise, DateTime sunset)
        {
            Test(dateTime, strtimeZoneInfo, lat, lng, sunrise, sunset, Sun.NauticalPeriod);
        }

        [Theory]
        [InlineData("2016-08-09 10:10:10", "GMT Standard Time", "51.5", "-0.13", "2016-08-09 03:04:00", "2016-08-09 23:05:00")] //London
        //[InlineData("2016-08-09 10:10:10", "India Standard Time", "28.6", "77.2", "2016-08-09 05:47:00", "2016-08-09 19:06:00")] //NewDelhi
        //[InlineData("2016-08-09 10:10:10", "New Zealand Standard Time", "-36.847", "174.77", "2016-08-09 07:11:00", "2016-08-09 17:42:00")] //Auckland
        public void SunRiseSetAstroTest(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime sunrise, DateTime sunset)
        {
            Test(dateTime, strtimeZoneInfo, lat, lng, sunrise, sunset, Sun.AstronomicalPeriod);
        }
    }
}
