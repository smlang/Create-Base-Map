using System;
using System.Collections.Generic;
using System.Text;
using Geometry;
using Osm_Model = Osm.Model;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace Ocad.Import.Osm
{
    public class Conversion : IXmlSerializable
    {
        internal const string KEY_FORMAT = "{0}::{1}";

        public SortedDictionary<string, OcadSymbolSet> OsmToOcadDictionary = new SortedDictionary<string, OcadSymbolSet>();

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if ((reader == null) ||
                (reader.NodeType != XmlNodeType.Element) ||
                (reader.LocalName != "Conversion"))
            {
                return;
            }

            while (reader.Read())
            {
                if ((reader == null) ||
                    (reader.NodeType != XmlNodeType.Element) ||
                    (reader.LocalName != "OcadSymbolSet"))
                {
                    break;
                }

                string pointSymbolNumber = reader.GetAttribute("PointSymbolNumber");
                string lineSymbolNumber = reader.GetAttribute("LineSymbolNumber");
                string areaSymbolNumber = reader.GetAttribute("AreaSymbolNumber");
                OcadSymbolSet ocadSymbolSet = new OcadSymbolSet(pointSymbolNumber, lineSymbolNumber, areaSymbolNumber);

                while (reader.Read())
                {
                    if ((reader == null) ||
                        (reader.NodeType != XmlNodeType.Element) ||
                        (reader.LocalName != "OsmTag"))
                    {
                        break;
                    }

                    string tagKey = reader.GetAttribute("Key");
                    string tagValue = reader.GetAttribute("Value");
                    OsmToOcadDictionary.Add(String.Format(KEY_FORMAT, tagKey, tagValue), ocadSymbolSet);
                }
            }
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
