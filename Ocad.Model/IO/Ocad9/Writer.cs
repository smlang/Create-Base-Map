using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Geometry;

namespace Ocad.IO.Ocad9
{
    public class Writer : BinaryWriter
    {
        internal const Double DOUBLE_BLANK = 0;
        internal const Int32 INT32_BLANK = 0;
        internal const Int16 INT16_BLANK = 0;
        internal const Byte BYTE_BLANK = 0;

        internal Model.Map Map { get; set; }

        public Writer(Stream stream)
            : base(stream, Encoding.Unicode)
        {
        }

        #region Write Methods
        public void Write(Model.Map value)
        {
            Map = value;

            BaseStream.Seek(0, SeekOrigin.Begin);

            #region Determine Size and Position of all records
            Int32 fileNameByteSize = SizeAsciiString(value.FileName.Value);
            Int32 nextOffset = fileNameByteSize + Constant.FILE_RECORD_BYTE_SIZE;

            Int32 objectHeaderBlockPointer = 0;
            List<Block<Record.Object>> objectBlocks;
            if (value.Objects.Count == 0)
            {
                objectHeaderBlockPointer = 0;
                objectBlocks = new List<Block<Record.Object>>();
            }
            else
            {
                objectHeaderBlockPointer = nextOffset;
                objectBlocks = SizeBlocks<Record.Object>(objectHeaderBlockPointer, ToList<Model.AbstractObject>(value.Objects), Constant.OBJECT_HEADER_BLOCK_BYTE_SIZE, out nextOffset);
            }

            Int32 symbolHeaderBlockPointer;
            List<Block<Record.Symbol>> symbolBlocks;
            if (value.Symbols.Count == 0)
            {
                symbolHeaderBlockPointer = 0;
                symbolBlocks = new List<Block<Record.Symbol>>();
            }
            else
            {
                symbolHeaderBlockPointer = nextOffset;
                symbolBlocks = SizeBlocks<Record.Symbol>(symbolHeaderBlockPointer, ToList<Model.AbstractSymbol>(value.Symbols), Constant.SYMBOL_HEADER_BLOCK_BYTE_SIZE, out nextOffset);
            }

            Int32 settingHeaderBlockPointer;
            List<Block<Record.Setting>> settingBlocks;
            List<Record.Helper.Setting> settings = Record.Helper.Setting.CopyFromModel(Map);
            if (settings.Count == 0)
            {
                settingHeaderBlockPointer = 0;
                settingBlocks = new List<Block<Record.Setting>>();
            }
            else
            {
                settingHeaderBlockPointer = nextOffset;
                settingBlocks = SizeBlocks<Record.Setting>(settingHeaderBlockPointer, ToList<Record.Helper.Setting>(settings), Constant.SETTING_HEADER_BLOCK_SIZE, out nextOffset);
            }
            #endregion

            Write(Constant.OCAD_MARK);
            Write((Byte)value.FileType.Value);
            Write(BYTE_BLANK); // fileStatus
            Write(value.FileVersion.Version);
            Write(value.FileVersion.Subversion);
            Write(symbolHeaderBlockPointer); // Int32 symbolHeaderBlockPointer = WriteInt();
            Write(objectHeaderBlockPointer); // Int32 objectHeaderBlockPointer = WriteInt();
            Write(INT32_BLANK); // reserved0
            Write(INT32_BLANK); // reserved1
            Write(INT32_BLANK); // reserved2
            Write(INT32_BLANK); // infoSize
            Write(settingHeaderBlockPointer); // Int32 settingHeaderBlockPointer = WriteInt();
            Write(Constant.FILE_RECORD_BYTE_SIZE); // Int32 fileNamePointer = WriteInt();
            Write(fileNameByteSize);
            Write(INT32_BLANK); // reserved4

            #region Write Records
            WriteAsciiString(value.FileName.Value);
            Write<Record.Object>(objectBlocks, Constant.OBJECT_HEADER_BYTE_SIZE, Constant.OBJECT_HEADER_BLOCK_BYTE_SIZE);
            Write<Record.Symbol>(symbolBlocks, Constant.SYMBOL_HEADER_BYTE_SIZE, Constant.SYMBOL_HEADER_BLOCK_BYTE_SIZE);
            Write<Record.Setting>(settingBlocks, Constant.SETTING_HEADER_BYTE_BYTE_SIZE, Constant.SETTING_HEADER_BLOCK_SIZE);
            #endregion
        }

        private static List<object> ToList<T>(List<T> value)
        {
            List<object> output = new List<object>();
            foreach (object o in value)
            {
                output.Add(o);
            }
            return output;
        }

        internal void WriteWordBoolean(Boolean value)
        {
            Write(BYTE_BLANK);
            Write(value);
        }

        internal void WriteEncodedString(String value)
        {
            Write(value.ToCharArray());

            Int32 padding = SizeEncodedString(value);
            padding -= (value.Length * 2);
            Write(new byte[padding]);
        }

        internal void WriteAsciiString(String value)
        {
            Write(Encoding.ASCII.GetBytes(value));

            Int32 padding = SizeAsciiString(value);
            padding -= value.Length;
            Write(new byte[padding]);
        }

        internal void WritePascalString(String value, Int32 maxSize)
        {
            if (value.Length > maxSize)
            {
                value = value.Substring(0, maxSize);
            }

            Write((byte)value.Length);
            Write(Encoding.ASCII.GetBytes(value));
            Int32 padding = maxSize - value.Length;
            Write(new byte[padding]);
        }

        internal void Write(Distance distance, Int16 precision, Distance.Unit unit, Scale scale)
        {
            if (precision == 0)
            {
                Write((Int16)distance[precision, unit, scale]);
            }
            else
            {
                // String
                Write((Decimal)distance[precision, unit, scale]);
            }
        }

        internal void Write(Model.Point value)
        {
            Int32 x, y;
            if (value.SecondBezier != null)
            {
                x = ((int)(value.SecondBezier.X[0, Distance.Unit.Metre, Scale.ten_minus_5]) << Constant.POINT_TYPE_BYTE_SIZE) + (((int)Type.PointFlag.SecondBezierPoint) & Constant.POINT_TYPE_MASK);
                Write(x);
                y = ((int)(value.SecondBezier.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]) << Constant.POINT_TYPE_BYTE_SIZE) + ((((int)Type.PointFlag.SecondBezierPoint) >> Constant.POINT_TYPE_BYTE_SIZE) & Constant.POINT_TYPE_MASK);
                Write(y);
            }

            x = ((int)(value.X[0, Distance.Unit.Metre, Scale.ten_minus_5]) << Constant.POINT_TYPE_BYTE_SIZE) + (((int)value.MainPointFlag) & Constant.POINT_TYPE_MASK);
            Write(x);
            y = ((int)(value.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]) << Constant.POINT_TYPE_BYTE_SIZE) + ((((int)value.MainPointFlag) >> Constant.POINT_TYPE_BYTE_SIZE) & Constant.POINT_TYPE_MASK);
            Write(y);

            if (value.FirstBezier != null)
            {
                x = ((int)(value.FirstBezier.X[0, Distance.Unit.Metre, Scale.ten_minus_5]) << Constant.POINT_TYPE_BYTE_SIZE) + (((int)Type.PointFlag.FirstBezierPoint) & Constant.POINT_TYPE_MASK);
                Write(x);
                y = ((int)(value.FirstBezier.Y[0, Distance.Unit.Metre, Scale.ten_minus_5]) << Constant.POINT_TYPE_BYTE_SIZE) + ((((int)Type.PointFlag.FirstBezierPoint) >> Constant.POINT_TYPE_BYTE_SIZE) & Constant.POINT_TYPE_MASK);
                Write(y);
            }
        }

        internal void Write(List<Model.Point> value)
        {
            foreach (Model.Point p in value)
            {
                Write(p);
            }
        }

        internal void Write(Model.Shape value)
        {
            Write((Int16)value.Type);
            Write((UInt16)value.Style);
            Write(value.Colour);
            Write((Int16)(value.LineWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            Write((Int16)(value.Diameter[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            Write((Int16)(Size(value.Points) / Constant.DATA_BLOCK_BYTE_SIZE));
            Write(INT16_BLANK);  // reserved0
            Write(INT16_BLANK);  // reserved1
            Write(value.Points);
        }

        internal void Write(List<Model.Shape> value)
        {
            foreach (Model.Shape s in value)
            {
                Write(s);
            }
        }

        internal void Write(Model.Colour value)
        {
            if (value != null)
            {
                Write(value.Number);
            }
            else
            {
                Write(INT16_BLANK);
            }
        }

        internal void Write(Model.SymbolTreeNode value)
        {
            if (value != null)
            {
                Write((Byte)value.Id);
            }
            else
            {
                Write(BYTE_BLANK);
            }
        }

        internal void Write(Model.ImportLayer value)
        {
            if (value != null)
            {
                Write((Int16)value.LayerNumber);
            }
            else
            {
                Write(INT16_BLANK);
            }
        }

        internal void Write<R>(List<Block<R>> blocks, Int32 headerByteSize, Int32 blockByteSize) where R : Record.AbstractRecord, new()
        {
            R blankRecord = new R();
            for (int i = 0; i < blocks.Count; i++)
            {
                long startPosition = this.BaseStream.Position;
                Int32 nextBlockPoint = (i == blocks.Count - 1) ? 0 : blocks[i + 1].BlockPointer;
                Write(nextBlockPoint);
                for (int j = 0; j < blocks[i].Records.Count; j++)
                {
                    R record = blocks[i].Records[j];
                    record.WriteHeader(this);
                }
                Int32 blankBlockHeaders = Constant.MAX_HEADERS - blocks[i].Records.Count;
                Write(new byte[blankBlockHeaders * headerByteSize]);

                long writtenBlockByteSize = this.BaseStream.Position - startPosition;
                if (writtenBlockByteSize != blockByteSize)
                {
                    throw (new ApplicationException(String.Format("Size of {0} header block should be {1} bytes but wrote {2} bytes.", typeof(R).Name, blockByteSize, writtenBlockByteSize)));
                }


                for (int j = 0; j < blocks[i].Records.Count; j++)
                {
                    R record = blocks[i].Records[j];
                    record.WriteBody(this);

                    long writtenBodyByteSize = this.BaseStream.Position - record.BodyPointer;
                    if (writtenBodyByteSize != record.BodyByteSize)
                    {
                        throw (new ApplicationException(String.Format("Size of {0} body record should be {1} bytes but wrote {2} bytes.", typeof(R).Name, record.BodyByteSize, writtenBodyByteSize)));
                    }
                }
            }
        }

        internal protected static Int32 ConvertFromSymbolNumber(String symbolNumber)
        {
            String[] parts = symbolNumber.Split('.');
            Int32 symbolNumberInteger = Int32.Parse(parts[0]) * 1000;
            if (parts.Length > 1)
            {
                symbolNumberInteger += Int32.Parse(parts[1]);
            }
            return symbolNumberInteger;
        }

        #region Size Methods
        internal Int32 SizeEncodedString(String value)
        {
            return Constant.STRING_BLOCK_BYTE_SIZE * (((value.Length * 2) + Constant.STRING_BLOCK_BYTE_SIZE) / Constant.STRING_BLOCK_BYTE_SIZE);
        }

        internal Int32 SizeAsciiString(String value)
        {
            return Constant.STRING_BLOCK_BYTE_SIZE * ((value.Length + Constant.STRING_BLOCK_BYTE_SIZE) / Constant.STRING_BLOCK_BYTE_SIZE);
        }

        internal Int32 Size(List<Model.Point> value)
        {
            Int32 size = 0;
            foreach (Model.Point p in value)
            {
                size += Constant.POINT_BYTE_SIZE;
                if (p.SecondBezier != null)
                {
                    size += Constant.POINT_BYTE_SIZE;
                }
                if (p.FirstBezier != null)
                {
                    size += Constant.POINT_BYTE_SIZE;
                }
            }
            return size;
        }

        internal Int32 Size(List<Model.Shape> value)
        {
            Int32 size = 0;
            foreach (Model.Shape s in value)
            {
                size += 16;
                size += Size(s.Points);
            }
            return size;
        }

        private List<Block<R>> SizeBlocks<R>(Int32 offset, List<object> items, Int32 blockSize, out Int32 newOffset) where R : Record.AbstractRecord, new()
        {
            List<Block<R>> blocks = new List<Block<R>>();
            Block<R> block = new Block<R>();
            newOffset = offset;
            int i = 0;
            foreach (object item in items)
            {
                if (i == Constant.MAX_HEADERS)
                {
                    i = 0;
                }
                if (i == 0)
                {
                    block = new Block<R>();
                    block.BlockPointer = newOffset;
                    block.Records = new List<R>();
                    blocks.Add(block);

                    newOffset += blockSize;
                }

                R record = new R();
                newOffset = record.SizeBody(this, newOffset, item);
                block.Records.Add(record);

                i++;
            }
            return blocks;
        }
        #endregion

        internal struct Block<R>
        {
            internal Int32 BlockPointer;
            internal List<R> Records;
        }
        #endregion
    }
}
