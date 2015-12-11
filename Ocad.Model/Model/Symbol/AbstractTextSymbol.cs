using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public abstract class AbstractTextSymbol : AbstractSymbol
    {
        [VersionsSupported(V9 = true)]
        public String FontName { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour FontColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal FontSize { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.FontWeight FontWeight { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean FontItalic { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16 CharacterSpacingPercentageOfSpace { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16 WordSpacingPercentageOfSpace { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.TextAlignment TextAlignment { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.FramingMode FramingMode { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour FramingColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance FramingWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance FramingShadowX { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance FramingShadowY { get; set; }

        [VersionsSupported(V9 = true)]
        public override List<Colour> Colours
        {
            get
            {
                List<Colour> colours = new List<Colour>();
                colours.Add(FontColour);
                if ((FramingColour != null) && (FramingMode != Ocad.Model.Type.FramingMode.Off) && (!colours.Contains(FramingColour)))
                {
                    colours.Add(FramingColour);
                }
                return colours;
            }
        }
    }
}
