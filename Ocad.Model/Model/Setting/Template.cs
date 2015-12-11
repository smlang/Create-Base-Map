using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class Template
    {
        [VersionsSupported(V9 = true)]
        public String FileName { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal OmegaAngle { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal PhiAngle { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte? Dim { get; set; }
        [VersionsSupported(V9 = true)]
        public String RenderWithSpotColour { get; set; }
        [VersionsSupported(V9 = true)]
        public String AssignedToSpotColour { get; set; }
        [VersionsSupported(V9 = true)]
        public String SubtractFromSpotColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean VisibleInDraftMode { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean VisibleInNormalMode { get; set; }
        [VersionsSupported(V9 = true)]
        public String Transparent { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance OffsetCentreX { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance OffsetCentreY { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance PixelSizeX { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance PixelSizeY { get; set; }
    }
}
