using System;

namespace Twilight
{
    public sealed class MoonPeriod
    {
        internal MoonPeriod(DateTimeOffset? rise, DateTimeOffset? set, bool isAlwaysUp, bool isAlwaysDown)
        {
            Rise = rise;
            Set = set;
            IsAlwaysUp = isAlwaysUp;
            IsAlwaysDown = isAlwaysDown;
        }

        internal MoonPeriod(DateTimeOffset? rise, DateTimeOffset? set, MoonPeriod2Type moonPeriodToday)
        {
            Rise = rise;
            Set = set;
            MoonPeriodToday = moonPeriodToday;
        }

        public DateTimeOffset? Rise { get; }
        public DateTimeOffset? Set { get; }

        public bool IsAlwaysUp { get; }

        public bool IsAlwaysDown { get; }

        public MoonPeriod2Type MoonPeriodToday { get; }

    }


    public enum MoonPeriod2Type : byte
    {
        RiseAndSet,
        RiseOnly,
        SetOnly,
        UpAllDay,
        DownAllDay,
    }

   
}