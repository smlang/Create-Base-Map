using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class FileName
    {
        [VersionsSupported(V9 = true)]
        public String Value { get; set; }
    }
}
