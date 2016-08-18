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
    public class SunTests
    {
        public static double ConvertDegreeAngleToDouble(string point)
        {
            //Example: 17.21.18S

            //Degrees Lat Long    51.5000000°, -000.1300000°
            //Degrees Minutes 51°30.00000', 000°07.80000'
            //Degrees Minutes Seconds     51°30'00.0000", 000°07'48.0000"

            var multiplier = (point.Contains("S") || point.Contains("W")) ? -1 : 1; //handle south and west

            point = Regex.Replace(point, "[^0-9.]", "."); //remove the characters

            var pointArray = point.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries); //split the string.

            //Decimal degrees = 
            //   whole number of degrees, 
            //   plus minutes divided by 60, 
            //   plus seconds divided by 3600

            var degrees = Double.Parse(pointArray[0]);
            var minutes = Double.Parse(pointArray[1]) / 60;
            //var seconds = Double.Parse(pointArray[2]) / 3600;

            return (degrees + minutes /*+ seconds*/) * multiplier;
        }

        private void Test(string dateTime, string lat, string lng, string sunrise, string sunset, bool isAlwaysUp,bool isAlwaysDown, Func<DateTimeOffset, double, double, SunPeriod> fun)
        {
            //const string pattern = "yyyy-MM-dd'T'HH:mm:ss.FFFK";
            const string pattern = "yyyy-MM-dd'T'HH:mm:ssK";
            DateTimeOffset inputOffset = DateTimeOffset.ParseExact(dateTime, pattern, CultureInfo.InvariantCulture);
            DateTimeOffset sunriseOffset = DateTimeOffset.ParseExact(sunrise, pattern, CultureInfo.InvariantCulture);
            DateTimeOffset sunsetOffset = DateTimeOffset.ParseExact(sunset, pattern, CultureInfo.InvariantCulture);

            double dLat = ConvertDegreeAngleToDouble(lat);
            double dLng = ConvertDegreeAngleToDouble(lng);

            SunPeriod sp = fun(inputOffset, dLat, dLng);

            sp.Rise.Should().Be(sunriseOffset, "SunRise");
            sp.Set.Should().Be(sunsetOffset, "SunSet");
            sp.IsAlwaysUp.Should().Be(isAlwaysUp);
            sp.IsAlwaysDown.Should().Be(isAlwaysDown);
        }


        [Theory]
        [InlineData("London", "2016-08-09T10:10:10+01:00", "51°30'N", "0°08'W", "2016-08-09T05:37:00+01:00", "2016-08-09T20:34:00+01:00", false, false)]
        [InlineData("New Delhi", "2016-08-09T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-09T05:47:00+05:30", "2016-08-09T19:06:00+05:30", false, false)]
        [InlineData("Auckland", "2016-08-09T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-09T07:11:00+12:00", "2016-08-09T17:42:00+12:00", false, false)]
        [InlineData("Alert", "2016-08-09T10:10:10-04:00", "82°31'N", "62°18'W", "2016-04-06T00:00:00-00:00", "2016-09-06T00:00:00-00:00", true, false)]
        [InlineData("Alert", "2016-01-09T10:10:10-04:00", "82°31'N", "62°18'W", "2016-02-28T00:00:00-00:00", "2015-10-15T00:00:00-00:00", false, true)]
        public void SunRiseSetTest(string city, string dateTimeOffset, string lat, string lng, string sunrise, string sunset, bool isAlwaysUp, bool isAlwaysDown)
        {
            Test(dateTimeOffset, lat, lng, sunrise, sunset, isAlwaysUp, isAlwaysDown, Sun.SunPeriod);
        }


        [Theory]
        [InlineData("London", "2016-08-09T10:10:10+01:00", "51°30'N", "0°08'W", "2016-08-09T04:58:00+01:00", "2016-08-09T21:12:00+01:00", false, false)]
        [InlineData("New Delhi", "2016-08-09T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-09T05:22:00+05:30", "2016-08-09T19:31:00+05:30", false, false)]
        [InlineData("Auckland", "2016-08-09T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-09T06:44:00+12:00", "2016-08-09T18:09:00+12:00", false, false)]
        public void SunRiseSetCivilTest(string city, string dateTimeOffset, string lat, string lng, string sunrise, string sunset, bool isAlwaysUp, bool isAlwaysDown)
        {
            Test(dateTimeOffset, lat, lng, sunrise, sunset, isAlwaysUp, isAlwaysDown, Sun.CivilPeriod);
        }


        [Theory]
        [InlineData("London", "2016-08-09T10:10:10+01:00", "51°30'N", "0°08'W", "2016-08-09T04:08:00+01:00", "2016-08-09T22:02:00+01:00", false, false)]
        [InlineData("New Delhi", "2016-08-09T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-09T04:52:00+05:30", "2016-08-09T20:00:00+05:30", false, false)]
        [InlineData("Auckland", "2016-08-09T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-09T06:13:00+12:00", "2016-08-09T18:40:00+12:00", false, false)]
        public void SunRiseSetNauticalTest(string city, string dateTimeOffset, string lat, string lng, string sunrise, string sunset, bool isAlwaysUp, bool isAlwaysDown)
        {
            Test(dateTimeOffset, lat, lng, sunrise, sunset, isAlwaysUp, isAlwaysDown, Sun.NauticalPeriod);
        }

        [Theory]
        [InlineData("London", "2016-08-09T10:10:10+01:00", "51°30'N", "0°08'W", "2016-08-09T03:04:00+01:00", "2016-08-09T23:05:00+01:00", false, false)]
        [InlineData("New Delhi", "2016-08-09T10:10:10+05:30", "28°37'N", "77°13'E", "2016-08-09T04:21:00+05:30", "2016-08-09T20:31:00+05:30", false, false)]
        [InlineData("Auckland", "2016-08-09T10:10:10+12:00", "36°51'S", "174°46'E", "2016-08-09T05:43:00+12:00", "2016-08-09T19:10:00+12:00", false, false)]
        public void SunRiseSetAstroTest(string city, string dateTimeOffset, string lat, string lng, string sunrise, string sunset, bool isAlwaysUp, bool isAlwaysDown)
        {
            Test(dateTimeOffset, lat, lng, sunrise, sunset, isAlwaysUp, isAlwaysDown, Sun.AstronomicalPeriod);
        }
    }
}
