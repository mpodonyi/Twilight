using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;
using static Twilight.Internal.SunHelper;
using System.Diagnostics;

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

        private static SunPeriod CalculatePeriod(DateTimeOffset localDate, double lat, double lng, SunRiseTypes sunRiseTypes)
        {
            if(localDate.Year < -2000)
                throw new ArgumentOutOfRangeException(nameof(localDate));

            if(localDate.Year > 3000)
                throw new ArgumentOutOfRangeException(nameof(localDate));

            if(lat < -85.0)
                throw new ArgumentOutOfRangeException(nameof(lat));

            if(lat > 85.0)
                throw new ArgumentOutOfRangeException(nameof(lat));

            if(lng < -180.0)
                throw new ArgumentOutOfRangeException(nameof(lng));

            if(lng > 180.0)
                throw new ArgumentOutOfRangeException(nameof(lng));

            var jday = GetJulianDay(localDate.Date);

            DateTimeOffset rise = CalcSunriseSet(true, jday, lat, lng, localDate, sunRiseTypes);
            DateTimeOffset set = CalcSunriseSet(false, jday, lat, lng, localDate, sunRiseTypes);
            return new SunPeriod(rise, set, Between(rise, localDate, set), Between(set, localDate, rise));
        }



      

        public static SunPeriod SunPeriod(DateTimeOffset localDate, double lat, double lng) => CalculatePeriod(localDate, lat, lng, SunRiseTypes.Default);

        public static SunPeriod CivilPeriod(DateTimeOffset localDate,  double lat, double lng) => CalculatePeriod(localDate, lat, lng, SunRiseTypes.Civil);

        public static SunPeriod NauticalPeriod(DateTimeOffset localDate,  double lat, double lng) => CalculatePeriod(localDate, lat, lng, SunRiseTypes.Nautical);

        public static SunPeriod AstronomicalPeriod(DateTimeOffset localDate, double lat, double lng) => CalculatePeriod(localDate, lat, lng, SunRiseTypes.Astro);
    }
}
