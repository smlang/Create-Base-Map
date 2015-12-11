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
        public static void ConvertToDashPoints(this Ocad.Model.Map ocadMap)
        {
            #region Setup data structure to search for neighbours
            Dictionary<long, PointDetails> pointDetailsList = new Dictionary<long, PointDetails>();
            foreach (Ocad.Model.AbstractObject obj in ocadMap.Objects)
            {
                AddPoint(pointDetailsList, obj.Points[0], false);
                AddPoint(pointDetailsList, obj.Points[obj.Points.Count - 1], false);
                for (int i = 1; i <= obj.Points.Count - 2; i++)
                {
                    AddPoint(pointDetailsList, obj.Points[i], true);
                }
            }
            #endregion

            foreach (long pointId in pointDetailsList.Keys)
            {
                if (pointDetailsList[pointId].MidPointList.Count == 0)
                {
                    // no midpoint to convert to dash
                    continue;
                }

                List<Ocad.Model.Point> neighbouringPointList = new List<Ocad.Model.Point>();

                // Get line object in centre
                long index = pointId;
                MergePointDetailsList(neighbouringPointList, pointDetailsList, index);
                // Get line object to near east
                index += Int32.MaxValue;
                MergePointDetailsList(neighbouringPointList, pointDetailsList, index);
                // Get line object to near southeast
                index -= 1;
                MergePointDetailsList(neighbouringPointList, pointDetailsList, index);
                // Get line object to near south
                index -= Int32.MaxValue;
                MergePointDetailsList(neighbouringPointList, pointDetailsList, index);
                // Get line object to near southwest
                index -= Int32.MaxValue;
                MergePointDetailsList(neighbouringPointList, pointDetailsList, index);
                // Get line object to near west
                index += 1;
                MergePointDetailsList(neighbouringPointList, pointDetailsList, index);
                // Get line object to near northwest
                index += 1;
                MergePointDetailsList(neighbouringPointList, pointDetailsList, index);
                // Get line object to near north
                index += Int32.MaxValue;
                MergePointDetailsList(neighbouringPointList, pointDetailsList, index);
                // Get line object to near northeast
                index += Int32.MaxValue;
                MergePointDetailsList(neighbouringPointList, pointDetailsList, index);

                if (neighbouringPointList.Count == 1)
                {
                    // need at least two neighbouring points to make a junction
                    continue;
                }

                foreach (Ocad.Model.Point currentMidPoint in pointDetailsList[pointId].MidPointList)
                {
                    if (currentMidPoint.MainPointFlag != Model.Type.PointFlag.BasicPoint)
                    {
                        // mid point is not basic, do not convert to a dash point
                        // move to next midpoint
                        continue;
                    }

                    foreach (Ocad.Model.Point neighbouringPoint in neighbouringPointList)
                    {
                        if (currentMidPoint.Equals(neighbouringPoint))
                        {
                            // move to next neighbour
                            continue;
                        }

                        if (currentMidPoint.GetRelationship(neighbouringPoint, Common.EPSILON) != Relationship.Apart)
                        {
                            // found a close neighbour, look no further
                            currentMidPoint.MainPointFlag |= Model.Type.PointFlag.DashPoint;
                            break;
                        }
                    }
                }
            }
        }

        private static void AddPoint(Dictionary<long, PointDetails> pointDetailsList, Ocad.Model.Point p, bool isMidPoint)
        {
            long pointId = GetPointId(p);
            PointDetails pointDetails;
            if (pointDetailsList.ContainsKey(pointId))
            {
                pointDetails = pointDetailsList[pointId];
            }
            else
            {
                pointDetails = new PointDetails();
                pointDetailsList.Add(pointId, pointDetails);
            }

            if (isMidPoint)
            {
                pointDetails.MidPointList.Add(p);
            }
            else
            {
                pointDetails.EndPointList.Add(p);
            }
        }

        private class PointDetails
        {
            public List<Ocad.Model.Point> EndPointList = new List<Ocad.Model.Point>();
            public List<Ocad.Model.Point> MidPointList = new List<Ocad.Model.Point>();
        }

        private static void MergePointDetailsList(List<Ocad.Model.Point> pointList, Dictionary<long, PointDetails> pointDetailsList, long index)
        {
            if (pointDetailsList.ContainsKey(index))
            {
                PointDetails pointDetails = pointDetailsList[index];
                pointList.AddRange(pointDetails.EndPointList);
                pointList.AddRange(pointDetails.MidPointList);
            }
        }

    }
}
