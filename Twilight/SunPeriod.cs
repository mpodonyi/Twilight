using System;

namespace Twilight
{
    public enum SunPeriodTypes : byte
    {
        RiseAndSet = 0,
        RiseOnly,
        SetOnly,
        UpAllDay,
        DownAllDay,
    }

    public sealed class SunPeriod
    {
        


        internal SunPeriod(DateTimeOffset? rise, DateTimeOffset? set, SunPeriodTypes sunPeriodType)
        {
            Rise = rise;
            Set = set;
            SunPeriodType = sunPeriodType;
            
        }

        public DateTimeOffset? Rise { get; }
        public DateTimeOffset? Set { get; }

        public SunPeriodTypes SunPeriodType { get; }
    }
}