//*********************************************************************/

// START of modified subroutines based on SUNposition
function setLatLong(f, index) {
    if (City[index].name != "Enter Lat/Long -->")  //added by AWK in order to prevent replacing existing data with zeros
    {
        //Decimal degrees are passed in the array.  Temporarily store these decimal
        // degs in lat and lon deg and have convLatLong modify them.
        f["lat_degrees"].value = City[index].lat;
        f["lon_degrees"].value = City[index].lng;
        // added by AWK
        if (f["lat_degrees"].value < 0) { calc.south.checked = true; calc.north.checked = false; }
        else { calc.north.checked = true; calc.south.checked = false; };
        if (f["lon_degrees"].value < 0) { calc.east.checked = true; calc.west.checked = false; }
        else { calc.west.checked = true; calc.east.checked = false; };
        f["lat_degrees"].value = Math.abs(f["lat_degrees"].value);
        f["lon_degrees"].value = Math.abs(f["lon_degrees"].value);
        // end of code added by AWK	

        //These are needed to prevent iterative adding of min and sec when set
        //button is clicked.
        f["lat_minutes"].value = 0;
        f["lat_secundes"].value = 0;
        f["lon_minutes"].value = 0;
        f["lon_secundes"].value = 0;

        //call convLatLong to convert decimal degrees into table form.
        convLatLong(f);

        //Local time zone value set in table
        calc.time_zone.value = City[index].zoneHr;
        // added by AWK
        var year = Now.getFullYear();
        var month = Now.getMonth();
        var day = Now.getDate();
        var hour = Now.getHours();

        if (calc.north.checked) { lat = f["lat_degrees"].value * 1 } else { lat = -f["lat_degrees"].value * 1 };
        var i = any_date_latitude_DST(lat, year, month + 1, day, 3) / -60;
        DSTfact = -i;
        if (i == 0) { calc.dstYes.checked = false; calc.dstNo.checked = true }
        else { calc.dstYes.checked = true; calc.dstNo.checked = false };
        showdate(Now);
        // end of code added by AWK
    }
}

//*********************************************************************/

//convLatLong converts any type of lat/long input
//into  the table form and then handles bad input
//it is nested in the calcSun function.

function convLatLong(f) {

    if (f["lat_degrees"].value == "") {
        f["lat_degrees"].value = 0;
    }
    if (f["lat_minutes"].value == "") {
        f["lat_minutes"].value = 0;
    }
    if (f["lat_secundes"].value == "") {
        f["lat_secundes"].value = 0;
    }
    if (f["lon_degrees"].value == "") {
        f["lon_degrees"].value = 0;
    }
    if (f["lon_minutes"].value == "") {
        f["lon_minutes"].value = 0;
    }
    if (f["lon_secundes"].value == "") {
        f["lon_secundes"].value = 0;
    }

    var neg = 0;
    if (f["lat_degrees"].value.charAt(0) == '-') {
        neg = 1;
    }

    if (neg != 1) {
        var latSeconds = (parseFloat(f["lat_degrees"].value)) * 3600 + parseFloat(f["lat_minutes"].value) * 60 + parseFloat(f["lat_secundes"].value) * 1 + 0.5;

        f["lat_degrees"].value = Math.floor(latSeconds / 3600);
        f["lat_minutes"].value = Math.floor((latSeconds - (parseFloat(f["lat_degrees"].value) * 3600)) / 60);
        f["lat_secundes"].value = Math.floor((latSeconds - (parseFloat(f["lat_degrees"].value) * 3600) - (parseFloat(f["lat_minutes"].value) * 60)) + 0.0);

    }
    else if (parseFloat(f["lat_degrees"].value) > -1) {
        var latSeconds = parseFloat(f["lat_degrees"].value) * 3600 - parseFloat(f["lat_minutes"].value) * 60 - parseFloat(f["lat_secundes"].value) * 1 + 0.5;

        f["lat_degrees"].value = "-0";
        f["lat_minutes"].value = Math.floor((-latSeconds) / 60);
        f["lat_secundes"].value = Math.floor((-latSeconds - (parseFloat(f["lat_minutes"].value) * 60)) + 0.0);

    }
    else {
        var latSeconds = parseFloat(f["lat_degrees"].value) * 3600 - parseFloat(f["lat_minutes"].value) * 60 - parseFloat(f["lat_secundes"].value) * 1 + 0.5;

        f["lat_degrees"].value = Math.ceil(latSeconds / 3600);
        f["lat_minutes"].value = Math.floor((-latSeconds + (parseFloat(f["lat_degrees"].value) * 3600)) / 60);
        f["lat_secundes"].value = Math.floor((-latSeconds + (parseFloat(f["lat_degrees"].value) * 3600) - (parseFloat(f["lat_minutes"].value) * 60)) + 0.0);
    }


    neg = 0;
    if (f["lon_degrees"].value.charAt(0) == '-') {
        neg = 1;
    }

    if (neg != 1) {
        var lonSeconds = parseFloat(f["lon_degrees"].value) * 3600 + parseFloat(f["lon_minutes"].value) * 60 + parseFloat(f["lon_secundes"].value) * 1 + 0.5;
        f["lon_degrees"].value = Math.floor(lonSeconds / 3600);
        f["lon_minutes"].value = Math.floor((lonSeconds - (parseFloat(f["lon_degrees"].value) * 3600)) / 60);
        f["lon_secundes"].value = Math.floor((lonSeconds - (parseFloat(f["lon_degrees"].value) * 3600) - (parseFloat(f["lon_minutes"].value)) * 60) + 0.0);
    }
    else if (parseFloat(f["lon_degrees"].value) > -1) {
        var lonSeconds = parseFloat(f["lon_degrees"].value) * 3600 - parseFloat(f["lon_minutes"].value) * 60 - parseFloat(f["lon_secundes"].value) * 1 + 0.5;

        f["lon_degrees"].value = "-0";
        f["lon_minutes"].value = Math.floor((-lonSeconds) / 60);
        f["lon_secundes"].value = Math.floor((-lonSeconds - (parseFloat(f["lon_minutes"].value) * 60)) + 0.0);

    }
    else {
        var lonSeconds = parseFloat(f["lon_degrees"].value) * 3600 - parseFloat(f["lon_minutes"].value) * 60 - parseFloat(f["lon_secundes"].value) * 1 + 0.5;
        f["lon_degrees"].value = Math.ceil(lonSeconds / 3600);
        f["lon_minutes"].value = Math.floor((-lonSeconds + (parseFloat(f["lon_degrees"].value) * 3600)) / 60);
        f["lon_secundes"].value = Math.floor((-lonSeconds + (parseFloat(f["lon_degrees"].value) * 3600) - (parseFloat(f["lon_minutes"].value) * 60)) + 0.0);
    }
    //Test for invalid lat/long input

    if (latSeconds > 323280) {
        alert("You have entered an invalid latitude.\nSetting lat=89.8.");
        f["lat_degrees"].value = 89.8;
        f["lat_minutes"].value = 0;
        f["lat_secundes"].value = 0;
    }
    if (latSeconds < -323280) {
        alert("You have entered an invalid latitude.\n  Setting lat= -89.8.");
        f["lat_degrees"].value = -89.8;
        f["lat_minutes"].value = 0;
        f["lat_secundes"].value = 0;
    }
    if (lonSeconds > 648000) {
        alert("You have entered an invalid longitude.\n Setting lon= 180.");
        f["lon_degrees"].value = 180;
        f["lon_minutes"].value = 0;
        f["lon_secundes"].value = 0;
    }
    if (lonSeconds < -648000) {
        alert("You have entered an invalid latitude.\n Setting lon= -180.");
        f["lon_degrees"].value = -180;
        f["lon_minutes"].value = 0;
        f["lon_secundes"].value = 0;
    }
}

// END of modified subroutines based on SUNposition
//*********************************************************************/

//*********************************************************************/
//START OF MOONRISE/MOONSET

var PI = Math.PI;
var DR = PI / 180;
var K1 = 15 * DR * 1.0027379

var Moonrise = false;
var Moonset = false;

var Rise_time = [0, 0];
var Set_time = [0, 0];
var Rise_az = 0.0;
var Set_az = 0.0;

var Sky = [0.0, 0.0, 0.0];
var RAn = [0.0, 0.0, 0.0];
var Dec = [0.0, 0.0, 0.0];
var VHz = [0.0, 0.0, 0.0];

var Now;
var dateLocal;
var DSTfact = -1;
var Cookie_name = "script_moon_rise_set";

// initialize date
function initdate() {
    Now = new Date();
    dateLocal = new Date();
    showdate(Now);
    //    load_latlon(); change by AWK
}

// display the date
function showdate(d) {
    // CORRECTIONS by Adam Wiktor Kamela
    //    var winter = new Date(2001, 11, 30);       // for northern hemisphere
    //    var summer = new Date(2001,  5, 30);

    //    var summer_tz = summer.getTimezoneOffset();
    //    var winter_tz = winter.getTimezoneOffset();
    //    var Local_dst = (summer_tz == winter_tz) ? false : true;

    //    if (summer_tz > winter_tz)                 // in southern hemisphere
    //    {
    //        var tz = summer_tz;                    // swap
    //        summer_tz = winter_tz;
    //        winter_tz = tz;
    //    }
    //    var thisday = (d.getMonth() + 1) + "/";
    //    thisday    += d.getDate() + "/";
    //    thisday    += d.getFullYear();

    //    if ((Local_dst == true)&&(summer_tz == d.getTimezoneOffset()))
    //        thisday += " [DST]";

    var date = d.getDate();
    dateLocal.setDate(date);

    var thisday = d.getDate() + " ";
    var thismonth = d.getMonth();
    var thisyear = " " + d.getFullYear();
    thismonth = monthList[thismonth].abbr;
    thisday = thisday + thismonth + thisyear;
    if (DSTfact < 0) thisday += " [DST]";
    // END of CORRECTIONS

    document.calc.thisday.value = thisday;
}

function load_latlon() {
    var latlon = getCookie(Cookie_name);

    if (!latlon) return;                       // no cookie

    s = latlon.substr(0, 8);
    lat_deg = parseInt(s);

    s = latlon.substr(8, 8);
    lat_min = parseInt(s);

    s = latlon.substr(16, 8);
    lat_sec = parseInt(s);

    s = latlon.substr(24, 8);
    lon_deg = parseInt(s);

    s = latlon.substr(32, 8);
    lon_min = parseInt(s);

    s = latlon.substr(40, 8);
    lon_sec = parseInt(s);

    if (lat_deg < 0)                           // south
    {
        calc.south.checked = true;
        calc.north.checked = false;
        lat_deg = -lat_deg;
    }
    else {
        calc.south.checked = false;
        calc.north.checked = true;
    }

    if (lon_deg < 0)                           // west
    {
        calc.west.checked = true;
        calc.east.checked = false;
        lon_deg = -lon_deg;
    }
    else {
        calc.west.checked = false;
        calc.east.checked = true;
    }

    calc.lat_degrees.value = lat_deg;
    calc.lat_minutes.value = lat_min;
    calc.lat_secundes.value = lat_sec;
    calc.lon_degrees.value = lon_deg;
    calc.lon_minutes.value = lon_min;
    calc.lon_secundes.value = lon_sec;
}

// save to cookie after checking for valid data
function save_latlon() {
    var lat_deg = parseInt(calc.lat_degrees.value, 10);
    var lat_min = parseInt(calc.lat_minutes.value, 10);
    var lat_sec = parseInt(calc.lat_secundes.value, 10);
    var lon_deg = parseInt(calc.lon_degrees.value, 10);
    var lon_min = parseInt(calc.lon_minutes.value, 10);
    var lon_sec = parseInt(calc.lon_secundes.value, 10);

    if (isNaN(lat_deg) || (lat_deg < 0) || (lat_deg >= 90) ||
        isNaN(lat_min) || (lat_min < 0) || (lat_min >= 60) ||
        isNaN(lat_sec) || (lat_sec < 0) || (lat_sec >= 60) ||
        isNaN(lon_deg) || (lon_deg < 0) || (lon_deg >= 180) ||
        isNaN(lon_min) || (lon_min < 0) || (lon_min >= 60) ||
        isNaN(lon_sec) || (lon_sec < 0) || (lon_sec >= 60)) {
        return;
    }

    if (calc.south.checked == true) lat_deg = -lat_deg;
    if (calc.west.checked == true) lon_deg = -lon_deg;
    var latlon = cintstr(lat_deg, 8) + cintstr(lat_min, 8) + cintstr(lat_sec, 8)
               + cintstr(lon_deg, 8) + cintstr(lon_min, 8) + cintstr(lon_sec, 8);

    setCookie(Cookie_name, latlon, 365);
}

// compute ...
function compute() {
    var lat_degrees = parseInt(calc.lat_degrees.value, 10);
    var lat_minutes = parseInt(calc.lat_minutes.value, 10);
    var lat_secundes = parseInt(calc.lat_secundes.value, 10);
    var lon_degrees = parseInt(calc.lon_degrees.value, 10);
    var lon_minutes = parseInt(calc.lon_minutes.value, 10);
    var lon_secundes = parseInt(calc.lon_secundes.value, 10);

    if (isNaN(lat_degrees) || (lat_degrees < 0) || (lat_degrees >= 90) ||
        isNaN(lat_minutes) || (lat_minutes < 0) || (lat_minutes >= 60) ||
        isNaN(lat_secundes) || (lat_secundes < 0) || (lat_secundes >= 60) ||
        isNaN(lon_degrees) || (lon_degrees < 0) || (lon_degrees >= 180) ||
        isNaN(lon_minutes) || (lon_minutes < 0) || (lon_minutes >= 60) ||
        isNaN(lon_secundes) || (lon_secundes < 0) || (lon_secundes >= 60)) {
        window.alert("Invalid input!");
        return;
    }

    var lat = lat_degrees + lat_minutes / 60.0 + lat_secundes / 3600.0;
    var lon = lon_degrees + lon_minutes / 60.0 + lon_secundes / 3600.0;

    if (calc.south.checked == true) lat = -lat;
    if (calc.west.checked == true) lon = -lon;

    showdate(Now);

    calc.moonrise.value = "";
    calc.moonset.value = "";
    riseset(lat, lon);
    //    save_latlon();	//changed by AWK
}

// do next day
function advance() {
    var date = Now.getDate();
    Now.setDate(date + 1);
    // added by AWK
    dateLocal.setDate(date + 1);
    var thisD = Now.getDate();
    var thisM = Now.getMonth();
    var thisY = Now.getFullYear();
    if (calc.north.checked) { lat = calc.lat_degrees.value * 1 } else { lat = -calc.lat_degrees.value * 1 };
    var i = any_date_latitude_DST(lat, thisY, thisM + 1, thisD, 3) / -60;
    DSTfact = -i;
    if (i == 0) { calc.dstYes.checked = false; calc.dstNo.checked = true }
    else { calc.dstYes.checked = true; calc.dstNo.checked = false };
    // end of code added by AWK	
    compute();
}

// do previous day
function backup() {
    var date = Now.getDate();
    Now.setDate(date - 1);
    // added by AWK
    dateLocal.setDate(date - 1);
    var thisD = Now.getDate();
    var thisM = Now.getMonth();
    var thisY = Now.getFullYear();
    if (calc.north.checked) { lat = calc.lat_degrees.value * 1 } else { lat = -calc.lat_degrees.value * 1 };
    var i = any_date_latitude_DST(lat, thisY, thisM + 1, thisD, 3) / -60;
    DSTfact = -i;
    if (i == 0) { calc.dstYes.checked = false; calc.dstNo.checked = true }
    else { calc.dstYes.checked = true; calc.dstNo.checked = false };
    // end of code added by AWK	
    compute();
}

// change N/S flags
function north_lat() {
    if (calc.south.checked == true)
        calc.south.checked = false;
}

// change N/S flags
function south_lat() {
    if (calc.north.checked == true)
        calc.north.checked = false;
}

// change E/W flags
function east_lon() {
    if (calc.west.checked == true)
        calc.west.checked = false;
}

// change E/W flags
function west_lon() {
    if (calc.east.checked == true)
        calc.east.checked = false;
}

// change DST Yes/No flags
function DST_yes() {
    if (calc.dstNo.checked == true)
    { calc.dstNo.checked = false; DSTfact = -1 };
}

// change DST Yes/No flags
function DST_no() {
    if (calc.dstYes.checked == true)
    { calc.dstYes.checked = false; DSTfact = 0 };
}


// calculate moonrise and moonset times
function riseset(lat, lon) {
    var i, j, k;
    // corrections by Adam Wiktor Kamela
    //    var zone = Math.round(Now.getTimezoneOffset()/60);
    var zone = parseFloat(document.calc.time_zone.value * 1.0);
    // end of corrections by AWK	
    var jdlp = julian_day();			// stored for Lunar Phase calculation
    var jd = jdlp - 2451545;           // Julian day relative to Jan 1.5, 2000

    if ((sgn(zone) == sgn(lon)) && (zone != 0))
        window.alert("WARNING: time zone and longitude are incompatible! \nThis is warning only - calculations will be performed anyway." + "\n" + "Time zone=" + zone + "   sgn(Time zone)=" + sgn(zone) + "   sgn(longitude)=" + sgn(lon));
    var zone = parseFloat(document.calc.time_zone.value * 1.0) + DSTfact;

    var mp = new Array(3);                     // create a 3x3 array
    for (i = 0; i < 3; i++) {
        mp[i] = new Array(3);
        for (j = 0; j < 3; j++)
            mp[i][j] = 0.0;
    }

    lon = lon / 360;
    var tz = zone / 24;
    var t0 = lst(lon, jd, tz);                 // local sidereal time

    jd = jd + tz;                              // get moon position at start of day

    for (k = 0; k < 3; k++) {
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
        ph = (k + 1) / 24;

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

// Local Sidereal Time for zone
function lst(lon, jd, z) {
    var s = 24110.5 + 8640184.812999999 * jd / 36525 + 86636.6 * z + 86400 * lon;
    s = s / 86400;
    s = s - Math.floor(s);
    return s * 360 * DR;
}

// 3-point interpolation
function interpolate(f0, f1, f2, p) {
    var a = f1 - f0;
    var b = f2 - f1 - a;
    var f = f0 + p * (2 * a + b * (2 * p - 1));

    return f;
}

// test an hour for an event
function test_moon(k, zone, t0, lat, plx) {
    var ha = [0.0, 0.0, 0.0];
    var a, b, c, d, e, s, z;
    var hr, min, time;
    var az, hz, nz, dz;

    if (RAn[2] < RAn[0])
        RAn[2] = RAn[2] + 2 * PI;

    ha[0] = t0 - RAn[0] + k * K1;
    ha[2] = t0 - RAn[2] + k * K1 + K1;

    ha[1] = (ha[2] + ha[0]) / 2;                // hour angle at half hour
    Dec[1] = (Dec[2] + Dec[0]) / 2;              // declination at half hour

    s = Math.sin(DR * lat);
    c = Math.cos(DR * lat);

    // refraction + sun semidiameter at horizon + parallax correction
    z = Math.cos(DR * (90.567 - 41.685 / plx));

    if (k <= 0)                                // first call of function
        VHz[0] = s * Math.sin(Dec[0]) + c * Math.cos(Dec[0]) * Math.cos(ha[0]) - z;

    VHz[2] = s * Math.sin(Dec[2]) + c * Math.cos(Dec[2]) * Math.cos(ha[2]) - z;

    if (sgn(VHz[0]) == sgn(VHz[2]))
        return VHz[2];                         // no event this hour

    VHz[1] = s * Math.sin(Dec[1]) + c * Math.cos(Dec[1]) * Math.cos(ha[1]) - z;

    a = 2 * VHz[2] - 4 * VHz[1] + 2 * VHz[0];
    b = 4 * VHz[1] - 3 * VHz[0] - VHz[2];
    d = b * b - 4 * a * VHz[0];

    if (d < 0)
        return VHz[2];                         // no event this hour

    d = Math.sqrt(d);
    e = (-b + d) / (2 * a);

    if ((e > 1) || (e < 0))
        e = (-b - d) / (2 * a);

    time = k + e + 1 / 120;                      // time of an event + round up
    hr = Math.floor(time);
    min = Math.floor((time - hr) * 60);

    hz = ha[0] + e * (ha[2] - ha[0]);            // azimuth of the moon at the event
    nz = -Math.cos(Dec[1]) * Math.sin(hz);
    dz = c * Math.sin(Dec[1]) - s * Math.cos(Dec[1]) * Math.cos(hz);
    az = Math.atan2(nz, dz) / DR;
    if (az < 0) az = az + 360;

    if ((VHz[0] < 0) && (VHz[2] > 0)) {
        Rise_time[0] = hr;
        Rise_time[1] = min;
        Rise_az = az;
        Moonrise = true;
    }

    if ((VHz[0] > 0) && (VHz[2] < 0)) {
        Set_time[0] = hr;
        Set_time[1] = min;
        Set_az = az;
        Moonset = true;
    }

    return VHz[2];
}

// test an hour for an event
function test_sun(k, zone, t0, lat) {
    var ha = new Array(3);
    var a, b, c, d, e, s, z;
    var hr, min, time;
    var az, dz, hz, nz;

    ha[0] = t0 - RAn[0] + k * K1;
    ha[2] = t0 - RAn[2] + k * K1 + K1;

    ha[1] = (ha[2] + ha[0]) / 2;               // hour angle at half hour
    Dec[1] = (Dec[2] + Dec[0]) / 2;             // declination at half hour

    s = Math.sin(lat * DR);
    c = Math.cos(lat * DR);
    z = Math.cos(90.833 * DR);                   // refraction + sun semidiameter at horizon

    if (k <= 0)
        VHz[0] = s * Math.sin(Dec[0]) + c * Math.cos(Dec[0]) * Math.cos(ha[0]) - z;

    VHz[2] = s * Math.sin(Dec[2]) + c * Math.cos(Dec[2]) * Math.cos(ha[2]) - z;

    if (sgn(VHz[0]) == sgn(VHz[2]))
        return VHz[2];                         // no event this hour

    VHz[1] = s * Math.sin(Dec[1]) + c * Math.cos(Dec[1]) * Math.cos(ha[1]) - z;

    a = 2 * VHz[0] - 4 * VHz[1] + 2 * VHz[2];
    b = -3 * VHz[0] + 4 * VHz[1] - VHz[2];
    d = b * b - 4 * a * VHz[0];

    if (d < 0)
        return VHz[2];                         // no event this hour

    d = Math.sqrt(d);
    e = (-b + d) / (2 * a);

    if ((e > 1) || (e < 0))
        e = (-b - d) / (2 * a);

    time = k + e + 1 / 120;                      // time of an event

    hr = Math.floor(time);
    min = Math.floor((time - hr) * 60);

    hz = ha[0] + e * (ha[2] - ha[0]);            // azimuth of the sun at the event
    nz = -Math.cos(Dec[1]) * Math.sin(hz);
    dz = c * Math.sin(Dec[1]) - s * Math.cos(Dec[1]) * Math.cos(hz);
    az = Math.atan2(nz, dz) / DR;
    if (az < 0) az = az + 360;

    if ((VHz[0] < 0) && (VHz[2] > 0)) {
        Rise_time[0] = hr;
        Rise_time[1] = min;
        Rise_az = az;
        Moonrise = true;
    }

    if ((VHz[0] > 0) && (VHz[2] < 0)) {
        Set_time[0] = hr;
        Set_time[1] = min;
        Set_az = az;
        Moonset = true;
    }

    return VHz[2];
}

// check for no moonrise and/or no moonset
function special_message() {
    if ((!Moonrise) && (!Moonset))               // neither moonrise nor moonset
    {
        if (VHz[2] < 0)
            calc.moonrise.value = "Moon down all day";
        else
            calc.moonrise.value = "Moon up all day";

        calc.moonset.value = "";
    }
    else                                       // moonrise or moonset
    {
        if (!Moonrise)
            calc.moonrise.value = "No moonrise this date";
        else if (!Moonset)
            calc.moonset.value = "No moonset this date";
    }
}

// moon's position using fundamental arguments 
// (Van Flandern & Pulkkinen, 1979)
function moon(jd) {
    var d, f, g, h, m, n, s, u, v, w;

    h = 0.606434 + 0.03660110129 * jd;
    m = 0.374897 + 0.03629164709 * jd;
    f = 0.259091 + 0.0367481952 * jd;
    d = 0.827362 + 0.03386319198 * jd;
    n = 0.347343 - 0.00014709391 * jd;
    g = 0.993126 + 0.0027377785 * jd;

    h = h - Math.floor(h);
    m = m - Math.floor(m);
    f = f - Math.floor(f);
    d = d - Math.floor(d);
    n = n - Math.floor(n);
    g = g - Math.floor(g);

    h = h * 2 * PI;
    m = m * 2 * PI;
    f = f * 2 * PI;
    d = d * 2 * PI;
    n = n * 2 * PI;
    g = g * 2 * PI;

    v = 0.39558 * Math.sin(f + n);
    v = v + 0.082 * Math.sin(f);
    v = v + 0.03257 * Math.sin(m - f - n);
    v = v + 0.01092 * Math.sin(m + f + n);
    v = v + 0.00666 * Math.sin(m - f);
    v = v - 0.00644 * Math.sin(m + f - 2 * d + n);
    v = v - 0.00331 * Math.sin(f - 2 * d + n);
    v = v - 0.00304 * Math.sin(f - 2 * d);
    v = v - 0.0024 * Math.sin(m - f - 2 * d - n);
    v = v + 0.00226 * Math.sin(m + f);
    v = v - 0.00108 * Math.sin(m + f - 2 * d);
    v = v - 0.00079 * Math.sin(f - n);
    v = v + 0.00078 * Math.sin(f + 2 * d + n);

    u = 1 - 0.10828 * Math.cos(m);
    u = u - 0.0188 * Math.cos(m - 2 * d);
    u = u - 0.01479 * Math.cos(2 * d);
    u = u + 0.00181 * Math.cos(2 * m - 2 * d);
    u = u - 0.00147 * Math.cos(2 * m);
    u = u - 0.00105 * Math.cos(2 * d - g);
    u = u - 0.00075 * Math.cos(m - 2 * d + g);

    w = 0.10478 * Math.sin(m);
    w = w - 0.04105 * Math.sin(2 * f + 2 * n);
    w = w - 0.0213 * Math.sin(m - 2 * d);
    w = w - 0.01779 * Math.sin(2 * f + n);
    w = w + 0.01774 * Math.sin(n);
    w = w + 0.00987 * Math.sin(2 * d);
    w = w - 0.00338 * Math.sin(m - 2 * f - 2 * n);
    w = w - 0.00309 * Math.sin(g);
    w = w - 0.0019 * Math.sin(2 * f);
    w = w - 0.00144 * Math.sin(m + n);
    w = w - 0.00144 * Math.sin(m - 2 * f - n);
    w = w - 0.00113 * Math.sin(m + 2 * f + 2 * n);
    w = w - 0.00094 * Math.sin(m - 2 * d + g);
    w = w - 0.00092 * Math.sin(2 * m - 2 * d);

    s = w / Math.sqrt(u - v * v);                  // compute moon's right ascension ...  
    Sky[0] = h + Math.atan(s / Math.sqrt(1 - s * s));

    s = v / Math.sqrt(u);                        // declination ...
    Sky[1] = Math.atan(s / Math.sqrt(1 - s * s));

    Sky[2] = 60.40974 * Math.sqrt(u);          // and parallax
}

// determine Julian day from calendar date
// (Jean Meeus, "Astronomical Algorithms", Willmann-Bell, 1991)
function julian_day()							// be carefull, the function of the similare name (Julian_Day) is used in astroAWK1.js library
    // current function uses date in the form of "date object", while that other function uses three arguments of calendar date
{
    var a, b, jd;
    var gregorian;

    
    var month = Now.getMonth() + 1;
    var day = Now.getDate();
    var year = Now.getFullYear();

    gregorian = (year < 1583) ? false : true;

    if ((month == 1) || (month == 2)) {
        year = year - 1;
        month = month + 12;
    }

    a = Math.floor(year / 100);
    if (gregorian) b = 2 - a + Math.floor(a / 4);
    else b = 0.0;

    jd = Math.floor(365.25 * (year + 4716))
       + Math.floor(30.6001 * (month + 1))
       + day + b - 1524.5;

    return jd;
}

// returns value for sign of argument
function sgn(x) {
    var rv;
    if (x > 0.0) rv = 1;
    else if (x < 0.0) rv = -1;
    else rv = 0;
    return rv;
}

// format a positive integer with leading zeroes
function zintstr(num, width) {
    var str = num.toString(10);
    var len = str.length;
    var intgr = "";
    var i;

    for (i = 0; i < width - len; i++)          // append leading zeroes
        intgr += '0';

    for (i = 0; i < len; i++)                  // append digits
        intgr += str.charAt(i);

    return intgr;
}

// format an integer
function cintstr(num, width) {
    var str = num.toString(10);
    var len = str.length;
    var intgr = "";
    var i;

    for (i = 0; i < width - len; i++)          // append leading spaces
        intgr += ' ';

    for (i = 0; i < len; i++)                  // append digits
        intgr += str.charAt(i);

    return intgr;
}

// format a real number
function frealstr(num, width, fract) {
    var str = num.toFixed(fract);
    var len = str.length;
    var real = "";
    var i;

    for (i = 0; i < width - len; i++)          // append leading spaces
        real += ' ';

    for (i = 0; i < len; i++)                  // append digits
        real += str.charAt(i);

    return real;
}

// read data from cookie
function getCookie(name) {
    if (document.cookie.length > 0) {
        begin = document.cookie.indexOf(name + "=");
        if (begin != -1) // Note: != means "is not equal to"
        {
            begin += name.length + 1;
            end = document.cookie.indexOf(";", begin);
            if (end == -1) end = document.cookie.length;

            return unescape(document.cookie.substring(begin, end));
        }
    }
    return null;
}

// write data to cookie
function setCookie(name, value, expiredays) {
    var ExpireDate = new Date();
    ExpireDate.setTime(ExpireDate.getTime() + (expiredays * 24 * 3600 * 1000));

    document.cookie = name
                    + "=" + escape(value)
                    + ((expiredays == null) ? "" : "; expires=" + ExpireDate.toGMTString());
}
//END OF MOONRISE/MOONSET
//*********************************************************************/
