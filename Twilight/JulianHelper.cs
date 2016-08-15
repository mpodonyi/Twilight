using System;

namespace Twilight
{
    static class JulianHelper
    {
        const int dayMs = 1000*60*60*24;
        const int J1970 = 2440588;
        const int J2000 = 2451545;

        static readonly DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        private static double DateTimeToUnixTimestamp(DateTime dateTime) => (dateTime - UnixTime).TotalMilliseconds;
        private static DateTime UnixTimestampToDateTime(double unixTime) => UnixTime + TimeSpan.FromMilliseconds(unixTime);


        internal static double toJulian(DateTime date) { return DateTimeToUnixTimestamp(date) / dayMs - 0.5 + J1970; }
        //internal static DateTime fromJulian(double j) { return UnixTimestampToDateTime((j + 0.5 - J1970) * dayMs); }
        internal static double toDays(DateTime date) {
            var t1 = GetJulianDay(date);
            var t2 = toJulian(date); //mor accurate



            return toJulian(date) - J2000;



        } //MP: merge with GetJulianDay

      

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

            var a = Math.Floor(docyear / 100);
            var b = 2 - a + Math.Floor(a / 4);

            return Math.Floor(365.25 * (docyear + 4716)) + Math.Floor(30.6001 * (docmonth + 1)) + docday + b - 1524.5;
        }

      

    }
}