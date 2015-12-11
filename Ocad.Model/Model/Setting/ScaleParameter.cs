using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class ScaleParameter
    {
        [VersionsSupported(V9 = true)]
        public Decimal? RealWorldAngle { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance RealWorldGridDistance { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance PaperGridDistance { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.CoordinateSystemType? RealWorldCoordinateSystem { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal MapScale { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean UseRealWorldGrid { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance RealWorldOffsetX { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance RealWorldOffsetY { get; set; }

        public ScaleParameter()
        {
            MapScale = 15000;
            UseRealWorldGrid = false;
            PaperGridDistance = new Distance(66.67M, Distance.Unit.Metre, Scale.milli);
        }
    }
}
