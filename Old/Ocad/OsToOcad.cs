using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace OcadConsole
{
    public static partial class ModelDeserialize
    {
        public static void CopyTo(this OS.Model.FeatureCollectionType osMap, Ocad.Model.Map ocadMap)
        {
            //Distance left = new Distance(-20000, Distance.Unit.Metre, Scale.one);
            //Distance right = new Distance(20000, Distance.Unit.Metre, Scale.one);
            //Distance bottom = new Distance(-20000, Distance.Unit.Metre, Scale.one);
            //Distance top = new Distance(20000, Distance.Unit.Metre, Scale.one);
            //Rectangle boundary = new Rectangle(left, right, top, bottom);

            decimal scale = ocadMap.ScaleParameter.MapScale;
            Geometry.Point offset = new Geometry.Point(ocadMap.ScaleParameter.RealWorldOffsetX, ocadMap.ScaleParameter.RealWorldOffsetY);

            foreach (OS.Model.FeatureCollectionTypeFeatureMember member in osMap.featureMember)
            {
                ConvertItem(ocadMap, offset, scale, member.Item);
            }
        }

        #region Convert
        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.AbstractFeatureType item)
        {
            String type = item.GetType().FullName;
            switch (type)
            {
                case "OS.Model.AdministrativeBoundaryType":
                    break;
                case "OS.Model.AirportType":
                    break;
                case "OS.Model.BuildingType":
                    break;
                case "OS.Model.ElectricityTransmissionLineType":
                    break;
                case "OS.Model.ForeshoreType":
                    break;
                case "OS.Model.GlasshouseType":
                    break;
                case "OS.Model.HeritageSiteType":
                    break;
                case "OS.Model.LandType":
                    // Background Wash
                    break;
                case "OS.Model.MotorwayJunctionType":
                    break;
                case "OS.Model.NamedPlaceType":
                    break;
                case "OS.Model.OrnamentType":
                    // Slope Triangles
                    break;
                case "OS.Model.PublicAmenityType":
                    break;
                case "OS.Model.RailwayStationType":
                    break;
                case "OS.Model.RailwayTrackType":
                    ConvertItem(ocadMap, offset, scale, (OS.Model.RailwayTrackType)item);
                    break;
                case "OS.Model.RailwayTunnelType":
                    break;
                case "OS.Model.RoadTunnelType":
                    ConvertItem(ocadMap, offset, scale, (OS.Model.RoadTunnelType)item);
                    break;
                case "OS.Model.RoadType":
                    ConvertItem(ocadMap, offset, scale, (OS.Model.RoadType)item);
                    break;
                case "OS.Model.RoundaboutType":
                    break;
                case "OS.Model.SpotHeightType":
                    break;
                case "OS.Model.SurfaceWater_AreaType":
                    ConvertItem(ocadMap, offset, scale, (OS.Model.SurfaceWater_AreaType)item);
                    break;
                case "OS.Model.SurfaceWater_LineType":
                    ConvertItem(ocadMap, offset, scale, (OS.Model.SurfaceWater_LineType)item);
                    break;
                case "OS.Model.TidalBoundaryType":
                    break;
                case "OS.Model.TidalWaterType":
                    break;
                case "OS.Model.WoodlandType":
                    ConvertItem(ocadMap, offset, scale, (OS.Model.WoodlandType)item);
                    break;
                default:
                    throw (new NotImplementedException());
            }
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.RailwayTrackType item)
        {
            DrawAsOcadLineObject(ocadMap, offset, scale, item.geometry.Item, "515.0");
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.RoadTunnelType item)
        {
            DrawAsOcadLineObject(ocadMap, offset, scale, item.geometry.Item, "504.0");
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.RoadType item)
        {
            String symbolNumber = "504.0";
            switch (item.classification)
            {
                case OS.Model.RoadClassificationValueType.ARoad:
                    symbolNumber = "502.0";
                    break;
                case OS.Model.RoadClassificationValueType.ARoadCollapsedDualCarriageway:
                    symbolNumber = "501.0";
                    break;
                case OS.Model.RoadClassificationValueType.BRoad:
                    symbolNumber = "503.0";
                    break;
                case OS.Model.RoadClassificationValueType.BRoadCollapsedDualCarriageway:
                    symbolNumber = "503.0";
                    break;
                case OS.Model.RoadClassificationValueType.LocalStreet:
                    break;
                case OS.Model.RoadClassificationValueType.LocalStreetCollapsedDualCarriageway:
                    break;
                case OS.Model.RoadClassificationValueType.MinorRoad:
                    break;
                case OS.Model.RoadClassificationValueType.MinorRoadCollapsedDualCarriageway:
                    break;
                case OS.Model.RoadClassificationValueType.Motorway:
                    symbolNumber = "501.0";
                    break;
                case OS.Model.RoadClassificationValueType.MotorwayCollapsedDualCarriageway:
                    symbolNumber = "501.0";
                    break;
                case OS.Model.RoadClassificationValueType.PedestrianisedStreet:
                    symbolNumber = "507.0";
                    break;
                case OS.Model.RoadClassificationValueType.PrimaryRoad:
                    symbolNumber = "502.0";
                    break;
                case OS.Model.RoadClassificationValueType.PrimaryRoadCollapsedDualCarriageway:
                    symbolNumber = "501.0";
                    break;
                case OS.Model.RoadClassificationValueType.PrivateRoadPubliclyAccessible:
                    break;
            }

            DrawAsOcadLineObject(ocadMap, offset, scale, item.geometry.Item, symbolNumber);
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.SurfaceWater_AreaType item)
        {
            DrawAsOcadAreaObject(ocadMap, offset, scale, (OS.Model.SurfaceType)item.geometry.Item, "301.0", "301.1");
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.SurfaceWater_LineType item)
        {
            DrawAsOcadLineObject(ocadMap, offset, scale, item.geometry.Item, "305.0");
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.WoodlandType item)
        {
            DrawAsOcadAreaObject(ocadMap, offset, scale, (OS.Model.SurfaceType)item.geometry.Item, "408.0", null);
        }

        private static void DrawAsOcadLineObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.AbstractCurveType item, string symbolNumber)
        {
            Ocad.Model.AbstractSymbol symbol = ocadMap.GetSymbol(symbolNumber);
            if (symbol == null)
            {
                return;
            }

            if (item is OS.Model.LineStringType)
            {
                foreach (object elementItem in ((OS.Model.LineStringType)item).Items)
                {
                    foreach (Geometry.Path path in ClipOSLine((OS.Model.DirectPositionListType)elementItem))
                    {
                        Ocad.Model.SymbolObject ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, symbol);
                        BuildOcadPointList(offset, scale, path.Segments, ocadItem.Points);
                    }
                }
            }
            else if (item is OS.Model.CurveType)
            {
                foreach (OS.Model.LineStringSegmentType segmentItem in ((OS.Model.CurveType)item).segments.Items)
                {
                    foreach (object elementItem in segmentItem.Items)
                    {
                        foreach (Geometry.Path path in ClipOSLine((OS.Model.DirectPositionListType)elementItem))
                        {
                            Ocad.Model.SymbolObject ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, symbol);
                            BuildOcadPointList(offset, scale, path.Segments, ocadItem.Points);
                        }
                    }
                }
            }
            else
            {
                throw (new NotImplementedException());
            }
        }

        private static void DrawAsOcadAreaObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.SurfaceType item, string areaSymbolNumber, string holeSymbolNumber)
        {
            Ocad.Model.AbstractSymbol areaSymbol = ocadMap.GetSymbol(areaSymbolNumber);
            if (areaSymbol == null)
            {
                return;
            }
            Ocad.Model.AbstractSymbol holeBorderSymbol = ocadMap.GetSymbol(holeSymbolNumber);

            foreach (OS.Model.PolygonPatchType patch in item.Item.Items)
            {

                foreach (Object elementItem in ((OS.Model.LinearRingType)patch.exterior.Item).Items)
                {
                    foreach (Polygon polygon in ClipOSPolygon((OS.Model.DirectPositionListType)elementItem))
                    {
                        // TO DO handle multiple polygons, and put hole in correct polygon
                        Ocad.Model.SymbolObject areaOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Area, areaSymbol);
                        BuildOcadPointList(offset, scale, polygon.Segments, areaOcadItem.Points);

                        if (holeBorderSymbol != null)
                        {
                            Ocad.Model.SymbolObject borderOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, holeBorderSymbol);
                            BuildOcadPointList(offset, scale, polygon.Segments, borderOcadItem.Points);
                        }

                        if (patch.interior != null)
                        {
                            foreach (OS.Model.AbstractRingPropertyType hole in patch.interior)
                            {
                                OS.Model.LinearRingType holeRing = (OS.Model.LinearRingType)hole.Item;

                                foreach (Object holeElementItem in holeRing.Items)
                                {
                                    foreach (Polygon holePolygon in ClipOSPolygon((OS.Model.DirectPositionListType)holeElementItem))
                                    {
                                        BuildOcadPointList(offset, scale, holePolygon.Segments, areaOcadItem.Points, Ocad.Model.Type.PointFlag.HolePoint);

                                        if (holeBorderSymbol != null)
                                        {
                                            Ocad.Model.SymbolObject borderOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, holeBorderSymbol);
                                            BuildOcadPointList(offset, scale, holePolygon.Segments, borderOcadItem.Points);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static List<Geometry.Path> ClipOSLine(OS.Model.DirectPositionListType item)
        {
            // path may be empty if consecutive points are less than epsilon (1m apart)
            Geometry.Path path = new Geometry.Path(BuildOSSegmentList(item), epsilon);
            if (path.Segments.Count == 0)
            {
                return new List<Geometry.Path> { };
            }
            return new List<Geometry.Path> { path };
        }

        private static List<Polygon> ClipOSPolygon(OS.Model.DirectPositionListType item)
        {
            // polygon may be empty if consecutive points are less than epsilon (1m apart)
            Polygon polygon = new Polygon(BuildOSSegmentList(item), epsilon);
            if (polygon.Segments.Count == 0)
            {
                return new List<Polygon> { };
            }
            return new List<Polygon> { polygon };

            //return new List<Polygon> { }; // _boundary.Clip(polygon, epsilon);
        }

        private static List<ISegment> BuildOSSegmentList(OS.Model.DirectPositionListType item)
        {
            List<ISegment> segmentList = new List<ISegment>();

            Geometry.Point start = new Geometry.Point(
                    new Distance(item.textField[0], Distance.Unit.Metre, Scale.one),
                    new Distance(item.textField[1], Distance.Unit.Metre, Scale.one));
            Geometry.Point end;

            for (int i = 2; i < item.textField.Length; i += 2)
            {
                end = new Geometry.Point(
                        new Distance(item.textField[i], Distance.Unit.Metre, Scale.one),
                        new Distance(item.textField[i + 1], Distance.Unit.Metre, Scale.one));

                segmentList.Add(new LinearSegment(start, end));

                start = end;
            }

            return segmentList;
        }

        /*
        private static void BuildOcadPointList(Geometry.Point offset, decimal scale, List<ISegment> segments, List<Ocad.Model.Point> points, Ocad.Model.Type.PointFlag firstPointFlag = Ocad.Model.Type.PointFlag.BasicPoint)
        {
            Ocad.Model.Point firstPoint = new Ocad.Model.Point(segments[0].Start, offset, scale);
            firstPoint.MainPointFlag = firstPointFlag;
            points.Add(firstPoint);
            foreach (ISegment segment in segments)
            {
                points.Add(new Ocad.Model.Point(segment.End, offset, scale));
            }
        }
         */
        #endregion
    }
}
