using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class DatabaseCreateObjectParameter
    {
        [VersionsSupported(V9 = true)]
        public String Condition { get; set; }
        [VersionsSupported(V9 = true)]
        public String DataSet { get; set; }
        [VersionsSupported(V9 = true)]
        public String TextField { get; set; }
        [VersionsSupported(V9 = true)]
        public String UnitOfMeasure { get; set; }
        [VersionsSupported(V9 = true)]
        public String HorizontalOffset { get; set; }
        [VersionsSupported(V9 = true)]
        public String VerticalOffset { get; set; }
        [VersionsSupported(V9 = true)]
        public String HorizontalField { get; set; }
        [VersionsSupported(V9 = true)]
        public String VerticalField { get; set; }
    }
}
