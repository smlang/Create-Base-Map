using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class Colour
    {
        [VersionsSupported(V9 = true)]
        public Int16 Number { get; set; }
        [VersionsSupported(V9 = true)]
        public String Name { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Cyan { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Magenta { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Yellow { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal Black { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean Overprint { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal? Transparency { get; set; }
        [VersionsSupported(V9 = true)]
        public Dictionary<SpotColour, Decimal> SpotColours { get; set; }

        internal Colour()
        {
            SpotColours = new Dictionary<SpotColour, Decimal>();
        }
    }
}
