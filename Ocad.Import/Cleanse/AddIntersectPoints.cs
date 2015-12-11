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
        public static void AddIntersectPoints(this Ocad.Model.Map ocadMap)
        {
            #region Build Data Structure
            SortedDictionary<decimal, EventDetail> eventDetailList = new SortedDictionary<decimal, EventDetail>();
            IComparer<SegmentDetail> comparer = new SegmentDetailComparer();
            List<SegmentDetail> segmentDetailList = new List<SegmentDetail>();

            foreach (Ocad.Model.AbstractObject abstractObject in ocadMap.Objects)
            {
                Ocad.Model.SymbolObject symbolObject = abstractObject as Ocad.Model.SymbolObject;
                if (symbolObject == null)
                {
                    continue;
                }

                Ocad.Model.Point aPoint = symbolObject.Points[0];
                decimal aX = aPoint.X[0, Distance.Unit.Metre, Scale.ten_minus_5];
                decimal aY = aPoint.Y[0, Distance.Unit.Metre, Scale.ten_minus_5];

                EventDetail aEventDetail;
                if (eventDetailList.ContainsKey(aX))
                {
                    aEventDetail = eventDetailList[aX];
                }
                else
                {
                    aEventDetail = new EventDetail();
                    eventDetailList.Add(aX, aEventDetail);
                }

                Ocad.Model.Point bPoint;
                decimal bX;
                decimal bY;
                EventDetail bEventDetail;

                for (int i = 1; i < symbolObject.Points.Count; i++)
                {
                    bPoint = symbolObject.Points[i];
                    bX = bPoint.X[0, Distance.Unit.Metre, Scale.ten_minus_5];
                    bY = bPoint.Y[0, Distance.Unit.Metre, Scale.ten_minus_5];

                    SegmentDetail segmentDetail = new SegmentDetail()
                    {
                        MappedObject = symbolObject,
                        StartPoint = aPoint,
                        EndPoint = bPoint,
                        Segment = new LinearSegment(aPoint, bPoint),
                        IntersectRatios = new List<decimal>(),
                        UpperY = (aY >= bY) ? aY : bY,
                        LowerY = (aY < bY) ? aY : bY
                    };
                    segmentDetailList.Add(segmentDetail);

                    if (aX == bX)
                    {
                        bEventDetail = aEventDetail;

                        // aEvent is same as bEvent
                        int aIndex = aEventDetail.CompareOnlySegmentList.BinarySearch(segmentDetail, comparer);
                        aIndex = aIndex >= 0 ? aIndex : -aIndex - 1;
                        aEventDetail.CompareOnlySegmentList.Insert(aIndex, segmentDetail);

                    }
                    else
                    {
                        if (eventDetailList.ContainsKey(bX))
                        {
                            bEventDetail = eventDetailList[bX];
                        }
                        else
                        {
                            bEventDetail = new EventDetail();
                            eventDetailList.Add(bX, bEventDetail);
                        }

                        if (aX < bX)
                        {
                            int aIndex = aEventDetail.AddSegmentList.BinarySearch(segmentDetail, comparer);
                            aIndex = aIndex >= 0 ? aIndex : -aIndex - 1;
                            aEventDetail.AddSegmentList.Insert(aIndex, segmentDetail);

                            int bIndex = bEventDetail.RemoveSegmentList.BinarySearch(segmentDetail, comparer);
                            bIndex = bIndex >= 0 ? bIndex : -bIndex - 1;
                            bEventDetail.RemoveSegmentList.Insert(bIndex, segmentDetail);
                        }
                        else
                        {
                            int aIndex = aEventDetail.RemoveSegmentList.BinarySearch(segmentDetail, comparer);
                            aIndex = aIndex >= 0 ? aIndex : -aIndex - 1;
                            aEventDetail.RemoveSegmentList.Insert(aIndex, segmentDetail);

                            int bIndex = bEventDetail.AddSegmentList.BinarySearch(segmentDetail, comparer);
                            bIndex = bIndex >= 0 ? bIndex : -bIndex - 1;
                            bEventDetail.AddSegmentList.Insert(bIndex, segmentDetail);
                        }
                    }

                    aPoint = bPoint;
                    aX = bX;
                    aY = bY;
                    aEventDetail = bEventDetail;
                }
            }
            #endregion

            #region Search for intersecting points
            List<SegmentDetail> activeSegmentList = new List<SegmentDetail>();
            foreach (EventDetail eventDetail in eventDetailList.Values)
            {
                #region Find Intersection between New Segments and Active Segments
                for (int i = 0; i < eventDetail.AddSegmentList.Count; i++)
                {
                    SegmentDetail newSegmentDetail = eventDetail.AddSegmentList[i];
                    AddIntersectPointsWithNewSegment(newSegmentDetail, activeSegmentList);
                }
                #endregion

                #region Add New Segments to Active Segments
                int addIndex = 0;
                for (int i = 0; i < eventDetail.AddSegmentList.Count; i++)
                {
                    SegmentDetail newSegmentDetail = eventDetail.AddSegmentList[i];
                    addIndex = activeSegmentList.BinarySearch(addIndex, activeSegmentList.Count - addIndex, newSegmentDetail, comparer);
                    addIndex = addIndex >= 0 ? addIndex : -addIndex - 1;

                    activeSegmentList.Insert(addIndex, newSegmentDetail);
                }
                #endregion

                #region Find Intersection between Vertical Segments and Active Segments, could include new and old segments
                for (int i = 0; i < eventDetail.CompareOnlySegmentList.Count; i++)
                {
                    SegmentDetail newSegmentDetail = eventDetail.CompareOnlySegmentList[i];
                    AddIntersectPointsWithNewSegment(newSegmentDetail, activeSegmentList);
                }
                #endregion

                #region Remove Old Active Segments
                int removeIndex = 0;
                for (int i = 0; i < eventDetail.RemoveSegmentList.Count; i++)
                {
                    removeIndex = activeSegmentList.BinarySearch(removeIndex, activeSegmentList.Count - removeIndex, eventDetail.RemoveSegmentList[i], comparer);
                    activeSegmentList.RemoveAt(removeIndex);
                }
                #endregion
            }
            #endregion

            #region Insert discovered intersecting points
            Dictionary<Ocad.Model.SymbolObject, List<SegmentDetail>> cutSegmentDetailList = new Dictionary<Model.SymbolObject, List<SegmentDetail>>();
            foreach (SegmentDetail segmentDetail in segmentDetailList)
            {
                if (segmentDetail.IntersectRatios.Count == 0)
                {
                    continue;
                }

                List<Ocad.Model.Point> currentPoints = segmentDetail.MappedObject.Points;
                int insertIndex = currentPoints.IndexOf(segmentDetail.StartPoint);
                if (insertIndex == -1)
                {
                    continue;
                }

                #region Add new points
                if (!segmentDetail.Remove)
                {
                    LinearSegment segment = segmentDetail.Segment;
                    Distance startX = segmentDetail.StartPoint.X;
                    Distance startY = segmentDetail.StartPoint.Y;
                    Distance dx = segmentDetail.EndPoint.X - startX;
                    Distance dy = segmentDetail.EndPoint.Y - startY;

                    Ocad.Model.Point startPoint = segmentDetail.StartPoint;
                    if (segmentDetail.StartRatio != 0M)
                    {
                        Geometry.Point newPoint = new Point(
                            startX + (dx * segmentDetail.StartRatio),
                            startY + (dy * segmentDetail.StartRatio));
                        startPoint = new Model.Point(newPoint);
                    }
                    Ocad.Model.Point previousPoint = startPoint;

                    Ocad.Model.Point endPoint = segmentDetail.EndPoint;
                    if (segmentDetail.EndRatio != 1M)
                    {
                        Geometry.Point newPoint = new Point(
                            startX + (dx * segmentDetail.EndRatio),
                            startY + (dy * segmentDetail.EndRatio));
                        endPoint = new Model.Point(newPoint);
                    }

                    if (startPoint.GetRelationship(endPoint, Common.EPSILON) != Relationship.Apart)
                    {
                        segmentDetail.Remove = true;
                    }
                    else
                    {
                        #region Adjust object start if start segment, otherwise insert
                        if (startPoint.GetRelationship(segmentDetail.StartPoint, Common.EPSILON) == Relationship.Apart)
                        {
                            if (insertIndex == 0)
                            {
                                currentPoints[0] = startPoint;
                            }
                            else
                            {
                                if (cutSegmentDetailList.ContainsKey(segmentDetail.MappedObject))
                                {
                                    cutSegmentDetailList[segmentDetail.MappedObject].Add(segmentDetail);
                                }
                                else
                                {
                                    cutSegmentDetailList.Add(segmentDetail.MappedObject, new List<SegmentDetail>() { segmentDetail });
                                }

                                insertIndex++;
                                currentPoints.Insert(insertIndex, startPoint);
                            }
                        }
                        #endregion

                        #region Insert new points
                        segmentDetail.IntersectRatios.Sort();
                        int originalInsertIndex = insertIndex;
                        foreach (decimal intersectRatio in segmentDetail.IntersectRatios)
                        {
                            if ((intersectRatio < segmentDetail.StartRatio) || (intersectRatio > segmentDetail.EndRatio))
                            {
                                continue;
                            }

                            Geometry.Point newPoint = new Point(
                                startX + (dx * intersectRatio),
                                startY + (dy * intersectRatio));

                            if ((newPoint.GetRelationship(previousPoint, Common.EPSILON) != Relationship.Apart) ||
                                (newPoint.GetRelationship(endPoint, Common.EPSILON) != Relationship.Apart))
                            {
                                continue;
                            }

                            Ocad.Model.Point newOcadPoint = new Model.Point(newPoint);
                            insertIndex++;
                            currentPoints.Insert(insertIndex, newOcadPoint);

                            previousPoint = newOcadPoint;
                        }
                        #endregion

                        #region Adjust object end if end segment, other insert
                        if (endPoint.GetRelationship(segmentDetail.EndPoint, Common.EPSILON) == Relationship.Apart)
                        {
                            if (insertIndex == (currentPoints.Count - 2))
                            {
                                currentPoints[currentPoints.Count - 1] = endPoint;
                            }
                            else
                            {
                                if (cutSegmentDetailList.ContainsKey(segmentDetail.MappedObject))
                                {
                                    cutSegmentDetailList[segmentDetail.MappedObject].Add(segmentDetail);
                                }
                                else
                                {
                                    cutSegmentDetailList.Add(segmentDetail.MappedObject, new List<SegmentDetail>() { segmentDetail });
                                }

                                insertIndex++;
                                currentPoints.Insert(insertIndex, endPoint);
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                if (segmentDetail.Remove)
                {
                    if (cutSegmentDetailList.ContainsKey(segmentDetail.MappedObject))
                    {
                        cutSegmentDetailList[segmentDetail.MappedObject].Add(segmentDetail);
                    }
                    else
                    {
                        cutSegmentDetailList.Add(segmentDetail.MappedObject, new List<SegmentDetail>() { segmentDetail });
                    }
                }

            }
            #endregion

            #region Remove overlapping segments
            foreach (Ocad.Model.SymbolObject symbolObject in cutSegmentDetailList.Keys)
            {
                List<Model.Point> currentPoints = symbolObject.Points;
                List<int> cutIndexList = new List<int>();
                foreach (SegmentDetail segmentDetail in cutSegmentDetailList[symbolObject])
                {
                    if (segmentDetail.Remove)
                    {
                        int cutIndex = currentPoints.IndexOf(segmentDetail.EndPoint);
                        if (cutIndex != -1)
                        {
                            cutIndexList.Add(cutIndex);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (segmentDetail.StartRatio != 0M)
                        {
                            int cutIndex = currentPoints.IndexOf(segmentDetail.StartPoint);
                            if (cutIndex != -1)
                            {
                                cutIndexList.Add(cutIndex + 1);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (segmentDetail.EndRatio != 1M)
                        {
                            int cutIndex = currentPoints.IndexOf(segmentDetail.EndPoint);
                            if (cutIndex != -1)
                            {
                                cutIndexList.Add(cutIndex);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                cutIndexList.Sort();
                cutIndexList.Reverse();

                foreach (int cutIndex in cutIndexList)
                {
                    if (cutIndex == 1)
                    {
                        currentPoints.RemoveAt(0);
                    } 
                    else if (cutIndex == (currentPoints.Count - 1))
                    {
                        currentPoints.RemoveAt(cutIndex);
                    }
                    else
                    {
                        Ocad.Model.SymbolObject ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, symbolObject.Symbol);
                        ocadItem.Points = currentPoints.GetRange(cutIndex, currentPoints.Count - cutIndex);
                        currentPoints.RemoveRange(cutIndex, currentPoints.Count - cutIndex);
                    }

                    if (currentPoints.Count < 2)
                    {
                        ocadMap.Objects.Remove(symbolObject);
                    }
                }
            }
            #endregion
        }

        private static void AddIntersectPointsWithNewSegment(SegmentDetail newSegmentDetail, List<SegmentDetail> activeSegmentList)
        {
            LinearSegment newSegment = newSegmentDetail.Segment;

            for (int i = 0; i < activeSegmentList.Count; i++)
            {
                SegmentDetail currentSegmentDetail = activeSegmentList[i];
                if (newSegmentDetail.UpperY < currentSegmentDetail.LowerY)
                {
                    // all remaining active segments are higher than new segments
                    break;
                }

                if (newSegmentDetail.LowerY > currentSegmentDetail.UpperY)
                {
                    // this active segment is lower than new segment
                    continue;
                }

                if (!newSegment.IntersectPoints(currentSegmentDetail.Segment, Common.EPSILON, newSegmentDetail.IntersectRatios, currentSegmentDetail.IntersectRatios))
                {
                    #region Search for overlaps, and record
                    /*
                    if ((newSegmentDetail.MappedObject.Symbol.Number == currentSegmentDetail.MappedObject.Symbol.Number) &&
                        (newSegmentDetail.MappedObject.FeatureType == Model.Type.FeatureType.Line) &&
                        (newSegmentDetail.MappedObject.Symbol.Number != "518.0"))
                    {
                        // TO DO IF BRIDGE MUST BE IN SAME DIRECTION !!!!

                        bool currentStartOnNewSegment = newSegment.LieOn(currentSegmentDetail.Segment.Start, Common.EPSILON, null);
                        bool currentEndOnNewSegment = newSegment.LieOn(currentSegmentDetail.Segment.End, Common.EPSILON, null);
                        // Does start/end of one segment lie on other segment
                        if (currentStartOnNewSegment && currentEndOnNewSegment)
                        {
                            newSegmentDetail.Remove = true;
                        }
                        else if (currentStartOnNewSegment || currentEndOnNewSegment)
                        {
                            List<decimal> currentIntersectRatios = new List<decimal>();
                            bool newStartOnCurrent = currentSegmentDetail.Segment.LieOn(newSegment.Start, Common.EPSILON, currentIntersectRatios);
                            bool newEndOnCurrent = currentSegmentDetail.Segment.LieOn(newSegment.End, Common.EPSILON, currentIntersectRatios);
                            if (newStartOnCurrent && newEndOnCurrent)
                            {
                                currentSegmentDetail.Remove = true;
                            }
                            else if ((newStartOnCurrent || newEndOnCurrent) &&
                                (currentIntersectRatios[0] < currentSegmentDetail.EndRatio) &&
                                (currentIntersectRatios[0] > currentSegmentDetail.StartRatio))
                            {
                                if (currentEndOnNewSegment)
                                {
                                    // <---XXX      current
                                    //     <------  new
                                    //
                                    // <---XXX      current
                                    //     ------>  new
                                    currentSegmentDetail.StartRatio = currentIntersectRatios[0];
                                }
                                else
                                {
                                    // --->XXX      current
                                    //     <------  new
                                    //
                                    // --->XXX      current
                                    //     ------>  new
                                    currentSegmentDetail.EndRatio = currentIntersectRatios[0];
                                }
                            }
                        }
                    }
                    */
                    #endregion
                }
            }
        }

        private class EventDetail
        {
            public List<SegmentDetail> AddSegmentList;
            public List<SegmentDetail> CompareOnlySegmentList;
            public List<SegmentDetail> RemoveSegmentList;

            public EventDetail()
            {
                AddSegmentList = new List<SegmentDetail>();
                CompareOnlySegmentList = new List<SegmentDetail>();
                RemoveSegmentList = new List<SegmentDetail>();
            }
        }

        private class SegmentDetail
        {
            public Ocad.Model.SymbolObject MappedObject;
            public Ocad.Model.Point StartPoint;
            public Ocad.Model.Point EndPoint;
            public Geometry.LinearSegment Segment;
            public List<decimal> IntersectRatios;
            public decimal UpperY;
            public decimal LowerY;
            public bool Remove = false;
            public decimal StartRatio = 0M;
            public decimal EndRatio = 1M;
        }

        private class SegmentDetailComparer : IComparer<SegmentDetail>
        {
            int IComparer<SegmentDetail>.Compare(SegmentDetail x, SegmentDetail y)
            {
                int index = x.LowerY.CompareTo(y.LowerY);
                if (index == 0)
                {
                    index = x.GetHashCode().CompareTo(y.GetHashCode());
                }
                return index;
            }
        }
    }
}
