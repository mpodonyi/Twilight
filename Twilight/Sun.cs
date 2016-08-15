using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using static Twilight.SunHelper;

namespace Twilight
{
   

    public static class Sun
    {
        //public static Period RiseSetPeriod(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        //{

        //}

        //public static Period CivilPeriod(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        //{
          
        //}


        //public static Period NauticalPeriod(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        //{

        //}

        //public static Period AstronomicalPeriod(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        //{

        //}

        //-------------------

        //public static DateTime Rise(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        //{
        //    //MP: Ensure date is local time
        //    //MP: check year between -2000, 3000
        //    var jday = GetJulianDay(localDate);

        //    double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
        //    var dst = timeZone.IsDaylightSavingTime(localDate);

        //    return calcSunriseSet(true, jday, lat, lng, tz, dst);
        //}

        //public static DateTime Set(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        //{
        //    //MP: Ensure date is local time
        //    //MP: check year between -2000, 3000
        //    var jday = GetJulianDay(localDate);

        //    double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
        //    var dst = timeZone.IsDaylightSavingTime(localDate);

        //    return calcSunriseSet(false, jday, lat, lng, tz, dst);

        //}


        public static DateTime CivilDawn(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            return CalcSunriseSet(true, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Civil);
        }

        public static DateTime CivilDusk(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            return CalcSunriseSet(false, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Civil);
        }


        public static DateTime NauticalDawn(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            return CalcSunriseSet(true, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Nautical);
        }

        public static DateTime NauticalDusk(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            return CalcSunriseSet(false, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Nautical);
        }

        public static DateTime AstronomicalDawn(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            return CalcSunriseSet(true, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Astro);
        }

        public static DateTime AstronomicalDusk(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            return CalcSunriseSet(false, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Astro);
        }

        public static DateTime Rise(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            return CalcSunriseSet(true, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Default);
        }

        public static DateTime Set(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            return CalcSunriseSet(false, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Default);
        }




      





    }
}
