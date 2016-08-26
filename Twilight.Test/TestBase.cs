using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;

namespace Twilight.Test
{
    public class TestBase
    {

        private static double ConvertDegreeAngleToDouble(string point)
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

        protected void SunTest(string dateTime, string lat, string lng, string rise, string set, SunPeriodTypes sunPeriodType, Func<DateTimeOffset, double, double, SunPeriod> fun)
        {
            //const string pattern = "yyyy-MM-dd'T'HH:mm:ss.FFFK";
            const string pattern = "yyyy-MM-dd'T'HH:mm:ssK";
            DateTimeOffset inputOffset = DateTimeOffset.ParseExact(dateTime, pattern, CultureInfo.InvariantCulture);
            DateTimeOffset? riseOffset = rise == null ? (DateTimeOffset?)null : DateTimeOffset.ParseExact(rise, pattern, CultureInfo.InvariantCulture);
            DateTimeOffset? setOffset = set == null ? (DateTimeOffset?)null : DateTimeOffset.ParseExact(set, pattern, CultureInfo.InvariantCulture);

            double dLat = ConvertDegreeAngleToDouble(lat);
            double dLng = ConvertDegreeAngleToDouble(lng);

            SunPeriod sp = fun(inputOffset, dLat, dLng);

            sp.Rise.Should().Be(riseOffset, "Rise");
            sp.Set.Should().Be(setOffset, "Set");
            sp.SunPeriodType.Should().Be(sunPeriodType);
        }

        protected void MoonTest(string dateTime, string lat, string lng, string rise, string set, MoonPeriodTypes moonPeriodType, Func<DateTimeOffset, double, double, MoonPeriod> fun)
        {
            //const string pattern = "yyyy-MM-dd'T'HH:mm:ss.FFFK";
            const string pattern = "yyyy-MM-dd'T'HH:mm:ssK";

            DateTimeOffset inputOffset = DateTimeOffset.Parse(dateTime, CultureInfo.InvariantCulture);
            DateTimeOffset? riseOffset = rise == null? (DateTimeOffset?) null : DateTimeOffset.ParseExact(rise,pattern, CultureInfo.InvariantCulture);
            DateTimeOffset? setOffset = set == null ? (DateTimeOffset?) null : DateTimeOffset.ParseExact(set,pattern, CultureInfo.InvariantCulture);

            double dLat = ConvertDegreeAngleToDouble(lat);
            double dLng = ConvertDegreeAngleToDouble(lng);

            MoonPeriod sp = fun(inputOffset, dLat, dLng);

            sp.Rise.Should().Be(riseOffset, "Rise");
            sp.Set.Should().Be(setOffset, "Set");
            sp.MoonPeriodType.Should().Be(moonPeriodType);
        }
    }
}
