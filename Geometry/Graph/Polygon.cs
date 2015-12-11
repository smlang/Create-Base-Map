using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Polygon : Path
    {

        public Area Area
        {
            get
            {
                Area area = new Area(0M, Distance.Unit.Metre, Scale.ten_minus_5);
                foreach (ISegment segment in this)
                {
                    area += segment.Start.Y * segment.End.X;
                    area -= segment.Start.X * segment.End.Y;
                }
                area = area / 2M;
                return area.Absolute();
            }
        }

        public Rotation Rotation
        {
            get
            {
                Area area = new Area(0M, Distance.Unit.Metre, Scale.ten_minus_5);
                foreach (ISegment segment in this)
                {
                    area += segment.Start.Y * segment.End.X;
                    area -= segment.Start.X * segment.End.Y;
                }
                switch (area.Sign)
                {
                    case 1:
                        return Rotation.Clockwise;
                    case -1:
                        return Rotation.Anticlockwise;
                    default:
                        return Rotation.Point;
                }
            }
        }

        public Polygon()
            : base()
        {
        }

        public Polygon(List<ISegment> segments, Distance epsilon)
            : base()
        {
            AddRange(segments, epsilon);
        }

        public Boolean AreaIsZero(Distance epsilon)
        {
            return (Area.IsZero(epsilon));
        }

        public new void Add(ISegment segment, Distance epsilon)
        {
            if (Count > 0)
            {
                RemoveAt(this.Count - 1);
            }

            base.Add(segment, epsilon);

            // close the polygon, if not already closed
            if (End.GetRelationship(Start, epsilon) == Relationship.Apart)
            {
                // insert extra segment if last one does not end where
                // the first segment starts
                Add2(new LinearSegment(End, Start));
            }
            else
            {
                // make start absolutely the same as the end
                this[0].Start = End;
            }
        }

        public new void AddRange(List<ISegment> segments, Distance epsilon)
        {
            if ((segments == null) || (segments.Count == 0))
            {
                return;
            }

            if (Count > 0)
            {
                RemoveAt(this.Count - 1);
            }

            foreach (ISegment segment in segments)
            {
                base.Add(segment, epsilon);
            }

            // close the polygon, if not already closed
            if (End.GetRelationship(Start, epsilon) == Relationship.Apart)
            {
                // insert extra segment if last one does not end where
                // the first segment starts
                Add2(new LinearSegment(End, Start));
            }
            else
            {
                // make start absolutely the same as the end
                this[0].Start = End;
            }
        }

        public bool Merge(Polygon b, List<Polygon> holes, Distance epsilon)
        {
            #region Build data structures
            Rotation aRotation = this.Rotation;
            List<Point> aPointList = new List<Point>();
            List<bool> isAPointInsideList = new List<bool>();
            foreach (ISegment aSegment in this)
            {
                aPointList.Add(aSegment.Start);
                isAPointInsideList.Add(aSegment.Start.GetRelationship(b, epsilon) == Relationship.VertexTouchesFace);
            }

            Rotation bRotation = b.Rotation;
            List<Point> bPointList = new List<Point>();
            List<bool> isBPointInsideList = new List<bool>();
            foreach (ISegment bSegment in b)
            {
                bPointList.Add(bSegment.Start);
                isBPointInsideList.Add(bSegment.Start.GetRelationship(this, epsilon) == Relationship.VertexTouchesFace);
            }
            if (aRotation != bRotation)
            {
                bPointList.Reverse();
                isBPointInsideList.Reverse();
            }

            Dictionary<Point, int> aTransitionPointList = new Dictionary<Point, int>();
            Dictionary<Point, int> bTransitionPointList = new Dictionary<Point, int>();
            for (int aIndex2 = 0; aIndex2 < aPointList.Count; aIndex2++)
            {
                for (int bIndex2 = 0; bIndex2 < bPointList.Count; bIndex2++)
                {
                    if ((aPointList[aIndex2].GetRelationship(bPointList[bIndex2], epsilon) != Relationship.Apart) &&
                        (!aTransitionPointList.ContainsValue(bIndex2)))                         // TODO problems with neighbours
                    {
                        aTransitionPointList.Add(aPointList[aIndex2], bIndex2);
                        bTransitionPointList.Add(bPointList[bIndex2], aIndex2);
                        break;
                    }
                }
            }

            // TODO problem if odd number of transition points - where points either side are both inside or both outside
            if ((aTransitionPointList.Count % 2) == 1)
            {
                return false;
                throw new ApplicationException("Unexpected");
            }
            if (aTransitionPointList.Count == 0)
            {
                return false;
                throw new ApplicationException("Unexpected");
            }
            #endregion

            if (this.Merge2(aPointList, isAPointInsideList, aTransitionPointList, bPointList, isBPointInsideList, bTransitionPointList))
            {
                if (holes != null)
                {
                    Polygon hole = new Polygon();
                    while (hole.Merge2(aPointList, isAPointInsideList, aTransitionPointList, bPointList, isBPointInsideList, bTransitionPointList))
                    {
                        holes.Add(hole);
                        hole = new Polygon();
                    }
                }
            }

            return true;
        }

        private bool Merge2(List<Point> aPointList, List<bool> isAPointInsideList, Dictionary<Point, int> aTransitionPointList, List<Point> bPointList, List<bool> isBPointInsideList, Dictionary<Point, int> bTransitionPointList)
        {
            #region Follow outside of polygons
            List<Point> newPoints = new List<Point>();
            int aIndex = -1;
            int bIndex = -1;

            #region Find A point to start/continue from
            int count = 0;
            while (count < (aPointList.Count - 1))
            {
                count++;
                aIndex++;
                Point aPoint = aPointList[aIndex];
                if ((!isAPointInsideList[aIndex]) && (aPoint != null))
                {
                    newPoints.Add(aPoint);
                    aPointList[aIndex] = null;
                    break;
                }
            }
            #endregion

            if (newPoints.Count == 0)
            {
                return false;
            }

            bool finished = false;
            while (!finished)
            {
                #region Follow A until transition point
                count = 0;
                finished = true;
                while (count < (aPointList.Count - 1))
                {
                    count++;

                    aIndex = (aIndex + 1) % aPointList.Count;
                    Point aPoint = aPointList[aIndex];
                    // Come to end, stop now
                    if (aPoint == null)
                    {
                        break;
                    }

                    newPoints.Add(aPoint);
                    aPointList[aIndex] = null;

                    if (aTransitionPointList.ContainsKey(aPoint))
                    {
                        bIndex = aTransitionPointList[aPoint];
                        bPointList[bIndex] = null;
                        finished = false;
                        break;
                    }
                }

                if (finished)
                {
                    break;
                }
                #endregion

                #region Follow B until transition point
                count = 0;
                finished = true;
                while (count < (bPointList.Count - 1))
                {
                    count++;

                    bIndex = (bIndex + 1) % bPointList.Count;
                    Point bPoint = bPointList[bIndex];
                    // Come to end, stop now
                    if (bPoint == null)
                    {
                        // Should not happen
                        break;
                    }

                    newPoints.Add(bPoint);
                    bPointList[bIndex] = null;

                    // Is next a point still outside, add it to new points
                    if (bTransitionPointList.ContainsKey(bPoint))
                    {
                        aIndex = bTransitionPointList[bPoint];
                        aPointList[aIndex] = null;
                        finished = false;
                        break;
                    }
                }
                #endregion
            }

            #region Build new segment list
            this.Clear();
            Geometry.Point s = newPoints[0];
            Geometry.Point p1 = s;
            Geometry.Point p2 = s;
            for (int i = 1; i < newPoints.Count; i++)
            {
                p2 = newPoints[i];
                this.Add(new LinearSegment(p1, p2));

                p1 = p2;
            }
            this.Add(new LinearSegment(p2, s));
            #endregion
            #endregion

            return true;
        }

        /*
                                 Meet/ | Vertex   Vertex  Vertex  |  Edge     Edge    Edge   |  Face     Face    Face   |  Edge   | Overlap Sub Identical Super
        Object       Subject     Apart | Touches  Touches Touches | Touches  Touches Touches | Touches  Touches Touches | Crosses |   Set   Set    Set     Set
                                       | Vertex    Edge    Face   | Vertex    Edge    Face   | Vertex    Edge    Face   |  Edge   |
        -------------------------------+--------------------------+--------------------------+--------------------------+---------+----------------------------
        2-Dimension  0-Dimension   X   |    X        -       -    |    X        -       -    |    X        -       -    |    -    |    -     -      -       - 
         * */
        public new Relationship GetRelationship(Point point, Distance epsilon)
        {
            if (AreaIsZero(epsilon))
            {
                throw new ApplicationException("Subject Area is a line");
            }

            // Cheap test, point is outside of polygon sphere
            if (point.Y.IsGreaterThan(Top, epsilon)) { return Relationship.Apart; }
            if (point.Y.IsLessThan(Bottom, epsilon)) { return Relationship.Apart; }
            if (point.X.IsGreaterThan(Right, epsilon)) { return Relationship.Apart; }
            if (point.X.IsLessThan(Left, epsilon)) { return Relationship.Apart; }

            Stack<int> position = new Stack<int>();
            int crossCount = 0;

            foreach (ISegment polygonEdge in this)
            {
                #region Skip edges that are below the point y, and do not crosses or touches point x
                if (point.Y.IsGreaterThan(polygonEdge.Top, epsilon)) { continue; }
                if (point.X.IsGreaterThan(polygonEdge.Right, epsilon)) { continue; }
                if (point.X.IsLessThan(polygonEdge.Left, epsilon)) { continue; }
                #endregion

                #region Stop now if point is on edge
                switch (polygonEdge.GetRelationship(point, epsilon, null))
                {
                    case Relationship.VertexTouchesVertex:
                        return Relationship.VertexTouchesVertex;
                    case Relationship.EdgeTouchesVertex:
                        return Relationship.VertexTouchesEdge;
                    default:
                        break;
                }
                #endregion

                #region Simple case where edge crosses point's X, but is the crossing bottom above point's Y
                if ((point.X.IsLessThan(polygonEdge.Right, epsilon)) && (point.X.IsGreaterThan(polygonEdge.Left, epsilon)))
                {
                    // Simple case where edge is completely above point's Y axis 
                    if (point.Y.IsLessThan(polygonEdge.Bottom, epsilon))
                    {
                        crossCount++;
                    }
                    // Need to calculate polygon's intersect of point's X axis
                    else
                    {
                        // Crossing points Y axis
                        decimal ratio = (point.X - polygonEdge.Start.X) / (polygonEdge.End.X - polygonEdge.Start.X);
                        Distance yIntersect = (ratio * (polygonEdge.End.Y - polygonEdge.Start.Y)) + polygonEdge.Start.Y;
                        if (point.Y.IsLessThan(yIntersect, epsilon))
                        {
                            crossCount++;
                        }
                    }
                    continue;
                }
                #endregion

                #region Complex Case, where segment terminates on point's X, and on or above point's Y
                bool startAtX = (point.X.IsEqualTo(polygonEdge.Start.X, epsilon) && point.Y.IsNotGreaterThan(polygonEdge.Start.Y, epsilon));
                bool endAtX = (point.X.IsEqualTo(polygonEdge.End.X, epsilon) && point.Y.IsNotGreaterThan(polygonEdge.End.Y, epsilon));

                if (startAtX != endAtX)
                {
                    if (startAtX)
                    {
                        if (point.X.IsLessThan(polygonEdge.End.X, epsilon))
                        {
                            position.Push(1);
                        }
                        else
                        {
                            position.Push(-1);
                        }
                    }
                    if (endAtX)
                    {
                        if (point.X.IsGreaterThan(polygonEdge.Start.X, epsilon))
                        {
                            position.Push(-1);
                        }
                        else
                        {
                            position.Push(1);
                        }
                    }
                }
                #endregion
            }

            #region Process Complex Case
            while (position.Count >= 2)
            {
                int p2 = position.Pop();
                int p1 = position.Pop();

                if (p1 != p2)
                {
                    crossCount++;
                }
            }
            #endregion

            if ((crossCount % 2) == 1) { return Relationship.FaceTouchesVertex; } // moving points upwards crosses odd number of times

            return Relationship.Apart;
        }

        public new Relationship GetRelationship(ISegment segment, Distance epsilon)
        {
            if (segment is LinearSegment)
            {
                return GetRelationship((LinearSegment)segment, epsilon);
            }

            throw (new NotImplementedException());
        }

        public new Relationship GetRelationship(LinearSegment segment, Distance epsilon)
        {
            if (AreaIsZero(epsilon))
            {
                throw new ApplicationException("Subject Area is a line");
            }

            if (segment.LengthIsZero(epsilon))
            {
                throw new ApplicationException("Object Segment is a point");
            }

            throw (new NotImplementedException());
        }

        public new Relationship GetRelationship(Path path, Distance epsilon)
        {
            if (AreaIsZero(epsilon))
            {
                throw new ApplicationException("Subject Area is a line");
            }

            if (path.LengthIsZero(epsilon))
            {
                throw new ApplicationException("Object Path is a point");
            }

            throw (new NotImplementedException());
        }

        /*
                         Meet/ |         | Vertex   Vertex  Vertex  |  Edge     Edge    Edge   |  Face     Face    Face   | Overlap Sub Identical Super
Object       Subject     Apart | Crosses | Touches  Touches Touches | Touches  Touches Touches | Touches  Touches Touches |   Set   Set    Set     Set
                               |         | Vertex    Edge    Face   | Vertex    Edge    Face   | Vertex    Edge    Face   |
-------------------------------+---------+--------------------------+--------------------------+--------------------------+----------------------------
2-Dimension  1-Dimension   X   |    X    |    X        X       -    |    X        X       -    |    X        X       -    |    -     -      -       - <---+
2-Dimension  2-Dimension   X   |    X    |    X        X       X    |    X        X       X    |    X        X       X    |    X     X      X       X
*/
        public new Relationship GetRelationship(Polygon polygon, Distance epsilon)
        {
            if (AreaIsZero(epsilon))
            {
                throw new ApplicationException("Subject Area is a line");
            }

            if (polygon.AreaIsZero(epsilon))
            {
                throw new ApplicationException("Object Area is a line");
            }
            /*
            Relationship result = Relationship.Apart;

            #region Relationship between both Polygons
            bool aAndBOverlap = false;
            bool bInsideA = false;
            int aPointsOutsideOfB = 0;
            int aPointsInsideOfB = 0;
            foreach (ISegment aSegment in this)
            {
                switch (aSegment.GetRelationship(polygon, epsilon))
                {
                    case Relationship.Cross:

                }

                if (aPoint.GetRelationship(b, Common.EPSILON) != Relationship.Apart)
                {
                    aPointsInsideOfB++;
                }
                else
                {
                    aPointsOutsideOfB++;
                }
                if ((aPointsInsideOfB != 0) && (aPointsOutsideOfB != 0))
                {
                    aAndBOverlap = true;
                    break;
                }
            }
            if (!aAndBOverlap)
            {
                if (aPointsInsideOfB != 0)
                {
                    aInsideB = true;
                }
                else
                {
                    // is B inside A ?
                    Ocad.Model.Point bPoint = areaObjectsSymbolList[bIndex].MappedObject.Points[0];
                    if (bPoint.GetRelationship(a, Common.EPSILON) != Relationship.Apart)
                    {
                        bInsideA = true;
                    }
                }
            }
            #endregion
            */

            throw (new NotImplementedException());
        }
    }
}
