using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class RectangleSymbol : AbstractSymbol
    {
        [VersionsSupported(V9 = true)]
        public Colour LineColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance LineWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance Radius { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.GridFlag GridFlags { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance CellWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance CellHeight { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16 UnnumberedCells { get; set; }
        [VersionsSupported(V9 = true)]
        public String UnnumberedCellsText { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal FontSize { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.SymbolType Type
        {
            get
            {
                return Model.Type.SymbolType.Rectangle;
            }
        }

        [VersionsSupported(V9 = true)]
        public override List<Colour> Colours 
        { 
            get 
            {
                List<Colour> colours = new List<Colour>();
                colours.Add(LineColour);
                return colours;
            } 
        }
    }
}
