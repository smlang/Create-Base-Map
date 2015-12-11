using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model.Type
{
    [VersionsSupported(V9 = true)]
    public enum TiffCompression
    {
        [VersionsSupported(V9 = true)]
        NoCompression = 1,
        [VersionsSupported(V9 = true)]
        Ccitt = 2,
        [VersionsSupported(V9 = true)]
        FaxG4 = 5,
        [VersionsSupported(V9 = true)]
        Lzw = 5
    }

    [VersionsSupported(V9 = true)]
    public enum ColourFormat
    {
        [VersionsSupported(V9 = true)]
        _32BitRgb = 0,
        [VersionsSupported(V9 = true)]
        _24BitRgb = 1,
        [VersionsSupported(V9 = true)]
        _8BitRgb = 2,
        [VersionsSupported(V9 = true)]
        Grayscale = 3,
        [VersionsSupported(V9 = true)]
        _8BitCmyk = 4,
        [VersionsSupported(V9 = true)]
        _1BitBlackAndWhite = 5,
        [VersionsSupported(V9 = true)]
        HalftoneScreen = 6
    }

    [VersionsSupported(V9 = true)]
    public enum FileType
    {
        [VersionsSupported(V9 = true)]
        Normal = 0,
        [VersionsSupported(V9 = true)]
        Event = 1,
        [VersionsSupported(V9 = true)]
        EventOcad8 = 3
    }

    [VersionsSupported(V9 = true)]
    public enum ObjectType
    {
        [VersionsSupported(V9 = true)]
        Image = -3,
        [VersionsSupported(V9 = true)]
        Graphic = -2,
        [VersionsSupported(V9 = true)]
        Imported = -1,
        [VersionsSupported(V9 = true)]
        Symbol = 0
    }

    [VersionsSupported(V9 = true)]
    public enum FeatureType
    {
        [VersionsSupported(V9 = true)]
        Point = 1,
        [VersionsSupported(V9 = true)]
        Line = 2,
        [VersionsSupported(V9 = true)]
        Area = 3,
        [VersionsSupported(V9 = true)]
        UnformattedText = 4,
        [VersionsSupported(V9 = true)]
        FormattedText = 5,
        [VersionsSupported(V9 = true)]
        LineText = 6,
        [VersionsSupported(V9 = true)]
        Rectangle = 7
    }

    [VersionsSupported(V9 = true)]
    public enum SymbolType
    {
        [VersionsSupported(V9 = true)]
        Point = 1,
        [VersionsSupported(V9 = true)]
        Line = 2,
        [VersionsSupported(V9 = true)]
        Area = 3,
        [VersionsSupported(V9 = true)]
        BlockText = 4,
        [VersionsSupported(V9 = true)]
        LineText = 6,
        [VersionsSupported(V9 = true)]
        Rectangle = 7
    }

    [VersionsSupported(V9 = true)]
    public enum ObjectStatus
    {
        [VersionsSupported(V9 = true)]
        DeletedParent = 0,
        [VersionsSupported(V9 = true)]
        Normal = 1,
        [VersionsSupported(V9 = true)]
        Hidden = 2,
        [VersionsSupported(V9 = true)]
        Deleted = 3
    }

    [VersionsSupported(V9 = true)]
    public enum ObjectView
    {
        [VersionsSupported(V9 = true)]
        Normal = 0,
        [VersionsSupported(V9 = true)]
        Event = 1,
        [VersionsSupported(V9 = true)]
        ModifiedPreview = 2,
        [VersionsSupported(V9 = true)]
        UnmodifiedPreview = 3,
        [VersionsSupported(V9 = true)]
        Temporary = 4,
        [VersionsSupported(V9 = true)]
        Import = 10
    }

    [Flags]
    [VersionsSupported(V9 = true)]
    public enum PointFlag
    {
        [VersionsSupported(V9 = true)]
        BasicPoint = 0,
        //FirstBezierPoint = 1,
        //SecondBezierPoint = 2,
        [VersionsSupported(V9 = true)]
        NoLeftLinePoint = 4,
        [VersionsSupported(V9 = true)]
        BorderGapPoint = 8,
        [VersionsSupported(V9 = true)]
        CornerPoint = 256,
        [VersionsSupported(V9 = true)]
        HolePoint = 512,
        [VersionsSupported(V9 = true)]
        NoRightLinePoint = 1024,
        [VersionsSupported(V9 = true)]
        DashPoint = 2048
    }

    [Flags]
    [VersionsSupported(V9 = true)]
    public enum SymbolFlag
    {
        [VersionsSupported(V9 = true)]
        None = 0,
        [VersionsSupported(V9 = true)]
        Rotatable = 1,
        [VersionsSupported(V9 = true)]
        IsFavourite = 4
    }

    [VersionsSupported(V9 = true)]
    public enum SymbolStatus
    {
        [VersionsSupported(V9 = true)]
        Normal = 0,
        [VersionsSupported(V9 = true)]
        Protected = 1,
        [VersionsSupported(V9 = true)]
        Hidden = 2
    }

    [VersionsSupported(V9 = true)]
    public enum DefaultDrawingMode
    {
        [VersionsSupported(V9 = true)]
        None = 0,
        [VersionsSupported(V9 = true)]
        CurveMode = 1,
        [VersionsSupported(V9 = true)]
        EllipseMode = 2,
        [VersionsSupported(V9 = true)]
        CircleMode = 3,
        [VersionsSupported(V9 = true)]
        RectanglarLineMode = 4,
        [VersionsSupported(V9 = true)]
        RectanglarAreaMode = 5,
        [VersionsSupported(V9 = true)]
        StraightLineMode = 6,
        [VersionsSupported(V9 = true)]
        FreehandMode = 7
    }

    [VersionsSupported(V9 = true)]
    public enum EventMode
    {
        [VersionsSupported(V9 = true)]
        NotUsedForEvent = 0,
        [VersionsSupported(V9 = true)]
        CourseSymbol = 1,
        [VersionsSupported(V9 = true)]
        ControlDescriptionSymbol = 2
    }

    [VersionsSupported(V9 = true)]
    public enum ControlObjectType
    {
        [VersionsSupported(V9 = true)]
        StartSymbolPoint = 0,
        [VersionsSupported(V9 = true)]
        ControlSymbolPoint = 1,
        [VersionsSupported(V9 = true)]
        FinishSymbolPoint = 2,
        [VersionsSupported(V9 = true)]
        MarkedRouteLine = 3,
        [VersionsSupported(V9 = true)]
        ControlDescriptionSymbolPoint = 4,
        [VersionsSupported(V9 = true)]
        CourseTitleText = 5,
        [VersionsSupported(V9 = true)]
        StartNumberText = 6,
        [VersionsSupported(V9 = true)]
        VariantText = 7,
        [VersionsSupported(V9 = true)]
        BlockText = 8
    }

    [Flags]
    [VersionsSupported(V9 = true)]
    public enum CourseDescriptionFlag
    {
        [VersionsSupported(V9 = true)]
        None = 0,
        [VersionsSupported(V9 = true)]
        ColumnH = 1,
        [VersionsSupported(V9 = true)]
        ColumnG = 2,
        [VersionsSupported(V9 = true)]
        ColumnF = 4,
        [VersionsSupported(V9 = true)]
        ColumnE = 8,
        [VersionsSupported(V9 = true)]
        ColumnD = 16,
        [VersionsSupported(V9 = true)]
        ColumnC = 32,
    }

    [VersionsSupported(V9 = true)]
    public enum FontWeight
    {
        [VersionsSupported(V9 = true)]
        Normal = 400,
        [VersionsSupported(V9 = true)]
        Bold = 700
    }

    [VersionsSupported(V9 = true)]
    public enum TextAlignment
    {
        [VersionsSupported(V9 = true)]
        Left = 0,
        [VersionsSupported(V9 = true)]
        Centre = 1,
        [VersionsSupported(V9 = true)]
        Right = 2,
        [VersionsSupported(V9 = true)]
        Justified = 3
    }

    [VersionsSupported(V9 = true)]
    public enum FramingMode
    {
        [VersionsSupported(V9 = true)]
        Off = 0,
        [VersionsSupported(V9 = true)]
        ShadowFraming = 1,
        [VersionsSupported(V9 = true)]
        LineFraming = 2
    }

    [VersionsSupported(V9 = true)]
    public enum FramingStyle
    {
        [VersionsSupported(V9 = true)]
        Bevel = 0,
        [VersionsSupported(V9 = true)]
        Round = 1,
        [VersionsSupported(V9 = true)]
        Miter = 4
    }

    [VersionsSupported(V9 = true)]
    public enum LineStyle
    {
        [VersionsSupported(V9 = true)]
        Default = 0,
        [VersionsSupported(V9 = true)]
        Round = 1,
        [VersionsSupported(V9 = true)]
        Bevel = 2,
        [VersionsSupported(V9 = true)]
        Miter = 4
    }

    [VersionsSupported(V9 = true)]
    public enum DoubleLineMode
    {
        [VersionsSupported(V9 = true)]
        Off = 0,
        [VersionsSupported(V9 = true)]
        FullLines = 1,
        [VersionsSupported(V9 = true)]
        LeftLineDashed = 2,
        [VersionsSupported(V9 = true)]
        BothLineDashed = 3,
        [VersionsSupported(V9 = true)]
        AllDashed = 4
    }

    [VersionsSupported(V9 = true)]
    public enum ShapeType
    {
        [VersionsSupported(V9 = true)]
        Line = 1,
        [VersionsSupported(V9 = true)]
        Area = 2,
        [VersionsSupported(V9 = true)]
        Circle = 3,
        [VersionsSupported(V9 = true)]
        Dot = 4
    }

    [Flags]
    [VersionsSupported(V9 = true)]
    public enum DoubleLineFlag
    {
        [VersionsSupported(V9 = true)]
        None = 0,
        [VersionsSupported(V9 = true)]
        FillColourOn = 1,
        [VersionsSupported(V9 = true)]
        BackgroundColourOn = 2
    }

    [VersionsSupported(V9 = true)]
    public enum DecreaseMode
    {
        [VersionsSupported(V9 = true)]
        Off = 0,
        [VersionsSupported(V9 = true)]
        DecreaseTowardsOneEnd = 1,
        [VersionsSupported(V9 = true)]
        DecreaseTowardsBothEnds = 2
    }

    [VersionsSupported(V9 = true)]
    public enum HatchMode
    {
        [VersionsSupported(V9 = true)]
        Off = 0,
        [VersionsSupported(V9 = true)]
        SingleHatch = 1,
        [VersionsSupported(V9 = true)]
        CrossHatch = 2
    }

    [VersionsSupported(V9 = true)]
    public enum StructureMode
    {
        [VersionsSupported(V9 = true)]
        Off = 0,
        [VersionsSupported(V9 = true)]
        AlignedRows = 1,
        [VersionsSupported(V9 = true)]
        ShiftedRows = 2
    }

    [Flags]
    [VersionsSupported(V9 = true)]
    public enum GridFlag
    {
        [VersionsSupported(V9 = true)]
        None = 0,
        [VersionsSupported(V9 = true)]
        GridOn = 1,
        [VersionsSupported(V9 = true)]
        NumberingOn = 2,
        [VersionsSupported(V9 = true)]
        NumberedFromBottom = 4
    }

    [VersionsSupported(V9 = true)]
    public enum CoordinateSystemType
    {
        [VersionsSupported(V9 = true)]
        UK_NationalGrid = 5000,
        [VersionsSupported(V9 = true)]
        FI_Zone1 = 6001
    }
}
