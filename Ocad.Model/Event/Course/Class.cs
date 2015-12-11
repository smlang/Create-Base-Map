using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public class Class
    {
        [VersionsSupported(V9 = true)]
        public String Name { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32? NumberOfRunners { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32? FromBibNumber { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32? ToBibNumber { get; set; }
    }
}
