using System;

namespace Twilight
{
    public enum MoonPeriodTypes : byte
    {
        RiseAndSet = 0,
        RiseOnly,
        SetOnly,
        UpAllDay,
        DownAllDay,
    }


    public sealed class MoonPeriod
    {
        //internal MoonPeriod(DateTimeOffset? rise, DateTimeOffset? set, bool isAlwaysUp, bool isAlwaysDown)
        //{
        //    Rise = rise;
        //    Set = set;
        //    IsAlwaysUp = isAlwaysUp;
        //    IsAlwaysDown = isAlwaysDown;
        //}

        internal MoonPeriod(DateTimeOffset? rise, DateTimeOffset? set, MoonPeriodTypes moonPeriodType)
        {
            Rise = rise;
            Set = set;
            MoonPeriodType = moonPeriodType;
        }

        public DateTimeOffset? Rise { get; }
        public DateTimeOffset? Set { get; }

        //public bool IsAlwaysUp { get; }

        //public bool IsAlwaysDown { get; }

        public MoonPeriodTypes MoonPeriodType { get; }

    }


  


   
}