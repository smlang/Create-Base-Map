using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class Shape
    {
        [VersionsSupported(V9 = true)]
        public Type.ShapeType Type { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.LineStyle Style { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour Colour { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance LineWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance Diameter { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Point> Points { get; set; }

        internal Shape()
        {
            Points = new List<Point>();
        }
    }
}
