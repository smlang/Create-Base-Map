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
        public static void RemoveShortSegments(this Ocad.Model.Map ocadMap)
        {
            List<Ocad.Model.AbstractObject> objectToRemove = new List<Model.AbstractObject>();

            foreach (Ocad.Model.AbstractObject obj in ocadMap.Objects)
            {
                int minimumPointCount;
                switch (obj.FeatureType)
                {
                    case Model.Type.FeatureType.Line:
                        minimumPointCount = 2;
                        break;
                    case Model.Type.FeatureType.Area:
                        minimumPointCount = 3;
                        break;
                    default:
                        continue;
                }


                List<Ocad.Model.Point> points = obj.Points;
                if ((points == null) ||
                    (points.Count < minimumPointCount))
                {
                    objectToRemove.Add(obj);
                    continue;
                }

                int index = 0;
                bool reverse = false;
                int fixPointIndex = 0;
                int neighbourPointIndex = 1;
                Ocad.Model.Point fixPoint = points[0];

                while (true)
                {
                    Ocad.Model.Point neighbourPoint = points[neighbourPointIndex];

                    if (fixPoint.GetRelationship(neighbourPoint, Common.EPSILON) != Relationship.Apart)
                    {
                        #region Remove neighbouring point
                        if (reverse)
                        {
                            if (neighbourPoint.FirstBezier != null)
                            {
                                fixPoint.FirstBezier = neighbourPoint.FirstBezier;
                                throw (new ApplicationException("TODO check if this works"));
                            }
                        }
                        else
                        {
                            if (neighbourPoint.SecondBezier != null)
                            {
                                fixPoint.SecondBezier = neighbourPoint.SecondBezier;
                                throw (new ApplicationException("TODO check if this works"));
                            }
                        }

                        points.RemoveAt(neighbourPointIndex);
                        if (index >= (points.Count / 2))
                        {
                            break;
                        }
                        if (reverse)
                        {
                            fixPointIndex--;
                            neighbourPointIndex--;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Move on to next fix point
                        if (reverse)
                        {
                            index++;
                            if (index >= (points.Count / 2))
                            {
                                break;
                            }
                        }
                        reverse = !reverse;

                        fixPointIndex = reverse ? (points.Count - 1) - index : index;
                        fixPoint = points[fixPointIndex];
                        neighbourPointIndex = reverse ? fixPointIndex - 1 : fixPointIndex + 1;
                        #endregion
                    }
                }

                if (points.Count < minimumPointCount) 
                { 
                    objectToRemove.Add(obj); 
                }
                else if ((minimumPointCount == 2) && obj.GetPath(Common.EPSILON).LengthIsZero(Common.EPSILON))
                {
                    objectToRemove.Add(obj); 
                }
                else if ((minimumPointCount == 3) && obj.GetPolygon(Common.EPSILON).AreaIsZero(Common.EPSILON))
                {
                    objectToRemove.Add(obj);
                }


            }

            // Remove objects with insufficent points
            foreach (Ocad.Model.AbstractObject obj in objectToRemove)
            {
                ocadMap.Objects.Remove(obj);
            }
        }

    }
}
