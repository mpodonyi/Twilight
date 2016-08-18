using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twilight.Internal
{
    internal static class ThrowHelper
    {
        internal static void CheckLat(double lat)
        {
            if (lat < -85.0)
                throw new ArgumentOutOfRangeException(nameof(lat));

            if (lat > 85.0)
                throw new ArgumentOutOfRangeException(nameof(lat));
        }

        internal static void CheckLng(double lng)
        {
            if (lng < -180.0)
                throw new ArgumentOutOfRangeException(nameof(lng));

            if (lng > 180.0)
                throw new ArgumentOutOfRangeException(nameof(lng));
        }
    }
}
