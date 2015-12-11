using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class Cleanse
    {
        private static Distance MaxShortLegLength = new Distance(500, Distance.Unit.Metre, Scale.ten_minus_5); //5mm c. 4 dashes
        private static Distance MinFeatureFrequency = new Distance(150, Distance.Unit.Metre, Scale.ten_minus_5);
        private static int MinLineObjects = 4;
        private static decimal CornerWeighting = 0.5M;

        /// <summary>
        /// Favour Endpoints and work inwards, to prevent drifting if the object consists only of short segments.
        /// </summary>
        /// <param name="ocadMap"></param>
        public static void ConvertLineObjects(this Ocad.Model.Map ocadMap)
        {
            #region Build Networks
            List<Network> networkList = new List<Network>();
            foreach (Ocad.Model.AbstractObject obj in ocadMap.Objects)
            {
                if (!(obj is Ocad.Model.SymbolObject))
                {
                    continue;
                }

                if (obj.FeatureType != Model.Type.FeatureType.Line)
                {
                    continue;
                }

                Ocad.Model.SymbolObject lineObject = obj as Ocad.Model.SymbolObject;
                switch (lineObject.Symbol.Number)
                {
                    case "507.0":
                    case "507.1":
                        break;
                    default:
                        continue;
                }

                #region Split into short paths between junction
                List<Leg> legList = new List<Leg>();
                Point p1 = lineObject.Points[0];
                Point p2;

                Leg newLeg = null;
                for (int i = 1; i < lineObject.Points.Count; i++)
                {
                    // Is start of leg
                    if (newLeg == null)
                    {
                        newLeg = new Leg(p1);
                    }

                    // Add segment to leg
                    p2 = lineObject.Points[i];
                    LinearSegment segment = new LinearSegment(p1, p2);
                    newLeg.Length += segment.Length;
                    newLeg.Segments.Add(segment);

                    // Is End of Leg
                    if (((lineObject.Points[i].MainPointFlag & Model.Type.PointFlag.DashPoint) == Model.Type.PointFlag.DashPoint) || 
                        (i == (lineObject.Points.Count - 1)))
                    {
                        if (newLeg.Length <= MaxShortLegLength)
                        {
                            newLeg.End = p2;
                            legList.Add(newLeg);
                        }
                        newLeg = null;
                    }
                    else if ((lineObject.Points[i].MainPointFlag & Model.Type.PointFlag.DashPoint) == Model.Type.PointFlag.CornerPoint)
                    {
                        newLeg.CornerCount += 1;
                    }

                    p1 = p2;
                }
                #endregion

                #region Add legs to networks
                foreach (Leg leg in legList)
                {
                    List<Network> joinNetworkList = new List<Network>();
                    foreach (Network network in networkList)
                    {
                        if (LegJoinsNetwork(leg, network))
                        {
                            joinNetworkList.Add(network);
                        }
                    }

                    #region Add leg to networks
                    Network joinNetwork;
                    if (joinNetworkList.Count == 0)
                    {
                        joinNetwork = new Network();
                        networkList.Add(joinNetwork);
                    }
                    else
                    {
                        joinNetwork = joinNetworkList[0];
                    }
                    joinNetwork.Add(lineObject, leg);

                    for (int i = 1; i < joinNetworkList.Count; i++)
                    {
                        Network otherNetwork = joinNetworkList[i];
                        networkList.Remove(otherNetwork);

                        joinNetwork.Add(otherNetwork);
                    }
                    #endregion
                }
                #endregion
            }
            #endregion

            Dictionary<Ocad.Model.SymbolObject, List<Ocad.Model.SymbolObject>> replacementLineObjectsList = new Dictionary<Model.SymbolObject, List<Model.SymbolObject>>();
            foreach (Network network in networkList)
            {
                #region Check network pass minimum complexity test
                // complex network should have 4 or more objects
                if (network.LineLegsDictionary.Count <= MinLineObjects)
                {
                    continue;
                }

                decimal weightedFeatureCount = (network.CornerCount * CornerWeighting) + network.JunctionPoints.Count;
                Distance featureFrequency = network.Length / weightedFeatureCount;
                if (featureFrequency >= MinFeatureFrequency) // 1.50mm
                {
                    continue;
                }
                #endregion

                foreach (Ocad.Model.SymbolObject originalLineObject in network.LineLegsDictionary.Keys)
                {
                    List<Ocad.Model.SymbolObject> subLineObjectList = new List<Model.SymbolObject>() { originalLineObject };
                    if (replacementLineObjectsList.ContainsKey(originalLineObject))
                    {
                        subLineObjectList.AddRange(replacementLineObjectsList[originalLineObject]);
                    }

                    foreach (Ocad.Model.SymbolObject subLineObject in subLineObjectList)
                    {
                        #region Identify Complex and Simple Parts in Sub-line
                        List<List<Ocad.Model.Point>> complexSegmentList = new List<List<Ocad.Model.Point>>();
                        List<Ocad.Model.Point> complexSegment = null;

                        List<List<Ocad.Model.Point>> simpleSegmentList = new List<List<Ocad.Model.Point>>();
                        List<Ocad.Model.Point> simpleSegment = null;

                        Ocad.Model.Point p1 = subLineObject.Points[0];
                        Ocad.Model.Point p2;
                        for (int i = 1; i < subLineObject.Points.Count; i++)
                        {
                            p2 = subLineObject.Points[i];
                            bool isComplexSegment = false;
                            foreach (Leg leg in network.LineLegsDictionary[originalLineObject])
                            {
                                foreach (LinearSegment segment in leg.Segments)
                                {
                                    if ((segment.Start == p1) && (segment.End == p2))
                                    {
                                        isComplexSegment = true;
                                        break;
                                    }
                                }
                                if (isComplexSegment)
                                {
                                    break;
                                }
                            }

                            if (isComplexSegment)
                            {
                                if (complexSegment == null)
                                {
                                    complexSegment = new List<Ocad.Model.Point>();
                                    complexSegmentList.Add(complexSegment);
                                    complexSegment.Add(p1);

                                    simpleSegment = null;
                                }
                                complexSegment.Add(p2);
                            }
                            else
                            {
                                if (simpleSegment == null)
                                {
                                    simpleSegment = new List<Ocad.Model.Point>();
                                    simpleSegmentList.Add(simpleSegment);
                                    simpleSegment.Add(p1);

                                    complexSegment = null;
                                }
                                simpleSegment.Add(p2);
                            }

                            p1 = p2;
                        }
                        #endregion

                        #region Amend line into complex and simple parts
                        if (complexSegmentList.Count == 0)
                        {
                            // If line belongs to multiple networks, then this sub-part might not belong to this network
                            continue;
                        }

                        subLineObject.Symbol = ocadMap.GetSymbol("507.1");
                        if ((complexSegmentList.Count > 1) || (simpleSegmentList.Count > 0))
                        {
                            List<Ocad.Model.SymbolObject> replacementLineObjects = new List<Model.SymbolObject>();
                            replacementLineObjectsList.Add(originalLineObject, replacementLineObjects);

                            subLineObject.Points = complexSegmentList[0];

                            for (int i = 1; i < complexSegmentList.Count; i++)
                            {
                                Ocad.Model.SymbolObject complexLine = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, ocadMap.GetSymbol("507.1"));
                                complexLine.Points = complexSegmentList[i];
                                replacementLineObjects.Add(complexLine);
                            }

                            for (int i = 0; i < simpleSegmentList.Count; i++)
                            {
                                Ocad.Model.SymbolObject simpleLine = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, ocadMap.GetSymbol("507.0"));
                                simpleLine.Points = simpleSegmentList[i];
                            }
                        }
                        #endregion
                    }
                }
            }
        }

        private static bool LegJoinsNetwork(Leg leg, Network network)
        {
            if ((leg.Start.X <= network.Top) &&
                (leg.Start.X >= network.Bottom) &&
                (leg.Start.Y <= network.Right) &&
                (leg.Start.Y >= network.Left))
            {
                foreach (Point np in network.JunctionPoints)
                {
                    if (np.GetRelationship(leg.Start, Common.EPSILON) != Relationship.Apart)
                    {
                        return true;
                    }
                }
            }

            if ((leg.End.X <= network.Top) &&
                (leg.End.X >= network.Bottom) &&
                (leg.End.Y <= network.Right) &&
                (leg.End.Y >= network.Left))
            {
                foreach (Point np in network.JunctionPoints)
                {
                    if (np.GetRelationship(leg.End, Common.EPSILON) != Relationship.Apart)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private class Leg
        {
            public List<LinearSegment> Segments = new List<LinearSegment>();
            public Distance Length = new Distance(0, Distance.Unit.Metre, Scale.ten_minus_5);
            public int CornerCount = 0;

            public Point Start;
            private Point _end;
            public Point End
            {
                get
                {
                    return _end;
                }
                set
                {
                    _end = value;
                    if (Start.X > _end.X)
                    {
                        Top = Start.X;
                        Bottom = End.X;
                    }
                    else
                    {
                        Top = End.X;
                        Bottom = Start.X;
                    }
                    if (Start.Y > _end.Y)
                    {
                        Right = Start.Y;
                        Left = End.Y;
                    }
                    else
                    {
                        Right = End.Y;
                        Left = Start.Y;
                    }
                }
            }

            public Distance Top;
            public Distance Bottom;
            public Distance Right;
            public Distance Left;

            public Leg(Point start)
            {
                Start = start;
            }

        }

        private class Network
        {
            public List<Point> JunctionPoints = new List<Point>();
            public Dictionary<Ocad.Model.SymbolObject, List<Leg>> LineLegsDictionary = new Dictionary<Ocad.Model.SymbolObject, List<Leg>>();
            public Distance Length = new Distance(0, Distance.Unit.Metre, Scale.ten_minus_5);
            public int CornerCount = 0;

            public Distance Top;
            public Distance Bottom;
            public Distance Right;
            public Distance Left;

            public void Add(Ocad.Model.SymbolObject line, Leg leg)
            {
                Point start = leg.Start;
                Point end = leg.End;

                if (LineLegsDictionary.Count == 0)
                {
                    JunctionPoints.Add(start);
                    JunctionPoints.Add(end);

                    Bottom = leg.Bottom - Common.EPSILON;
                    Top = leg.Top + Common.EPSILON;
                    Left = leg.Left - Common.EPSILON;
                    Right = leg.Right + Common.EPSILON;
                }
                else
                {
                    if (!JunctionPoints.Contains(start))
                    {
                        JunctionPoints.Add(start);
                    }
                    if (!JunctionPoints.Contains(end))
                    {
                        JunctionPoints.Add(end);
                    }

                    if (leg.Bottom - Common.EPSILON < Bottom) { Bottom = leg.Bottom - Common.EPSILON; }
                    if (leg.Top + Common.EPSILON > Top) { Top = leg.Top + Common.EPSILON; }
                    if (leg.Left - Common.EPSILON < Left) { Left = leg.Left - Common.EPSILON; }
                    if (leg.Right + Common.EPSILON > Right) { Right = leg.Right + Common.EPSILON; }
                }

                Length += leg.Length;
                CornerCount += leg.CornerCount;

                if (LineLegsDictionary.ContainsKey(line))
                {
                    LineLegsDictionary[line].Add(leg);
                }
                else
                {
                    LineLegsDictionary.Add(line, new List<Leg>() { leg });
                }
            }

            public void Add(Network network)
            {
                JunctionPoints.AddRange(network.JunctionPoints);

                if (network.Bottom < Bottom) { Bottom = network.Bottom; }
                if (network.Top > Top) { Top = network.Top; }
                if (network.Left < Left) { Left = network.Left; }
                if (network.Right > Right) { Right = network.Right; }

                Length += network.Length;
                CornerCount += network.CornerCount;

                foreach (Ocad.Model.SymbolObject line in network.LineLegsDictionary.Keys)
                {
                    if (LineLegsDictionary.ContainsKey(line))
                    {
                        LineLegsDictionary[line].AddRange(network.LineLegsDictionary[line]);
                    }
                    else
                    {
                        LineLegsDictionary.Add(line, network.LineLegsDictionary[line]);
                    }
                }
            }
        }
    }
}
