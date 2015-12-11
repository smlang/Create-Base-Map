using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public class OcadSymbolSet
    {
        public string PointSymbolNumber;
        public string LineSymbolNumber;
        public string AreaSymbolNumber;

        public OcadSymbolSet(string pointSymbolNumber, string lineSymbolNumber, string areaSymbolNumber)
        {
            PointSymbolNumber = pointSymbolNumber;
            LineSymbolNumber = lineSymbolNumber;
            AreaSymbolNumber = areaSymbolNumber;
        }
    }
}
