using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class CubicBezierCurve : ISegment
    {
        private Point _start;
        public Point Start
        {
            get {
                return _start;
            }
            set
            {
                _start = value;
                _pointList = null;
            }
        }

        private Point _end;
        public Point End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                _pointList = null;
            }
        }

        private Distance _top;
        public Distance Top
        {
            get
            {
                RefreshLookup();
                return _top;
            }
        }
        private Distance _bottom;
        public Distance Bottom
        {
            get
            {
                RefreshLookup();
                return _bottom;
            }
        }

        private Distance _right;
        public Distance Right
        {
            get
            {
                RefreshLookup();
                return _right;
            }
        }

        private Distance _left;
        public Distance Left
        {
            get
            {
                RefreshLookup();
                return _left;
            }
        }

        private Point _startControl;
        public Point StartControl
        {
            get
            {
                return _startControl;
            }
            set
            {
                _startControl = value;
                _pointList = null;
            }
        }

        private Point _endControl;
        public Point EndControl
        {
            get
            {
                return _endControl;
            }
            set
            {
                _endControl = value;
                _pointList = null;
            }
        }


        private Distance _epsilon;


        private List<Point[]> _pointList = null;

        private void RefreshLookup()
        {
            if (_pointList != null)
            {
                return;
            }
            LinearSegment a = new LinearSegment(Start, StartControl ?? Start);
            LinearSegment b = new LinearSegment(StartControl ?? Start, EndControl ?? End);
            LinearSegment c = new LinearSegment(EndControl ?? End, End);

            _pointList = new List<Point[]>();
            _pointList.Add(new Point[] { Start, a.Start, a.End });
            FlattenCurve(a, b, c, Start, 0, End, 1, _epsilon);
            _pointList.Add(new Point[] { End, c.Start, c.End });

            _top = Start.Y;
            _bottom = Start.Y;
            _right = Start.X;
            _left = Start.X;
            _length = Start.X - Start.X; // Get unit and scale from start

            Point p = _pointList[0][0];
            for (int i=1; i < _pointList.Count; i++)
            {
                Point q = _pointList[i][0];
                _top = Distance.Max(_top, q.Y);
                _bottom = Distance.Min(_bottom, q.Y);
                _right = Distance.Max(_right, q.X);
                _left = Distance.Min(_left, q.X);
                LinearSegment pq = new LinearSegment(p, q);
                _length += pq.Length;

                p = q;
            }
        }

        private void FlattenCurve(LinearSegment a, LinearSegment b, LinearSegment c, Point x, Decimal xRatio, Point y, Decimal yRatio, Distance epsilon)
        {
            Decimal midRatio = xRatio + ((yRatio - xRatio) / 2);
            Point midEndControl;
            Point midStartControl;
            Point mid = CutCurve(a, b, c, midRatio, out midEndControl, out midStartControl);

            LinearSegment p = new LinearSegment(x, mid);
            if (!p.LengthIsZero(epsilon))
            {
                FlattenCurve(a, b, c, x, xRatio, mid, midRatio, epsilon);
            }

            _pointList.Add(new Point[] { mid, midEndControl, midStartControl });

            LinearSegment q = new LinearSegment(mid, y);
            if (!q.LengthIsZero(epsilon))
            {
                FlattenCurve(a, b, c, mid, midRatio, y, yRatio, epsilon);
            }
        }

        private Point CutCurve(LinearSegment a, LinearSegment b, LinearSegment c, Decimal ratio, out Point newEndControl, out Point newStartControl)
        {
            Point mida = a.IntermediatePoint(ratio);
            Point midb = b.IntermediatePoint(ratio);
            Point midc = c.IntermediatePoint(ratio);

            LinearSegment ab = new LinearSegment(mida, midb);
            newEndControl = ab.IntermediatePoint(ratio);

            LinearSegment bc = new LinearSegment(midb, midc);
            newStartControl = bc.IntermediatePoint(ratio);

            LinearSegment abc = new LinearSegment(newEndControl, newStartControl);
            return abc.IntermediatePoint(ratio);
        }

        private Distance _length;
        public Distance Length
        {
            get
            {
                RefreshLookup();
                return _length;
            }
        }

        public CubicBezierCurve(Point start, Point startControl, Point end, Point endControl, Distance epsilon)
        {
            Start = start;
            StartControl = startControl;
            End = end;
            EndControl = endControl;
            _epsilon = epsilon;
        }

        public Boolean LengthIsZero(Distance epsilon)
        {
            if (Start.GetRelationship(End, epsilon) == Relationship.Apart)
            {
                return false;
            }
            // if start and end at the same point, check if they always match the control points
            if (Start.GetRelationship(StartControl, epsilon) == Relationship.Apart)
            {
                return false;
            }
            return (Start.GetRelationship(EndControl, epsilon) != Relationship.Apart);
        }

        public Relationship GetRelationship(Point point, Distance epsilon, List<Decimal> thisIntersectRatios)
        {
            throw (new NotImplementedException());
        }

        public Relationship GetRelationship(ISegment segment, Distance epsilon)
        {
            throw (new NotImplementedException());
        }

        public Relationship GetRelationship(Path path, Distance epsilon)
        {
            throw (new NotImplementedException());
        }

        public Relationship GetRelationship(Polygon polygon, Distance epsilon)
        {
            throw (new NotImplementedException());
        }

        public DirectionSide SideOf(Point point, Distance epsilon)
        {
            throw (new NotImplementedException());
        }

        public Boolean IntersectPoints(ISegment other, Distance epsilon, List<Decimal> thisIntersectRatios, List<Decimal> otherIntersectRatios)
        {
            throw (new NotImplementedException());
        }
    }
}
