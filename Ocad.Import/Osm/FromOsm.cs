using System;
using System.Collections.Generic;
using System.Text;
using Geometry;
using Osm_Model = Osm.Model;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace Ocad.Import
{
    public static partial class FromOsm
    {
        public static Ocad.Import.Osm.Conversion Setting = Ocad.Import.Osm.Setting.Default.Conversion;

        public static void CopyTo(this Osm_Model.osm osmMap, Ocad.Model.Map ocadMap)
        {
            decimal scale = ocadMap.ScaleParameter.MapScale;
            Geometry.Point offset = new Geometry.Point(ocadMap.ScaleParameter.RealWorldOffsetX, ocadMap.ScaleParameter.RealWorldOffsetY);

            //Distance left = new Distance(-20000, Distance.Unit.Metre, Scale.one);
            //Distance right = new Distance(20000, Distance.Unit.Metre, Scale.one);
            //Distance bottom = new Distance(-20000, Distance.Unit.Metre, Scale.one);
            //Distance top = new Distance(20000, Distance.Unit.Metre, Scale.one);
            //Rectangle boundary = new Rectangle(left, right, top, bottom);

            if (osmMap.node == null)
            {
                return;
            }

            Dictionary<ulong, Geometry.Point> points = new Dictionary<ulong, Geometry.Point>();
            foreach (Osm_Model.node node in osmMap.node)
            {
                TDPG.GeoCoordConversion.PolarGeoCoordinate wgs84Point = new TDPG.GeoCoordConversion.PolarGeoCoordinate(
                    node.lat,
                    node.lon,
                    0,
                    TDPG.GeoCoordConversion.AngleUnit.Degrees,
                    TDPG.GeoCoordConversion.CoordinateSystems.WGS84);
                TDPG.GeoCoordConversion.GridReference osPoint = TDPG.GeoCoordConversion.PolarGeoCoordinate.ChangeToGridReference(wgs84Point);

                Geometry.Point p = new Geometry.Point(
                        new Geometry.Distance((int)osPoint.Easting, Geometry.Distance.Unit.Metre, Geometry.Scale.one),
                        new Geometry.Distance((int)osPoint.Northing, Geometry.Distance.Unit.Metre, Geometry.Scale.one));

                points.Add(node.id, p);
            }

            foreach (Osm_Model.way way in osmMap.way)
            {
                ConvertItem(ocadMap, offset, scale, way, points);
            }

            /*
            foreach (Osm_Model.relation relation in osmMap.relation)
            {
                //ConvertItem(member.Item, ocadMap);
            }
             * */
        }

        #region Convert
        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Osm_Model.way way, Dictionary<ulong, Geometry.Point> points)
        {
            if (way.tag == null) { return; }

            foreach (Osm_Model.tag tag in way.tag)
            {
                OcadSymbolSet ocadSymbolSet;
                if (Setting.OsmToOcadDictionary.TryGetValue(String.Format(Ocad.Import.Osm.Conversion.KEY_FORMAT, tag.k, tag.v), out ocadSymbolSet))
                {
                    ConvertItem(ocadMap, ocadSymbolSet, offset, scale, way, points);
                    break;
                }
            }
        }

        private static void ConvertItem(Ocad.Model.Map ocadMap, OcadSymbolSet ocadSymbolSet, Geometry.Point offset, decimal scale, Osm_Model.way way, Dictionary<ulong, Geometry.Point> points)
        {
            #region Draw Line, including Bridge and Tunnel
            if (ocadSymbolSet.LineSymbolNumber != null)
            {
                #region Add Bridge/Tunnel
                bool isTunnel = false;
                foreach (Osm_Model.tag tag in way.tag)
                {
                    bool isBridge = false;

                    if (tag.k == Ocad.Import.Osm.Setting.Default.BridgeTagKey)
                    {
                        isBridge = true;
                    }
                    else if (tag.k == Ocad.Import.Osm.Setting.Default.TunnelTagKey)
                    {
                        isTunnel = true;
                    }
                    else
                    {
                        continue;
                    }

                    #region Calculate Width of Bridge
                    Distance width;
                    Ocad.Model.LineSymbol lineSymbol = (Ocad.Model.LineSymbol)ocadMap.GetSymbol(ocadSymbolSet.LineSymbolNumber);
                    if (lineSymbol == null)
                    {
                        width = new Distance(Ocad.Import.Osm.Setting.Default.MinimumRealWorldMetreWidthOfBridge * 0.5M, Distance.Unit.Metre, Scale.one);
                    }
                    else
                    {
                        if (lineSymbol.DoubleMode == Model.Type.DoubleLineMode.Off)
                        {
                            width = (lineSymbol.LineWidth * ocadMap.ScaleParameter.MapScale * 0.5M) + new Distance(Ocad.Import.Osm.Setting.Default.AdditionalRealWorldMetreWidthOfBridge * 0.5M, Distance.Unit.Metre, Scale.one);
                        }
                        else
                        {
                            width = ((lineSymbol.DoubleWidth + lineSymbol.DoubleRightWidth + lineSymbol.DoubleLeftWidth) * ocadMap.ScaleParameter.MapScale * 0.5M) + new Distance(Ocad.Import.Osm.Setting.Default.AdditionalRealWorldMetreWidthOfBridge * 0.5M, Distance.Unit.Metre, Scale.one);
                        }
                    }
                    #endregion

                    #region Draw Bridge/Tunnel
                    if (isBridge)
                    {
                        DrawAsOcadBridgeObject(ocadMap, offset, scale, way, points, width);
                    }
                    else if (isTunnel)
                    {
                        DrawAsOcadTunnelObject(ocadMap, offset, scale, way, points, width);
                    }
                    #endregion

                    break;
                }
                #endregion

                if (!isTunnel)
                {
                    DrawAsOcadLineObject(ocadMap, offset, scale, way, points, ocadSymbolSet.LineSymbolNumber);
                }
            }
            #endregion

            #region Draw Area
            if (ocadSymbolSet.AreaSymbolNumber != null)
            {
                DrawAsOcadAreaObject(ocadMap, offset, scale, way, points, ocadSymbolSet.AreaSymbolNumber);
            }
            #endregion
        }

        private static void DrawAsOcadLineObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Osm_Model.way way, Dictionary<ulong, Geometry.Point> points, string symbolNumber)
        {
            Ocad.Model.AbstractSymbol lineSymbol = ocadMap.GetSymbol(symbolNumber);
            if (lineSymbol == null)
            {
                return;
            }

            List<ISegment> segments = BuildOsmSegmentList(way, points);
            if (segments.Count >= 1)
            {
                Ocad.Model.SymbolObject ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, lineSymbol);
                Common.BuildOcadPointList(offset, scale, segments, ocadItem.Points, false);
            }
        }

        private static void DrawAsOcadBridgeObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Osm_Model.way way, Dictionary<ulong, Geometry.Point> points, Distance width)
        {
            OcadSymbolSet ocadSymbolSet;
            if (!Setting.OsmToOcadDictionary.TryGetValue(String.Format(Ocad.Import.Osm.Conversion.KEY_FORMAT, Ocad.Import.Osm.Setting.Default.BridgeTagKey, String.Empty), out ocadSymbolSet))
            {
                return;
            }

            Ocad.Model.AbstractSymbol bridgeSymbol = ocadMap.GetSymbol(ocadSymbolSet.LineSymbolNumber);
            if (bridgeSymbol == null)
            {
                return;
            }

            List<ISegment> lhsSegmentList, rhsSegmentList;
            BuildOsmSegmentList(way, points, width, out lhsSegmentList, out rhsSegmentList);
            if (lhsSegmentList.Count >= 1)
            {
                Ocad.Model.SymbolObject lhsOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, bridgeSymbol);
                Common.BuildOcadPointList(offset, scale, lhsSegmentList, lhsOcadItem.Points, false);

                Ocad.Model.SymbolObject rhsOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, bridgeSymbol);
                Common.BuildOcadPointList(offset, scale, rhsSegmentList, rhsOcadItem.Points, false);
            }
        }

        private static void DrawAsOcadTunnelObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Osm_Model.way way, Dictionary<ulong, Geometry.Point> points, Distance width)
        {
            OcadSymbolSet ocadSymbolSet;
            if (!Setting.OsmToOcadDictionary.TryGetValue(String.Format(Ocad.Import.Osm.Conversion.KEY_FORMAT, Ocad.Import.Osm.Setting.Default.BridgeTagKey, String.Empty), out ocadSymbolSet))
            {
                return;
            }

            Ocad.Model.AbstractSymbol bridgeSymbol = ocadMap.GetSymbol(ocadSymbolSet.LineSymbolNumber);
            if (bridgeSymbol == null)
            {
                return;
            }

            List<ISegment> lhsSegmentList, rhsSegmentList;
            BuildOsmSegmentList(way, points, width, out lhsSegmentList, out rhsSegmentList);
            if (lhsSegmentList.Count >= 1)
            {
                List<ISegment> startSegmentList = new List<ISegment> { new LinearSegment(rhsSegmentList[rhsSegmentList.Count - 1].End, lhsSegmentList[0].Start) };
                Ocad.Model.SymbolObject startOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, bridgeSymbol);
                Common.BuildOcadPointList(offset, scale, startSegmentList, startOcadItem.Points, false);

                List<ISegment> endSegmentList = new List<ISegment> { new LinearSegment(lhsSegmentList[lhsSegmentList.Count - 1].End, rhsSegmentList[0].Start) };
                Ocad.Model.SymbolObject endOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, bridgeSymbol);
                Common.BuildOcadPointList(offset, scale, endSegmentList, endOcadItem.Points, false);
            }
        }

        private static void DrawAsOcadAreaObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Osm_Model.way way, Dictionary<ulong, Geometry.Point> points, string symbolNumber)
        {
            Ocad.Model.AbstractSymbol areaSymbol = ocadMap.GetSymbol(symbolNumber);
            if (areaSymbol == null)
            {
                return;
            }

            List<ISegment> segments = BuildOsmSegmentList(way, points);
            if (segments.Count >= 2)
            {
                Ocad.Model.SymbolObject areaOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Area, areaSymbol);
                Common.BuildOcadPointList(offset, scale, segments, areaOcadItem.Points, true);
            }
        }

        private static List<ISegment> BuildOsmSegmentList(Osm_Model.way way, Dictionary<ulong, Geometry.Point> points)
        {
            List<ISegment> segmentList = new List<ISegment>();

            Geometry.Point start = points[way.nd[0].@ref];
            Geometry.Point end = null;

            for (int i = 1; i < way.nd.Length; i += 1)
            {
                end = points[way.nd[i].@ref];
                segmentList.Add(new LinearSegment(start, end));

                start = end;
            }

            return segmentList;
        }

        private static void BuildOsmSegmentList(Osm_Model.way way, Dictionary<ulong, Geometry.Point> points, Distance width, out List<ISegment> lhsSegmentList, out List<ISegment> rhsSegmentList)
        {
            lhsSegmentList = new List<ISegment>();
            rhsSegmentList = new List<ISegment>();

            List<double> rhsPointAngleList = new List<double>();

            #region Initialise P & Q
            Geometry.Point p = points[way.nd[0].@ref];
            Geometry.Point q = points[way.nd[1].@ref];

            double px = (double)p.X[0, Distance.Unit.Metre, Scale.one];
            double py = (double)p.Y[0, Distance.Unit.Metre, Scale.one];
            double qx = (double)q.X[0, Distance.Unit.Metre, Scale.one];
            double qy = (double)q.Y[0, Distance.Unit.Metre, Scale.one];

            double pqx = qx - px;
            double pqy = qy - py;
            #endregion

            double xpq = Math.Atan2(pqy, pqx) - (Math.PI / 2);
            rhsPointAngleList.Add(xpq);

            for (int i = 1; i <= way.nd.Length - 2; i++)
            {
                double previousXpq = xpq;

                p = q;
                q = points[way.nd[i+1].@ref];

                px = qx;
                py = qy;
                qx = (double)q.X[0, Distance.Unit.Metre, Scale.one];
                qy = (double)q.Y[0, Distance.Unit.Metre, Scale.one];

                pqx = qx - px;
                pqy = qy - py;
                xpq = Math.Atan2(pqy, pqx) - (Math.PI / 2);

                rhsPointAngleList.Add((xpq + xpq) / 2);
            }

            rhsPointAngleList.Add(xpq);

            List<Geometry.Point> lhsPointList = new List<Point>();
            List<Geometry.Point> rhsPointList = new List<Point>();
            for (int i = 0; i < way.nd.Length; i += 1)
            {
                q = points[way.nd[i].@ref];

                Distance dx = width * (decimal)Math.Cos(rhsPointAngleList[i]);
                Distance dy = width * (decimal)Math.Sin(rhsPointAngleList[i]);

                //TODO add bit to end if bridge - not tunnel

                rhsPointList.Add(new Geometry.Point(q.X + dx, q.Y + dy));
                lhsPointList.Add(new Geometry.Point(q.X - dx, q.Y - dy));
            }

            #region Build segment lists
            Geometry.Point start = lhsPointList[0];
            for (int i = 1; i < lhsPointList.Count; i += 1)
            {
                Geometry.Point end = lhsPointList[i];
                lhsSegmentList.Add(new LinearSegment(start, end));

                start = end;
            }

            start = rhsPointList[rhsPointList.Count - 1];
            for (int i = rhsPointList.Count - 2; i >= 0; i -= 1)
            {
                Geometry.Point  end = rhsPointList[i];
                rhsSegmentList.Add(new LinearSegment(start, end));
                start = end;
            }
            #endregion
        }
        #endregion
    }
}
