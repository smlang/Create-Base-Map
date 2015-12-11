using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class LineSymbol : AbstractSymbol
    {
        [VersionsSupported(V9 = true)]
        public Colour LineColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance LineWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.LineStyle LineStyle { get; set; }

        [VersionsSupported(V9 = true)]
        public Distance DistanceFromStart { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance DistanceFromEnd { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance MainLengthA { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance EndLengthB { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance MainGapC { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance MinorGapD { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance EndGapE { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16 MinimumNumberOfGaps { get; set; }

        [VersionsSupported(V9 = true)]
        public Int16 NMainSymbolA { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance MainSymbolDistance { get; set; }

        [VersionsSupported(V9 = true)]
        public Type.DoubleLineMode DoubleMode { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.DoubleLineFlag DoubleFlag { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour DoubleFillColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour DoubleLeftColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Colour DoubleRightColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance DoubleWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance DoubleLeftWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance DoubleRightWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance DoubleDashedLength { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance DoubleDashedGap { get; set; }

        [VersionsSupported(V9 = true)]
        public Type.DecreaseMode DecreaseMode { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16 DecreaseLastSymbolPercentageOfNormalSize { get; set; }

        [VersionsSupported(V9 = true)]
        public Colour FramingColour { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance FramingWidth { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.FramingStyle FramingStyle { get; set; }

        [VersionsSupported(V9 = true)]
        public List<Shape> MainSymbolAShapes { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Shape> SecondarySymbolBShapes { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Shape> CornerSymbolShapes { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Shape> StartSymbolCShapes { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Shape> EndSymbolDShapes { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.SymbolType Type
        {
            get
            {
                return Model.Type.SymbolType.Line;
            }
        }

        [VersionsSupported(V9 = true)]
        public override List<Colour> Colours
        {
            get
            {
                List<Colour> colours = new List<Colour>();
                colours.Add(LineColour);

                if (DoubleMode != Ocad.Model.Type.DoubleLineMode.Off)
                {
                    if ((DoubleFillColour != null) && (DoubleFlag != Ocad.Model.Type.DoubleLineFlag.None) && (!colours.Contains(DoubleFillColour)))
                    {
                        colours.Add(DoubleFillColour);
                    }

                    if ((DoubleLeftColour != null) && (!colours.Contains(DoubleLeftColour)))
                    {
                        colours.Add(DoubleLeftColour);
                    }

                    if ((DoubleRightColour != null) && (!colours.Contains(DoubleRightColour)))
                    {
                        colours.Add(DoubleRightColour);
                    }
                }

                if ((FramingColour != null) && (!colours.Contains(FramingColour)))
                {
                    colours.Add(FramingColour);
                }

                foreach (Shape s in MainSymbolAShapes)
                {
                    if ((s != null) && (!colours.Contains(s.Colour)))
                    {
                        colours.Add(s.Colour);
                    }
                }
                foreach (Shape s in SecondarySymbolBShapes)
                {
                    if ((s != null) && (!colours.Contains(s.Colour)))
                    {
                        colours.Add(s.Colour);
                    }
                }
                foreach (Shape s in CornerSymbolShapes)
                {
                    if ((s != null) && (!colours.Contains(s.Colour)))
                    {
                        colours.Add(s.Colour);
                    }
                }
                foreach (Shape s in StartSymbolCShapes)
                {
                    if ((s != null) && (!colours.Contains(s.Colour)))
                    {
                        colours.Add(s.Colour);
                    }
                }
                foreach (Shape s in EndSymbolDShapes)
                {
                    if ((s != null) && (!colours.Contains(s.Colour)))
                    {
                        colours.Add(s.Colour);
                    }
                }
                return colours;
            }
        }

        internal LineSymbol()
        {
            MainSymbolAShapes = new List<Shape>();
            SecondarySymbolBShapes = new List<Shape>();
            CornerSymbolShapes = new List<Shape>();
            StartSymbolCShapes = new List<Shape>();
            EndSymbolDShapes = new List<Shape>();
        }
    }
}
