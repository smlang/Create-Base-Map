using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class Cleanse
    {
        /// <summary>
        /// Favour Endpoints and work inwards, to prevent drifting if the object consists only of short segments.
        /// </summary>
        /// <param name="ocadMap"></param>
        public static void RemoveBorders(this Ocad.Model.Map ocadMap)
        {
            #region Get Lakes
            List<Polygon> areaPolygonList = new List<Polygon>();
            List<Ocad.Model.SymbolObject> lineObjectList = new List<Model.SymbolObject>();
            foreach (Ocad.Model.AbstractObject obj in ocadMap.Objects)
            {
                if (!(obj is Ocad.Model.SymbolObject))
                {
                    continue;
                }

                Ocad.Model.SymbolObject areaObject = obj as Ocad.Model.SymbolObject;

                switch (areaObject.Symbol.Number)
                {
                    case "301.0" :
                    case "302.0":
                        Polygon polygon = areaObject.GetPolygon(Common.EPSILON);
                        areaPolygonList.Add(polygon);
                        break;
                    case "301.1":
                    case "302.1":
                    case "305.0":
                        lineObjectList.Add(areaObject);
                        break;
                    default:
                        continue;
                }
            }
            #endregion

            foreach (Ocad.Model.SymbolObject lineObject in lineObjectList)
            {
                #region Find segments to keep
                List<List<Ocad.Model.Point>> keepLegList = new List<List<Ocad.Model.Point>>();
                List<Ocad.Model.Point> keepLeg = null;
                bool altered = false;

                Path linePath = lineObject.GetPath(Common.EPSILON);
                Ocad.Model.Point p1 = lineObject.Points[0];
                Ocad.Model.Point p2;
                for (int i = 1; i < lineObject.Points.Count; i++)
                {
                    p2 = lineObject.Points[i];
                    bool remove = false;

                    foreach (Polygon areaPolygon in areaPolygonList)
                    {
                        // Is path within sphere of polygon
                        if (linePath.Top.IsLessThan(areaPolygon.Bottom, Common.EPSILON)) { continue; }
                        if (linePath.Bottom.IsGreaterThan(areaPolygon.Top, Common.EPSILON)) { continue; }
                        if (linePath.Right.IsLessThan(areaPolygon.Left, Common.EPSILON)) { continue; }
                        if (linePath.Left.IsGreaterThan(areaPolygon.Right, Common.EPSILON)) { continue; }

                        if ((areaPolygon.GetRelationship(p1, Common.EPSILON) != Relationship.Apart) &&
                            (areaPolygon.GetRelationship(p2, Common.EPSILON) != Relationship.Apart))
                        {
                            // Segment is inside of area, and should not be kept
                            // Assumes No segments cross
                            remove = true;
                            break;
                        }
                    }

                    if (!remove)
                    {
                        if (keepLeg == null)
                        {
                            keepLeg = new List<Ocad.Model.Point>();
                            keepLegList.Add(keepLeg);
                            keepLeg.Add((Ocad.Model.Point)p1);
                        }
                        keepLeg.Add((Ocad.Model.Point)p2);
                    }
                    else
                    {
                        keepLeg = null;
                    }

                    p1 = p2;
                }
                #endregion

                if (keepLegList.Count == 0)
                {
                    // Completely remove
                    ocadMap.Objects.Remove(lineObject);
                }
                else if ((lineObject.Points.Count != keepLegList[0].Count) || altered)
                {
                    // Keep a bit
                    lineObject.Points = keepLegList[0];

                    for (int i = 1; i < keepLegList.Count; i++)
                    {
                        Ocad.Model.SymbolObject newSubLineObject = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, lineObject.Symbol);
                        newSubLineObject.Points = keepLegList[i];
                    }
                }
                // Otherwise leave intact
            }
        }

    }
}
