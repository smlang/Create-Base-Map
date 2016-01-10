using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public abstract class AbstractObject
    {
        private Model.Map _map;

        private Boolean _supportAngleDegree = false;
        private Boolean _supportText = false;
        private Boolean _supportLineStyle = false;
        private Boolean _supportLineWidth = false;
        private Boolean _supportImportLayer = false;

        private Decimal _angleDegree;
        private String _text;
        private Type.LineStyle _lineStyle;
        private Distance _lineWidth;
        public Type.FeatureType _featureType;
        public ImportLayer _importLayer;

        public Boolean SupportAngleDegree { get { return _supportAngleDegree; } }
        public Boolean SupportText { get { return _supportText; } }
        public Boolean SupportLineStyle { get { return _supportLineStyle; } }
        public Boolean SupportLineWidth { get { return _supportLineWidth; } }
        public Boolean SupportImportLayer { get { return _supportImportLayer; } }

        [VersionsSupported(V9 = true)]
        public Type.ObjectStatus Status { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.ObjectView View { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.FeatureType FeatureType { get { return _featureType; } }
        [VersionsSupported(V9 = true)]
        public List<Point> Points { get; set; }  // 5 points if unformatted text; 4 points if formatted text

        [VersionsSupported(V9 = true)]
        public Int32 Index { get { return (_map.Objects.IndexOf(this) + 1); } }

        [VersionsSupported(V9 = true)]
        public Decimal AngleDegree
        {
            get
            {
                if (!_supportAngleDegree)
                {
                    throw new NotSupportedException(String.Format("Angle Degree is not supported for {0} Objects.", FeatureType));
                }
                return _angleDegree;
            }
            set
            {
                if (!_supportAngleDegree)
                {
                    throw new NotSupportedException(String.Format("Angle Degree is not supported for {0} Objects.", FeatureType));
                }
                _angleDegree = value;
            }
        }
        [VersionsSupported(V9 = true)]
        public String Text
        {
            get
            {
                if (!_supportText)
                {
                    throw new NotSupportedException(String.Format("Text is not supported for {0} Objects.", FeatureType));
                }
                return _text;
            }
            set
            {
                if (!_supportText)
                {
                    throw new NotSupportedException(String.Format("Text is not supported for {0} Objects.", FeatureType));
                }
                _text = value;
            }
        }
        [VersionsSupported(V9 = true)]
        public Type.LineStyle LineStyle
        {
            get
            {
                if (!_supportLineStyle)
                {
                    throw new NotSupportedException(String.Format("Line Style is not supported for {0} {1} Objects.", this.Type, FeatureType));
                }
                return _lineStyle;
            }
            set
            {
                if (!_supportLineStyle)
                {
                    throw new NotSupportedException(String.Format("Line Style is not supported for {0} {1} Objects.", this.Type, FeatureType));
                }
                _lineStyle = value;
            }
        }
        [VersionsSupported(V9 = true)]
        public Distance LineWidth
        {
            get
            {
                if (!_supportLineWidth)
                {
                    throw new NotSupportedException(String.Format("Line Width is not supported for {0} {1} Objects.", this.Type, FeatureType));
                }
                return _lineWidth;
            }
            set
            {
                if (!_supportLineWidth)
                {
                    throw new NotSupportedException(String.Format("Line Width is not supported for {0} {1} Objects.", this.Type, FeatureType));
                }
                _lineWidth = value;
            }
        }
        [VersionsSupported(V9 = true)]
        public ImportLayer ImportLayer
        {
            get
            {
                if (!_supportImportLayer)
                {
                    throw new NotSupportedException(String.Format("Import Layer is not supported for {0} Objects.", this.Type));
                }
                return _importLayer;
            }
            set
            {
                if (!_supportImportLayer)
                {
                    throw new NotSupportedException(String.Format("Import Layer is not supported for {0} Objects.", this.Type));
                }
                _importLayer = value;
            }
        }

        [VersionsSupported(V9 = true)]
        public abstract Type.ObjectType Type { get; }

        [VersionsSupported(V9 = true)]
        public Point BoundingBoxBottomLeft
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

                return new Point(x, y);
            }
        }

        [VersionsSupported(V9 = true)]
        public Point BoundingBoxTopRight
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

                return new Point(x, y);
            }
        }

        internal protected AbstractObject(Model.Map map, Model.Type.FeatureType featureType)
        {
            Status = Model.Type.ObjectStatus.Normal;
            View = Model.Type.ObjectView.Normal;

            _map = map;
            _featureType = featureType;

            if ((this.Type == Model.Type.ObjectType.Image) || (this.Type == Model.Type.ObjectType.Imported))
            {
                _supportImportLayer = true;
            }

            switch (_featureType)
            {
                case Model.Type.FeatureType.Area:
                    _supportAngleDegree = true;
                    break;
                case Model.Type.FeatureType.FormattedText:
                    _supportAngleDegree = true;
                    _supportText = true;
                    break;
                case Model.Type.FeatureType.Line:
                    if (this.Type == Model.Type.ObjectType.Graphic)
                    {
                        _supportLineStyle = true;
                        _supportLineWidth = true;
                    }
                    else if (this.Type == Model.Type.ObjectType.Image)
                    {
                        _supportLineWidth = true;
                    }
                    break;
                case Model.Type.FeatureType.LineText:
                    _supportText = true;
                    break;
                case Model.Type.FeatureType.Point:
                    _supportAngleDegree = true;
                    break;
                case Model.Type.FeatureType.Rectangle:
                    _supportAngleDegree = true;
                    break;
                case Model.Type.FeatureType.UnformattedText:
                    _supportAngleDegree = true;
                    _supportText = true;
                    break;
            }

            Points = new List<Point>();
        }

        public Geometry.Path GetPath(Distance epsilon)
        {
            if (FeatureType != Model.Type.FeatureType.Line)
            {
                return null;
            }

            Geometry.Path path = new Geometry.Path();

            Point p1 = Points[0];
            Point p2;
            for (int i = 1; i < Points.Count; i++)
            {
                p2 = Points[i];
                if ((p1.SecondBezier == null) && (p2.FirstBezier == null))
                {
                    path.Add(new LinearSegment(p1, p2), epsilon);
                }
                else
                {
                    path.Add(new CubicBezierCurve(p1, p1.SecondBezier, p2, p2.FirstBezier, epsilon), epsilon);
                }

                p1 = p2;
            }

            return path;
        }

        public Geometry.Polygon GetPolygon(Distance epsilon)
        {
            if (FeatureType != Model.Type.FeatureType.Area)
            {
                return null;
            }

            List<ISegment> segments = new List<ISegment>();
            Point p1 = Points[0];
            Point p2;
            for (int i = 1; i < Points.Count; i++)
            {
                p2 = Points[i];
                if ((p2.MainPointFlag & Model.Type.PointFlag.HolePoint) == Model.Type.PointFlag.HolePoint)
                {
                    break;
                }
                if ((p1.SecondBezier == null) && (p2.FirstBezier == null))
                {
                    segments.Add(new LinearSegment(p1, p2));
                }
                else
                {
                    segments.Add(new CubicBezierCurve(p1, p1.SecondBezier, p2, p2.FirstBezier, epsilon));
                }

                p1 = p2;
            }

            return new Geometry.Polygon(segments, epsilon);
        }

        public List<Geometry.Polygon> GetHolePolygons(Distance epsilon)
        {
            if (FeatureType != Model.Type.FeatureType.Area)
            {
                return null;
            }

            return null;
        }
    }
}
