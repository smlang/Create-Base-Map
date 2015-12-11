using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class DatabaseParameter
    {
        [VersionsSupported(V9 = true)]
        public String DataSet { get; set; }
        [VersionsSupported(V9 = true)]
        public String LastCode { get; set; }
        [VersionsSupported(V9 = true)]
        public String CreateNewRecord { get; set; }
    }
}
