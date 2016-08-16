using System;

namespace Twilight.Internal
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
        internal static double toDays(DateTime date) { return toJulian(date) - J2000; } 
      

    }


    public class JulianDate
    {
        private static bool isJulianDate(int year, int month, int day)
        {
            // All dates prior to 1582 are in the Julian calendar
            if (year < 1582)
                return true;
            // All dates after 1582 are in the Gregorian calendar
            else if (year > 1582)
                return false;
            else
            {
                // If 1582, check before October 4 (Julian) or after October 15 (Gregorian)
                if (month < 10)
                    return true;
                else if (month > 10)
                    return false;
                else
                {
                    if (day < 5)
                        return true;
                    else if (day > 14)
                        return false;
                    else
                        // Any date in the range 10/5/1582 to 10/14/1582 is invalid 
                        throw new ArgumentOutOfRangeException(
                            "This date is not valid as it does not exist in either the Julian or the Gregorian calendars.");
                }
            }
        }

        internal static double DateToJD(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            // Determine correct calendar based on date
            bool JulianCalendar = isJulianDate(year, month, day);

            int M = month > 2 ? month : month + 12;
            int Y = month > 2 ? year : year - 1;
            double D = day + hour / 24.0 + minute / 1440.0 + (second + millisecond * 1000) / 86400.0;
            int B = JulianCalendar ? 0 : 2 - Y / 100 + Y / 100 / 4;

            return (int)(365.25 * (Y + 4716)) + (int)(30.6001 * (M + 1)) + D + B - 1524.5;
        }

    }
}