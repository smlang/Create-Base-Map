using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class AreaSymbol : AbstractSymbol
    {
        [VersionsSupported(V9 = true)]
        public LineSymbol BorderSymbol { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour FillColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.HatchMode HatchMode { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour HatchColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance HatchLineWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance HatchDistance { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal HatchAngle1Degree { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal HatchAngle2Degree { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean FillOn { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean BorderOn { get; set; }

        [VersionsSupported(V9 = true)]
        public Type.StructureMode StuctureMode { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance StuctureWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance StuctureHeight { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal StuctureAngleDegree { get; set; }

        [VersionsSupported(V9 = true)]
        public List<Shape> Shapes { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.SymbolType Type
        {
            get
            {
                return Model.Type.SymbolType.Area;
            }
        }

        [VersionsSupported(V9 = true)]
        public override List<Colour> Colours
        {
            get 
            {
                List<Colour> colours = new List<Colour>();
                if ((BorderSymbol != null) && BorderOn)
                {
                    colours = BorderSymbol.Colours;
                }
                if ((FillColour != null) && FillOn)
                {
                    colours.Add(FillColour);
                }
                if ((HatchColour != null) && (HatchMode != Ocad.Model.Type.HatchMode.Off) && (!colours.Contains(HatchColour)))
                {
                    colours.Add(HatchColour);
                }
                return colours;
            } 
        }

        internal AreaSymbol()
        {
            Shapes = new List<Shape>();
        }
    }
}
