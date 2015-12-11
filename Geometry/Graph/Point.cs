using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Point
    {
        public Distance X { get; set; }
        public Distance Y { get; set; }

        public Point(Distance x, Distance y)
        {
            X = x;
            Y = y;
        }

        public Point(Point p, Distance d, Angle a)
        {
            X = p.X + (d * a.Cos());
            Y = p.Y + (d * a.Sin());
        }

        protected Point()
        {
        }

        public Distance DistanceFrom(Point other)
        {
            Distance xDiff = X - other.X;
            Distance yDiff = Y - other.Y;
            return ((xDiff * xDiff) + (yDiff * yDiff)).SquareRoot();
        }

        /*
                                 Meet/ | Vertex   Vertex  Vertex  |  Edge     Edge    Edge   |  Face     Face    Face   |  Edge   | Overlap Sub Identical Super
        Object       Subject     Apart | Touches  Touches Touches | Touches  Touches Touches | Touches  Touches Touches | Crosses |   Set   Set    Set     Set
                                       | Vertex    Edge    Face   | Vertex    Edge    Face   | Vertex    Edge    Face   |  Edge   |
        -------------------------------+--------------------------+--------------------------+--------------------------+---------+----------------------------
        0-Dimension  0-Dimension   X   |    X        -       -    |    -        -       -    |    -        -       -    |    -    |    -     -      X       -
        */
        public Relationship GetRelationship(Point other, Distance epsilon)
        {
            // Cheap calculation
            if (!X.IsEqualTo(other.X, epsilon)) { return Relationship.Apart; }
            if (!Y.IsEqualTo(other.Y, epsilon)) { return Relationship.Apart; }

            // More expensive
            if (DistanceFrom(other).IsZero(epsilon))
            {
                return (Relationship.VertexTouchesVertex | Relationship.IdenticalSet);
            }

            return Relationship.Apart;
        }

        /*
                                 Meet/ | Vertex   Vertex  Vertex  |  Edge     Edge    Edge   |  Face     Face    Face   |  Edge   | Overlap Sub Identical Super
        Object       Subject     Apart | Touches  Touches Touches | Touches  Touches Touches | Touches  Touches Touches | Crosses |   Set   Set    Set     Set
                                       | Vertex    Edge    Face   | Vertex    Edge    Face   | Vertex    Edge    Face   |  Edge   |
        -------------------------------+--------------------------+--------------------------+--------------------------+---------+----------------------------
        0-Dimension  1-Dimension   X   |    X        X       -    |    -        -       -    |    -        -       -    |    -    |    -     -      -       -  
        */
        public Relationship GetRelationship(ISegment segment, Distance epsilon, List<Decimal> thisIntersectRatios)
        {
            switch (segment.GetRelationship(this, epsilon, thisIntersectRatios))
            {
                case Relationship.VertexTouchesVertex:
                    return Relationship.VertexTouchesVertex;
                case Relationship.EdgeTouchesVertex:
                    return Relationship.VertexTouchesEdge;
                default:
                    return Relationship.Apart;
            }
        }

        public Relationship GetRelationship(Path path, Distance epsilon)
        {
            switch (path.GetRelationship(this, epsilon))
            {
                case Relationship.VertexTouchesVertex:
                    return Relationship.VertexTouchesVertex;
                case Relationship.EdgeTouchesVertex:
                    return Relationship.VertexTouchesEdge;
                default:
                    return Relationship.Apart;
            }
        }

        /*
                                 Meet/ | Vertex   Vertex  Vertex  |  Edge     Edge    Edge   |  Face     Face    Face   |  Edge   | Overlap Sub Identical Super
        Object       Subject     Apart | Touches  Touches Touches | Touches  Touches Touches | Touches  Touches Touches | Crosses |   Set   Set    Set     Set
                                       | Vertex    Edge    Face   | Vertex    Edge    Face   | Vertex    Edge    Face   |  Edge   |
        -------------------------------+--------------------------+--------------------------+--------------------------+---------+----------------------------
        0-Dimension  2-Dimension   X   |    X        X       X    |    -        -       -    |    -        -       -    |    -    |    -     -      -       - <---|-+
        */
        public Relationship GetRelationship(Polygon polygon, Distance epsilon)
        {
            switch (polygon.GetRelationship(this, epsilon))
            {
                case Relationship.VertexTouchesVertex:
                    return Relationship.VertexTouchesVertex;
                case Relationship.EdgeTouchesVertex:
                    return Relationship.VertexTouchesEdge;
                case Relationship.FaceTouchesVertex:
                    return Relationship.VertexTouchesFace;
                default:
                    return Relationship.Apart;
            }
        }

    }
}
