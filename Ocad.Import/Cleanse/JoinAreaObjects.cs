using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class Cleanse
    {
        public static void JoinAreaObjects(this Ocad.Model.Map ocadMap)
        {
            #region Setup data structure to search for neighbours
            List<JoinAreaDetails> areaObjectsSymbolList = new List<JoinAreaDetails>();
            foreach (Ocad.Model.AbstractObject obj in ocadMap.Objects)
            {
                #region Check object is a open line
                if ((obj.FeatureType != Model.Type.FeatureType.Area) ||
                    !(obj is Ocad.Model.SymbolObject))
                {
                    continue;
                }
                #endregion

                areaObjectsSymbolList.Add(
                    new JoinAreaDetails() { 
                        MappedObject = (Ocad.Model.SymbolObject)obj, 
                        Polygon = obj.GetPolygon(Common.EPSILON)});
            }
            #endregion

            for (int aIndex = 0; aIndex < areaObjectsSymbolList.Count - 1; aIndex++)
            {
                if (areaObjectsSymbolList[aIndex] == null)
                {
                    continue;
                }
                Polygon a = areaObjectsSymbolList[aIndex].Polygon;

                bool aInsideB = false;
                for (int bIndex = aIndex + 1; bIndex < areaObjectsSymbolList.Count; bIndex++)
                {
                    #region Check A and B are in bound and same type
                    if (areaObjectsSymbolList[bIndex] == null)
                    {
                        // B has already been removed or merged
                        continue;
                    }
                    if (areaObjectsSymbolList[aIndex].MappedObject.Symbol.Number != areaObjectsSymbolList[bIndex].MappedObject.Symbol.Number)
                    {
                        // A and B must be same type
                        continue;
                    }

                    Polygon b = areaObjectsSymbolList[bIndex].Polygon;
                    // Check if a and b are within bound of each other
                    if (a.Top.IsLessThan(b.Bottom, Common.EPSILON)) { continue; }
                    if (b.Top.IsLessThan(a.Bottom, Common.EPSILON)) { continue; }
                    if (a.Right.IsLessThan(b.Left, Common.EPSILON)) { continue; }
                    if (b.Right.IsLessThan(a.Left, Common.EPSILON)) { continue; }
                    #endregion

                    // TODO What about touching polygons - simple case of two or more equal points in both polygons
                    //Relationship r = a.GetRelationship(b, Common.EPSILON);

                    #region Relationship between both Polygons
                    bool aAndBOverlap = false;
                    bool bInsideA = false;
                    int aPointsOutsideOfB = 0;
                    int aPointsOnB = 0;
                    int aPointsInsideOfB = 0;
                    foreach (Ocad.Model.Point aPoint in areaObjectsSymbolList[aIndex].MappedObject.Points)
                    {
                        bool touch = false;
                        Relationship aRelationship = aPoint.GetRelationship(b, Common.EPSILON);
                        if (aRelationship == Relationship.Apart)
                        {
                            aPointsOutsideOfB++;
                        }
                        else if ((aRelationship & (Relationship.VertexTouchesEdge | Relationship.VertexTouchesVertex)) != Relationship.Apart)
                        {
                            touch = true;
                            aPointsOnB++;
                        }
                        else
                        {
                            aPointsInsideOfB++;
                        }
                        if ((aPointsInsideOfB != 0) && (aPointsOutsideOfB != 0))
                        {
                            aAndBOverlap = true;
                            break;
                        }
                        if ((aPointsInsideOfB != 0) && (aPointsOnB != 0))
                        {
                            aAndBOverlap = true;
                            break;
                        }
                        if (aPointsOnB == 2)
                        {
                            aAndBOverlap = true;
                            break;
                        }

                        if (!touch)
                        {
                            aPointsOnB = 0;
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

                    #region Remove, Merge or do nothing
                    if (aInsideB)
                    {
                        // Remove A
                        ocadMap.Objects.Remove(areaObjectsSymbolList[aIndex].MappedObject);
                    }
                    else if (bInsideA)
                    {
                        // Remove B
                        ocadMap.Objects.Remove(areaObjectsSymbolList[bIndex].MappedObject);
                        areaObjectsSymbolList[bIndex] = null;
                    }
                    else if (aAndBOverlap)
                    {
                        // Merge A and B
                        List<Polygon> holes = new List<Polygon>();
                        if (!a.Merge(b, holes, Common.EPSILON))
                        {
                            // TO DO ISSUE WITH MERGE
                            continue;
                        }

                        List<Ocad.Model.Point> mergedPoints = new List<Model.Point>();
                        foreach (Geometry.Point aPoint in a.Points)
                        {
                            mergedPoints.Add((Ocad.Model.Point)aPoint);
                        }
                        foreach (Polygon hole in holes)
                        {
                            Ocad.Model.Point startHole = (Ocad.Model.Point)hole.Points[0];
                            startHole.MainPointFlag = startHole.MainPointFlag | Model.Type.PointFlag.HolePoint;
                            foreach (Geometry.Point holePoint in hole.Points)
                            {
                                mergedPoints.Add((Ocad.Model.Point)holePoint);
                            }
                        }
                        areaObjectsSymbolList[aIndex].MappedObject.Points = mergedPoints;

                        ocadMap.Objects.Remove(areaObjectsSymbolList[bIndex].MappedObject);
                        areaObjectsSymbolList[bIndex] = null;
                    }
                    else
                    {
                        continue;
                    }
                    break;
                    #endregion
                }

                if (aInsideB)
                {
                    // A has been removed, move on to next one
                    continue;
                }
            }
        }


        private class JoinAreaDetails
        {
            public Ocad.Model.SymbolObject MappedObject;
            public Polygon Polygon;
        }
    }
}
