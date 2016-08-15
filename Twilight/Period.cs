using System;

namespace Twilight
{
    public class Period
    {
        internal Period(DateTime? rise, DateTime? set, bool isAlwaysUp, bool isAlwaysDown)
        {
            Rise = rise;
            Set = set;
            IsAlwaysUp = isAlwaysUp;
            IsAlwaysDown = isAlwaysDown;
        }

        public DateTime? Rise { get; }
        public DateTime? Set { get; }

        public bool IsAlwaysUp { get; }

        public bool IsAlwaysDown { get; }

    }
}