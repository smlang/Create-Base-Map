using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geometry;

namespace Ocad.IO.Ocad9
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

            Int16 ocadMark = ReadInt16();
            if (Constant.OCAD_MARK != ocadMark)
            {
                return null;
            }

            Model.Type.FileType fileType = (Model.Type.FileType)ReadByte();
            ReadByte(); // fileStatus
            Int16 version = ReadInt16();
            Int16 subversion = ReadInt16();
            Int32 symbolHeaderBlockPointer = ReadInt32();
            Int32 objectHeaderBlockPointer = ReadInt32();
            ReadInt32(); // reserved0
            ReadInt32(); // reserved1
            ReadInt32(); // reserved2
            ReadInt32(); // infoSize
            Int32 settingHeaderBlockPointer = ReadInt32();
            Int32 fileNamePointer = ReadInt32();
            Int32 fileNameSize = ReadInt32();
            ReadInt32(); // reserved4

            Map = new Model.Map();
            Map.FileType = new Model.FileType()
            {
                Value = fileType
            };
            Map.FileVersion = new Model.FileVersion()
            {
                Version = version,
                Subversion = subversion
            };

            // Read Symbols before Objects where they will be referenced
            ReadBlocks<Record.Symbol>(symbolHeaderBlockPointer, Constant.SYMBOL_HEADER_BLOCK_BYTE_SIZE);

            ReadBlocks<Record.Object>(objectHeaderBlockPointer, Constant.OBJECT_HEADER_BLOCK_BYTE_SIZE);

            // Read Settings last, so Event objects can be linked to model objects
            ReadBlocks<Record.Setting>(settingHeaderBlockPointer, Constant.SETTING_HEADER_BLOCK_SIZE);

            String text = ReadAsciiString(fileNamePointer, fileNameSize);
            Map.FileName = new Model.FileName()
            {
                Value = text
            };

            return Map;
        }

        internal Boolean ReadWordBoolean()
        {
            this.BaseStream.Seek(1, SeekOrigin.Current);
            return ReadBoolean();
        }

        internal String ReadEncodedString(Int32 dataByteSize)
        {
            StringBuilder b = new StringBuilder();
            while (true)
            {
                if (dataByteSize == 0)
                {
                    throw new ApplicationException("Encoded String exceeds allocated size.");
                }

                char c = this.ReadChar();
                dataByteSize -= 2;
                if (c == '\0')
                {
                    ReadBytes(dataByteSize);
                    return b.ToString();
                }
                b.Append(c);
            }
        }

        internal String ReadAsciiString(Int32 stringPointer, Int32 dataByteSize)
        {
            this.BaseStream.Seek(stringPointer, SeekOrigin.Begin);

            StringBuilder b = new StringBuilder();
            while (true)
            {
                if (dataByteSize == 0)
                {
                    throw new ApplicationException("ASCII String exceeds allocated size.");
                }

                char c = (char)this.ReadByte();
                dataByteSize--;
                if (c == '\0')
                {
                    ReadBytes(dataByteSize);
                    return b.ToString();
                }
                b.Append(c);
            }
        }

        internal String ReadPascalString(Byte stringMaxSize)
        {
            Byte stringLength = ReadByte();
            if (stringLength > stringMaxSize)
            {
                stringLength = stringMaxSize;
            }

            String result = Encoding.ASCII.GetString(this.ReadBytes(stringLength));
            if (stringMaxSize > stringLength)
            {
                this.BaseStream.Seek(stringMaxSize - stringLength, SeekOrigin.Current);
            }

            return result;
        }

        internal Distance ReadDistance(Distance.Unit unit, Scale scale)
        {
            return new Distance(ReadInt16(), unit, scale);
        }

        internal Distance ReadDistance32(Distance.Unit unit, Scale scale)
        {
            return new Distance(ReadInt32(), unit, scale);
        }

        internal Model.Point ReadPoint(out Type.PointFlag internalPointFlag)
        {
            Int32 x32 = ReadInt32();
            Int32 y32 = ReadInt32();

            Model.Point point = new Model.Point(
                new Distance(x32 >> Constant.POINT_TYPE_BYTE_SIZE, Distance.Unit.Metre, Scale.ten_minus_5),
                new Distance(y32 >> Constant.POINT_TYPE_BYTE_SIZE, Distance.Unit.Metre, Scale.ten_minus_5));

            internalPointFlag = (Type.PointFlag)((x32 & Constant.POINT_TYPE_MASK) + ((y32 & Constant.POINT_TYPE_MASK) << Constant.POINT_TYPE_BYTE_SIZE));
            point.MainPointFlag = (Model.Type.PointFlag)internalPointFlag;

            return point;
        }

        internal List<Model.Point> ReadPoints(Int32 nPoints)
        {
            List<Model.Point> mergePoints = new List<Model.Point>();
            Model.Point secondBezierPoint = null;
            Model.Point mainPoint = null;
            for (int i = 0; i < nPoints; i++)
            {
                Type.PointFlag internalPointFlag;
                Model.Point p = ReadPoint(out internalPointFlag);
                if (internalPointFlag == Type.PointFlag.SecondBezierPoint)
                {
                    secondBezierPoint = p;
                }
                else if (internalPointFlag == Type.PointFlag.FirstBezierPoint)
                {
                    mainPoint.FirstBezier = new Point(p.X, p.Y);
                }
                else
                {
                    mainPoint = p;
                    mergePoints.Add(mainPoint);
                    if (secondBezierPoint != null)
                    {
                        mainPoint.SecondBezier = new Point(secondBezierPoint.X, secondBezierPoint.Y);
                        secondBezierPoint = null;
                    }
                }
            }

            return mergePoints;
        }

        internal Model.Shape ReadShape()
        {
            Model.Shape shape = new Model.Shape();

            shape.Type = (Model.Type.ShapeType)ReadInt16();
            shape.Style = (Model.Type.LineStyle)ReadUInt16();
            shape.Colour = MapExtension.GetOrCreateColour(Map, ReadInt16());
            shape.LineWidth = ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            shape.Diameter = ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            Int16 nPoints = ReadInt16();
            Int16 reserved0 = ReadInt16();
            Int16 reserved1 = ReadInt16();
            shape.Points = ReadPoints(nPoints);

            return shape;
        }

        internal List<Model.Shape> ReadShapes(Int32 shapesDataSize)
        {
            List<Model.Shape> shapes = new List<Model.Shape>();
            long offsetLimit = this.BaseStream.Position + (shapesDataSize * Constant.DATA_BLOCK_BYTE_SIZE);
            while (this.BaseStream.Position < offsetLimit)
            {
                shapes.Add(ReadShape());
            }
            return shapes;
        }

        private void ReadBlocks<R>(Int32 blockPointer, Int32 blockByteSize) where R : Record.AbstractRecord, new()
        {
            while (blockPointer != 0)
            {
                Int32 nextBlockPointer;
                ReadBlock<R>(blockPointer, blockByteSize, out nextBlockPointer);
                blockPointer = nextBlockPointer;
            }
        }

        private void ReadBlock<R>(Int32 blockPointer, Int32 blockByteSize, out Int32 nextBlockPointer) where R : Record.AbstractRecord, new()
        {
            this.BaseStream.Seek(blockPointer, SeekOrigin.Begin);

            nextBlockPointer = this.ReadInt32();
            List<R> records = new List<R>();
            // Read all headers, before reading any body
            for (int i = 0; i < Constant.MAX_HEADERS; i++)
            {
                R record = new R();
                record.ReadHeader(this);
                if (record.BodyPointer != 0)
                {
                    records.Add(record);
                }
            }
            long readBlockByteSize = this.BaseStream.Position - blockPointer;
            if (readBlockByteSize != blockByteSize)
            {
                throw (new ApplicationException(String.Format("Size of {0} header block should be {1} bytes but read {2} bytes.", typeof(R).Name, blockByteSize, readBlockByteSize)));
            }

            // Can now read bodies
            foreach (R record in records)
            {
                record.ReadBody(this);

                long readBodyByteSize = this.BaseStream.Position - record.BodyPointer;
                if (readBodyByteSize != record.BodyByteSize)
                {
                    Console.WriteLine(String.Format("Size of {0} body record should be {1} bytes but read {2} bytes.", typeof(R).Name, record.BodyByteSize, readBodyByteSize));
                    //throw (new ApplicationException(String.Format("Size of {0} body record should be {1} bytes but read {2} bytes.", typeof(R).Name, record.BodyByteSize, readBodyByteSize)));
                }
            }
        }

        internal protected static String ConvertToSymbolNumber(Int32 symbolNumberInteger)
        {
            return String.Format("{0}.{1}", (symbolNumberInteger / 1000), (symbolNumberInteger % 1000));
        }
        #endregion
    }
}
