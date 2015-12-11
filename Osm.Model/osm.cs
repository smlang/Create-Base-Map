using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Osm.Model
{
    public partial class osm
    {
        private static XmlSerializer osmSerializer = new XmlSerializer(typeof(Osm.Model.osm));

        public static Osm.Model.osm Import(String osmDataFilePath)
        {
            using (FileStream osmDataStream = new FileStream(osmDataFilePath, FileMode.Open, FileAccess.Read))
            {
                return Import(osmDataStream);
            }
        }

        public static Osm.Model.osm Import(Stream osmDataStream)
        {
            using (BufferedStream buffer = new BufferedStream(osmDataStream))
            {
                return (Osm.Model.osm)osmSerializer.Deserialize(buffer);
            }
        }
    }
}
