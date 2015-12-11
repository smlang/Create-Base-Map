using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class ExportParameter
    {
        [VersionsSupported(V9 = true)]
        public String Format { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? AntiAliasing { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? SpotColoursCombined { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.ColourFormat? ColourFormat { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? ColourCorrection { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? SpotColours { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? PartialMap { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16? Resolution { get; set; }
        [VersionsSupported(V9 = true)]
        public String Scale { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? Tiles { get; set; }
        [VersionsSupported(V9 = true)]
        public String ZParameter { get; set; }
    }
}
