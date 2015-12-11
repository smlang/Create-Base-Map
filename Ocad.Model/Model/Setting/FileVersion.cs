using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class FileVersion
    {
        [VersionsSupported(V9 = true)]
        public Int16 Version { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16 Subversion { get; set; }
    }
}
