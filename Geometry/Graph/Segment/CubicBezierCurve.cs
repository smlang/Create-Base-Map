using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class CubicBezierCurve : ISegment
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public Distance Top { get { throw (new NotImplementedException()); } }
        public Distance Bottom { get { throw (new NotImplementedException()); } }
        public Distance Right { get { throw (new NotImplementedException()); } }
        public Distance Left { get { throw (new NotImplementedException()); } }

        public Point StartControlPoint { get; set; }
        public Point EndControlPoint { get; set; }

        public Distance Length
        {
            get
            {
                throw (new NotImplementedException());
            }
        }

        public CubicBezierCurve(Point start, Point startControlPoint, Point end, Point endControlPoint)
        {
            Start = start;
            StartControlPoint = startControlPoint;
            End = end;
            EndControlPoint = endControlPoint;
        }

        public Boolean LengthIsZero(Distance epsilon)
        {
            if (Start.GetRelationship(End, epsilon) == Relationship.Apart)
            {
                return false;
            }
            // if start and end at the same point, check if they always match the control points
            if (Start.GetRelationship(StartControlPoint, epsilon) == Relationship.Apart)
            {
                return false;
            }
            return (Start.GetRelationship(EndControlPoint, epsilon) != Relationship.Apart);
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
