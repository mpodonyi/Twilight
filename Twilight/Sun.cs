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
        internal SunPeriod(DateTimeOffset? rise, DateTimeOffset? set, bool isAlwaysUp, bool isAlwaysDown)
        {
            Rise = rise;
            Set = set;
            IsAlwaysUp = isAlwaysUp;
            IsAlwaysDown = isAlwaysDown;
        }

        public DateTimeOffset? Rise { get; }
        public DateTimeOffset? Set { get; }

        public bool IsAlwaysUp { get; }

        public bool IsAlwaysDown { get; }
    }


    public static class Sun
    {
        private static bool Between(DateTimeOffset datefrom, DateTimeOffset now, DateTimeOffset dateto) => (datefrom.Date < now.Date) && (now.Date < dateto.Date);


        //MP: Ensure date is local time
        //MP: check year between -2000, 3000
        //MP: use TimeZoneInfo to calculate the day

        public static SunPeriod SunPeriod(DateTimeOffset localDate, double lat, double lng)
        {
            var jday = GetJulianDay(localDate.Date);

            DateTime rise =  CalcSunriseSet(true, jday, lat, lng, localDate, SunRiseTypes.Default);
            DateTime set = CalcSunriseSet(false, jday, lat, lng, localDate, SunRiseTypes.Default);
            return new SunPeriod(rise,set, Between(rise, localDate, set), Between(set, localDate, rise));
        }

        public static SunPeriod CivilPeriod(DateTimeOffset localDate,  double lat, double lng)
        {
            var jday = GetJulianDay(localDate.Date);

            DateTime rise = CalcSunriseSet(true, jday, lat, lng, localDate, SunRiseTypes.Civil);
            DateTime set = CalcSunriseSet(false, jday, lat, lng, localDate, SunRiseTypes.Civil);
            return new SunPeriod(rise, set, Between(rise, localDate, set), Between(set, localDate, rise));
        }

        public static SunPeriod NauticalPeriod(DateTimeOffset localDate,  double lat, double lng)
        {
            var jday = GetJulianDay(localDate.Date);


            DateTime rise = CalcSunriseSet(true, jday, lat, lng, localDate, SunRiseTypes.Nautical);
            DateTime set = CalcSunriseSet(false, jday, lat, lng, localDate, SunRiseTypes.Nautical);
            return new SunPeriod(rise, set, Between(rise, localDate, set), Between(set, localDate, rise));
        }

        public static SunPeriod AstronomicalPeriod(DateTimeOffset localDate, double lat, double lng)
        {
            var jday = GetJulianDay(localDate.Date);

            DateTime rise = CalcSunriseSet(true, jday, lat, lng, localDate, SunRiseTypes.Astro);
            DateTime set = CalcSunriseSet(false, jday, lat, lng, localDate, SunRiseTypes.Astro);
            return new SunPeriod(rise, set, Between(rise, localDate, set), Between(set, localDate, rise));
        }

    }
}
