using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Esri.Model
{
    public class Map
    {
        public List<Shape.AbstractShape> Shapes { get; set; }

        public int FileCode { get; set; }
        public int FileLength { get; set; }
        public int Version { get; set; }
        public ShapeType ShareType { get; set; }

        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public double ZMin { get; set; }
        public double ZMax { get; set; }
        public double MMin { get; set; }
        public double MMax { get; set; }

        internal Map()
        {
            Shapes = new List<Shape.AbstractShape>();

            FileCode = 9994;
            Version = 1000;
        }

        public static Map Import(String esriDataFilePath)
        {
            using (FileStream esriDataStream = new FileStream(esriDataFilePath, FileMode.Open, FileAccess.Read))
            {
                return Import(esriDataStream);
            }
        }

        public static Map Import(Stream esriDataStream)
        {
            using (BufferedStream buffer = new BufferedStream(esriDataStream))
            {
                using (Esri.IO.Esri3.Reader reader = new Esri.IO.Esri3.Reader(buffer))
                {
                    return reader.ReadContent();
                }
            }
        }
    }
}
