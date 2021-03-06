using System;
using static System.Math;

namespace Twilight.Internal
{
    static class SunHelper
    {
        internal enum SunRiseTypes : byte
        {
            Default,
            Civil,
            Nautical,
            Astro
        }

        internal static double GetJulianDay(DateTime date) //Julian Date
        {


            double docmonth = date.Month;
            double docday = date.Day;
            double docyear = date.Year;

            if (docmonth <= 2)
            {
                docyear -= 1;
                docmonth += 12;
            }

            var a = Floor(docyear / 100);
            var b = 2 - a + Floor(a / 4);

            return Floor(365.25 * (docyear + 4716)) + Floor(30.6001 * (docmonth + 1)) + docday + b - 1524.5;
        }

        private static double CalcTimeJulianCentury(double jd)
        {
            return (jd - 2451545.0) / 36525.0;
        }

        private static double CalcMeanObliquityOfEcliptic(double t)
        {
            var seconds = 21.448 - t * (46.8150 + t * (0.00059 - t * (0.001813)));
            return 23.0 + (26.0 + (seconds / 60.0)) / 60.0; // in degrees
        }

        private static double DegToRad(double angle) => angle * (PI / 180.0);

        private static double RadToDeg(double angle) => angle * (180.0 / PI);

        private static double CalcObliquityCorrection(double t)
        {
            var e0 = CalcMeanObliquityOfEcliptic(t);
            var omega = 125.04 - 1934.136 * t;
            return e0 + 0.00256 * Cos(DegToRad(omega));  // in degrees
        }

        private static double CalcGeomMeanLongSun(double t)
        {
            var l0 = 280.46646 + t * (36000.76983 + t * (0.0003032));
            while (l0 > 360.0)
            {
                l0 -= 360.0;
            }

            while (l0 < 0.0)
            {
                l0 += 360.0;
            }
            return l0; // in degrees
        }

        private static double CalcEccentricityEarthOrbit(double t)
        {
            return 0.016708634 - t * (0.000042037 + 0.0000001267 * t);  // unitless
        }

        private static double CalcGeomMeanAnomalySun(double t)
        {
            return 357.52911 + t * (35999.05029 - 0.0001537 * t); // in degrees
        }

        private static double CalcEquationOfTime(double t)
        {
            var epsilon = CalcObliquityCorrection(t);
            var l0 = CalcGeomMeanLongSun(t);
            var e = CalcEccentricityEarthOrbit(t);
            var m = CalcGeomMeanAnomalySun(t);

            var y = Tan(DegToRad(epsilon) / 2.0);
            y *= y;

            var sin2L0 = Sin(2.0 * DegToRad(l0));
            var sinm = Sin(DegToRad(m));
            var cos2L0 = Cos(2.0 * DegToRad(l0));
            var sin4L0 = Sin(4.0 * DegToRad(l0));
            var sin2M = Sin(2.0 * DegToRad(m));

            var etime = y * sin2L0 - 2.0 * e * sinm + 4.0 * e * y * sinm * cos2L0 - 0.5 * y * y * sin4L0 - 1.25 * e * e * sin2M;
            return RadToDeg(etime) * 4.0;   // in minutes of time
        }

        private static double CalcSunEqOfCenter(double t)
        {
            var m = CalcGeomMeanAnomalySun(t);
            var mrad = DegToRad(m);
            var sinm = Sin(mrad);
            var sin2M = Sin(mrad + mrad);
            var sin3M = Sin(mrad + mrad + mrad);
            return sinm * (1.914602 - t * (0.004817 + 0.000014 * t)) + sin2M * (0.019993 - 0.000101 * t) + sin3M * 0.000289;       // in degrees
        }

        private static double CalcSunTrueLong(double t)
        {
            var l0 = CalcGeomMeanLongSun(t);
            var c = CalcSunEqOfCenter(t);
            return l0 + c; // in degrees
        }

        private static double CalcSunApparentLong(double t)
        {
            var o = CalcSunTrueLong(t);
            var omega = 125.04 - 1934.136 * t;
            return o - 0.00569 - 0.00478 * Sin(DegToRad(omega)); // in degrees
        }

        private static double CalcSunDeclination(double t)
        {
            var e = CalcObliquityCorrection(t);
            var lambda = CalcSunApparentLong(t);

            var sint = Sin(DegToRad(e)) * Sin(DegToRad(lambda));
            return RadToDeg(Asin(sint));  // in degrees
        }

        private static double CalcHourAngleSunrise(double lat, double solarDec, SunRiseTypes sunRiseTypes)
        {
            double zenith = 90.833;
            switch (sunRiseTypes)
            {
                case SunRiseTypes.Civil:
                    zenith = 96.0;
                    break;
                case SunRiseTypes.Nautical:
                    zenith = 102.0;
                    break;
                case SunRiseTypes.Astro:
                    zenith = 108.0;
                    break;
            }



            var latRad = DegToRad(lat);
            var sdRad = DegToRad(solarDec);
            var hAarg = (Cos(DegToRad(zenith)) / (Cos(latRad) * Cos(sdRad)) - Tan(latRad) * Tan(sdRad));
            return Acos(hAarg); // in radians (for sunset, use -HA)
        }

        internal static double CalcDayOfYearFromJulianDay(double jd)
        {
            var z = Floor(jd + 0.5);
            var f = (jd + 0.5) - z;
            double a;
            if (z < 2299161)
            {
                a = z;
            }
            else
            {
                var alpha = Floor((z - 1867216.25) / 36524.25);
                a = z + 1 + alpha - Floor(alpha / 4);
            }
            var b = a + 1524;
            var c = Floor((b - 122.1) / 365.25);
            var d = Floor(365.25 * c);
            var e = Floor((b - d) / 30.6001);
            var day = b - d - Floor(30.6001 * e) + f;
            var month = (e < 14) ? e - 1 : e - 13;
            var year = (month > 2) ? c - 4716 : c - 4715;

            var k = (DateTime.IsLeapYear((int)year) ? 1 : 2);
            return Floor((275 * month) / 9) - k * Floor((month + 9) / 12) + day - 30;
        }

        private static double CalcSunriseSetUtc(bool rise, double jd, double latitude, double longitude, SunRiseTypes sunRiseTypes)
        {
            var t = CalcTimeJulianCentury(jd);
            var eqTime = CalcEquationOfTime(t);
            var solarDec = CalcSunDeclination(t);
            var hourAngle = CalcHourAngleSunrise(latitude, solarDec, sunRiseTypes);
            //alert("HA = " + radToDeg(hourAngle));
            if (!rise) hourAngle = -hourAngle;
            var delta = longitude + RadToDeg(hourAngle);
            return 720 - (4.0 * delta) - eqTime; // in minutes
        }

        //internal static DateTimeOffset DateFromJulianDay(double jd)//false, 2
        //{
        //    // returns a string in the form DDMMMYYYY[ next] to display prev/next rise/set
        //    // flag=2 for DD MMM, 3 for DD MM YYYY, 4 for DDMMYYYY next/prev
        //    if ((jd < 900000) || (jd > 2817000))
        //    {
        //        throw new Exception("error");
        //    }

        //    var z = Floor(jd + 0.5);
        //    var f = (jd + 0.5) - z;
        //    double a;
        //    if (z < 2299161)
        //    {
        //        a = z;
        //    }
        //    else
        //    {
        //        var alpha = Floor((z - 1867216.25) / 36524.25);
        //        a = z + 1 + alpha - Floor(alpha / 4);
        //    }
        //    var b = a + 1524;
        //    var c = Floor((b - 122.1) / 365.25);
        //    var d = Floor(365.25 * c);
        //    var e = Floor((b - d) / 30.6001);
        //    var day = b - d - Floor(30.6001 * e) + f;
        //    var month = (e < 14) ? e - 1 : e - 13;
        //    var year = ((month > 2) ? c - 4716 : c - 4715);

        //    return new DateTimeOffset((int)year, (int)month, (int)day,0,0,0,TimeSpan.Zero);
        //}

        // timeString returns a zero-padded string (HH:MM:SS) given time in minutes
        // flag=2 for HH:MM, 3 for HH:MM:SS
        //internal static TimeSpan TimeSpanFromMinutes(double minutes, int flag)
        //{
        //    //if ((!(minutes >= 0)) || (!(minutes < 1440))) throw new Exception("error");

        //    TimeSpan retval = TimeSpan.FromMinutes(minutes);

        //    if (flag == 2 && retval.Seconds >= 30)
        //    {
        //        retval = new TimeSpan(retval.Hours, retval.Minutes + 1, 0);
        //    }
        //    else
        //    {
        //        retval = new TimeSpan(retval.Hours, retval.Minutes, 0);
        //    }

        //    return retval;
        //}

        //MP: improve this
        private static DateTimeOffset RoundToMinute(DateTimeOffset offset)
        {
            //if ((!(minutes >= 0)) || (!(minutes < 1440))) throw new Exception("error");

            const int minute = 1 *60 * 1000;
            int off = offset.Second * 1000 + offset.Millisecond;

            return offset.Second >= 30
                ? offset.AddMilliseconds(minute - off)
                : offset.AddMilliseconds(-off);
        }





        //private static double CalcJulianDayOfNextPrevRiseSet(bool next, bool rise, double jd, double latitude, double longitude, TimeSpan offset, SunRiseTypes sunRiseTypes)
        //{
        //    var julianday = jd;
        //    var increment = ((next) ? 1.0 : -1.0);

        //    var time = CalcSunriseSetUtc(rise, julianday, latitude, longitude, sunRiseTypes);
        //    while (double.IsNaN(time))
        //    {
        //        julianday += increment;
        //        time = CalcSunriseSetUtc(rise, julianday, latitude, longitude, sunRiseTypes);
        //    }
        //    var timeLocal = time + offset.TotalMinutes;
        //    while ((timeLocal < 0.0) || (timeLocal >= 1440.0))
        //    {
        //        var incr = ((timeLocal < 0) ? 1 : -1);
        //        timeLocal += (incr * 1440.0);
        //        julianday -= incr;
        //    }
        //    return julianday;
        //}


        //---------------------------


        private static DateTimeOffset DateBuilder(DateTimeOffset origDay, double time)
        {
            TimeSpan tsNewTime = TimeSpan.FromMinutes(time) + origDay.Offset;

            return RoundToMinute(new DateTimeOffset(origDay.Date, origDay.Offset) + tsNewTime);

        }

        internal static DateTimeOffset? CalcSunriseSet(bool rise, double jd, double latitude, double longitude, DateTimeOffset dt, SunRiseTypes sunRiseTypes)
        {
            var timeUtc = CalcSunriseSetUtc(rise, jd, latitude, longitude, sunRiseTypes);
            var newTimeUtc = CalcSunriseSetUtc(rise, jd + timeUtc / 1440.0, latitude, longitude, sunRiseTypes);
            if (!double.IsNaN(newTimeUtc))
            {
                return DateBuilder(dt, newTimeUtc);
            }

            return null;
        }



       

    }
}