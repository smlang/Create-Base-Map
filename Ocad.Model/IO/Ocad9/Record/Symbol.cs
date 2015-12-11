using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geometry;

namespace Ocad.IO.Ocad9.Record
{
    internal class Symbol : AbstractRecord
    {
        private const Int32 AREA_BASIC_BODY_SIZE = 604;
        private const Int32 BLOCK_TEXT_BASIC_BODY_SIZE = 812;
        private const Int32 LINE_BASIC_BODY_SIZE = 648;
        private const Int32 LINE_TEXT_BASIC_BODY_SIZE = 664;
        private const Int32 POINT_BASIC_BODY_SIZE = 576;
        private const Int32 RECTANGLE_BASIC_BODY_SIZE = 640;
        private const Int32 ICON_SIZE = 484;
        
        private const Int32 MAX_COLOURS = 14;
        private const Int32 MAX_TABS = 32;

        private const Int32 MAX_DESCRIPTION_SIZE = 31;
        private const Int32 MAX_FONT_NAME_SIZE = 31;
        private const Int32 MAX_LINE_RESERVED_SIZE = 31;
        private const Int32 MAX_BLOCK_TEXT_RESERVED_SIZE = 23;
        private const Int32 MAX_UNNUMBERED_CELLS_SIZE = 3;
        private const Int32 MAX_RECTANGLE_RESERVED_SIZE = 31;        

        private Model.AbstractSymbol symbol;
        private Int32 dataByteSize = 0;

        internal override void ReadHeader(Reader reader)
        {
            BodyPointer = reader.ReadInt32();
        }

        internal override void ReadBody(Reader reader)
        {
            reader.BaseStream.Seek(BodyPointer, SeekOrigin.Begin);

            BodyByteSize = reader.ReadInt32(); // Record Size
            String symbolNumber = Reader.ConvertToSymbolNumber(reader.ReadInt32()); // Symbol Number
            Model.Type.SymbolType symbolType = (Model.Type.SymbolType)reader.ReadByte();

            Model.AbstractSymbol symbol = MapExtension.GetOrCreateSymbol(reader.Map, symbolNumber, symbolType, true);

            switch (symbolType)
            {
                case Model.Type.SymbolType.Area:
                    ReadArea(reader, (Model.AreaSymbol)symbol);
                    break;
                case Model.Type.SymbolType.BlockText:
                    ReadBlockText(reader, (Model.BlockTextSymbol)symbol);
                    break;
                case Model.Type.SymbolType.Line:
                    ReadLine(reader, (Model.LineSymbol)symbol);
                    break;
                case Model.Type.SymbolType.LineText:
                    ReadLineText(reader, (Model.LineTextSymbol)symbol);
                    break;
                case Model.Type.SymbolType.Point:
                    ReadPoint(reader, (Model.PointSymbol)symbol);
                    break;
                case Model.Type.SymbolType.Rectangle:
                    ReadRectangle(reader, (Model.RectangleSymbol)symbol);
                    break;
                default:
                    symbol = null; // todo throw null
                    break;
            }           
        }

        #region Read Body Methods
        private void ReadAbstractSymbol(Reader reader, Model.AbstractSymbol symbol)
        {
            symbol.Flag = (Model.Type.SymbolFlag)reader.ReadByte();
            symbol.Selected = reader.ReadBoolean();
            symbol.Status = (Model.Type.SymbolStatus)reader.ReadByte();
            symbol.DefaultDrawingMode = (Model.Type.DefaultDrawingMode)reader.ReadByte();
            symbol.EventMode = (Model.Type.EventMode)reader.ReadByte();
            symbol.CourseObjectType = (Model.Type.ControlObjectType)reader.ReadByte();
            symbol.CourseDescriptionFlag = (Model.Type.CourseDescriptionFlag)reader.ReadByte();
            symbol.Extent = reader.ReadDistance32(Distance.Unit.Metre, Scale.ten_minus_5);
            reader.ReadInt32(); // filePosition
            symbol.SymbolTree1 = MapExtension.GetOrCreateSymbolTree(reader.Map, reader.ReadByte());
            symbol.SymbolTree2 = MapExtension.GetOrCreateSymbolTree(reader.Map, reader.ReadByte());
            reader.ReadInt16(); // Int16 nColours
            for (int i = 0; i < MAX_COLOURS; i++)
            {
                reader.ReadInt16(); // In16 Colours[i] -- colours are described in other properties
            }

            symbol.Description = reader.ReadPascalString(MAX_DESCRIPTION_SIZE);
            symbol.IconBits = reader.ReadBytes(ICON_SIZE);
        }

        private void ReadArea(Reader reader, Model.AreaSymbol symbol)
        {
            ReadAbstractSymbol(reader, symbol);
            Int32 borderSymbolInteger = reader.ReadInt32();
            Int16 fillColourNumber = reader.ReadInt16();
            symbol.HatchMode = (Model.Type.HatchMode)reader.ReadInt16();
            Int16 hatchColourNumber = reader.ReadInt16();
            if (symbol.HatchMode != Model.Type.HatchMode.Off)
            {
                symbol.HatchColour = MapExtension.GetOrCreateColour(reader.Map, hatchColourNumber);
            }
            symbol.HatchLineWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.HatchDistance = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.HatchAngle1Degree = Decimal.Divide(reader.ReadInt16(), 10);
            symbol.HatchAngle2Degree = Decimal.Divide(reader.ReadInt16(), 10);
            symbol.FillOn = reader.ReadBoolean();
            if (symbol.FillOn)
            {
                symbol.FillColour = MapExtension.GetOrCreateColour(reader.Map, fillColourNumber);
            }
            symbol.BorderOn = reader.ReadBoolean();
            if (symbol.BorderOn)
            {
                String borderSymbolNumber = Reader.ConvertToSymbolNumber(borderSymbolInteger); // Symbol Number
                symbol.BorderSymbol = (Model.LineSymbol)MapExtension.GetOrCreateSymbol(reader.Map, borderSymbolNumber, Model.Type.FeatureType.Line);
            }
            symbol.StuctureMode = (Model.Type.StructureMode)reader.ReadInt16();
            symbol.StuctureWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.StuctureHeight = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.StuctureAngleDegree = Decimal.Divide(reader.ReadInt16(), 10);
            reader.ReadInt16(); // reserved0
            UInt16 dataSize = reader.ReadUInt16();
            symbol.Shapes = reader.ReadShapes(dataSize);
        }

        private void ReadLine(Reader reader, Model.LineSymbol symbol)
        {
            ReadAbstractSymbol(reader, symbol);

            symbol.LineColour = MapExtension.GetOrCreateColour(reader.Map, reader.ReadInt16());
            symbol.LineWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.LineStyle = (Model.Type.LineStyle)reader.ReadInt16();

            symbol.DistanceFromStart = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.DistanceFromEnd = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.MainLengthA = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.EndLengthB = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.MainGapC = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.MinorGapD = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.EndGapE = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.MinimumNumberOfGaps = reader.ReadInt16();
            symbol.MinimumNumberOfGaps++;

            symbol.NMainSymbolA = reader.ReadInt16();
            symbol.MainSymbolDistance = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);

            symbol.DoubleMode = (Model.Type.DoubleLineMode)reader.ReadUInt16();
            symbol.DoubleFlag = (Model.Type.DoubleLineFlag)reader.ReadUInt16();
            Int16 doubleFillColourNumber = reader.ReadInt16();
            Int16 doubleLeftColourNumber = reader.ReadInt16();
            Int16 doubleRightColourNumber = reader.ReadInt16();
            if (symbol.DoubleMode != Model.Type.DoubleLineMode.Off)
            {
                if (symbol.DoubleFlag != Model.Type.DoubleLineFlag.None)
                {
                    symbol.DoubleFillColour = MapExtension.GetOrCreateColour(reader.Map, doubleFillColourNumber);
                }
                symbol.DoubleLeftColour = MapExtension.GetOrCreateColour(reader.Map, doubleLeftColourNumber);
                symbol.DoubleRightColour = MapExtension.GetOrCreateColour(reader.Map, doubleRightColourNumber);
            }

            symbol.DoubleWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.DoubleLeftWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.DoubleRightWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.DoubleDashedLength = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.DoubleDashedGap = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);

            reader.ReadInt16(); // reserved0
            reader.ReadInt16(); // reserved1[0]
            reader.ReadInt16(); // reserved1[1]

            symbol.DecreaseMode = (Model.Type.DecreaseMode)reader.ReadUInt16();
            symbol.DecreaseLastSymbolPercentageOfNormalSize = reader.ReadInt16();

            reader.ReadInt16(); // reserved2
            symbol.FramingColour = MapExtension.GetOrCreateColour(reader.Map, reader.ReadInt16());
            symbol.FramingWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.FramingStyle = (Model.Type.FramingStyle)reader.ReadInt16();

            UInt16 mainSymbolADataSize = reader.ReadUInt16();
            UInt16 secondarySymbolBDataSize = reader.ReadUInt16();
            UInt16 cornerSymbolDataSize = reader.ReadUInt16();
            UInt16 startSymbolCDataSize = reader.ReadUInt16();
            UInt16 endSymbolDDataSize = reader.ReadUInt16();
            reader.ReadInt16(); // reserved3

            symbol.MainSymbolAShapes = reader.ReadShapes(mainSymbolADataSize);
            symbol.SecondarySymbolBShapes = reader.ReadShapes(secondarySymbolBDataSize);
            symbol.CornerSymbolShapes = reader.ReadShapes(cornerSymbolDataSize);
            symbol.StartSymbolCShapes = reader.ReadShapes(startSymbolCDataSize);
            symbol.EndSymbolDShapes = reader.ReadShapes(endSymbolDDataSize);
        }

        private void ReadPoint(Reader reader, Model.PointSymbol symbol)
        {
            ReadAbstractSymbol(reader, symbol);

            UInt16 dataSize = reader.ReadUInt16();
            reader.ReadInt16(); //reserved
            symbol.Shapes = reader.ReadShapes(dataSize);
        }

        private void ReadAbstractText(Reader reader, Model.AbstractTextSymbol symbol)
        {
            ReadAbstractSymbol(reader, symbol);

            symbol.FontName = reader.ReadPascalString(MAX_FONT_NAME_SIZE);
            symbol.FontColour = MapExtension.GetOrCreateColour(reader.Map, reader.ReadInt16());
            symbol.FontSize = Decimal.Divide(reader.ReadInt16(), 10);
            symbol.FontWeight = (Model.Type.FontWeight)reader.ReadInt16();
            symbol.FontItalic = reader.ReadBoolean();
            reader.ReadByte(); // reserved0
            symbol.CharacterSpacingPercentageOfSpace = reader.ReadInt16();
            symbol.WordSpacingPercentageOfSpace = reader.ReadInt16();
            symbol.TextAlignment = (Model.Type.TextAlignment)reader.ReadInt16();
        }

        private void ReadLineText(Reader reader, Model.LineTextSymbol symbol)
        {
            ReadAbstractText(reader, symbol);

            symbol.FramingMode = (Model.Type.FramingMode)reader.ReadByte();
            reader.ReadByte(); // reserved1
            reader.ReadPascalString(MAX_LINE_RESERVED_SIZE); // reserved2
            Int16 framingColourNumber = reader.ReadInt16();
            if (symbol.FramingMode != Model.Type.FramingMode.Off)
            {
                symbol.FramingColour = MapExtension.GetOrCreateColour(reader.Map, framingColourNumber);
            }
            symbol.FramingWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            reader.ReadInt16(); // reserved3
            reader.ReadWordBoolean(); // reserved4
            symbol.FramingShadowX = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.FramingShadowY = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
        }

        private void ReadBlockText(Reader reader, Model.BlockTextSymbol symbol)
        {
            ReadAbstractText(reader, symbol);

            symbol.LineSpacingPercentageOfFontSize = reader.ReadInt16();
            symbol.SpaceAfterParagraph = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.IdentFirstLine = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.IdentOtherLines = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            Int16 nTabs = reader.ReadInt16();
            for (int i = 0; i < MAX_TABS; i++)
            {
                if (i < nTabs)
                {
                    symbol.Tabs.Add(reader.ReadDistance32(Distance.Unit.Metre, Scale.ten_minus_5));
                }
                else
                {
                    reader.ReadInt32();
                }
            }

            symbol.LineBelowOn = reader.ReadWordBoolean();
            Int16 lineBelowColourNumber = reader.ReadInt16();
            if (symbol.LineBelowOn)
            {
                symbol.LineBelowColour = MapExtension.GetOrCreateColour(reader.Map, lineBelowColourNumber);
            }
            symbol.LineBelowWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.LineBelowDistance = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            reader.ReadInt16(); // reserved1

            symbol.FramingMode = (Model.Type.FramingMode)reader.ReadByte();
            symbol.FramingLineStyle = (Model.Type.LineStyle)reader.ReadByte();
            reader.ReadPascalString(MAX_BLOCK_TEXT_RESERVED_SIZE); // reserved2
            symbol.FramingLeft = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.FramingBottom = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.FramingRight = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.FramingTop = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            Int16 framingColourNumber = reader.ReadInt16();
            if (symbol.FramingMode != Model.Type.FramingMode.Off)
            {
                symbol.FramingColour = MapExtension.GetOrCreateColour(reader.Map, framingColourNumber);
            }
            symbol.FramingWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);

            reader.ReadInt16(); // reserved3
            reader.ReadWordBoolean(); // reserved4
            symbol.FramingShadowX = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.FramingShadowY = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
        }

        private void ReadRectangle(Reader reader, Model.RectangleSymbol symbol)
        {
            ReadAbstractSymbol(reader, symbol);

            symbol.LineColour = MapExtension.GetOrCreateColour(reader.Map, reader.ReadInt16());
            symbol.LineWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.Radius = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.GridFlags = (Model.Type.GridFlag)reader.ReadUInt16();
            symbol.CellWidth = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            symbol.CellHeight = reader.ReadDistance(Distance.Unit.Metre, Scale.ten_minus_5);
            reader.ReadInt16(); // reserved0
            reader.ReadInt16(); // reserved1
            symbol.UnnumberedCells = reader.ReadInt16();
            symbol.UnnumberedCellsText = reader.ReadPascalString(MAX_UNNUMBERED_CELLS_SIZE);
            reader.ReadInt16(); // reserved2
            reader.ReadPascalString(MAX_RECTANGLE_RESERVED_SIZE); // reserved3
            reader.ReadInt16(); // reserved4
            symbol.FontSize = Decimal.Divide(reader.ReadInt16(), 10);
            reader.ReadInt16(); // reserved6
            reader.ReadWordBoolean(); // reserved7
            reader.ReadInt16(); // reserved8
            reader.ReadInt16(); // reserved9
        }
        #endregion

        internal override Int32 SizeBody(Writer writer, Int32 offset, object o)
        {
            symbol = (Model.AbstractSymbol)o;
            BodyPointer = offset;

            switch (symbol.Type)
            {
                case Model.Type.SymbolType.Area:
                    BodyByteSize = AREA_BASIC_BODY_SIZE; // plus shapes
                    Model.AreaSymbol area = (Model.AreaSymbol)symbol;
                    dataByteSize = writer.Size(area.Shapes);
                    BodyByteSize += dataByteSize;
                    break;
                case Model.Type.SymbolType.BlockText:
                    BodyByteSize = BLOCK_TEXT_BASIC_BODY_SIZE;
                    break;
                case Model.Type.SymbolType.Line:
                    BodyByteSize = LINE_BASIC_BODY_SIZE; // plus shapes
                    Model.LineSymbol line = (Model.LineSymbol)symbol;
                    dataByteSize = writer.Size(line.MainSymbolAShapes);
                    dataByteSize += writer.Size(line.SecondarySymbolBShapes);
                    dataByteSize += writer.Size(line.CornerSymbolShapes);
                    dataByteSize += writer.Size(line.StartSymbolCShapes);
                    dataByteSize += writer.Size(line.EndSymbolDShapes);
                    BodyByteSize += dataByteSize;
                    break;
                case Model.Type.SymbolType.LineText:
                    BodyByteSize = LINE_TEXT_BASIC_BODY_SIZE;
                    break;
                case Model.Type.SymbolType.Point:
                    BodyByteSize = POINT_BASIC_BODY_SIZE; // plus shapes
                    Model.PointSymbol point = (Model.PointSymbol)symbol;
                    dataByteSize = writer.Size(point.Shapes);
                    BodyByteSize += dataByteSize;
                    break;
                case Model.Type.SymbolType.Rectangle:
                    BodyByteSize = RECTANGLE_BASIC_BODY_SIZE;
                    break;
            }
            
            return BodyPointer + BodyByteSize;
        }

        internal override void WriteHeader(Writer writer)
        {
            writer.Write(BodyPointer);
        }

        internal override void WriteBody(Writer writer)
        {
            writer.Write(BodyByteSize);
            writer.Write(Writer.ConvertFromSymbolNumber(symbol.Number));
            writer.Write((Byte)symbol.Type);

            switch (symbol.Type)
            {
                case Model.Type.SymbolType.Area:
                    WriteArea(writer, (Model.AreaSymbol)this.symbol);
                    break;
                case Model.Type.SymbolType.BlockText:
                    WriteBlockText(writer, (Model.BlockTextSymbol)this.symbol);
                    break;
                case Model.Type.SymbolType.Line:
                    WriteLine(writer, (Model.LineSymbol)this.symbol);
                    break;
                case Model.Type.SymbolType.LineText:
                    WriteLineText(writer, (Model.LineTextSymbol)this.symbol);
                    break;
                case Model.Type.SymbolType.Point:
                    WritePoint(writer, (Model.PointSymbol)this.symbol);
                    break;
                case Model.Type.SymbolType.Rectangle:
                    WriteRectangle(writer, (Model.RectangleSymbol)this.symbol);
                    break;
            }
        }

        #region Write Body Methods
        private void WriteAbstractSymbol(Writer writer, Model.AbstractSymbol symbol)
        {
            writer.Write((Byte)symbol.Flag);
            writer.Write(symbol.Selected);
            writer.Write((Byte)symbol.Status);
            writer.Write((Byte)symbol.DefaultDrawingMode);
            writer.Write((Byte)symbol.EventMode);
            writer.Write((Byte)symbol.CourseObjectType);
            writer.Write((Byte)symbol.CourseDescriptionFlag);
            writer.Write((Int32)(symbol.Extent[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write(Writer.INT32_BLANK); // filePosition
            writer.Write(symbol.SymbolTree1);
            writer.Write(symbol.SymbolTree2);
            List<Model.Colour> colours = symbol.Colours;
            if (colours.Count <= MAX_COLOURS)
            {
                writer.Write((Int16)colours.Count);
            }
            else
            {
                writer.Write((Int16)(-1));
            }

            for (int i = 0; i < MAX_COLOURS; i++)
            {
                if (i < colours.Count)
                {
                    writer.Write(colours[i]);
                }
                else
                {
                    writer.Write(Writer.INT16_BLANK);
                }
            }
            writer.WritePascalString(symbol.Description, MAX_DESCRIPTION_SIZE);
            writer.Write(symbol.IconBits);            
        }

        private void WriteArea(Writer writer, Model.AreaSymbol symbol)
        {
            WriteAbstractSymbol(writer, symbol);

            if (symbol.BorderOn)
            {
                writer.Write(Writer.ConvertFromSymbolNumber(symbol.BorderSymbol.Number));
            }
            else
            {
                writer.Write(Writer.INT32_BLANK);
            }

            writer.Write(symbol.FillColour);
            writer.Write((Int16)symbol.HatchMode);
            writer.Write(symbol.HatchColour);
            writer.Write((Int16)(symbol.HatchLineWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.HatchDistance[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.HatchAngle1Degree * 10));
            writer.Write((Int16)(symbol.HatchAngle2Degree * 10));
            writer.Write(symbol.FillOn);
            writer.Write(symbol.BorderOn);
            writer.Write((Int16)symbol.StuctureMode);
            writer.Write((Int16)(symbol.StuctureWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.StuctureHeight[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.StuctureAngleDegree * 10));
            writer.Write(Writer.INT16_BLANK); // reserved0
            writer.Write((UInt16)(dataByteSize / Constant.DATA_BLOCK_BYTE_SIZE));
            writer.Write(symbol.Shapes);
        }

        private void WriteLine(Writer writer, Model.LineSymbol symbol)
        {
            WriteAbstractSymbol(writer, symbol);

            writer.Write(symbol.LineColour);
            writer.Write((Int16)(symbol.LineWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)symbol.LineStyle);
            writer.Write((Int16)(symbol.DistanceFromStart[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.DistanceFromEnd[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.MainLengthA[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.EndLengthB[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.MainGapC[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.MinorGapD[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.EndGapE[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.MinimumNumberOfGaps - 1));
            writer.Write(symbol.NMainSymbolA);
            writer.Write((Int16)(symbol.MainSymbolDistance[0, Distance.Unit.Metre, Scale.ten_minus_5]));

            writer.Write((UInt16)symbol.DoubleMode);
            writer.Write((UInt16)symbol.DoubleFlag);
            writer.Write(symbol.DoubleFillColour);
            writer.Write(symbol.DoubleLeftColour);
            writer.Write(symbol.DoubleRightColour);
            writer.Write((Int16)(symbol.DoubleWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.DoubleLeftWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.DoubleRightWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.DoubleDashedLength[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.DoubleDashedGap[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write(Writer.INT16_BLANK); // reserved0
            writer.Write(Writer.INT16_BLANK); // reserved1[0]
            writer.Write(Writer.INT16_BLANK); // reserved1[1]
            writer.Write((UInt16)symbol.DecreaseMode);
            writer.Write(symbol.DecreaseLastSymbolPercentageOfNormalSize);
            writer.Write(Writer.INT16_BLANK); // reserved2
            writer.Write(symbol.FramingColour);
            writer.Write((Int16)(symbol.FramingWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)symbol.FramingStyle);

            writer.Write((UInt16)(writer.Size(symbol.MainSymbolAShapes) / Constant.DATA_BLOCK_BYTE_SIZE));
            writer.Write((UInt16)(writer.Size(symbol.SecondarySymbolBShapes) / Constant.DATA_BLOCK_BYTE_SIZE));
            writer.Write((UInt16)(writer.Size(symbol.CornerSymbolShapes) / Constant.DATA_BLOCK_BYTE_SIZE));
            writer.Write((UInt16)(writer.Size(symbol.StartSymbolCShapes) / Constant.DATA_BLOCK_BYTE_SIZE));
            writer.Write((UInt16)(writer.Size(symbol.EndSymbolDShapes) / Constant.DATA_BLOCK_BYTE_SIZE));
            writer.Write(Writer.INT16_BLANK); // reserved3
            writer.Write(symbol.MainSymbolAShapes);
            writer.Write(symbol.SecondarySymbolBShapes);
            writer.Write(symbol.CornerSymbolShapes);
            writer.Write(symbol.StartSymbolCShapes);
            writer.Write(symbol.EndSymbolDShapes);
        }

        private void WritePoint(Writer writer, Model.PointSymbol symbol)
        {
            WriteAbstractSymbol(writer, symbol);

            writer.Write((UInt16)(dataByteSize / Constant.DATA_BLOCK_BYTE_SIZE));
            writer.Write(Writer.INT16_BLANK);
            writer.Write(symbol.Shapes);
        }

        private void WriteAbstractText(Writer writer, Model.AbstractTextSymbol symbol)
        {
            WriteAbstractSymbol(writer, symbol);

            writer.WritePascalString(symbol.FontName, MAX_FONT_NAME_SIZE);
            writer.Write(symbol.FontColour);
            writer.Write((Int16)(symbol.FontSize * 10));
            writer.Write((Int16)symbol.FontWeight);
            writer.Write(symbol.FontItalic);
            writer.Write(Writer.BYTE_BLANK); // reserved0
            writer.Write(symbol.CharacterSpacingPercentageOfSpace);
            writer.Write(symbol.WordSpacingPercentageOfSpace);
            writer.Write((Int16)symbol.TextAlignment);
        }

        private void WriteLineText(Writer writer, Model.LineTextSymbol symbol)
        {
            WriteAbstractText(writer, symbol);

            writer.Write((Byte)symbol.FramingMode);
            writer.Write(Writer.BYTE_BLANK); // reserved1
            writer.WritePascalString(String.Empty, MAX_LINE_RESERVED_SIZE); // reserved2
            writer.Write(symbol.FramingColour);
            writer.Write((Int16)(symbol.FramingWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write(Writer.INT16_BLANK); // reserved3
            writer.WriteWordBoolean(false); //reserved4
            writer.Write((Int16)(symbol.FramingShadowX[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.FramingShadowY[0, Distance.Unit.Metre, Scale.ten_minus_5]));
        }

        private void WriteBlockText(Writer writer, Model.BlockTextSymbol symbol)
        {
            WriteAbstractText(writer, symbol);

            writer.Write(symbol.LineSpacingPercentageOfFontSize);
            writer.Write((Int16)(symbol.SpaceAfterParagraph[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.IdentFirstLine[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.IdentOtherLines[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            Int32 nTabs = symbol.Tabs.Count;
            if (nTabs > MAX_TABS)
            {
                nTabs = MAX_TABS;
            }
            writer.Write((Int16)nTabs);
            for (int i = 0; i < MAX_TABS; i++)
            {
                if (i < nTabs)
                {
                    writer.Write((Int32)(symbol.Tabs[i][0, Distance.Unit.Metre, Scale.ten_minus_5]));
                }
                else
                {
                    writer.Write(Writer.INT32_BLANK);
                }
            }
            writer.WriteWordBoolean(symbol.LineBelowOn);
            writer.Write(symbol.LineBelowColour);
            writer.Write((Int16)(symbol.LineBelowWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.LineBelowDistance[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write(Writer.INT16_BLANK); // reserved1
            writer.Write((Byte)symbol.FramingMode);
            writer.Write((Byte)symbol.FramingLineStyle);
            writer.WritePascalString(String.Empty, MAX_BLOCK_TEXT_RESERVED_SIZE); // reserved2
            writer.Write((Int16)(symbol.FramingLeft[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.FramingBottom[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.FramingRight[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.FramingTop[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write(symbol.FramingColour);
            writer.Write((Int16)(symbol.FramingWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write(Writer.INT16_BLANK); // reserved3
            writer.WriteWordBoolean(false); //reserved4
            writer.Write((Int16)(symbol.FramingShadowX[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.FramingShadowY[0, Distance.Unit.Metre, Scale.ten_minus_5]));
        }

        private void WriteRectangle(Writer writer, Model.RectangleSymbol symbol)
        {
            WriteAbstractSymbol(writer, symbol);

            writer.Write(symbol.LineColour);
            writer.Write((Int16)(symbol.LineWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.Radius[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((UInt16)symbol.GridFlags);
            writer.Write((Int16)(symbol.CellWidth[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write((Int16)(symbol.CellHeight[0, Distance.Unit.Metre, Scale.ten_minus_5]));
            writer.Write(Writer.INT16_BLANK); // reserved0
            writer.Write(Writer.INT16_BLANK); // reserved1
            writer.Write(symbol.UnnumberedCells);
            writer.WritePascalString(symbol.UnnumberedCellsText, MAX_UNNUMBERED_CELLS_SIZE);
            writer.Write(Writer.INT16_BLANK); // reserved2
            writer.WritePascalString(String.Empty, MAX_RECTANGLE_RESERVED_SIZE); // reserved3
            writer.Write(Writer.INT16_BLANK); // reserved4
            writer.Write((Int16)(symbol.FontSize * 10));
            writer.Write(Writer.INT16_BLANK); // reserved6
            writer.WriteWordBoolean(false); // reserved7
            writer.Write(Writer.INT16_BLANK); // reserved8
            writer.Write(Writer.INT16_BLANK); // reserved9
        }
        #endregion
    }
}
