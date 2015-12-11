using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Path : List<ISegment>
    {
        public Point Start { get { return this[0].Start; } }
        public Point End { get { return this[Count - 1].End; } }

        internal protected Distance _top = null;
        internal protected Distance _bottom = null;
        internal protected Distance _right = null;
        internal protected Distance _left = null;

        public Distance Top { get { return _top; } }
        public Distance Bottom { get { return _bottom; } }
        public Distance Right { get { return _right; } }
        public Distance Left { get { return _left; } }

        public Distance Length
        {
            get
            {
                Distance length = new Distance(0M, Distance.Unit.Metre, Scale.ten_minus_5);
                foreach (ISegment segment in this)
                {
                    length += segment.Length;
                }
                return length;
            }
        }

        public List<Point> Points
        {
            get
            {
                List<Point> points = new List<Point>();
                if (this.Count > 0)
                {
                    points.Add(this[0].Start);
                    foreach (ISegment segments in this)
                    {
                        points.Add(segments.End);
                    }
                }
                return points;
            }
        }

        public Path()
        {
        }

        public Path(List<ISegment> segments, Distance epsilon) : base()
        {
            AddRange(segments, epsilon);
        }

        public Boolean LengthIsZero(Distance epsilon)
        {
            Distance length = new Distance(0M, Distance.Unit.Metre, Scale.ten_minus_5);
            foreach (ISegment segment in this)
            {
                length += segment.Length;
                if (!length.IsZero(epsilon))
                {
                    return false;
                }
            }
            return true;
        }

        public Relationship GetRelationship(Point point, Distance epsilon)
        {
            throw (new NotImplementedException());
        }

        public Relationship GetRelationship(ISegment segment, Distance epsilon)
        {
            throw (new NotImplementedException());
        }

        public Relationship GetRelationship(LinearSegment segment, Distance epsilon)
        {
            throw (new NotImplementedException());
        }

        public Relationship GetRelationship(Path path, Distance epsilon)
        {
            throw (new NotImplementedException());
        }

        public Relationship GetRelationship(Polygon polygon, Distance epsilon)
        {
            Relationship r = polygon.GetRelationship(this, epsilon);
            switch (r)
            {
                case Relationship.VertexTouchesEdge:
                    return Relationship.EdgeTouchesVertex;
                case Relationship.EdgeTouchesVertex:
                    return Relationship.VertexTouchesEdge;
                case Relationship.FaceTouchesVertex:
                    return Relationship.VertexTouchesFace;
                case Relationship.FaceTouchesEdge:
                    return Relationship.EdgeTouchesFace;
                default:
                    return r;
            }
        }
        
        internal protected void Add2(ISegment segment)
        {
            if (Count != 0)
            {
                if (segment.End.Y > _top) { _top = segment.End.Y; }
                else if (segment.End.Y < _bottom) { _bottom = segment.End.Y; }
                if (segment.End.X > _right) { _right = segment.End.X; }
                else if (segment.End.X < _left) { _left = segment.End.X; }
            }
            else
            {
                _top = segment.Top;
                _bottom = segment.Bottom;
                _right = segment.Right;
                _left = segment.Left;
            }

            base.Add(segment);
        }

        public void Add(ISegment segment, Distance epsilon)
        {
            if (!segment.LengthIsZero(epsilon))
            {
                if (Count != 0)
                {
                    if (End.GetRelationship(segment.Start, epsilon) == Relationship.Apart)
                    {
                        // insert extra segment if current one does not start where
                        // the previous one ended
                        Add2(new LinearSegment(End, segment.Start));
                    }
                    else
                    {
                        // make start absolutely the same as the end of the previous segment
                        // 
                        segment.Start = End;
                    }
                }

                Add2(segment);
            }
        }

        public void AddRange(List<ISegment> segments, Distance epsilon)
        {
            foreach (ISegment segment in segments)
            {
                Add(segment, epsilon);
            }
        }
    }
}
