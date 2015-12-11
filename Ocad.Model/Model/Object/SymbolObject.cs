using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class SymbolObject : AbstractObject
    {
        [VersionsSupported(V9 = true)]
        public AbstractSymbol Symbol { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.ObjectType Type
        {
            get
            {
                return Model.Type.ObjectType.Symbol;
            }
        }

        [VersionsSupported(V9 = true)]
        public new Point BoundingBoxBottomLeft
        {
            get
            {
                Geometry.Distance x = Points[0].X;
                Geometry.Distance y = Points[0].Y;

                foreach (Point p2 in Points)
                {
                    if (p2.SecondBezier != null)
                    {
                        if (p2.SecondBezier.X[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] < x[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                        {
                            x = p2.SecondBezier.X;
                        }
                        if (p2.SecondBezier.Y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] < y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                        {
                            y = p2.SecondBezier.Y;
                        }
                    }

                    if (p2.X[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] < x[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                    {
                        x = p2.X;
                    }
                    if (p2.Y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] < y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                    {
                        y = p2.Y;
                    }

                    if (p2.FirstBezier != null)
                    {
                        if (p2.FirstBezier.X[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] < x[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                        {
                            x = p2.FirstBezier.X;
                        }
                        if (p2.FirstBezier.Y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] < y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                        {
                            y = p2.FirstBezier.Y;
                        }
                    }
                }

                x -= Symbol.Extent;
                y -= Symbol.Extent;

                return new Point(x, y);
            }
        }

        [VersionsSupported(V9 = true)]
        public new Point BoundingBoxTopRight
        {
            get
            {
                Geometry.Distance x = Points[0].X;
                Geometry.Distance y = Points[0].Y;

                foreach (Point p2 in Points)
                {
                    if (p2.SecondBezier != null)
                    {
                        if (p2.SecondBezier.X[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] > x[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                        {
                            x = p2.SecondBezier.X;
                        }
                        if (p2.SecondBezier.Y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] > y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                        {
                            y = p2.SecondBezier.Y;
                        }
                    }

                    if (p2.X[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] > x[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                    {
                        x = p2.X;
                    }
                    if (p2.Y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] > y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                    {
                        y = p2.Y;
                    }

                    if (p2.FirstBezier != null)
                    {
                        if (p2.FirstBezier.X[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] > x[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                        {
                            x = p2.FirstBezier.X;
                        }
                        if (p2.FirstBezier.Y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5] > y[Geometry.Distance.Unit.Metre, Geometry.Scale.ten_minus_5])
                        {
                            y = p2.FirstBezier.Y;
                        }
                    }
                }

                x += Symbol.Extent;
                y += Symbol.Extent;

                return new Point(x, y);
            }
        }

        internal SymbolObject(Model.Map map, Model.Type.FeatureType featureType)
            : base(map, featureType)
        {
        }

        public SymbolObject(Model.Map map, Model.Type.FeatureType featureType, Ocad.Model.AbstractSymbol symbol)
            : base(map, featureType)
        {
            Symbol = symbol;
            _featureType = featureType;

            map.Objects.Add(this);
        }
    }
}
