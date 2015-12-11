using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Import;

namespace OcadConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Ocad.Model.Map ocadMap = Ocad.Model.Map.Import(@"D:\Users\Steve\Desktop\Street Base Map 15000.ocd");

            TDPG.GeoCoordConversion.GridReference bottomLeftOSPoint = new TDPG.GeoCoordConversion.GridReference("SJ", "89", "88");
            TDPG.GeoCoordConversion.PolarGeoCoordinate bottomLeftWgs84Point = TDPG.GeoCoordConversion.GridReference.ChangeToPolarGeo(bottomLeftOSPoint);
            double minlat = bottomLeftWgs84Point.Lat;
            double minlon = bottomLeftWgs84Point.Lon;

            TDPG.GeoCoordConversion.GridReference topRightOSPoint = new TDPG.GeoCoordConversion.GridReference(bottomLeftOSPoint.Easting + 1000, bottomLeftOSPoint.Northing + 1000);
            TDPG.GeoCoordConversion.PolarGeoCoordinate topRightWgs84Point = TDPG.GeoCoordConversion.GridReference.ChangeToPolarGeo(topRightOSPoint);
            double maxlat = topRightWgs84Point.Lat;
            double maxlon = topRightWgs84Point.Lon;

            string url = String.Format(@"http://www.overpass-api.de/api/xapi_meta?*[bbox={0},{1},{2},{3}]", minlon, minlat, maxlon, maxlat);
            using (Net.Resource x = new Net.Resource(url))
            {
                Osm.Model.osm osmModel = Osm.Model.osm.Import(x.Stream);
                osmModel.CopyTo(ocadMap);
            }

/*
            String[] osmDataFilePaths = new String[] 
            { 
                //@"D:\Users\Steve\Desktop\manchester.osm", 
                @"D:\Users\Steve\Desktop\small.osm", 
            };
            foreach (String osmDataFilePath in osmDataFilePaths)
            {
                Osm.Model.osm osmModel = Osm.Model.osm.Import(osmDataFilePath);
                osmModel.CopyTo(ocadMap);
            }
*/
            /*
            String[] osDataFilePaths = new String[] 
            { 
                @"Z:\Documents\Orienteering\Mapping\Street Maps\OS VectorMap District\GML\SJ\SJ66.gml"
                //@"Z:\Documents\Orienteering\Mapping\Street Maps\OS VectorMap District\GML\SJ\SJ68.gml", 
                //@"Z:\Documents\Orienteering\Mapping\Street Maps\OS VectorMap District\GML\SJ\SJ86.gml", 
                //@"Z:\Documents\Orienteering\Mapping\Street Maps\OS VectorMap District\GML\SJ\SJ88.gml",
                //@"Z:\Documents\Orienteering\Mapping\Street Maps\OS VectorMap District\GML\SK\SK06.gml", 
                //@"Z:\Documents\Orienteering\Mapping\Street Maps\OS VectorMap District\GML\SK\SK08.gml" 
            };
            foreach (String osDataFilePath in osDataFilePaths)
            {
                OS.Model.FeatureCollectionType osModel = OS.Model.FeatureCollectionType.Import(osDataFilePath);
                osModel.CopyTo(ocadMap);
            }
            */

            ocadMap.Export(@"D:\Users\Steve\Desktop\New MDOC Street Base Map 15000 (2014-07-05).ocd");
        }
    }
}
