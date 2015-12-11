using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class DataSet
    {
        [VersionsSupported(V9 = true)]
        public String DataSetName { get; set; }
        [VersionsSupported(V9 = true)]
        public String DBaseFile { get; set; }
        [VersionsSupported(V9 = true)]
        public String OdbcDataSource { get; set; }
        [VersionsSupported(V9 = true)]
        public String Table { get; set; }
        [VersionsSupported(V9 = true)]
        public String KeyField { get; set; }
        [VersionsSupported(V9 = true)]
        public String SymbolField { get; set; }
        [VersionsSupported(V9 = true)]
        public String TextField { get; set; }
        [VersionsSupported(V9 = true)]
        public String SizeField { get; set; }
        [VersionsSupported(V9 = true)]
        public String LengthUnit { get; set; }
        [VersionsSupported(V9 = true)]
        public String AreaUnit { get; set; }
        [VersionsSupported(V9 = true)]
        public String Decimals { get; set; }
        [VersionsSupported(V9 = true)]
        public String HorizontalCoordinate { get; set; }
        [VersionsSupported(V9 = true)]
        public String VerticalCoordinate { get; set; }
    }
}
