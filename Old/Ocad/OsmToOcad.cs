using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class
         | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute {}
}

namespace OcadConsole
{
    public static partial class ModelDeserialize
    {
        /*
http://overpass.osm.rambler.ru/cgi/xapi_meta?*[bbox=minlon,minlat,maxlon,maxlat]
http://overpass.osm.rambler.ru/cgi/xapi_meta?*[bbox=-2.41025,53.34575,-2.05475,53.56325]
http://www.overpass-api.de/api/xapi_meta?*[bbox=-2.41025,53.34575,-2.05475,53.56325]
         */

        private static Distance epsilon = new Distance(1M, Distance.Unit.Metre, Scale.one);

        public static void CopyTo(this Osm.Model.osm osmMap, Ocad.Model.Map ocadMap)
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
            foreach (Osm.Model.node node in osmMap.node)
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

            int i = 0;
            foreach (Osm.Model.way way in osmMap.way)
            {
                ConvertItem(ocadMap, offset, scale, way, points);
                i++;
            }

            /*
            foreach (Osm.Model.relation relation in osmMap.relation)
            {
                //ConvertItem(member.Item, ocadMap);
            }
             * */
        }

        #region Convert
        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Osm.Model.way way, Dictionary<ulong, Geometry.Point> points)
        {
            if (way.tag == null) { return; }

            foreach (Osm.Model.tag tag in way.tag)
            {
                switch (tag.k)
                {
                    case "building":
                    case "shop":
                    case "sport":
                    case "power":
                        return;
                    case "highway":
                    case "water":
                    case "waterway":
                    case "natural":
                    case "wood":
                    case "leisure":
                    case "aeroway":
                    case "landuse":
                    case "amenity":
                    case "man_made":
                    case "railway":
                        if (ConvertItem(ocadMap, offset, scale, way, points, tag.k + "::" + tag.v))
                        {
                            return;
                        }
                        break;
                }
            }
        }

        private static bool ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Osm.Model.way way, Dictionary<ulong, Geometry.Point> points, string value)
        {
            bool result = false;

            String lineSymbolNumber = null;
            String areaSymbolNumber = null;

            switch (value)
            {
                case "waterway::riverbank":
                case "waterway::dock":
                case "waterway::feature":
                case "waterway::res":
                case "waterway::reservoir":
                case "natural::water":
                case "leisure::fishing":
                case "leisure::marina":
                case "landuse::reservoir":
                case "landuse::basin":
                    areaSymbolNumber = "301.0";
                    lineSymbolNumber = "301.1";
                    break;
                case "water::pond":
                case "natural::pond":
                case "leisure::paddling_pool":
                case "landuse::pond":
                case "amenity::fountain":
                    areaSymbolNumber = "302.0";
                    break;
                case "waterway::river":
                case "waterway::canal":
                case "waterway::derelict_canal":
                case "waterway::disused_canal":
                    lineSymbolNumber = "302.1";
                    break;
                case "waterway::stream":
                case "waterway::drain":
                case "waterway::ditch":
                case "leisure::slipway":
                    lineSymbolNumber = "305.0";
                    break;
                case "leisure::pitch":
                case "leisure::playground":
                case "leisure::recreation_ground":
                case "leisure::village_green":
                case "leisure::sports_field":
                case "landuse::grass":
                case "landuse::recreation_ground":
                case "landuse::village_green":
                case "landuse::picnic_area":
                case "landuse::pitch":
                case "amenity::park":
                    areaSymbolNumber = "401.0";
                    break;
                case "natural::scrub":
                case "natural::scrubland":
                case "natural::heath":
                case "natural::grassland":
                case "leisure::park":
                case "leisure::common":
                case "landuse::meadow":
                    areaSymbolNumber = "403.0";
                    break;
                case "natural::wood":
                case "natural::woodland":
                case "natural::tree":
                case "natural::tree_row":
                case "natural::trees":
                case "wood::mixed":
                case "wood::deciduous":
                case "wood::coniferous":
                case "landuse::forest":
                case "landuse::orchard":
                    areaSymbolNumber = "408.0";
                    break;
                case "highway::motorway":
                    lineSymbolNumber = "501.0";
                    break;
                case "highway::turning_circle":
                case "highway::layby":
                    areaSymbolNumber = "501.3";
                    break;
                case "highway::trunk":
                case "highway::primary":
                case "highway::motorway_link":
                case "highway::trunk_link":
                case "highway::primary_link":
                    lineSymbolNumber = "502.0";
                    break;
                case "highway::secondary":
                case "highway::tertiary":
                case "highway::secondary_link":
                case "highway::tertiary_link":
                    lineSymbolNumber = "503.0";
                    break;
                case "highway::unclassified":
                case "highway::residential":
                case "highway::service":
                case "highway::services":
                case "highway::living_street":
                case "highway::raceway":
                case "highway::road":
                case "highway::lane":
                case "highway::FIXME":
                case "man_made::jetty":
                case "man_made::pier":
                    lineSymbolNumber = "504.0";
                    break;
                case "highway::track":
                case "highway::unsurfaced":
                case "highway::byway":
                    lineSymbolNumber = "505.0";
                    break;
                case "highway::pedestrian":
                case "highway::footway":
                case "highway::cycleway":
                case "highway::bridleway":
                case "highway::steps":
                case "highway::path":
                case "leisure::track":
                    lineSymbolNumber = "507.0";
                    break;
                case "highway::bus_guideway":
                case "railway::rail":
                case "railway::tram":
                case "railway::disused":
                case "railway::preserved":
                case "railway::narrow_gauge":
                case "railway::miniature":
                case "railway::construction":
                case "railway::historic":
                    lineSymbolNumber = "515.0";
                    break;
                case "highway::platform":
                case "leisure::skate_park":
                case "aeroway::apron":
                case "aeroway::runway":
                case "aeroway::taxiway":
                case "landuse::traffic_island":
                case "amenity::parking":
                    areaSymbolNumber = "529.0";
                    break;
                /*
            case "highway::proposed":
            case "highway::construction":
            case "highway::crossing":
            case "railway::abandoned":
            case "railway::subway":
            case "railway::dismantled":
            case "railway::platform":
            case "railway::proposed":
            case "railway::station":
                case "man_made::arch":
                case "man_made::cutline":
                case "man_made::embankment":
                case "man_made::gasometer":
                case "man_made::pipeline":
                case "man_made::reservoir_covered":
                case "man_made::silo":
                case "man_made::spoil_heap":
                case "man_made::storage_tank":
                case "man_made::telephone_exchange":
                case "man_made::tower":
                case "man_made::wastewater_plant":
                case "man_made::water_works":
                case "man_made::works":
                case "man_made::sewage_works":
                case "man_made::wastewater_works":
                case "waterway::boatyard":
                case "waterway::weir":
                case "waterway::dam":
                case "waterway::lock_gate":
                case "waterway::sluice":
                case "waterway::barrier":
                case "waterway::mooring":
                case "natural::coastline":
                case "natural::cliff":
                case "natural::wetland":
                case "natural::land":
                case "natural::beach":
                case "natural::marsh":
                case "natural::sand":
                case "natural::yes":
                case "leisure::stadium":
                case "leisure::nature_reserve":
                case "leisure::golf_course":
                case "leisure::sports_centre":
                case "leisure::garden":
                case "leisure::gym":
                case "leisure::yes":
                case "leisure::walled garden":
                case "leisure::driving_range":
                case "leisure::social_club":
                case "leisure::bingo":
                case "leisure::bingo_hall":
                case "leisure::ice_rink":
                case "leisure::swimming_pool":
                case "leisure::club":
                case "leisure::theatre":
                case "leisure::swimming_pool;sauna":
                case "landuse::industrial":
                case "landuse::allotments":
                case "landuse::cemetery":
                case "landuse::railway":
                case "landuse::farm":
                case "landuse::residential":
                case "landuse::construction":
                case "landuse::commercial":
                case "landuse::retail":
                case "landuse::quarry":
                case "landuse::sports":
                case "landuse::farmland":
                case "landuse::farmyard":
                case "landuse::landfill":
                case "landuse::brownfield":
                case "landuse::mobile_home_park":
                case "landuse::wasteland":
                case "landuse::plant_orchard":
                case "landuse::proposed_construction":
                case "landuse::greenfield":
                case "landuse::observatory":
                case "landuse::telecommunications":
                case "landuse::filming":
                case "landuse::field":
                case "landuse::garages":
                case "landuse::grave_yard":
                case "landuse::graveyard":
                case "landuse::conservation":
                case "landuse::resdi":
                case "landuse::leisure":
                case "landuse::rail":
                case "landuse::plant_nursery":
                case "landuse::garden":
                case "landuse::disused":
                case "amenity::school":
                case "amenity::police":
                case "amenity::bus_station":
                case "amenity::place_of_worship":
                case "amenity::university":
                case "amenity::college":
                case "amenity::grave_yard":
                case "amenity::doctors":
                case "amenity::hospital":
                case "amenity::toilets":
                case "amenity::scout_hall":
                case "amenity::prison":
                case "amenity::recycling":
                case "amenity::fuel":
                 */
                default:
                    return false;
            }

            if (lineSymbolNumber != null)
            {
                DrawAsOcadLineObject(ocadMap, offset, scale, way, points, lineSymbolNumber);

                foreach (Osm.Model.tag tag in way.tag)
                {
                    bool isBridge = false;
                    bool isTunnel = false;

                    switch (tag.k)
                    {
                        case "bridge":
                            isBridge = true;
                            // bridge=yes
                            break;
                        case "tunnel":
                            isTunnel = true;
                            // tunnel=yes
                            break;
                        default:
                            break;
                    }

                    if (isBridge)
                    {
                        break;
                    }
                    if (isTunnel)
                    {
                        break;
                    }
                }
            }

            if (areaSymbolNumber != null)
            {
                DrawAsOcadAreaObject(ocadMap, offset, scale, way, points, areaSymbolNumber);
            }

            return result;
        }

        private static void DrawAsOcadLineObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Osm.Model.way way, Dictionary<ulong, Geometry.Point> points, string symbolNumber)
        {
            Ocad.Model.AbstractSymbol symbol = ocadMap.GetSymbol(symbolNumber);
            if (symbol == null)
            {
                return;
            }

            foreach (Geometry.Path path in ClipOsmLine(way, points))
            {
                Ocad.Model.SymbolObject ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, symbol);
                BuildOcadPointList(offset, scale, path.Segments, ocadItem.Points);
            }
        }

        private static void DrawAsOcadAreaObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Osm.Model.way way, Dictionary<ulong, Geometry.Point> points, string symbolNumber)
        {
            Ocad.Model.AbstractSymbol symbol = ocadMap.GetSymbol(symbolNumber);
            if (symbol == null)
            {
                return;
            }

            foreach (Polygon polygon in ClipOsmPolygon(way, points))
            {
                // TO DO handle multiple polygons, and put hole in correct polygon
                Ocad.Model.SymbolObject areaOcadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Area, symbol);
                BuildOcadPointList(offset, scale, polygon.Segments, areaOcadItem.Points);
            }
        }

        private static List<Geometry.Path> ClipOsmLine(Osm.Model.way way, Dictionary<ulong, Geometry.Point> points)
        {
            // path may be empty if consecutive points are less than epsilon (1m apart)
            Geometry.Path path = new Geometry.Path(BuildOsmSegmentList(way, points), epsilon);
            if (path.Segments.Count == 0)
            {
                return new List<Geometry.Path> { };
            }
            return new List<Geometry.Path> { path };
        }

        private static List<Polygon> ClipOsmPolygon(Osm.Model.way way, Dictionary<ulong, Geometry.Point> points)
        {
            // polygon may be empty if consecutive points are less than epsilon (1m apart)
            Polygon polygon = new Polygon(BuildOsmSegmentList(way, points), epsilon);
            if (polygon.Segments.Count == 0)
            {
                return new List<Polygon> { };
            }
            return new List<Polygon> { polygon };
        }

        private static List<ISegment> BuildOsmSegmentList(Osm.Model.way way, Dictionary<ulong, Geometry.Point> points)
        {
            List<ISegment> segmentList = new List<ISegment>();

            Geometry.Point start = points[way.nd[0].@ref];
            Geometry.Point end;

            for (int i = 1; i < way.nd.Length; i += 1)
            {
                end = points[way.nd[i].@ref];
                segmentList.Add(new LinearSegment(start, end));

                start = end;
            }

            return segmentList;
        }

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
        #endregion
    }
}
