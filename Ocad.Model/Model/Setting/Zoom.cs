using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class Zoom
    {
        [VersionsSupported(V9 = true)]
        public Distance OffsetCentreX { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance OffsetCentreY { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Depth { get; set; }
    }
}
