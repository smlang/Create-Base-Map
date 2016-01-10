using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class LinearSegment : ISegment
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        private Distance _top = null;
        private Distance _bottom = null;
        private Distance _right = null;
        private Distance _left = null;

        public Distance Top { get { return _top; } }
        public Distance Bottom { get { return _bottom; } }
        public Distance Right { get { return _right; } }
        public Distance Left { get { return _left; } }

        public Distance Length
        {
            get
            {
                Distance x = Start.X - End.X;
                Distance y = Start.Y - End.Y;
                return ((x * x) + (y * y)).SquareRoot();
            }
        }

        public LinearSegment(Point start, Point end)
        {
            Start = start;
            End = end;

            if (start.Y > end.Y)
            {
                _top = start.Y;
                _bottom = end.Y;
            }
            else
            {
                _top = end.Y;
                _bottom = start.Y;
            }

            if (start.X > end.X)
            {
                _right = start.X;
                _left = end.X;
            }
            else
            {
                _right = end.X;
                _left = start.X;
            }
        }

        public Boolean LengthIsZero(Distance epsilon)
        {
            return (Start.GetRelationship(End, epsilon) != Relationship.Apart);
        }

        #region GetRelationship
        /*
                                 Meet/ | Vertex   Vertex  Vertex  |  Edge     Edge    Edge   |  Face     Face    Face   |  Edge   | Overlap Sub Identical Super
        Object       Subject     Apart | Touches  Touches Touches | Touches  Touches Touches | Touches  Touches Touches | Crosses |   Set   Set    Set     Set
                                       | Vertex    Edge    Face   | Vertex    Edge    Face   | Vertex    Edge    Face   |  Edge   |
        -------------------------------+--------------------------+--------------------------+--------------------------+---------+----------------------------   | |
        1-Dimension  0-Dimension   X   |    X        -       -    |    X        -       -    |    -        -       -    |    -    |    -     -      -       - <---+ |
        */
        public Relationship GetRelationship(Point point, Distance epsilon, List<Decimal> thisIntersectRatios)
        {
            #region Point not within this Segment's boundary
            if (LengthIsZero(epsilon))
            {
                throw new ApplicationException("Subject Segment is a point");
            }

            if (point.Y.IsGreaterThan(Top, epsilon)) { return Relationship.Apart; }
            if (point.Y.IsLessThan(Bottom, epsilon)) { return Relationship.Apart; }
            if (point.X.IsGreaterThan(Right, epsilon)) { return Relationship.Apart; }
            if (point.X.IsLessThan(Left, epsilon)) { return Relationship.Apart; }
            #endregion

            #region Point Matches this Segment's Start or End Point
            if (point.GetRelationship(this.Start, epsilon) != Relationship.Apart)
            {
                if (thisIntersectRatios != null)
                {
                    thisIntersectRatios.Add(0M);
                }
                return Relationship.VertexTouchesVertex;
            }

            if (point.GetRelationship(this.End, epsilon) != Relationship.Apart)
            {
                if (thisIntersectRatios != null)
                {
                    thisIntersectRatios.Add(1M);
                }
                return Relationship.VertexTouchesVertex;
            }
            #endregion

            #region Point Lies on this Segment
            Angle a = new Angle(this.Start, this.End, Angle.Unit.Radian);
            LinearSegment perpendicularLineThruPoint = new LinearSegment(
                new Point(point, epsilon, a + Angle.QuarterCircle),
                new Point(point, epsilon, a - Angle.QuarterCircle));

            if (this.IntersectPoints(perpendicularLineThruPoint, epsilon, thisIntersectRatios, null))
            {
                return Relationship.EdgeTouchesVertex;
            }
            #endregion

            return Relationship.Apart;
        }

        /*
                                 Meet/ | Vertex   Vertex  Vertex  |  Edge     Edge    Edge   |  Face     Face    Face   |  Edge   | Overlap Sub Identical Super
        Object       Subject     Apart | Touches  Touches Touches | Touches  Touches Touches | Touches  Touches Touches | Crosses |   Set   Set    Set     Set
                                       | Vertex    Edge    Face   | Vertex    Edge    Face   | Vertex    Edge    Face   |  Edge   |
        -------------------------------+--------------------------+--------------------------+--------------------------+---------+----------------------------
        1-Dimension  1-Dimension   X   |    X        X       -    |    X        X       -    |    -        -       -    |    X    |    X     X      X       X  
        */
        public Relationship GetRelationship(ISegment segment, Distance epsilon)
        {
            if (segment is LinearSegment)
            {
                return GetRelationship((LinearSegment)segment, epsilon);
            }
            throw (new NotImplementedException());
        }

        public Relationship GetRelationship(LinearSegment segment, Distance epsilon)
        {
            #region Segment not within this Segment's boundary
            if (LengthIsZero(epsilon))
            {
                throw new ApplicationException("Subject Segment is a point");
            }

            if (segment.LengthIsZero(epsilon))
            {
                throw new ApplicationException("Object Segment is a point");
            }

            if (Top.IsLessThan(segment.Bottom, epsilon)) { return Relationship.Apart; }
            if (Bottom.IsGreaterThan(segment.Top, epsilon)) { return Relationship.Apart; }
            if (Right.IsLessThan(segment.Left, epsilon)) { return Relationship.Apart; }
            if (Left.IsGreaterThan(segment.Right, epsilon)) { return Relationship.Apart; }
            #endregion

            #region All points match
            Relationship segmentStart = this.GetRelationship(segment.Start, epsilon, null);
            Relationship segmentEnd = this.GetRelationship(segment.End, epsilon, null);
            if ((segmentStart == Relationship.VertexTouchesVertex) && (segmentEnd == Relationship.VertexTouchesVertex))
            {
                return (Relationship.EdgeTouchesEdge | Relationship.IdenticalSet);
            }
            #endregion

            #region Both Object Segment Points Lie on Subject Segment
            if ((segmentStart != Relationship.Apart) && (segmentEnd != Relationship.Apart))
            {
                return (Relationship.EdgeTouchesEdge | Relationship.Superset);
            }
            #endregion

            #region Both Subject Segment Points Lie on Object Segment
            Relationship thisStart = segment.GetRelationship(this.Start, epsilon, null);
            Relationship thisEnd = segment.GetRelationship(this.End, epsilon, null);
            if ((thisStart != Relationship.Apart) && (thisEnd != Relationship.Apart))
            {
                return (Relationship.EdgeTouchesEdge | Relationship.Subset);
            }
            #endregion

            #region Connecting Points
            if ((thisStart == Relationship.VertexTouchesVertex) ||
                (thisEnd == Relationship.VertexTouchesVertex))
            {
                return Relationship.VertexTouchesVertex;
            }
            #endregion

            #region Subject Segment Edge touches Object Segment Point / Crosses / Separate
            if ((thisStart == Relationship.Apart) && (thisEnd == Relationship.Apart))
            {
                #region Check if cross
                if ((segmentStart == Relationship.Apart) && (segmentEnd == Relationship.Apart))
                {
                    if (this.IntersectPoints(segment, epsilon, null, null))
                    {
                        // Lines cross
                        return (Relationship.Cross);
                    }
                    return Relationship.Apart;
                }
                #endregion

                return Relationship.EdgeTouchesVertex;
            }
            #endregion

            #region Subject Segment Point touches Object Segment Edge
            if ((segmentStart == Relationship.Apart) && (segmentEnd == Relationship.Apart))
            {
                return Relationship.VertexTouchesEdge;
            }
            #endregion

            // Subject Point touches a Object Edge, and a Object Point touches a Subject Edge
            return (Relationship.EdgeTouchesEdge | Relationship.OverlapSet);
        }

        public Relationship GetRelationship(Path path, Distance epsilon)
        {
            return path.GetRelationship(this, epsilon);
        }

        /*
                                 Meet/ |         | Vertex   Vertex  Vertex  |  Edge     Edge    Edge   |  Face     Face    Face   | Overlap Sub Identical Super
        Object       Subject     Apart | Crosses | Touches  Touches Touches | Touches  Touches Touches | Touches  Touches Touches |   Set   Set    Set     Set
                                       |         | Vertex    Edge    Face   | Vertex    Edge    Face   | Vertex    Edge    Face   |
        -------------------------------+---------+--------------------------+--------------------------+--------------------------+----------------------------
        1-Dimension  2-Dimension   X   |    X    |    X        X       X    |    X        X       X    |    -        -       -    |    -     -      -       -  
        */
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
        #endregion

        #region Side Of
        public DirectionSide SideOf(Point point, Distance epsilon)
        {
            Area crossProduct = CrossProduct(Start, point);
            if (crossProduct.IsZero(epsilon))
            {
                return 0;
            }

            return (DirectionSide)crossProduct.Sign;
        }
        #endregion

        #region Intersect Points
        public Boolean IntersectPoints(ISegment other, Distance epsilon, List<Decimal> thisIntersectRatios, List<Decimal> otherIntersectRatios)
        {
            if (other is LinearSegment)
            {
                return IntersectPoints((LinearSegment)other, epsilon, thisIntersectRatios, otherIntersectRatios);
            }
            throw (new NotImplementedException());
        }

        public Boolean IntersectPoints(LinearSegment other, Distance epsilon, List<Decimal> thisIntersectRatios, List<Decimal> otherIntersectRatios)
        {
            //Area zero = Area.Zero(Distance.Unit.Metre, Scale.ten_minus_5);
            Area denominator = CrossProduct(other.Start, other.End);
            if (denominator.IsZero(epsilon))
            {
                // AB and PQ are co-linear
                return false;
            }
            bool denominatorPositive = denominator.IsPositive(epsilon); //  (denominator > zero);

            Area s_numerator = this.CrossProduct(other.Start, Start);
            if (denominatorPositive)
            {
                // denominator is positive and s_numerator is negative
                if (s_numerator.IsNegative(epsilon))
                {
                    return false;
                }
            }
            else
            {
                // s_numerator is positive and denominator is negative
                if (s_numerator.IsPositive(epsilon))
                {
                    return false;
                }
            }
            if ((s_numerator.IsLessThan(denominator, epsilon)) != denominatorPositive)
            {
                return false;
            }

            Area t_numerator = other.CrossProduct(other.Start, Start);
            if (denominatorPositive)
            {
                // denominator is positive and t_numerator is negative
                if (t_numerator.IsNegative(epsilon))
                {
                    return false;
                }
            }
            else
            {
                // t_numerator is positive and denominator is negative
                if (t_numerator.IsPositive(epsilon))
                {
                    return false;
                }
            }
            if ((t_numerator.IsLessThan(denominator, epsilon)) != denominatorPositive)
            {
                return false;
            }

            if (thisIntersectRatios != null)
            {
                thisIntersectRatios.Add(t_numerator / denominator);
            }
            if (otherIntersectRatios != null)
            {
                otherIntersectRatios.Add(s_numerator / denominator);
            }

            return true;
        }

        internal Point IntermediatePoint(Decimal ratio)
        {
            return (new Point(Start.X + (ratio * (End.X - Start.X)), Start.Y + (ratio * (End.Y - Start.Y))));
        }

        private Decimal Ratio(Point intermediatePoint)
        {
            Distance xScale = (End.X - Start.X).Absolute();
            Distance yScale = (End.Y - Start.Y).Absolute();

            if (xScale > yScale)
            {
                // Use X
                return (intermediatePoint.X - End.X) / xScale;
            }
            else
            {
                // Use Y
                return (intermediatePoint.Y - End.Y) / yScale;
            }
        }

        private Area CrossProduct(Point p, Point q)
        {
            Distance ab_x_length = End.X - Start.X;
            Distance ab_y_length = End.Y - Start.Y;

            Distance pq_x_length = q.X - p.X;
            Distance pq_y_length = q.Y - p.Y;

            return (ab_x_length * pq_y_length) - (ab_y_length * pq_x_length);
        }
        #endregion
    }
}
