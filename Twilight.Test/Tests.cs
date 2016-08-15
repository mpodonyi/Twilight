using System;
using System.Text.RegularExpressions;
using Xunit;
using Twilight;
using static System.Math;

namespace Twilight.Test
{
    public class Tests
    {

        [Theory]
        [InlineData("2016-08-09 10:10:10", "GMT Standard Time", "51.5", "-0.13", "2016-08-09 05:37:00", "2016-08-09 20:34:00")] //London
        [InlineData("2016-08-09 10:10:10", "India Standard Time", "28.6", "77.2", "2016-08-09 05:47:00", "2016-08-09 19:06:00")] //NewDelhi
        [InlineData("2016-08-09 10:10:10", "New Zealand Standard Time", "-36.847", "174.77", "2016-08-09 07:11:00", "2016-08-09 17:42:00")] //Auckland
        [InlineData("2016-08-09 10:10:10", "Atlantic Standard Time", "82.4508", "-62.5056", "2016-04-06 00:00:00", "2016-09-06 00:00:00")] //Auckland
        public void SunRiseSetTest(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime sunrise, DateTime sunset)
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(strtimeZoneInfo);

            Assert.Equal(sunrise, Sun.Rise(dateTime, tzi, lat, lng));
            Assert.Equal(sunset, Sun.Set(dateTime, tzi, lat, lng));
        }


        [Theory]
        [InlineData("2016-08-09 10:10:10", "GMT Standard Time", "51.5", "-0.13", "2016-08-09 04:58:00", "2016-08-09 21:12:00")] //London
        //[InlineData("2016-08-09 10:10:10", "India Standard Time", "28.6", "77.2", "2016-08-09 05:47:00", "2016-08-09 19:06:00")] //NewDelhi
        //[InlineData("2016-08-09 10:10:10", "New Zealand Standard Time", "-36.847", "174.77", "2016-08-09 07:11:00", "2016-08-09 17:42:00")] //Auckland
        public void SunRiseSetCivilTest(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime sunrise, DateTime sunset)
        {

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(strtimeZoneInfo);

            Assert.Equal(sunrise, Sun.CivilDawn(dateTime, tzi, lat, lng));
            Assert.Equal(sunset, Sun.CivilDusk(dateTime, tzi, lat, lng));

        }

        [Theory]
        [InlineData("2016-08-09 10:10:10", "GMT Standard Time", "51.5", "-0.13", "2016-08-09 04:08:00", "2016-08-09 22:02:00")] //London
        //[InlineData("2016-08-09 10:10:10", "India Standard Time", "28.6", "77.2", "2016-08-09 05:47:00", "2016-08-09 19:06:00")] //NewDelhi
        //[InlineData("2016-08-09 10:10:10", "New Zealand Standard Time", "-36.847", "174.77", "2016-08-09 07:11:00", "2016-08-09 17:42:00")] //Auckland
        public void SunRiseSetNauticalTest(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime sunrise, DateTime sunset)
        {

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(strtimeZoneInfo);

            Assert.Equal(sunrise, Sun.NauticalDawn(dateTime, tzi, lat, lng));
            Assert.Equal(sunset, Sun.NauticalDusk(dateTime, tzi, lat, lng));

        }

        [Theory]
        [InlineData("2016-08-09 10:10:10", "GMT Standard Time", "51.5", "-0.13", "2016-08-09 03:04:00", "2016-08-09 23:05:00")] //London
        //[InlineData("2016-08-09 10:10:10", "India Standard Time", "28.6", "77.2", "2016-08-09 05:47:00", "2016-08-09 19:06:00")] //NewDelhi
        //[InlineData("2016-08-09 10:10:10", "New Zealand Standard Time", "-36.847", "174.77", "2016-08-09 07:11:00", "2016-08-09 17:42:00")] //Auckland
        public void SunRiseSetAstroTest(DateTime dateTime, string strtimeZoneInfo, double lat, double lng, DateTime sunrise, DateTime sunset)
        {

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(strtimeZoneInfo);

            Assert.Equal(sunrise, Sun.AstronomicalDawn(dateTime, tzi, lat, lng));
            Assert.Equal(sunset, Sun.AstronomicalDusk(dateTime, tzi, lat, lng));

        }

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
