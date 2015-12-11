using System;
using System.Collections.Generic;
using System.Text;

namespace Ocad.IO.Ocad9.Type
{
    internal enum SettingType
    {
        EventObject = 1,
        EventCourse = 2,
        EventClass = 3,
        DataSet = 4,
        DatabaseObject = 5,
        OimFile = 6,
        EventPreviewObject = 7,
        Template = 8,
        Colour = 9,
        SpotColour = 10,
        FileInfo = 11,
        Zoom = 12,
        ImportLayer = 13,
        OimFind = 14,
        SymbolTree = 15,
        DisplayParameter = 1024,
        OimParameter = 1025,
        PrintParameter = 1026,
        EventControlDescriptionPrintParameter = 1027,
        TemplateParameter = 1028,
        EpsParameter = 1029,
        ViewParameter = 1030,
        EventParameter = 1031,
        TiffParameter = 1032,
        TilesParameter = 1033,
        DatabaseParameter = 1034,
        ExportParameter = 1035,
        EventExportCoursesTextParameter = 1037,
        EventExportCoursesStatisticsParameter = 1038,
        ScaleParameter = 1039,
        DatabaseCreateObjectParameter = 1040,
        SelectedSpotColoursParameter = 1041,
        XmlScriptParameter = 1042
    }

    [Flags]
    internal enum PointFlag
    {
        BasicPoint = Model.Type.PointFlag.BasicPoint,
        FirstBezierPoint = 1,
        SecondBezierPoint = 2,
        NoLeftLinePoint = Model.Type.PointFlag.NoLeftLinePoint,
        BorderGapPoint = Model.Type.PointFlag.BorderGapPoint,
        CornerPoint = Model.Type.PointFlag.CornerPoint,
        HolePoint = Model.Type.PointFlag.HolePoint,
        NoRightLinePoint = Model.Type.PointFlag.NoRightLinePoint,
        DashPoint = Model.Type.PointFlag.DashPoint
    }
}
