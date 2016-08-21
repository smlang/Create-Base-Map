using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class FromOS
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
                if (member.Item != null)
                {
                    ConvertItem(ocadMap, offset, scale, member.Item);
                }
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
            DrawAsOcadLineObject(ocadMap, offset, scale, item.geometry.Item, Iof.Isom.Symbols._515_Railway);
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.RoadTunnelType item)
        {
            DrawAsOcadLineObject(ocadMap, offset, scale, item.geometry.Item, Iof.Isom.Symbols._504_Road);
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.RoadType item)
        {
            Iof.Isom.Symbols isomSymbol = Iof.Isom.Symbols._504_Road;
            switch (item.classification)
            {
                case OS.Model.RoadClassificationValueType.ARoad:
                    isomSymbol = Iof.Isom.Symbols._502_ARoad;
                    break;
                case OS.Model.RoadClassificationValueType.ARoadCollapsedDualCarriageway:
                    isomSymbol = Iof.Isom.Symbols._501_DualCarriageWay;
                    break;
                case OS.Model.RoadClassificationValueType.BRoad:
                    isomSymbol = Iof.Isom.Symbols._503_BRoad;
                    break;
                case OS.Model.RoadClassificationValueType.BRoadCollapsedDualCarriageway:
                    isomSymbol = Iof.Isom.Symbols._503_BRoad;
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
                    isomSymbol = Iof.Isom.Symbols._501_DualCarriageWay;
                    break;
                case OS.Model.RoadClassificationValueType.MotorwayCollapsedDualCarriageway:
                    isomSymbol = Iof.Isom.Symbols._501_DualCarriageWay;
                    break;
                case OS.Model.RoadClassificationValueType.PedestrianisedStreet:
                    isomSymbol = Iof.Isom.Symbols._507_Footpath;
                    break;
                case OS.Model.RoadClassificationValueType.PrimaryRoad:
                    isomSymbol = Iof.Isom.Symbols._502_ARoad;
                    break;
                case OS.Model.RoadClassificationValueType.PrimaryRoadCollapsedDualCarriageway:
                    isomSymbol = Iof.Isom.Symbols._501_DualCarriageWay;
                    break;
                case OS.Model.RoadClassificationValueType.PrivateRoadPubliclyAccessible:
                    break;
            }

            DrawAsOcadLineObject(ocadMap, offset, scale, item.geometry.Item, isomSymbol);
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.SurfaceWater_AreaType item)
        {
            DrawAsOcadAreaObject(ocadMap, offset, scale, (OS.Model.SurfaceType)item.geometry.Item, Iof.Isom.Symbols._301_Lake);
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.SurfaceWater_LineType item)
        {
            DrawAsOcadLineObject(ocadMap, offset, scale, item.geometry.Item, Iof.Isom.Symbols._306_Stream);
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.WoodlandType item)
        {
            DrawAsOcadAreaObject(ocadMap, offset, scale, (OS.Model.SurfaceType)item.geometry.Item, Iof.Isom.Symbols._408_Woodland);
        }

        private static void DrawAsOcadLineObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.AbstractCurveType item, Iof.Isom.Symbols isomSymbol)
        {
            string ocadAreaSymbolNumber, ocadLineSymbolNumber, ocadPointSymbolNumber;
            Common.GetOcadSymbol(isomSymbol, out ocadAreaSymbolNumber, out ocadLineSymbolNumber, out ocadPointSymbolNumber);

            Ocad.Model.AbstractSymbol symbol = ocadMap.GetSymbol(ocadLineSymbolNumber);
            if (symbol == null)
            {
                return;
            }

            if (item is OS.Model.LineStringType)
            {
                foreach (object elementItem in ((OS.Model.LineStringType)item).Items)
                {
                    List<ISegment> segments = BuildOSSegmentList((OS.Model.DirectPositionListType)elementItem);
                    if (segments.Count >= 1)
                    {
                        Ocad.Model.SymbolObject ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, symbol);
                        Common.BuildOcadPointList(offset, scale, segments, ocadItem.Points, false);
                    }
                }
            }
            else if (item is OS.Model.CurveType)
            {
                foreach (OS.Model.LineStringSegmentType segmentItem in ((OS.Model.CurveType)item).segments.Items)
                {
                    foreach (object elementItem in segmentItem.Items)
                    {
                        List<ISegment> segments = BuildOSSegmentList((OS.Model.DirectPositionListType)elementItem);
                        if (segments.Count >= 1)
                        {
                            Ocad.Model.SymbolObject ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, symbol);
                            Common.BuildOcadPointList(offset, scale, segments, ocadItem.Points, false);
                        }
                    }
                }
            }
            else
            {
                throw (new NotImplementedException());
            }
        }

        private static void DrawAsOcadAreaObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, OS.Model.SurfaceType item, Iof.Isom.Symbols isomSymbol)
        {
            string ocadAreaSymbolNumber, ocadLineSymbolNumber, ocadPointSymbolNumber;
            Common.GetOcadSymbol(isomSymbol, out ocadAreaSymbolNumber, out ocadLineSymbolNumber, out ocadPointSymbolNumber);

            Ocad.Model.AbstractSymbol areaSymbol = ocadMap.GetSymbol(ocadAreaSymbolNumber);
            if (areaSymbol == null)
            {
                return;
            }
            //Ocad.Model.AbstractSymbol borderSymbol = ocadMap.GetSymbol(ocadLineSymbolNumber);

            foreach (OS.Model.PolygonPatchType patch in item.Item.Items)
            {
                foreach (Object elementItem in ((OS.Model.LinearRingType)patch.exterior.Item).Items)
                {
                    // TO DO handle multiple polygons, and put hole in correct polygon
                    List<ISegment> segments = BuildOSSegmentList((OS.Model.DirectPositionListType)elementItem);
                    if (segments.Count >= 2)
                    {
                        Ocad.Model.SymbolObject areaOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Area, areaSymbol);

                        Common.BuildOcadPointList(offset, scale, segments, areaOcadItem.Points, true);
                        //if (borderSymbol != null)
                        //{
                        //    Ocad.Model.SymbolObject borderOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, borderSymbol);
                        //    BuildOcadPointList(offset, scale, segments, borderOcadItem.Points, true);
                        //}

                        if (patch.interior != null)
                        {
                            foreach (OS.Model.AbstractRingPropertyType hole in patch.interior)
                            {
                                OS.Model.LinearRingType holeRing = (OS.Model.LinearRingType)hole.Item;

                                foreach (Object holeElementItem in holeRing.Items)
                                {
                                    List<ISegment> holeSegments = BuildOSSegmentList((OS.Model.DirectPositionListType)elementItem);
                                    if (holeSegments.Count >= 2)
                                    {
                                        Common.BuildOcadPointList(offset, scale, holeSegments, areaOcadItem.Points, true, Ocad.Model.Type.PointFlag.HolePoint);
                                        //if (borderSymbol != null)
                                        //{
                                        //    Ocad.Model.SymbolObject holeBorderOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, borderSymbol);
                                        //    BuildOcadPointList(offset, scale, holeSegments, holeBorderOcadItem.Points, true);
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
        #endregion
    }
}
