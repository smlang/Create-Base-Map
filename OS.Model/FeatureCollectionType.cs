using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace OS.Model
{
    public partial class FeatureCollectionType
    {
        private static XmlSerializer osSerializer = new XmlSerializer(typeof(OS.Model.FeatureCollectionType));

        public static OS.Model.FeatureCollectionType Import(String osDataFilePath)
        {
            using (FileStream osDataStream = new FileStream(osDataFilePath, FileMode.Open, FileAccess.Read))
            {
                return Import(osDataStream);
            }
        }

        public static OS.Model.FeatureCollectionType Import(Stream osDataStream)
        {
            using (BufferedStream buffer = new BufferedStream(osDataStream))
            {
                return (OS.Model.FeatureCollectionType)osSerializer.Deserialize(buffer);
            }
        }
    }
}
