using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class ViewParameter
    {
        [VersionsSupported(V9 = true)]
        public Byte DraftModeIgnForOcadMap { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte DraftModeIgnForBackgroundMaps { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean HideBackgroundMaps { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte DraftModeForOcadMap { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte DraftModeForBackgroundMaps { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte ViewMode { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance OffsetCentreX { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance OffsetCentreY { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Zoom { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean Hatched { get; set; }
    }
}
