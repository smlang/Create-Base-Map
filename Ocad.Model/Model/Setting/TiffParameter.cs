using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class TiffParameter
    {
        [VersionsSupported(V9 = true)]
        public Type.TiffCompression? Compression { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? GeoTiff { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal? PixelSize { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? TfwFile { get; set; }
    }
}
