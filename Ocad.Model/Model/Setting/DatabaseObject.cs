using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class DatabaseObject
    {
        [VersionsSupported(V9 = true)]
        public String DataSet { get; set; }
    }
}
