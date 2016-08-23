using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Twilight.Test
{
    public class SunTests:TestBase
    {


        [Theory]
        [InlineData("London", "2016-08-09T10:10:10+01:00", "51°30'N", "0°08'W", "2016-08-09T05:37:00+01:00", "2016-08-09T20:34:00+01:00", SunPeriodTypes.RiseAndSet)]
        [InlineData("New Delhi", "2016-08-09T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-09T05:47:00+05:30", "2016-08-09T19:06:00+05:30", SunPeriodTypes.RiseAndSet)]
        [InlineData("Auckland", "2016-08-09T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-09T07:11:00+12:00", "2016-08-09T17:42:00+12:00", SunPeriodTypes.RiseAndSet)]
        [InlineData("Alert", "2016-08-09T10:10:10-04:00", "82°31'N", "62°18'W", "2016-04-06T00:00:00-00:00", "2016-09-06T00:00:00-00:00", SunPeriodTypes.UpAllDay)]
        [InlineData("Alert", "2016-01-09T10:10:10-04:00", "82°31'N", "62°18'W", "2016-02-28T00:00:00-00:00", "2015-10-15T00:00:00-00:00", SunPeriodTypes.DownAllDay)]
        public void SunRiseSetTest(string city, string dateTimeOffset, string lat, string lng, string sunrise, string sunset, SunPeriodTypes sunPeriodTypes)
        {
            SunTest(dateTimeOffset, lat, lng, sunrise, sunset, sunPeriodTypes, Sun.SunPeriod);
        }





        [Theory]
        [InlineData("London", "2016-08-09T10:10:10+01:00", "51°30'N", "0°08'W", "2016-08-09T04:58:00+01:00", "2016-08-09T21:12:00+01:00", SunPeriodTypes.RiseAndSet)]
        [InlineData("New Delhi", "2016-08-09T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-09T05:22:00+05:30", "2016-08-09T19:31:00+05:30", SunPeriodTypes.RiseAndSet)]
        [InlineData("Auckland", "2016-08-09T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-09T06:44:00+12:00", "2016-08-09T18:09:00+12:00", SunPeriodTypes.RiseAndSet)]
        public void SunRiseSetCivilTest(string city, string dateTimeOffset, string lat, string lng, string sunrise, string sunset, SunPeriodTypes sunPeriodTypes)
        {
            SunTest(dateTimeOffset, lat, lng, sunrise, sunset, sunPeriodTypes, Sun.CivilPeriod);
        }


        [Theory]
        [InlineData("London", "2016-08-09T10:10:10+01:00", "51°30'N", "0°08'W", "2016-08-09T04:08:00+01:00", "2016-08-09T22:02:00+01:00", SunPeriodTypes.RiseAndSet)]
        [InlineData("New Delhi", "2016-08-09T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-09T04:52:00+05:30", "2016-08-09T20:00:00+05:30", SunPeriodTypes.RiseAndSet)]
        [InlineData("Auckland", "2016-08-09T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-09T06:13:00+12:00", "2016-08-09T18:40:00+12:00", SunPeriodTypes.RiseAndSet)]
        public void SunRiseSetNauticalTest(string city, string dateTimeOffset, string lat, string lng, string sunrise, string sunset, SunPeriodTypes sunPeriodTypes)
        {
            SunTest(dateTimeOffset, lat, lng, sunrise, sunset, sunPeriodTypes, Sun.NauticalPeriod);
        }

        [Theory]
        [InlineData("London", "2016-08-09T10:10:10+01:00", "51°30'N", "0°08'W", "2016-08-09T03:04:00+01:00", "2016-08-09T23:05:00+01:00", SunPeriodTypes.RiseAndSet)]
        [InlineData("New Delhi", "2016-08-09T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-09T04:21:00+05:30", "2016-08-09T20:31:00+05:30", SunPeriodTypes.RiseAndSet)]
        [InlineData("Auckland", "2016-08-09T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-09T05:43:00+12:00", "2016-08-09T19:10:00+12:00", SunPeriodTypes.RiseAndSet)]
        public void SunRiseSetAstroTest(string city, string dateTimeOffset, string lat, string lng, string sunrise, string sunset, SunPeriodTypes sunPeriodTypes)
        {
            SunTest(dateTimeOffset, lat, lng, sunrise, sunset, sunPeriodTypes, Sun.AstronomicalPeriod);
        }
    }
}
