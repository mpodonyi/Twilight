using System;

namespace Twilight
{
    public sealed class SunPeriod
    {
        internal SunPeriod(DateTimeOffset? rise, DateTimeOffset? set, bool isAlwaysUp, bool isAlwaysDown)
        {
            Rise = rise;
            Set = set;
            IsAlwaysUp = isAlwaysUp;
            IsAlwaysDown = isAlwaysDown;
        }

        public DateTimeOffset? Rise { get; }
        public DateTimeOffset? Set { get; }

        public bool IsAlwaysUp { get; }

        public bool IsAlwaysDown { get; }
    }
}