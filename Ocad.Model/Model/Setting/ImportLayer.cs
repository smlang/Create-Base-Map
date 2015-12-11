using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class ImportLayer
    {
        [VersionsSupported(V9 = true)]
        public String LayerName { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16 LayerNumber { get; set; }
    }
}
