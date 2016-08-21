using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    internal static partial class Common
    {
        internal static Distance EPSILON = new Distance(Ocad.Import.Settings.Default.EpsilonDistanceMM * 100M, Distance.Unit.Metre, Scale.ten_minus_5);

        internal static void BuildOcadPointList(Geometry.Point offset, decimal scale, List<ISegment> segments, List<Ocad.Model.Point> points, bool forceClose, Ocad.Model.Type.PointFlag firstPointFlag = Ocad.Model.Type.PointFlag.BasicPoint)
        {
            Ocad.Model.Point firstPoint = new Ocad.Model.Point(segments[0].Start, offset, scale);
            firstPoint.MainPointFlag = firstPointFlag;
            points.Add(firstPoint);
            foreach (ISegment segment in segments)
            {
                points.Add(new Ocad.Model.Point(segment.End, offset, scale));
            }

            if (forceClose && (points[0].GetRelationship(points[points.Count - 1], EPSILON) == Relationship.Apart))
            {
                points.Add(new Ocad.Model.Point(segments[0].Start, offset, scale));
            }
        }

        internal static void GetOcadSymbol(Iof.Isom.Symbols symbol, out string ocadAreaSymbolNumber, out string ocadLineSymbolNumber, out string ocadPointSymbolNumber)
        {
            switch (symbol)
            {
                case Iof.Isom.Symbols._301_Lake:
                    ocadAreaSymbolNumber = "301.0";
                    ocadLineSymbolNumber = null; //"301.1";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._302_Pond:
                    ocadAreaSymbolNumber = "302.0";
                    ocadLineSymbolNumber = null;
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._304_River:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "304.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._306_Stream:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "306.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._401_OpenArea:
                    ocadAreaSymbolNumber = "401.0";
                    ocadLineSymbolNumber = null;
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._403_RoughOpenArea:
                    ocadAreaSymbolNumber = "403.0";
                    ocadLineSymbolNumber = null;
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._408_Woodland:
                    ocadAreaSymbolNumber = "408.0";
                    ocadLineSymbolNumber = null;
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._501_DualCarriageWay:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "501.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._502_ARoad:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "502.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._503_BRoad:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "503.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._504_Road:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "504.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._505_Track:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "505.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._507_Footpath:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "507.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._507_3_AccessPoint:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = null;
                    ocadPointSymbolNumber = "507.3";
                    return;
                case Iof.Isom.Symbols._515_Railway:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "515.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._515_1_TramwayOverRoad:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "515.1";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._518_Bridge:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = "518.0";
                    ocadPointSymbolNumber = null;
                    return;
                case Iof.Isom.Symbols._529_PavedArea:
                    ocadAreaSymbolNumber = "529.0";
                    ocadLineSymbolNumber = null;
                    ocadPointSymbolNumber = null;
                    return;
                default:
                    ocadAreaSymbolNumber = null;
                    ocadLineSymbolNumber = null;
                    ocadPointSymbolNumber = null;
                    return;
            }
        }
    }
}
