using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using static Twilight.Internal.SunHelper;

namespace Twilight
{

    public class SunPeriod
    {
        internal SunPeriod(DateTime? rise, DateTime? set, bool isAlwaysUp, bool isAlwaysDown)
        {
            Rise = rise;
            Set = set;
            IsAlwaysUp = isAlwaysUp;
            IsAlwaysDown = isAlwaysDown;
        }

        public DateTime? Rise { get; }
        public DateTime? Set { get; }

        public bool IsAlwaysUp { get; }

        public bool IsAlwaysDown { get; }
    }


    public static class Sun
    {
        private static bool Between(DateTime datefrom, DateTime now, DateTime dateto)
        {
            if ((datefrom.Date < now.Date) && (now.Date < dateto.Date))
                return true;

            return false;

        }


        //MP: Ensure date is local time
        //MP: check year between -2000, 3000
        //MP: use TimeZoneInfo to calculate the day

        public static SunPeriod SunPeriod(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            DateTime rise =  CalcSunriseSet(true, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Default);
            DateTime set = CalcSunriseSet(false, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Default);
            return new SunPeriod(rise,set, Between(rise, localDate, set), Between(set, localDate, rise));
        }

        public static SunPeriod CivilPeriod(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            DateTime rise = CalcSunriseSet(true, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Civil);
            DateTime set = CalcSunriseSet(false, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Civil);
            return new SunPeriod(rise, set, Between(rise, localDate, set), Between(set, localDate, rise));
        }

        public static SunPeriod NauticalPeriod(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            DateTime rise = CalcSunriseSet(true, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Nautical);
            DateTime set = CalcSunriseSet(false, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Nautical);
            return new SunPeriod(rise, set, Between(rise, localDate, set), Between(set, localDate, rise));
        }

        public static SunPeriod AstronomicalPeriod(DateTime localDate, TimeZoneInfo timeZone, double lat, double lng)
        {
            var jday = GetJulianDay(localDate);

            double tz = timeZone.BaseUtcOffset.TotalMinutes / 60;
            var dst = timeZone.IsDaylightSavingTime(localDate);

            DateTime rise = CalcSunriseSet(true, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Astro);
            DateTime set = CalcSunriseSet(false, jday, lat, lng, tz, dst, localDate, SunRiseTypes.Astro);
            return new SunPeriod(rise, set, Between(rise, localDate, set), Between(set, localDate, rise));
        }

    }
}
