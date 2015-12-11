using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class SpotColour
    {
        [VersionsSupported(V9 = true)]
        public Boolean Visible { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32 Number { get; set; }
        [VersionsSupported(V9 = true)]
        public String Name { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal FrequencyLpi { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal HalftoneAngle { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Cyan { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Magenta { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Yellow { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Black { get; set; }
    }
}
