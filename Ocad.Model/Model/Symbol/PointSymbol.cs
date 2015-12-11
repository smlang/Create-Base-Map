using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class PointSymbol : AbstractSymbol
    {
        [VersionsSupported(V9 = true)]
        public List<Shape> Shapes { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.SymbolType Type
        {
            get
            {
                return Model.Type.SymbolType.Point;
            }
        }

        [VersionsSupported(V9 = true)]
        public override List<Colour> Colours
        {
            get
            {
                List<Colour> colours = new List<Colour>();
                foreach (Shape s in Shapes)
                {
                    if ((s != null) && (!colours.Contains(s.Colour)))
                    {
                        colours.Add(s.Colour);
                    }
                }
                return colours;
            }
        }

        internal PointSymbol()
        {
            Shapes = new List<Shape>();
        }
    }
}
