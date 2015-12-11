using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class TilesParameter
    {
        [VersionsSupported(V9 = true)]
        public Int32 Width { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32 Height { get; set; }
    }
}
