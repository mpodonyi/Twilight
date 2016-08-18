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

        protected void Test(string dateTime, string lat, string lng, string rise, string set, bool isAlwaysUp, bool isAlwaysDown, Func<DateTimeOffset, double, double, dynamic> fun)
        {
            //const string pattern = "yyyy-MM-dd'T'HH:mm:ss.FFFK";
            const string pattern = "yyyy-MM-dd'T'HH:mm:ssK";
            DateTimeOffset inputOffset = DateTimeOffset.ParseExact(dateTime, pattern, CultureInfo.InvariantCulture);
            DateTimeOffset riseOffset = DateTimeOffset.ParseExact(rise, pattern, CultureInfo.InvariantCulture);
            DateTimeOffset setOffset = DateTimeOffset.ParseExact(set, pattern, CultureInfo.InvariantCulture);

            double dLat = ConvertDegreeAngleToDouble(lat);
            double dLng = ConvertDegreeAngleToDouble(lng);

            dynamic sp = fun(inputOffset, dLat, dLng);

            ((DateTimeOffset?)sp.Rise).Should().Be(riseOffset, "Rise");
            ((DateTimeOffset?)sp.Set).Should().Be(setOffset, "Set");
            ((bool)sp.IsAlwaysUp).Should().Be(isAlwaysUp);
            ((bool)sp.IsAlwaysDown).Should().Be(isAlwaysDown);
        }
    }
}
