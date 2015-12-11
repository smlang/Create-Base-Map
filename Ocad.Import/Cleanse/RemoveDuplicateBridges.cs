using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class Cleanse
    {
        private static Distance MAX_NON_FACING_DISTANCE = new Distance(Settings.Default.RemoveDuplicateBridges_MinDistanceBetweenNonFacingBridgeEdgeMM * 100, Distance.Unit.Metre, Scale.ten_minus_5);
        private static decimal MAX_FACING_RATIO = Settings.Default.RemoveDuplicateBridges_MinDistanceBetweenFacingBridgeEdgeMM / Settings.Default.RemoveDuplicateBridges_MinDistanceBetweenNonFacingBridgeEdgeMM;

        private static double ANGLE_EPSILON = Settings.Default.RemoveDuplicateBridges_AngleEpsilonFullCirclePercentage / 100;
        private static Angle LOWER_ANGLE_SAME_DIRECTION = new Angle(ANGLE_EPSILON * Math.PI, Angle.Unit.Radian);
        private static Angle UPPER_ANGLE_SAME_DIRECTION = new Angle((2 - ANGLE_EPSILON) * Math.PI, Angle.Unit.Radian);

        private static Angle LOWER_ANGLE_OPPOSITE_DIRECTION = new Angle((1 - ANGLE_EPSILON) * Math.PI, Angle.Unit.Radian);
        private static Angle UPPER_ANGLE_OPPOSITE_DIRECTION = new Angle((1 + ANGLE_EPSILON) * Math.PI, Angle.Unit.Radian);


        /// <summary>
        /// Favour Endpoints and work inwards, to prevent drifting if the object consists only of short segments.
        /// </summary>
        /// <param name="ocadMap"></param>
        public static void RemoveDuplicateBridges(this Ocad.Model.Map ocadMap)
        {
            #region Build Data Structure
            Dictionary<Ocad.Model.SymbolObject, BridgeDetail> bridgeList = new Dictionary<Model.SymbolObject, BridgeDetail>();
            foreach (Ocad.Model.AbstractObject abstractObject in ocadMap.Objects)
            {
                Ocad.Model.SymbolObject symbolObject = abstractObject as Ocad.Model.SymbolObject;
                if ((symbolObject == null) ||
                    (symbolObject.Symbol.Number != Settings.Default.RemoveDuplicateBridges_BridgeSymbolNumber))
                {
                    continue;
                }

                BridgeDetail detail = new BridgeDetail();
                bridgeList.Add(symbolObject, detail);

                #region Build segment list and ray list travelling outerwards from bridge
                Geometry.Point p1 = symbolObject.Points[0];
                if (p1.X[0, Distance.Unit.Metre, Scale.ten_minus_5] > detail.SegmentTop) { detail.SegmentTop = p1.X[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                if (p1.X[0, Distance.Unit.Metre, Scale.ten_minus_5] < detail.SegmentBottom) { detail.SegmentBottom = p1.X[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                if (p1.Y[0, Distance.Unit.Metre, Scale.ten_minus_5] > detail.SegmentRight) { detail.SegmentRight = p1.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                if (p1.Y[0, Distance.Unit.Metre, Scale.ten_minus_5] < detail.SegmentLeft) { detail.SegmentLeft = p1.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]; }

                Geometry.Point p2;
                for (int i = 1; i < symbolObject.Points.Count; i++)
                {
                    p2 = symbolObject.Points[i];

                    detail.SegmentList.Add(new LinearSegment(p1, p2));

                    if (p2.X[0, Distance.Unit.Metre, Scale.ten_minus_5] > detail.SegmentTop) { detail.SegmentTop = p2.X[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                    if (p2.X[0, Distance.Unit.Metre, Scale.ten_minus_5] < detail.SegmentBottom) { detail.SegmentBottom = p2.X[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                    if (p2.Y[0, Distance.Unit.Metre, Scale.ten_minus_5] > detail.SegmentRight) { detail.SegmentRight = p2.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                    if (p2.Y[0, Distance.Unit.Metre, Scale.ten_minus_5] < detail.SegmentLeft) { detail.SegmentLeft = p2.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]; }

                    Angle segmentAngle = new Geometry.Angle(p1, p2, Angle.Unit.Radian);  //GetRadian(p1, p2);
                    if (i == 1)
                    {
                        detail.FirstSegmentAngle = segmentAngle;
                    }
                    if (i == (symbolObject.Points.Count - 1))
                    {
                        detail.LastSegmentAngle = segmentAngle;
                    }

                    Angle rayAngle = segmentAngle + Angle.QuarterCircle;
                    Geometry.Point r1 = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                    if (r1.X[0, Distance.Unit.Metre, Scale.ten_minus_5] > detail.RayTop) { detail.RayTop = r1.X[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                    if (r1.X[0, Distance.Unit.Metre, Scale.ten_minus_5] < detail.RayBottom) { detail.RayBottom = r1.X[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                    if (r1.Y[0, Distance.Unit.Metre, Scale.ten_minus_5] > detail.RayRight) { detail.RayRight = r1.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                    if (r1.Y[0, Distance.Unit.Metre, Scale.ten_minus_5] < detail.RayLeft) { detail.RayLeft = r1.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]; }

                    Geometry.Point r2 = new Point(r1, MAX_NON_FACING_DISTANCE, rayAngle);
                    if (r2.X[0, Distance.Unit.Metre, Scale.ten_minus_5] > detail.RayTop) { detail.RayTop = r2.X[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                    if (r2.X[0, Distance.Unit.Metre, Scale.ten_minus_5] < detail.RayBottom) { detail.RayBottom = r2.X[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                    if (r2.Y[0, Distance.Unit.Metre, Scale.ten_minus_5] > detail.RayRight) { detail.RayRight = r2.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]; }
                    if (r2.Y[0, Distance.Unit.Metre, Scale.ten_minus_5] < detail.RayLeft) { detail.RayLeft = r2.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]; }

                    Ray ray = new Ray() { Segment = new LinearSegment(r1, r2) };
                    detail.RayList.Add(ray);

                    p1 = p2;
                }
                #endregion
            }
            #endregion
            /*
            foreach (BridgeDetail d in bridgeList.Values)
            {
                foreach (Ray r in d.RayList)
                {
                    Ocad.Model.AbstractSymbol lineSymbol = ocadMap.GetSymbol("507.1");
                    Ocad.Model.SymbolObject ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, lineSymbol);
                    ocadItem.Points.Add(new Model.Point(r.Segment.Start));
                    ocadItem.Points.Add(new Model.Point(r.Segment.End));
                }
            }
            */
            //Distance epsilon = new Distance(1M, Distance.Unit.Metre, Scale.ten_minus_5);

            foreach (Ocad.Model.SymbolObject innerBridge in bridgeList.Keys)
            {
                #region Find bridges on the outside of inner bridge
                foreach (Ocad.Model.SymbolObject outerBridge in bridgeList.Keys)
                {
                    if (innerBridge == outerBridge)
                    {
                        continue;
                    }

                    if (bridgeList[innerBridge].RayBottom > bridgeList[outerBridge].SegmentTop) { continue; }
                    if (bridgeList[innerBridge].RayTop < bridgeList[outerBridge].SegmentBottom) { continue; }
                    if (bridgeList[innerBridge].RayLeft > bridgeList[outerBridge].SegmentRight) { continue; }
                    if (bridgeList[innerBridge].RayRight < bridgeList[outerBridge].SegmentLeft) { continue; }

                    #region Check if outer bridge is somewhat parallel to inner bridge
                    Angle diffAngle = (bridgeList[innerBridge].FirstSegmentAngle - bridgeList[outerBridge].FirstSegmentAngle).Modulus();
                    bool sameDirection = ((diffAngle < LOWER_ANGLE_SAME_DIRECTION) || (diffAngle > UPPER_ANGLE_SAME_DIRECTION));
                    bool oppositeDirection = false;

                    if (!sameDirection)
                    {
                        diffAngle = (bridgeList[innerBridge].FirstSegmentAngle - bridgeList[outerBridge].LastSegmentAngle).Modulus();
                        oppositeDirection = ((diffAngle > LOWER_ANGLE_OPPOSITE_DIRECTION) && (diffAngle < UPPER_ANGLE_OPPOSITE_DIRECTION));

                        if (!oppositeDirection)
                        {
                            continue;
                        }
                    }
                    #endregion

                    if (sameDirection)
                    {
                        #region Check every ray crosses an outer bridge segment
                        for (int i = 0; i < bridgeList[innerBridge].RayList.Count; i++)
                        {
                            #region Check ray crosses an outer bridge segment
                            Ray ray = bridgeList[innerBridge].RayList[i];

                            bool isWithinRayRange = false;
                            foreach (LinearSegment outerBridgeSegment in bridgeList[outerBridge].SegmentList)
                            {
                                List<decimal> intersectRatio = new List<decimal>();
                                if ((ray.Segment.IntersectPoints(outerBridgeSegment, Distance.Zero, intersectRatio, null)) &&
                                    (ray.MaximumRatio > intersectRatio[0]))
                                {
                                    ray.OuterBridgeList.Add(outerBridge, intersectRatio[0]);
                                    isWithinRayRange = true;
                                    break;
                                }
                            }

                            if (!isWithinRayRange)
                            {
                                for (int j = 0; j < i; j++)
                                {
                                    Ray cleanRay = bridgeList[innerBridge].RayList[j];
                                    cleanRay.OuterBridgeList.Remove(outerBridge);
                                }
                                break;
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region Check if any ray crosses an outer bridge segment
                        List<Ocad.Model.SymbolObject> outOfRangeList = new List<Model.SymbolObject>();

                        foreach (Ray ray in bridgeList[innerBridge].RayList)
                        {
                            #region Check ray crosses an outer bridge segment
                            foreach (LinearSegment outerBridgeSegment in bridgeList[outerBridge].SegmentList)
                            {
                                List<decimal> intersectRatio = new List<decimal>();
                                if ((ray.Segment.IntersectPoints(outerBridgeSegment, Distance.Zero, intersectRatio, null)) &&
                                    (ray.MaximumRatio > intersectRatio[0]))
                                {
                                    ray.MaximumRatio = intersectRatio[0];
                                    foreach (Ocad.Model.SymbolObject previousOuterBridge in ray.OuterBridgeList.Keys)
                                    {
                                        if ((ray.OuterBridgeList[previousOuterBridge] > ray.MaximumRatio) &&
                                            (!outOfRangeList.Contains(previousOuterBridge)))
                                        {
                                            outOfRangeList.Add(previousOuterBridge);
                                        }
                                    }
                                    break;
                                }
                            }
                            #endregion
                        }

                        #region Special case - where bridge faces reverse bridge with very narrow gap
                        bool veryClose = true;
                        foreach (Ray ray in bridgeList[innerBridge].RayList)
                        {
                            if (ray.MaximumRatio > MAX_FACING_RATIO) // 20%
                            {
                                veryClose = false;
                            }
                        }
                        if (veryClose)
                        {
                            Ray ray = bridgeList[innerBridge].RayList[0];
                            ray.OuterBridgeList.Add(innerBridge, ray.MaximumRatio);
                            break;
                        }
                        #endregion

                        // remove any bridge that is now out of range
                        foreach (Ocad.Model.SymbolObject outOfRange in outOfRangeList)
                        {
                            foreach (Ray ray in bridgeList[innerBridge].RayList)
                            {
                                if (ray.OuterBridgeList.ContainsKey(outOfRange))
                                {
                                    ray.OuterBridgeList.Remove(outOfRange);
                                }
                            }
                        }
                        #endregion
                    }
                }

                #endregion

                #region Remove inner bridge if there are any outer bridge
                if (bridgeList[innerBridge].RayList[0].OuterBridgeList.Count != 0)
                {
                    ocadMap.Objects.Remove(innerBridge);
                }
                #endregion
            }
        }

        private class Ray
        {
            public LinearSegment Segment;
            public Dictionary<Ocad.Model.SymbolObject, decimal> OuterBridgeList = new Dictionary<Model.SymbolObject, decimal>();
            public decimal MaximumRatio = Decimal.MaxValue;
        }

        private class BridgeDetail
        {
            public List<LinearSegment> SegmentList = new List<LinearSegment>();
            public decimal SegmentTop = Decimal.MinValue;
            public decimal SegmentBottom = Decimal.MaxValue;
            public decimal SegmentLeft = Decimal.MaxValue;
            public decimal SegmentRight = Decimal.MinValue;
            public Angle FirstSegmentAngle;
            public Angle LastSegmentAngle;

            public List<Ray> RayList = new List<Ray>();
            public decimal RayTop = Decimal.MinValue;
            public decimal RayBottom = Decimal.MaxValue;
            public decimal RayLeft = Decimal.MaxValue;
            public decimal RayRight = Decimal.MinValue;
        }
    }
}
