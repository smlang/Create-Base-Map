using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class PrintParameter
    {
        [VersionsSupported(V9 = true)]
        public Decimal PrintScale { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? Landscape { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? PrintSpotSeparation { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean PrintGrid { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour GridColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte Intensity { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte WidthForLinesAndDotsPercentage { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte? Range { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance PartialMapLeft { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance PartialMapBottom { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance PartialMapRight { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance PartialMapTop { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance HorizontalOverlap { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance VerticalOverlap { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? PrintBlack { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? PrintMirror { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal? HorizontalScale { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal? VerticalScale { get; set; }
    }
}
