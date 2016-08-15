using System;

namespace Twilight
{
    static class MoonHelper
    {
        const double rad = Math.PI / 180;

        const double e = rad * 23.4397; // obliquity of the Earth

        private static double rightAscension(double l, double b) { return Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l)); }
        private static double declination(double l, double b) { return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l)); }

        private static double siderealTime(double d, double lw) { return rad * (280.16 + 360.9856235 * d) - lw; }

        private static double altitude(double H, double phi, double dec) { return Math.Asin(Math.Sin(phi) * Math.Sin(dec) + Math.Cos(phi) * Math.Cos(dec) * Math.Cos(H)); }

        private static double azimuth(double H, double phi, double dec) { return Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(phi) - Math.Tan(dec) * Math.Cos(phi)); }



        private static double astroRefraction(double h)
        {
            if (h < 0) // the following formula works for positive altitudes only.
                h = 0; // if h = -0.08901179 a div/0 would occur.

            // formula 16.4 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            // 1.02 / tan(h + 10.26 / (h + 5.10)) h in degrees, result in arc minutes -> converted to rad:
            return 0.0002967 / Math.Tan(h + 0.00312536 / (h + 0.08901179));
        }


        private static Tuple<double, double, double> moonCoords(double d)
        { // geocentric ecliptic coordinates of the moon

            var L = rad * (218.316 + 13.176396 * d); // ecliptic longitude
            var M = rad * (134.963 + 13.064993 * d); // mean anomaly
            var F = rad * (93.272 + 13.229350 * d);  // mean distance

            var l = L + rad * 6.289 * Math.Sin(M); // longitude
            var b = rad * 5.128 * Math.Sin(F);     // latitude
            var dt = 385001 - 20905 * Math.Cos(M);  // distance to the moon in km

            return Tuple.Create(rightAscension(l, b), declination(l, b), dt); //ra,dec,dist
        }

        internal static Tuple<double, double, double, double> getMoonPosition(DateTime date, double lat, double lng)
        {

            var lw = rad * -lng;
            var phi = rad * lat;
            var d = JulianHelper.toDays(date);

            var c = moonCoords(d);
            var H = siderealTime(d, lw) - c.Item1;
            var h = altitude(H, phi, c.Item2);
            // formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            var pa = Math.Atan2(Math.Sin(H), Math.Tan(phi) * Math.Cos(c.Item2) - Math.Sin(c.Item2) * Math.Cos(H));

            h = h + astroRefraction(h); // altitude correction for refraction

            return Tuple.Create(azimuth(H, phi, c.Item2), h, c.Item3, pa);//azimuth,  altitude,  distance,   parallacticAngle


        }

        const int dayMs = 1000 * 60 * 60 * 24;
        static readonly DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        private static double DateTimeToUnixTimestamp(DateTime dateTime) => (dateTime - UnixTime).TotalMilliseconds;
        private static DateTime UnixTimestampToDateTime(double unixTime) => UnixTime + TimeSpan.FromMilliseconds(unixTime);




        private static DateTime hoursLater(DateTime date, double h)
        {
            return UnixTimestampToDateTime(DateTimeToUnixTimestamp(date) + h * dayMs / 24);
        }

        // calculations for moon rise/set times are based on http://www.stargazing.net/kepler/moonrise.html article

        internal static Period getMoonTimes(DateTime t, double lat, double lng)
        {
            //var t = new Date(date);
            //if (inUTC) t.setUTCHours(0, 0, 0, 0);
            //else t.setHours(0, 0, 0, 0);

            var hc = 0.133 * rad;
            var h0 = getMoonPosition(t, lat, lng).Item2 - hc;
            double h1 = double.NaN, h2 = double.NaN, rise = double.NaN, set = double.NaN, a = double.NaN, b = double.NaN, xe = double.NaN, ye = double.NaN, d = double.NaN, x1 = double.NaN, x2 = double.NaN, dx = double.NaN;
            int roots;

            // go in 2-hour chunks, each time seeing if a 3-point quadratic curve crosses zero (which means rise or set)
            for (var i = 1; i <= 24; i += 2)
            {
                h1 = getMoonPosition(t.AddHours(i), lat, lng).Item2 - hc;
                h2 = getMoonPosition(t.AddHours(i + 1), lat, lng).Item2 - hc;

                a = (h0 + h2) / 2 - h1;
                b = (h2 - h0) / 2;
                xe = -b / (2 * a);
                ye = (a * xe + b) * xe + h1;
                d = b * b - 4 * a * h1;
                roots = 0;

                if (d >= 0)
                {
                    dx = Math.Sqrt(d) / (Math.Abs(a) * 2);
                    x1 = xe - dx;
                    x2 = xe + dx;
                    if (Math.Abs(x1) <= 1) roots++;
                    if (Math.Abs(x2) <= 1) roots++;
                    if (x1 < -1) x1 = x2;
                }

                if (roots == 1)
                {
                    if (h0 < 0) rise = i + x1;
                    else set = i + x1;

                }
                else if (roots == 2)
                {
                    rise = i + (ye < 0 ? x2 : x1);
                    set = i + (ye < 0 ? x1 : x2);
                }

                if (!double.IsNaN(rise) && !double.IsNaN(set)) break;

                h0 = h2;
            }

            return new Period(
                !double.IsNaN(rise) ? hoursLater(t, rise) : (DateTime?)null,
                !double.IsNaN(set) ? hoursLater(t, set) : (DateTime?)null,
                (double.IsNaN(rise) && double.IsNaN(set) && ye > 0),
                (double.IsNaN(rise) && double.IsNaN(set) && ye <= 0)
                );

            //var result = { };

            //if (rise) result.rise = hoursLater(t, rise);
            //if (set) result.set = hoursLater(t, set);

            //if (!rise && !set) result[ye > 0 ? 'alwaysUp' : 'alwaysDown'] = true;

            //return result;
        }



    }
}