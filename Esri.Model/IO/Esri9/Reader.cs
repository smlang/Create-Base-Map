using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
//using Geometry;

namespace Esri.IO.Esri3
{
    public class Reader : BinaryReader
    {
        internal Model.Map Map { get; set; }

        public Reader(Stream stream)
            : base(stream, Encoding.Unicode)
        {
        }

        #region Read Methods
        public Model.Map ReadContent()
        {
            BaseStream.Seek(0, SeekOrigin.Begin);

            Model.Map map = new Model.Map();

            map.FileCode = ReadBigEndianInt32(); // file code
            ReadBigEndianInt32(); // unused
            ReadBigEndianInt32(); // unused
            ReadBigEndianInt32(); // unused
            ReadBigEndianInt32(); // unused
            ReadBigEndianInt32(); // unused
            map.FileLength = ReadBigEndianInt32();
            map.Version = ReadInt32();
            map.ShareType = (Model.ShapeType)ReadInt32();
            map.XMin = ReadDouble();
            map.YMin = ReadDouble();
            map.XMax = ReadDouble();
            map.YMax = ReadDouble();
            map.ZMin = ReadDouble();
            map.ZMax = ReadDouble();
            map.MMin = ReadDouble();
            map.MMax = ReadDouble();

            ReadShapes(map);

            return Map;
        }

        internal Int32 ReadBigEndianInt32()
        {
            byte[] bytes = this.ReadBytes(4);
            Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

        internal void ReadShapes(Model.Map map)
        {
            while (this.BaseStream.Position < map.FileLength)
            {
                int recordNumber = ReadBigEndianInt32();
                int recordLength = ReadBigEndianInt32();
                Model.ShapeType shapeType = (Model.ShapeType)ReadInt32();

                switch (shapeType)
                {
                    case Model.ShapeType.Point:
                        ReadPoint(map);
                        break;
                    case Model.ShapeType.PolyLine:
                        ReadPolyLine(map);
                        break;
                    default:
                        this.ReadBytes((recordLength * 2) - 4);
                        break;
                }
            }
        }

        internal void ReadPoint(Model.Map map)
        {
            Model.Shape.Point p = new Model.Shape.Point();
            map.Shapes.Add(p);

            p.X = ReadDouble();
            p.Y = ReadDouble();
        }

        internal void ReadPolyLine(Model.Map map)
        {
            Model.Shape.PolyLine line = new Model.Shape.PolyLine();
            map.Shapes.Add(line);
            line.XMin = ReadDouble();
            line.YMin = ReadDouble();
            line.XMax = ReadDouble();
            line.YMax = ReadDouble();
            line.PartCount = ReadInt32();
            line.PointCount = ReadInt32();

            for (int i=0; i < line.PartCount; i++)
            {
                line.PartStartPointIndexes.Add(ReadInt32());
            }

            for (int i = 0; i < line.PointCount; i++)
            {
                Model.Shape.Point p = new Model.Shape.Point();
                line.Points.Add(p);
                p.X = ReadDouble();
                p.Y = ReadDouble();
            }
        }
        #endregion
    }
}
