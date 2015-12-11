using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public interface ISegment
    {
        Point Start { get; set; }
        Point End { get; set; }

        Distance Top { get; }
        Distance Bottom { get; }
        Distance Right { get; }
        Distance Left { get; }

        Distance Length { get; }
        Boolean LengthIsZero(Distance epsilon);

        Relationship GetRelationship(Point point, Distance epsilon, List<Decimal> thisIntersectRatios);
        Relationship GetRelationship(ISegment segment, Distance epsilon);
        Relationship GetRelationship(Path path, Distance epsilon);
        Relationship GetRelationship(Polygon polygon, Distance epsilon);
        
        DirectionSide SideOf(Point point, Distance epsilon);

        Boolean IntersectPoints(ISegment other, Distance epsilon, List<Decimal> thisIntersectRatios, List<Decimal> otherIntersectRatios);
    }
}
