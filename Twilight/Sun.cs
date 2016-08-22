using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Resources;
using static Twilight.Internal.SunHelper;
using System.Diagnostics;
using Twilight.Internal;

namespace Twilight
{
    public static class Sun
    {
        private static bool Between(DateTimeOffset datefrom, DateTimeOffset now, DateTimeOffset dateto) => (datefrom.Date < now.Date) && (now.Date < dateto.Date);

        //MP: Ensure date is local time
        //MP: check year between -2000, 3000
        //MP: use TimeZoneInfo to calculate the day

        private static SunPeriod CalculatePeriod(DateTimeOffset localDate, double lat, double lng, SunRiseTypes sunRiseTypes)
        {
            ThrowHelper.CheckLat(lat);
            ThrowHelper.CheckLng(lng);

            if (localDate.Year < -2000)
                throw new ArgumentOutOfRangeException(nameof(localDate));

            if(localDate.Year > 3000)
                throw new ArgumentOutOfRangeException(nameof(localDate));

            var jday = GetJulianDay(localDate.Date);

            DateTimeOffset rise = CalcSunriseSet(true, jday, lat, lng, localDate, sunRiseTypes);
            DateTimeOffset set = CalcSunriseSet(false, jday, lat, lng, localDate, sunRiseTypes);


            SunPeriodTypes type= SunPeriodTypes.RiseAndSet;
            if( Between(rise, localDate, set))
                type = SunPeriodTypes.UpAllDay;

            if (Between(set, localDate, rise))
                type = SunPeriodTypes.DownAllDay;

            return new SunPeriod(rise, set, type);
        }



      

        public static SunPeriod SunPeriod(DateTimeOffset localDate, double lat, double lng) => CalculatePeriod(localDate, lat, lng, SunRiseTypes.Default);

        public static SunPeriod CivilPeriod(DateTimeOffset localDate,  double lat, double lng) => CalculatePeriod(localDate, lat, lng, SunRiseTypes.Civil);

        public static SunPeriod NauticalPeriod(DateTimeOffset localDate,  double lat, double lng) => CalculatePeriod(localDate, lat, lng, SunRiseTypes.Nautical);

        public static SunPeriod AstronomicalPeriod(DateTimeOffset localDate, double lat, double lng) => CalculatePeriod(localDate, lat, lng, SunRiseTypes.Astro);
    }
}
