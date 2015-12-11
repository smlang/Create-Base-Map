using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class OimFind
    {
        [VersionsSupported(V9 = true)]
        public String Name { get; set; }
        [VersionsSupported(V9 = true)]
        public String Condition { get; set; }
        [VersionsSupported(V9 = true)]
        public String DataSet { get; set; }
        [VersionsSupported(V9 = true)]
        public String FromZoom { get; set; }
        [VersionsSupported(V9 = true)]
        public String HintField { get; set; }
        [VersionsSupported(V9 = true)]
        public String NameField { get; set; }
        [VersionsSupported(V9 = true)]
        public String ListNames { get; set; }
        [VersionsSupported(V9 = true)]
        public String HotspotType { get; set; }
        [VersionsSupported(V9 = true)]
        public String PointerType { get; set; }
        [VersionsSupported(V9 = true)]
        public String ShowHotspots { get; set; }
        [VersionsSupported(V9 = true)]
        public String ToZoom { get; set; }
        [VersionsSupported(V9 = true)]
        public String UrlField { get; set; }
        [VersionsSupported(V9 = true)]
        public String Prefix { get; set; }
        [VersionsSupported(V9 = true)]
        public String Postfix { get; set; }
        [VersionsSupported(V9 = true)]
        public String Target { get; set; }
        [VersionsSupported(V9 = true)]
        public String PointerColourRed { get; set; }
        [VersionsSupported(V9 = true)]
        public String PointerColourGreen { get; set; }
        [VersionsSupported(V9 = true)]
        public String PointerColourBlue { get; set; }
        [VersionsSupported(V9 = true)]
        public String HotspotColourRed { get; set; }
        [VersionsSupported(V9 = true)]
        public String HotspotColourGreen { get; set; }
        [VersionsSupported(V9 = true)]
        public String HotspotColourBlue { get; set; }
    }
}
