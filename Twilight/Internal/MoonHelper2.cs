using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Twilight.Internal
{
    class MoonHelper2
    {
        // determine Julian day from calendar date
        // (Jean Meeus, "Astronomical Algorithms", Willmann-Bell, 1991)
        private static double julian_day()                           // be carefull, the function of the similare name (Julian_Day) is used in astroAWK1.js library
                                                        // current function uses date in the form of "date object", while that other function uses three arguments of calendar date
        {
            double a, b, jd;
            bool gregorian;
            var Now = DateTimeOffset.Now;

            var month = Now.Month + 1;
            var day = Now.Day;
            double year = Now.Year;

            gregorian = (year < 1583) ? false : true;

            if ((month == 1) || (month == 2))
            {
                year = year - 1;
                month = month + 12;
            }

            a = Floor(year / 100);
            if (gregorian) b = 2 - a + Floor(a / 4);
            else b = 0.0;

            jd = Floor(365.25 * (year + 4716))
               + Floor(30.6001 * (month + 1))
               + day + b - 1524.5;

            return jd;
        }

        // returns value for sign of argument
        private static sbyte sgn(double x)
        {
            sbyte rv;
            if (x > 0.0) rv = 1;
            else if (x < 0.0) rv = -1;
            else rv = 0;
            return rv;
        }

        private static double[] Sky = {0.0, 0.0, 0.0};
        private static double[] RAn = { 0.0, 0.0, 0.0 };
        private static double[] Dec = { 0.0, 0.0, 0.0 };
        private static double[] VHz = { 0.0, 0.0, 0.0 };



        // moon's position using fundamental arguments 
        // (Van Flandern & Pulkkinen, 1979)
        private static void moon(double jd)
        {
            double d, f, g, h, m, n, s, u, v, w;

            h = 0.606434 + 0.03660110129 * jd;
            m = 0.374897 + 0.03629164709 * jd;
            f = 0.259091 + 0.0367481952 * jd;
            d = 0.827362 + 0.03386319198 * jd;
            n = 0.347343 - 0.00014709391 * jd;
            g = 0.993126 + 0.0027377785 * jd;

            h = h - Floor(h);
            m = m - Floor(m);
            f = f - Floor(f);
            d = d - Floor(d);
            n = n - Floor(n);
            g = g - Floor(g);

            h = h * 2 * PI;
            m = m * 2 * PI;
            f = f * 2 * PI;
            d = d * 2 * PI;
            n = n * 2 * PI;
            g = g * 2 * PI;

            v = 0.39558 * Sin(f + n);
            v = v + 0.082 * Sin(f);
            v = v + 0.03257 * Sin(m - f - n);
            v = v + 0.01092 * Sin(m + f + n);
            v = v + 0.00666 * Sin(m - f);
            v = v - 0.00644 * Sin(m + f - 2 * d + n);
            v = v - 0.00331 * Sin(f - 2 * d + n);
            v = v - 0.00304 * Sin(f - 2 * d);
            v = v - 0.0024 * Sin(m - f - 2 * d - n);
            v = v + 0.00226 * Sin(m + f);
            v = v - 0.00108 * Sin(m + f - 2 * d);
            v = v - 0.00079 * Sin(f - n);
            v = v + 0.00078 * Sin(f + 2 * d + n);

            u = 1 - 0.10828 * Cos(m);
            u = u - 0.0188 * Cos(m - 2 * d);
            u = u - 0.01479 * Cos(2 * d);
            u = u + 0.00181 * Cos(2 * m - 2 * d);
            u = u - 0.00147 * Cos(2 * m);
            u = u - 0.00105 * Cos(2 * d - g);
            u = u - 0.00075 * Cos(m - 2 * d + g);

            w = 0.10478 * Sin(m);
            w = w - 0.04105 * Sin(2 * f + 2 * n);
            w = w - 0.0213 * Sin(m - 2 * d);
            w = w - 0.01779 * Sin(2 * f + n);
            w = w + 0.01774 * Sin(n);
            w = w + 0.00987 * Sin(2 * d);
            w = w - 0.00338 * Sin(m - 2 * f - 2 * n);
            w = w - 0.00309 * Sin(g);
            w = w - 0.0019 * Sin(2 * f);
            w = w - 0.00144 * Sin(m + n);
            w = w - 0.00144 * Sin(m - 2 * f - n);
            w = w - 0.00113 * Sin(m + 2 * f + 2 * n);
            w = w - 0.00094 * Sin(m - 2 * d + g);
            w = w - 0.00092 * Sin(2 * m - 2 * d);

            s = w / Sqrt(u - v * v);                  // compute moon's right ascension ...  
            Sky[0] = h + Atan(s / Sqrt(1 - s * s));

            s = v / Sqrt(u);                        // declination ...
            Sky[1] = Atan(s / Sqrt(1 - s * s));

            Sky[2] = 60.40974 * Sqrt(u);          // and parallax
        }



        private const double DR = PI / 180;
        private const double K1 = 15*DR*1.0027379;
        private static double[] Rise_time = { 0, 0};
        private static double[] Set_time = { 0, 0};
        private static double Rise_az = 0.0;
        private static double Set_az = 0.0;

        // Local Sidereal Time for zone
        private static double lst(double lon, double jd, double z)
        {
            var s = 24110.5 + 8640184.812999999 * jd / 36525 + 86636.6 * z + 86400 * lon;
            s = s / 86400;
            s = s - Floor(s);
            return s * 360 * DR;
        }

        private static int DSTfact = -1;

        // 3-point interpolation
        private static double interpolate(double f0, double f1, double f2, double p)
        {
            var a = f1 - f0;
            var b = f2 - f1 - a;
            var f = f0 + p * (2 * a + b * (2 * p - 1));

            return f;
        }

        // test an hour for an event
        private static double test_moon(int k, double zone, double t0, double lat, double plx)
        {
            double[] ha = { 0.0, 0.0, 0.0};
            double a, b, c, d, e, s, z;
            double hr, min, time;
            double az, hz, nz, dz;

            if (RAn[2] < RAn[0])
                RAn[2] = RAn[2] + 2 * PI;

            ha[0] = t0 - RAn[0] + k * K1;
            ha[2] = t0 - RAn[2] + k * K1 + K1;

            ha[1] = (ha[2] + ha[0]) / 2;                // hour angle at half hour
            Dec[1] = (Dec[2] + Dec[0]) / 2;              // declination at half hour

            s = Sin(DR * lat);
            c = Cos(DR * lat);

            // refraction + sun semidiameter at horizon + parallax correction
            z = Cos(DR * (90.567 - 41.685 / plx));

            if (k <= 0)                                // first call of function
                VHz[0] = s * Sin(Dec[0]) + c * Cos(Dec[0]) * Cos(ha[0]) - z;

            VHz[2] = s * Sin(Dec[2]) + c * Cos(Dec[2]) * Cos(ha[2]) - z;

            if (sgn(VHz[0]) == sgn(VHz[2]))
                return VHz[2];                         // no event this hour

            VHz[1] = s * Sin(Dec[1]) + c * Cos(Dec[1]) * Cos(ha[1]) - z;

            a = 2 * VHz[2] - 4 * VHz[1] + 2 * VHz[0];
            b = 4 * VHz[1] - 3 * VHz[0] - VHz[2];
            d = b * b - 4 * a * VHz[0];

            if (d < 0)
                return VHz[2];                         // no event this hour

            d = Sqrt(d);
            e = (-b + d) / (2 * a);

            if ((e > 1) || (e < 0))
                e = (-b - d) / (2 * a);

            time = k + e + 1 / 120;                      // time of an event + round up
            hr = Floor(time);
            min = Floor((time - hr) * 60);

            hz = ha[0] + e * (ha[2] - ha[0]);            // azimuth of the moon at the event
            nz = -Cos(Dec[1]) * Sin(hz);
            dz = c * Sin(Dec[1]) - s * Cos(Dec[1]) * Cos(hz);
            az = Atan2(nz, dz) / DR;
            if (az < 0) az = az + 360;

            if ((VHz[0] < 0) && (VHz[2] > 0))
            {
                Rise_time[0] = hr;
                Rise_time[1] = min;
                Rise_az = az;
                Moonrise = true;
            }

            if ((VHz[0] > 0) && (VHz[2] < 0))
            {
                Set_time[0] = hr;
                Set_time[1] = min;
                Set_az = az;
                Moonset = true;
            }

            return VHz[2];
        }

        private static bool Moonrise ;                          // initialize
        private static bool Moonset ;


        // calculate moonrise and moonset times
        private static void riseset(double lat,double lon, DateTimeOffset offset)
        {
            int k;
            int i, j;
            // corrections by Adam Wiktor Kamela
            //    var zone = Math.round(Now.getTimezoneOffset()/60);

            //MP: offset here
            double zone = 1.0; //parseFloat(document.calc.time_zone.value * 1.0);
            // end of corrections by AWK	
            var jdlp = julian_day();            // stored for Lunar Phase calculation
            var jd = jdlp - 2451545;           // Julian day relative to Jan 1.5, 2000

            if ((sgn(zone) == sgn(lon)) && (zone != 0))
            {
                //MP: warning maybe
                //window.alert("WARNING: time zone and longitude are incompatible! \nThis is warning only - calculations will be performed anyway." + "\n" + "Time zone=" + zone + "   sgn(Time zone)=" + sgn(zone) + "   sgn(longitude)=" + sgn(lon));
            }


            //MP: offset here
            zone = 1.0 + DSTfact; //parseFloat(document.calc.time_zone.value * 1.0) + DSTfact;

            var mp = new double[3][];                     // create a 3x3 array
            for (i = 0; i < 3; i++)
            {
                mp[i] = new double[3];
                for (j = 0; j < 3; j++)
                    mp[i][j] = 0.0;
            }

            lon = lon / 360;
            var tz = zone / 24;
            var t0 = lst(lon, jd, tz);                 // local sidereal time

            jd = jd + tz;                              // get moon position at start of day

            for (k = 0; k < 3; k++)
            {
                moon(jd);
                mp[k][0] = Sky[0];
                mp[k][1] = Sky[1];
                mp[k][2] = Sky[2];
                jd = jd + 0.5;
            }

            if (mp[1][0] <= mp[0][0])
                mp[1][0] = mp[1][0] + 2 * PI;

            if (mp[2][0] <= mp[1][0])
                mp[2][0] = mp[2][0] + 2 * PI;

            RAn[0] = mp[0][0];
            Dec[0] = mp[0][1];

            Moonrise = false;                          // initialize
            Moonset = false;

            for (k = 0; k < 24; k++)                   // check each hour of this day
            {
                double ph = (k + 1) / 24;

                RAn[2] = interpolate(mp[0][0], mp[1][0], mp[2][0], ph);
                Dec[2] = interpolate(mp[0][1], mp[1][1], mp[2][1], ph);

                VHz[2] = test_moon(k, zone, t0, lat, mp[1][2]);

                RAn[0] = RAn[2];                       // advance to next hour
                Dec[0] = Dec[2];
                VHz[0] = VHz[2];
            }

            // display results
            // extensions and changes by AWK
            calc.moonrise.value = zintstr(Rise_time[0], 2) + ":" + zintstr(Rise_time[1], 2);
            calc.moonset.value = zintstr(Set_time[0], 2) + ":" + zintstr(Set_time[1], 2);

            var zoneInt = parseFloat(calc.time_zone.value * 1.0);
            var dstInt = DSTfact;
            var timeDiff = zoneInt + dstInt;
            var timeDiffHours = Math.floor(timeDiff);
            var timeDiffMinutes = (timeDiff - timeDiffHours) * 60;
            dateLocal.setHours(Rise_time[0] + timeDiffHours, Rise_time[1] + timeDiffMinutes, 0);
            var DayMonth = " ";
            if (Now.getDate() != dateLocal.getDate()) { DayMonth = " /" + dateLocal.getDate() + " " + monthList[dateLocal.getMonth()].abbr + "/" };
            if (Moonrise) { calc.moonriseUTC.value = zintstr(dateLocal.getHours(), 2) + ":" + zintstr(dateLocal.getMinutes(), 2) + DayMonth } else { calc.moonriseUTC.value = " " };

            LunarCyclePhasesData(dateLocal);
            MoonriseSec = ElapsedTime(firstNewMoon, dateLocal);
            if (Moonrise) { calc.LunPhasRise.value = curMoonPhase(MoonriseSec) } else { calc.LunPhasRise.value = " " };

            var yNow = Now.getFullYear();
            var mNow = Now.getMonth();
            var dNow = Now.getDate();
            dateLocal.setFullYear(yNow, mNow, dNow);
            dateLocal.setHours(Set_time[0] + timeDiffHours, Set_time[1] + timeDiffMinutes, 0);
            var DayMonth = " ";
            if (Now.getDate() != dateLocal.getDate()) { DayMonth = " /" + dateLocal.getDate() + " " + monthList[dateLocal.getMonth()].abbr + "/" };
            if (Moonset) { calc.moonsetUTC.value = zintstr(dateLocal.getHours(), 2) + ":" + zintstr(dateLocal.getMinutes(), 2) + DayMonth } else { calc.moonsetUTC.value = " " };
            LunarCyclePhasesData(dateLocal);
            MoonsetSec = ElapsedTime(firstNewMoon, dateLocal);
            if (Moonset) { calc.LunPhasSet.value = curMoonPhase(MoonsetSec) } else { calc.LunPhasSet.value = " " };

            if (Moonrise) { calc.azRise.value = frealstr(Rise_az, 5, 1) + "°" } else { calc.azRise.value = " " };
            if (Moonset) { calc.azSet.value = frealstr(Set_az, 5, 1) + "°" } else { calc.azSet.value = " " };
            // END of extensions and changes by AWK
            special_message();
        }



        internal static void compute(double lat, double lon, DateTimeOffset date)
        {
            




                  //  showdate(Now);

                    riseset(lat, lon, date);
                    //    save_latlon();	//changed by AWK
        }



    }
}
