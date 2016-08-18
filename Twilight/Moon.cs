using System;
using Twilight.Internal;

namespace Twilight
{
    public static class Moon
    {
        /*
              calculates the moon phase (0-7), accurate to 1 segment.
              0 = > new moon.
              4 => full moon.
              */
        //public static int Phase(DateTime localDate)
        //{
        //    int y, m, d;
        //    y = localDate.Year;
        //    m = localDate.Month;
        //    d = localDate.Day;



        //    int c, e;
        //    double jd;
        //    int b;

        //    if (m < 3)
        //    {
        //        y--;
        //        m += 12;
        //    }
        //    ++m;
        //    c = (int)(365.25 * y);
        //    e = (int)(30.6 * m);
        //    jd = c + e + d - 694039.09;  /* jd is total days elapsed */
        //    jd /= 29.53;           /* divide by the moon cycle (29.53 days) */
        //    b = (int) jd;		   /* int(jd) -> b, take integer part of jd */
        //    jd -= b;		   /* subtract integer part to leave fractional part of original jd */
        //    b = (int)(jd * 8 + 0.5);	   /* scale fraction from 0-8 and round by adding 0.5 */
        //    b = b & 7;		   /* 0 and 8 are the same so turn 8 into 0 */
        //    return b;
        //}


        public static MoonPeriod MoonTimes(DateTimeOffset localDate, double lat, double lng)
        {
            ThrowHelper.CheckLat(lat);
            ThrowHelper.CheckLng(lng);


            DateTime newDateTime = localDate.Date - localDate.Offset;

            var retval = MoonHelper.getMoonTimes(newDateTime, lat, lng);

            return new MoonPeriod(
                retval.Rise.HasValue ? retval.Rise + localDate.Offset : (DateTime?)null,
                retval.Set.HasValue ? retval.Set + localDate.Offset : (DateTime?)null,
                retval.IsAlwaysUp, retval.IsAlwaysDown);




        }
    }
}