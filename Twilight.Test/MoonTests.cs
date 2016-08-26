using System;
using System.Text.RegularExpressions;
using Xunit;
using Twilight;
using static System.Math;

namespace Twilight.Test
{
    public class MoonTests: TestBase
    {


        [Theory]
        [InlineData("London", "2016-08-22T10:10:10+01:00", "51°30'N", "0°08'W", "2016-08-22T22:12:00+01:00", "2016-08-22T10:55:00+01:00", MoonPeriodTypes.RiseAndSet)]
        [InlineData("New Delhi", "2016-08-26T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-26T00:07:00+05:30", "2016-08-26T13:55:00+05:30", MoonPeriodTypes.RiseAndSet)]
        [InlineData("Auckland", "2016-08-27T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-27T02:26:00+12:00", "2016-08-27T12:54:00+12:00", MoonPeriodTypes.RiseAndSet)]



        [InlineData("Alert", "2016-08-02T00:00:00-04:00", "82°31'N", "62°18'W", null, null, MoonPeriodTypes.UpAllDay)]
        [InlineData("Alert", "2016-08-14T00:00:00-04:00", "82°31'N", "62°18'W", null, null, MoonPeriodTypes.DownAllDay)]

        [InlineData("London", "2016-01-01T10:10:10+00:00", "51°30'N", "0°08'W", null, "2016-01-01T11:26:00+00:00", MoonPeriodTypes.SetOnly)]
        [InlineData("London", "2016-01-16T10:10:10+00:00", "51°30'N", "0°08'W", "2016-01-16T11:06:00+00:00", null, MoonPeriodTypes.RiseOnly)]


        //[InlineData("New Delhi", "2016-08-09T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-09T05:47:00+05:30", "2016-08-09T19:06:00+05:30", false, false)]
        //[InlineData("Auckland", "2016-08-09T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-09T07:11:00+12:00", "2016-08-09T17:42:00+12:00", false, false)]
        //[InlineData("Alert", "2016-08-09T10:10:10-04:00", "82°31'N", "62°18'W", "2016-04-06T00:00:00-00:00", "2016-09-06T00:00:00-00:00", true, false)]
        //[InlineData("Alert", "2016-01-09T10:10:10-04:00", "82°31'N", "62°18'W", "2016-02-28T00:00:00-00:00", "2015-10-15T00:00:00-00:00", false, true)]
        public void MoonRiseSetTest(string city, string dateTimeOffset, string lat, string lng, string moonrise, string moonset, MoonPeriodTypes moonPeriodTypes)
        {
            MoonTest(dateTimeOffset, lat, lng, moonrise, moonset, moonPeriodTypes, Moon.Period);
        }



    }
}
