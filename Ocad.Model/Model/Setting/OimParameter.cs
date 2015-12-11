using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class OimParameter
    {
        [VersionsSupported(V9 = true)]
        public String FileName { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? AntiAliasing { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32 BorderWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean CompressedSvg { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean ExternalScripting { get; set; }
        [VersionsSupported(V9 = true)]
        public String FindLabel { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32 Height { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32 OverviewHeight { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean DoNotCreateFiles { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal ZoomRange { get; set; }
        [VersionsSupported(V9 = true)]
        public String SelectLabel { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32 OverviewWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32 Width { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte ZoomLevels { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte FillColourRed { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte FillColourGreen { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte FillColourBlue { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte OverviewMapNumber { get; set; }
    }
}
