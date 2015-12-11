using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class Cleanse
    {
        public static void JoinLineObjects(this Ocad.Model.Map ocadMap)
        {
            #region Setup data structure to search for neighbours
            Dictionary<string, JoinLineObjectsSymbol> joinLineObjectsSymbolList = new Dictionary<string, JoinLineObjectsSymbol>();
            foreach (Ocad.Model.AbstractObject obj in ocadMap.Objects)
            {
                #region Check object is a open line
                if (obj.FeatureType != Model.Type.FeatureType.Line)
                {
                    continue;
                }

                Geometry.Point startPoint = obj.Points[0];
                Geometry.Point endPoint = obj.Points[obj.Points.Count - 1];

                // Line Object is Closed
                if (startPoint.GetRelationship(endPoint, Common.EPSILON) != Relationship.Apart)
                {
                    // Close it completely
                    endPoint.X = startPoint.X;
                    endPoint.Y = startPoint.Y;
                    continue;
                }
                #endregion

                #region Record open line object

                #region Get structure
                Ocad.Model.SymbolObject symbolObject = (Ocad.Model.SymbolObject)obj;
                string symbolNumber = symbolObject.Symbol.Number;
                JoinLineObjectsSymbol joinLineObjectsSymbol;
                if (joinLineObjectsSymbolList.ContainsKey(symbolNumber))
                {
                    joinLineObjectsSymbol = joinLineObjectsSymbolList[symbolNumber];
                }
                else
                {
                    joinLineObjectsSymbol = new JoinLineObjectsSymbol()
                    {
                        SymbolObjectToPointId = new Dictionary<Model.SymbolObject, ObjectEnds>(),
                        PointIdToSymbolObject = new Dictionary<long, List<Model.SymbolObject>>()
                    };
                    joinLineObjectsSymbolList.Add(symbolNumber, joinLineObjectsSymbol);
                }
                #endregion

                long startPointId = GetPointId(startPoint);
                Angle startAngleRadian = new Geometry.Angle(startPoint, obj.Points[1], Angle.Unit.Radian); //GetRadian(startPoint, obj.Points[1]);
                if (joinLineObjectsSymbol.PointIdToSymbolObject.ContainsKey(startPointId))
                {
                    joinLineObjectsSymbol.PointIdToSymbolObject[startPointId].Add(symbolObject);
                }
                else
                {
                    joinLineObjectsSymbol.PointIdToSymbolObject.Add(startPointId, new List<Model.SymbolObject> { symbolObject });
                }

                long endPointId = GetPointId(endPoint);
                Angle endAngleRadian = new Geometry.Angle(obj.Points[obj.Points.Count - 2], endPoint, Angle.Unit.Radian); //GetRadian(obj.Points[obj.Points.Count - 2], endPoint);
                if (joinLineObjectsSymbol.PointIdToSymbolObject.ContainsKey(endPointId))
                {
                    joinLineObjectsSymbol.PointIdToSymbolObject[endPointId].Add(symbolObject);
                }
                else
                {
                    joinLineObjectsSymbol.PointIdToSymbolObject.Add(endPointId, new List<Model.SymbolObject> { symbolObject });
                }

                ObjectEnds objectEnd = new ObjectEnds() { StartPointId = startPointId, EndPointId = endPointId, StartAngleRadian = startAngleRadian, EndAngleRadian = endAngleRadian };
                joinLineObjectsSymbol.SymbolObjectToPointId.Add(symbolObject, objectEnd);
                #endregion
            }
            #endregion

            #region Search for neighbours
            // Iterate through each symbol
            foreach (JoinLineObjectsSymbol joinLineObjectsSymbol in joinLineObjectsSymbolList.Values)
            {
                Dictionary<long, List<Ocad.Model.SymbolObject>> pointIdToSymbolObject = joinLineObjectsSymbol.PointIdToSymbolObject;
                Dictionary<Ocad.Model.SymbolObject, ObjectEnds> symbolObjectToPointId = joinLineObjectsSymbol.SymbolObjectToPointId;

                // Iterate through each end point
                foreach (long pointId in pointIdToSymbolObject.Keys)
                {
                    #region Get collection of neighbouring symbols
                    List<Ocad.Model.SymbolObject> neighbouringSymbolList = new List<Model.SymbolObject>();
                    // Get line object in centre
                    long index = pointId;
                    MergeSymbolObjectList(neighbouringSymbolList, pointIdToSymbolObject, index);
                    // Get line object to near east
                    index += Int32.MaxValue;
                    MergeSymbolObjectList(neighbouringSymbolList, pointIdToSymbolObject, index);
                    // Get line object to near southeast
                    index -= 1;
                    MergeSymbolObjectList(neighbouringSymbolList, pointIdToSymbolObject, index);
                    // Get line object to near south
                    index -= Int32.MaxValue;
                    MergeSymbolObjectList(neighbouringSymbolList, pointIdToSymbolObject, index);
                    // Get line object to near southwest
                    index -= Int32.MaxValue;
                    MergeSymbolObjectList(neighbouringSymbolList, pointIdToSymbolObject, index);
                    // Get line object to near west
                    index += 1;
                    MergeSymbolObjectList(neighbouringSymbolList, pointIdToSymbolObject, index);
                    // Get line object to near northwest
                    index += 1;
                    MergeSymbolObjectList(neighbouringSymbolList, pointIdToSymbolObject, index);
                    // Get line object to near north
                    index += Int32.MaxValue;
                    MergeSymbolObjectList(neighbouringSymbolList, pointIdToSymbolObject, index);
                    // Get line object to near northeast
                    index += Int32.MaxValue;
                    MergeSymbolObjectList(neighbouringSymbolList, pointIdToSymbolObject, index);
                    #endregion

                    #region Identity possible connection options
                    // Need at least two neighbours for a connection
                    if (neighbouringSymbolList.Count < 2)
                    {
                        continue;
                    }

                    List<ConnectionOptionDetails> optionDetailsList = new List<ConnectionOptionDetails>();
                    for (int aIndex = 0; aIndex < neighbouringSymbolList.Count; aIndex++)
                    {
                        #region Evaluate this neighbour with all subsequent neighbours
                        Ocad.Model.SymbolObject aSymbolObject = neighbouringSymbolList[aIndex];
                        ObjectEnds aObjectEnds = symbolObjectToPointId[aSymbolObject];
                        Geometry.Point aStartPoint = aSymbolObject.Points[0];
                        Geometry.Point aEndPoint = aSymbolObject.Points[neighbouringSymbolList[aIndex].Points.Count - 1];

                        for (int bIndex = aIndex + 1; bIndex < neighbouringSymbolList.Count; bIndex++)
                        {
                            #region Evaluate two neighbouring object symbols
                            Ocad.Model.SymbolObject bSymbolObject = neighbouringSymbolList[bIndex];
                            ObjectEnds bObjectEnds = symbolObjectToPointId[bSymbolObject];
                            Geometry.Point bStartPoint = bSymbolObject.Points[0];
                            Geometry.Point bEndPoint = bSymbolObject.Points[neighbouringSymbolList[bIndex].Points.Count - 1];

                            Angle angle = null;
                            int option = 0;
                            Angle EPSILON_ANGLE = new Angle((decimal)Math.PI / 6, Angle.Unit.Radian);

                            if (aEndPoint.GetRelationship(bStartPoint, Common.EPSILON) != Relationship.Apart)
                            {
                                // --> X -->  :  --X-->
                                angle = DifferenceRadian(aObjectEnds.EndAngleRadian, bObjectEnds.StartAngleRadian);
                                if (angle > EPSILON_ANGLE)
                                {
                                    option = 1;
                                }
                            }

                            if (aEndPoint.GetRelationship(bEndPoint, Common.EPSILON) != Relationship.Apart)
                            {
                                // --> X <--  :  --X-->
                                angle = DifferenceRadian(aObjectEnds.EndAngleRadian, bObjectEnds.EndAngleRadian.Reverse());
                                if (angle > EPSILON_ANGLE)
                                {
                                    option = 2;
                                }
                            }

                            if (aStartPoint.GetRelationship(bStartPoint, Common.EPSILON) != Relationship.Apart)
                            {
                                // <-- X -->  :  <--X--
                                angle = DifferenceRadian(bObjectEnds.StartAngleRadian.Reverse(), aObjectEnds.StartAngleRadian);
                                if (angle > EPSILON_ANGLE)
                                {
                                    option = 3;
                                }
                            }

                            if (aStartPoint.GetRelationship(bEndPoint, Common.EPSILON) != Relationship.Apart)
                            {
                                // <-- X <--  :  <--X--
                                angle = DifferenceRadian(bObjectEnds.EndAngleRadian, aObjectEnds.StartAngleRadian);
                                if (angle > EPSILON_ANGLE)
                                {
                                    option = 4;
                                }
                            }

                            if (option != 0)
                            {
                                ConnectionOptionDetails optionDetails = new ConnectionOptionDetails()
                                {
                                    ASymbolObject = aSymbolObject,
                                    AObjectEnds = aObjectEnds,
                                    AStartPoint = aStartPoint,
                                    AEndPoint = aEndPoint,
                                    BSymbolObject = bSymbolObject,
                                    BObjectEnds = bObjectEnds,
                                    BStartPoint = bStartPoint,
                                    BEndPoint = bEndPoint,
                                    Angle = angle,
                                    Option = option
                                };
                                optionDetailsList.Add(optionDetails);
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion

                    #region Connect neighbouring symbols
                    while (optionDetailsList.Count > 0)
                    {
                        #region Select best option
                        ConnectionOptionDetails bestOptionDetails = null;
                        Angle bestAngle = null;
                        foreach (ConnectionOptionDetails optionDetails in optionDetailsList)
                        {
                            if ((bestAngle == null) || (optionDetails.Angle > bestAngle))
                            {
                                bestAngle = optionDetails.Angle;
                                bestOptionDetails = optionDetails;
                            }
                        }
                        #endregion

                        #region Merge object B in object A
                        Ocad.Model.SymbolObject aSymbolObject = bestOptionDetails.ASymbolObject;
                        ObjectEnds aObjectEnds = bestOptionDetails.AObjectEnds;
                        Geometry.Point aStartPoint = bestOptionDetails.AStartPoint;
                        Geometry.Point aEndPoint = bestOptionDetails.AEndPoint;

                        Ocad.Model.SymbolObject bSymbolObject = bestOptionDetails.BSymbolObject;
                        ObjectEnds bObjectEnds = bestOptionDetails.BObjectEnds;
                        Geometry.Point bStartPoint = bestOptionDetails.BStartPoint;
                        Geometry.Point bEndPoint = bestOptionDetails.BEndPoint;

                        bool aStartPointChanged = false;
                        long newAStartPointId = 0;
                        long newAEndPointId = 0;
                        Angle newAStartAngleRadian = null;
                        Angle newAEndAngleRadian = null;

                        switch (bestOptionDetails.Option)
                        {
                            case 1:
                                // --> X -->  :  --X-->
                                bSymbolObject.Points.RemoveAt(0);
                                aSymbolObject.Points.AddRange(bSymbolObject.Points);
                                aEndPoint = bEndPoint;
                                newAEndPointId = bObjectEnds.EndPointId;
                                newAEndAngleRadian = bObjectEnds.EndAngleRadian;
                                break;
                            case 2:
                                // --> X <--  :  --X-->
                                bSymbolObject.Points.Reverse();
                                bSymbolObject.Points.RemoveAt(0);
                                aSymbolObject.Points.AddRange(bSymbolObject.Points);
                                aEndPoint = bStartPoint;
                                newAEndPointId = bObjectEnds.StartPointId;
                                newAEndAngleRadian = bObjectEnds.StartAngleRadian.Reverse(); //RotateHalfCircle(bObjectEnds.StartAngleRadian);
                                break;
                            case 3:
                                // <-- X -->  :  <--X--
                                bSymbolObject.Points.RemoveAt(0);
                                bSymbolObject.Points.Reverse();
                                aSymbolObject.Points.InsertRange(0, bSymbolObject.Points);
                                aStartPoint = bEndPoint;
                                newAStartPointId = bObjectEnds.EndPointId;
                                newAStartAngleRadian = bObjectEnds.EndAngleRadian.Reverse(); //RotateHalfCircle(bObjectEnds.EndAngleRadian);
                                aStartPointChanged = true;
                                break;
                            case 4:
                                // <-- X <--  :  <--X--
                                bSymbolObject.Points.RemoveAt(bSymbolObject.Points.Count - 1);
                                aSymbolObject.Points.InsertRange(0, bSymbolObject.Points);
                                aStartPoint = bStartPoint;
                                newAStartPointId = bObjectEnds.StartPointId;
                                newAStartAngleRadian = bObjectEnds.StartAngleRadian;
                                aStartPointChanged = true;
                                break;
                        }

                        #region Remove all reference to object B, now it has been joined to object A
                        ocadMap.Objects.Remove(bSymbolObject);

                        symbolObjectToPointId.Remove(bSymbolObject);

                        pointIdToSymbolObject[bObjectEnds.StartPointId].Remove(bSymbolObject);
                        pointIdToSymbolObject[bObjectEnds.EndPointId].Remove(bSymbolObject);
                        #endregion
                        #endregion

                        #region Adjust references to object A
                        if (aStartPoint.GetRelationship(aEndPoint, Common.EPSILON) != Relationship.Apart)
                        {
                            // Object A is now closed

                            // Close it completely
                            aEndPoint.X = aStartPoint.X;
                            aEndPoint.Y = aStartPoint.Y;

                            // Remove link to symbol - it cannot be joined to any other object
                            symbolObjectToPointId.Remove(aSymbolObject);
                            pointIdToSymbolObject[aObjectEnds.StartPointId].Remove(aSymbolObject);
                            pointIdToSymbolObject[aObjectEnds.EndPointId].Remove(aSymbolObject);
                        }
                        else if (aStartPointChanged)
                        {
                            // Change link to object A to its new start point
                            pointIdToSymbolObject[aObjectEnds.StartPointId].Remove(aSymbolObject);
                            pointIdToSymbolObject[newAStartPointId].Add(aSymbolObject);

                            aObjectEnds.StartPointId = newAStartPointId;
                            aObjectEnds.StartAngleRadian = newAStartAngleRadian;
                        }
                        else
                        {
                            // Change link to object A to its new end point
                            pointIdToSymbolObject[aObjectEnds.EndPointId].Remove(aSymbolObject);
                            pointIdToSymbolObject[newAEndPointId].Add(aSymbolObject);

                            aObjectEnds.EndPointId = newAEndPointId;
                            aObjectEnds.EndAngleRadian = newAEndAngleRadian;
                        }
                        #endregion

                        #region Remove options no longer available
                        optionDetailsList.Remove(bestOptionDetails);
                        List<ConnectionOptionDetails> removeOptionDetailsList = new List<ConnectionOptionDetails>();
                        foreach (ConnectionOptionDetails optionDetails in optionDetailsList)
                        {
                            if (optionDetails.ASymbolObject.Equals(aSymbolObject) ||
                                optionDetails.ASymbolObject.Equals(bSymbolObject) ||
                                optionDetails.BSymbolObject.Equals(aSymbolObject) ||
                                optionDetails.BSymbolObject.Equals(bSymbolObject))
                            {
                                removeOptionDetailsList.Add(optionDetails);
                            }
                        }
                        foreach (ConnectionOptionDetails removeOptionDetails in removeOptionDetailsList)
                        {
                            optionDetailsList.Remove(removeOptionDetails);
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion
        }

        internal static Angle DifferenceRadian(Angle a, Angle b)
        {
            return ((a - b).Absolute() - Angle.HalfCircle).Absolute();
        }

        private class JoinLineObjectsSymbol
        {
            public Dictionary<long, List<Ocad.Model.SymbolObject>> PointIdToSymbolObject;
            public Dictionary<Ocad.Model.SymbolObject, ObjectEnds> SymbolObjectToPointId;
        }

        private class ObjectEnds
        {
            public long StartPointId;
            public long EndPointId;
            public Angle StartAngleRadian;
            public Angle EndAngleRadian;
        }

        private class ConnectionOptionDetails
        {
            public Ocad.Model.SymbolObject ASymbolObject;
            public ObjectEnds AObjectEnds;
            public Geometry.Point AStartPoint;
            public Geometry.Point AEndPoint;

            public Ocad.Model.SymbolObject BSymbolObject;
            public ObjectEnds BObjectEnds;
            public Geometry.Point BStartPoint;
            public Geometry.Point BEndPoint;

            public Angle Angle;
            public int Option;
        }

        private static void MergeSymbolObjectList(List<Ocad.Model.SymbolObject> value, Dictionary<long, List<Ocad.Model.SymbolObject>> pointIdToSymbolObject, long index)
        {
            if (pointIdToSymbolObject.ContainsKey(index))
            {
                List<Ocad.Model.SymbolObject> other = pointIdToSymbolObject[index];
                foreach (Ocad.Model.SymbolObject symbolObject in other)
                {
                    if (!value.Contains(symbolObject))
                    {
                        value.Add(symbolObject);
                    }
                }
            }
        }
    }
}
