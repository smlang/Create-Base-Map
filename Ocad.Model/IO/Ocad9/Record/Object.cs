using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geometry;

namespace Ocad.IO.Ocad9.Record
{
    internal class Object : AbstractRecord
    {
        private const Int32 BODY_SIZE = 40;

        private Model.AbstractObject obj;

        #region Read Header
        internal override void ReadHeader(Reader reader)
        {
            Type.PointFlag internalPointFlag;
            reader.ReadPoint(out internalPointFlag); // obj.BoundingBoxBottomLeft can be derived from points and extent
            reader.ReadPoint(out internalPointFlag); // obj.BoundingBoxTopRight can be derived from points and extent
            BodyPointer = reader.ReadInt32();
            BodyByteSize = reader.ReadInt32(); // bodyDataSize

            Int32 symbolNumberInteger = reader.ReadInt32();
            Model.Type.FeatureType featureType = (Model.Type.FeatureType)reader.ReadByte();

            if (symbolNumberInteger >= 0)
            {
                Model.SymbolObject symbolObject = new Model.SymbolObject(reader.Map, featureType);
                obj = symbolObject;

                if (BodyPointer != 0)
                {
                    String symbolNumber = Reader.ConvertToSymbolNumber(symbolNumberInteger);
                    symbolObject.Symbol = MapExtension.GetOrCreateSymbol(reader.Map, symbolNumber, featureType);
                }
            }
            else
            {
                Model.Type.ObjectType type = (Model.Type.ObjectType)symbolNumberInteger;
                switch (type)
                {
                    case Model.Type.ObjectType.Imported:
                        obj = new Model.ImportedObject(reader.Map, featureType);
                        break;
                    case Model.Type.ObjectType.Image:
                        obj = new Model.ImageObject(reader.Map, featureType);
                        break;
                    case Model.Type.ObjectType.Graphic:
                        obj = new Model.GraphicObject(reader.Map, featureType);
                        break;
                }
            }

            reader.ReadByte(); // reserved1
            obj.Status = (Model.Type.ObjectStatus)reader.ReadByte();
            obj.View = (Model.Type.ObjectView)reader.ReadByte();
            if (obj is Model.GraphicObject)
            {
                ((Model.GraphicObject)obj).Colour = MapExtension.GetOrCreateColour(reader.Map, reader.ReadInt16()); // colour
            }
            else
            {
                reader.ReadInt16();
            }
            reader.ReadInt16(); // reserved2

            // import layer
            if (obj.SupportImportLayer)
            {
                obj.ImportLayer = MapExtension.GetOrCreateImportLayer(reader.Map, reader.ReadInt16()); // import layer
            }
            else
            {
                reader.ReadInt16();
            }

            reader.ReadInt16(); // reserved3
        }
        #endregion

        #region Read Body
        internal override void ReadBody(Reader reader)
        {
            reader.BaseStream.Seek(BodyPointer, SeekOrigin.Begin);

            reader.ReadInt32(); // symbolNumber
            reader.ReadByte();  // objectsymboltype
            reader.ReadByte(); // reserved0
            ReadAngle(reader);
            Int32 nPoints = reader.ReadInt32();
            Int16 nText = reader.ReadInt16();
            reader.ReadInt16(); // reserved1

            switch (obj.Type)
            {
                case Model.Type.ObjectType.Image:
                    Model.ImageObject imageObject = (Model.ImageObject)obj;
                    imageObject.Cyan = reader.ReadByte();
                    imageObject.Yellow = reader.ReadByte();
                    imageObject.Magenta = reader.ReadByte();
                    imageObject.Black = reader.ReadByte();
                    break;
                case Model.Type.ObjectType.Symbol:
                case Model.Type.ObjectType.Imported:
                case Model.Type.ObjectType.Graphic:
                    reader.ReadInt32(); // colour number of symbolized objects
                    break;
            }

            ReadLineWidth(reader);
            ReadLineStyle(reader);
            reader.ReadDouble(); // reserved2
            reader.ReadDouble(); // reserved3

            obj.Points = reader.ReadPoints(nPoints);
            ReadText(reader, nText);
            reader.Map.Objects.Add(obj);
        }

        private void ReadText(Reader reader, Int32 nText)
        {
            if (nText > 0)
            {
                String text = reader.ReadEncodedString(nText * Constant.DATA_BLOCK_BYTE_SIZE);  // blocks of 64 bytes - nText = block/8
                if (obj.SupportText)
                {
                    obj.Text = text;
                }
            }
        }

        private void ReadAngle(Reader reader)
        {
            if (obj.SupportAngleDegree)
            {
                obj.AngleDegree = Decimal.Divide(reader.ReadInt16(), 10);
            }
            else
            {
                reader.ReadInt16();
            }
        }

        private void ReadLineWidth(Reader reader)
        {
            if (obj.SupportLineWidth)
            {
                obj.LineWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            }
            else
            {
                reader.ReadInt16();
            }
        }

        private void ReadLineStyle(Reader reader)
        {
            if (obj.SupportLineStyle)
            {
                obj.LineStyle = (Model.Type.LineStyle)reader.ReadInt16();
            }
            else
            {
                reader.ReadInt16();
            }
        }
        #endregion

        internal override Int32 SizeBody(Writer writer, Int32 offset, object o)
        {
            this.obj = (Model.AbstractObject)o;
            BodyPointer = offset;
            BodyByteSize = BODY_SIZE;
            if (obj.Points.Count > 0)
            {
                BodyByteSize += writer.Size(obj.Points);
            }
            if (obj.SupportText)
            {
                String text = obj.Text;
                if (!String.IsNullOrEmpty(text))
                {
                    BodyByteSize += writer.SizeEncodedString(text);
                }
            }
            return BodyPointer + BodyByteSize;
        }

        #region Write Header
        internal override void WriteHeader(Writer writer)
        {
            writer.Write(obj.BoundingBoxBottomLeft);
            writer.Write(obj.BoundingBoxTopRight);
            writer.Write(BodyPointer);
            writer.Write(BodyByteSize);

            if (obj is Model.SymbolObject)
            {
                writer.Write(Writer.ConvertFromSymbolNumber(((Model.SymbolObject)obj).Symbol.Number));
            }
            else
            {
                writer.Write((Int32)obj.Type);
            }

            writer.Write((Byte)obj.FeatureType);
            writer.Write(Writer.BYTE_BLANK); // reserved1
            writer.Write((Byte)obj.Status);
            writer.Write((Byte)obj.View);
            if (obj is Model.GraphicObject)
            {
                writer.Write(((Model.GraphicObject)obj).Colour);
            }
            else
            {
                writer.Write((Int16)(-1));
            }
            writer.Write(Writer.INT16_BLANK); // reserved3

            // ImportLayer
            if (obj.SupportImportLayer)
            {
                writer.Write(obj.ImportLayer);
            }
            else
            {
                writer.Write(Writer.INT16_BLANK);
            }
            
            writer.Write(Writer.INT16_BLANK); // reserved4
        }
        #endregion

        #region Write Body
        internal override void WriteBody(Writer writer)
        {
            long a = writer.BaseStream.Position;
            long b;

            if (obj.Type == Model.Type.ObjectType.Symbol)
            {
                Model.SymbolObject symbolObject = (Model.SymbolObject)obj;
                writer.Write(Writer.ConvertFromSymbolNumber(symbolObject.Symbol.Number));
            }
            else
            {
                writer.Write((Int32)obj.Type);
            }

            writer.Write((Byte)obj.FeatureType);
            writer.Write(Writer.BYTE_BLANK); // reserved0
            WriteAngle(writer);
            writer.Write((Int32)(writer.Size(obj.Points) / Constant.DATA_BLOCK_BYTE_SIZE));

            if (obj.SupportText)
            {
                String text = obj.Text;
                writer.Write((Int16)(writer.SizeEncodedString(text) / Constant.DATA_BLOCK_BYTE_SIZE));
            }
            else
            {
                writer.Write(Writer.INT16_BLANK);
            }

            writer.Write(Writer.INT16_BLANK); // reserved1

            switch (obj.Type)
            {
                case Model.Type.ObjectType.Image:
                    Model.ImageObject imageObject = (Model.ImageObject)obj;
                    writer.Write(imageObject.Cyan);
                    writer.Write(imageObject.Yellow);
                    writer.Write(imageObject.Magenta);
                    writer.Write(imageObject.Black);
                    break;
                case Model.Type.ObjectType.Graphic:
                    Model.GraphicObject graphicObject = (Model.GraphicObject)obj;
                    writer.Write(graphicObject.Colour);
                    writer.Write(Writer.INT16_BLANK); // Colour last 2 bytes
                    break;
                case Model.Type.ObjectType.Symbol:
                case Model.Type.ObjectType.Imported:
                    writer.Write(Writer.INT32_BLANK); // colour number of symbolized objects
                    break;
            }

            WriteLineWidth(writer);
            WriteLineStyle(writer);
            writer.Write(Writer.DOUBLE_BLANK); // reserved2
            writer.Write(Writer.DOUBLE_BLANK); // reserved3

            writer.Write(obj.Points);
            WriteText(writer);

            b = writer.BaseStream.Position - a;
        }

        private void WriteText(Writer writer)
        {
            if ((obj.SupportText) && (!String.IsNullOrEmpty(obj.Text)))
            {
                writer.WriteEncodedString(obj.Text);
            }
        }

        private void WriteAngle(Writer writer)
        {
            if (obj.SupportAngleDegree)
            {
                writer.Write((Int16)(obj.AngleDegree * 10));
            }
            else
            {
                writer.Write(Writer.INT16_BLANK);
            }
        }

        private void WriteLineWidth(Writer writer)
        {
            if (obj.SupportLineWidth)
            {
                writer.Write((Int16)(obj.LineWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            }
            else
            {
                writer.Write(Writer.INT16_BLANK);
            }
        }

        private void WriteLineStyle(Writer writer)
        {
            if (obj.SupportLineStyle)
            {
                writer.Write((Int16)obj.LineStyle);
            }
            else
            {
                writer.Write(Writer.INT16_BLANK);
            }
        }
        #endregion
    }
}
