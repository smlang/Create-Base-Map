using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class Cleanse
    {
        private static long GetPointId(Geometry.Point p)
        {
            return (((long)(p.X / Common.EPSILON)) * Int32.MaxValue) + (long)(p.Y / Common.EPSILON);
        }
    }
}
