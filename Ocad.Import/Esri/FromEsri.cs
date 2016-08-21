using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class FromEsri
    {
        public static void CopyTo(this Esri.Model.Map esriMap, Ocad.Model.Map ocadMap)
        {
            decimal scale = ocadMap.ScaleParameter.MapScale;
            Geometry.Point offset = new Geometry.Point(ocadMap.ScaleParameter.RealWorldOffsetX, ocadMap.ScaleParameter.RealWorldOffsetY);

            foreach (Esri.Model.Shape.AbstractShape shape in esriMap.Shapes)
            {
                ConvertItem(ocadMap, offset, scale, shape);
            }
        }

        #region Convert
        private static void ConvertItem(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Esri.Model.Shape.AbstractShape item)
        {
            String type = item.GetType().FullName;
            switch (type)
            {
                case "Esri.Model.Shape.Point":
                    DrawAsOcadPointObject(ocadMap, offset, scale, (Esri.Model.Shape.Point)item, Iof.Isom.Symbols._507_3_AccessPoint);
                    break;
                case "Esri.Model.Shape.PolyLine":
                    DrawAsOcadLineObject(ocadMap, offset, scale, (Esri.Model.Shape.PolyLine)item, Iof.Isom.Symbols._306_Stream);
                    break;
                default:
                    throw (new NotImplementedException());
            }
        }

        private static void DrawAsOcadPointObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Esri.Model.Shape.Point item, Iof.Isom.Symbols isomSymbol)
        {
            string ocadAreaSymbolNumber, ocadLineSymbolNumber, ocadPointSymbolNumber;
            Common.GetOcadSymbol(isomSymbol, out ocadAreaSymbolNumber, out ocadLineSymbolNumber, out ocadPointSymbolNumber);

            Ocad.Model.AbstractSymbol symbol = ocadMap.GetSymbol(ocadPointSymbolNumber);
            if (symbol == null)
            {
                return;
            }

            Geometry.Point point = new Geometry.Point(
                    new Distance((decimal)item.X, Distance.Unit.Metre, Scale.one),
                    new Distance((decimal)item.Y, Distance.Unit.Metre, Scale.one));
            Ocad.Model.Point ocadPoint = new Ocad.Model.Point(point, offset, scale);

            Ocad.Model.SymbolObject ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Point, symbol);
            ocadPoint.MainPointFlag = Ocad.Model.Type.PointFlag.BasicPoint;
            ocadItem.Points.Add(ocadPoint);
        }

        private static void DrawAsOcadLineObject(Ocad.Model.Map ocadMap, Geometry.Point offset, decimal scale, Esri.Model.Shape.PolyLine item, Iof.Isom.Symbols isomSymbol)
        {
            string ocadAreaSymbolNumber, ocadLineSymbolNumber, ocadPointSymbolNumber;
            Common.GetOcadSymbol(isomSymbol, out ocadAreaSymbolNumber, out ocadLineSymbolNumber, out ocadPointSymbolNumber);

            Ocad.Model.AbstractSymbol symbol = ocadMap.GetSymbol(ocadLineSymbolNumber);
            if (symbol == null)
            {
                return;
            }

            List<ISegment> segmentList = new List<ISegment>();

            Ocad.Model.SymbolObject ocadItem;
            Geometry.Point start = new Geometry.Point(
                    new Distance((decimal)item.Points[0].X, Distance.Unit.Metre, Scale.one),
                    new Distance((decimal)item.Points[0].Y, Distance.Unit.Metre, Scale.one));
            Geometry.Point end;

            for (int i = 1; i < item.Points.Count; i += 1)
            {
                end = new Geometry.Point(
                        new Distance((decimal)item.Points[i].X, Distance.Unit.Metre, Scale.one),
                        new Distance((decimal)item.Points[i].Y, Distance.Unit.Metre, Scale.one));

                if (item.PartStartPointIndexes.Contains(i))
                {
                    // The point is start of a new line, and not part of the current line
                    // Convert the current line into an OCAD item, and start a new line
                    if (segmentList.Count > 1)
                    {
                        ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, symbol);
                        Common.BuildOcadPointList(offset, scale, segmentList, ocadItem.Points, false);
                        segmentList = new List<ISegment>();
                    }
                }
                else
                {
                    segmentList.Add(new LinearSegment(start, end));
                }

                start = end;
            }

            if (segmentList.Count > 1)
            {
                ocadItem = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, symbol);
                Common.BuildOcadPointList(offset, scale, segmentList, ocadItem.Points, false);
            }
        }

        #endregion
    }
}
