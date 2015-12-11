using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class BlockTextSymbol : AbstractTextSymbol
    {
        [VersionsSupported(V9 = true)]
        public Int16 LineSpacingPercentageOfFontSize { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance SpaceAfterParagraph { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance IdentFirstLine { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance IdentOtherLines { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Distance> Tabs { get; set; }

        [VersionsSupported(V9 = true)]
        public Boolean LineBelowOn { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour LineBelowColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance LineBelowWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance LineBelowDistance { get; set; }

        [VersionsSupported(V9 = true)]
        public Type.LineStyle FramingLineStyle { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance FramingLeft { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance FramingBottom { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance FramingRight { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance FramingTop { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.SymbolType Type
        {
            get
            {
                return Model.Type.SymbolType.BlockText;
            }
        }

        [VersionsSupported(V9 = true)]
        public override List<Colour> Colours
        {
            get
            {
                List<Colour> colours = base.Colours;
                colours.Add(FontColour);
                if ((LineBelowColour != null) && (!LineBelowOn) && (!colours.Contains(LineBelowColour)))
                {
                    colours.Add(LineBelowColour);
                }
                return colours;
            }
        }

        internal BlockTextSymbol()
        {
            Tabs = new List<Distance>();
        }
    }
}
