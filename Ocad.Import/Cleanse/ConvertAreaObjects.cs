using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class Cleanse
    {
        public static Area MinLargeArea = new Area(27777, Distance.Unit.Metre, Scale.ten_minus_5);

        /// <summary>
        /// Favour Endpoints and work inwards, to prevent drifting if the object consists only of short segments.
        /// </summary>
        /// <param name="ocadMap"></param>
        public static void ConvertAreaObjects(this Ocad.Model.Map ocadMap)
        {
            foreach (Ocad.Model.AbstractObject obj in ocadMap.Objects)
            {
                if (!(obj is Ocad.Model.SymbolObject))
                {
                    continue;
                }

                if (obj.FeatureType != Model.Type.FeatureType.Area)
                {
                    continue;
                }

                Ocad.Model.SymbolObject areaObject = obj as Ocad.Model.SymbolObject;

                string smallAreaNumber, largeAreaNumber;
                switch (areaObject.Symbol.Number)
                {
                    case "301.0" :
                    case "302.0":
                        largeAreaNumber = "301.0";
                        smallAreaNumber = "302.0";
                        break;
                    case "529.0":
                    case "529.1":
                        largeAreaNumber = "529.0";
                        smallAreaNumber = "529.1";
                        break;
                    default:
                        continue;
                }

                #region Calculate Area of Object
                Area areaObjectArea = new Area(0, Distance.Unit.Metre, Scale.one);
                Geometry.Point p1 = areaObject.Points[0];
                Geometry.Point p2 = null;
                for (int i = 1; i < areaObject.Points.Count; i++)
                {
                    p2 = areaObject.Points[i];
                    areaObjectArea += p1.X * p2.Y;
                    areaObjectArea -= p1.Y * p2.X;

                    p1 = p2;

                    #region Remove Hole Area
                    if ((areaObject.Points[i].MainPointFlag & Model.Type.PointFlag.HolePoint) == Model.Type.PointFlag.HolePoint)
                    {
                        p2 = null;
                        for (int j = i + 1; j < areaObject.Points.Count; j++)
                        {
                            p2 = areaObject.Points[i];
                            areaObjectArea -= p1.X * p2.Y;
                            areaObjectArea += p1.Y * p2.X;
                            p1 = p2;
                        }
                        if ((p2 != null) && (areaObject.Points[i].GetRelationship(p2, Common.EPSILON) == Relationship.Apart))
                        {
                            areaObjectArea -= p2.X * areaObject.Points[i].Y;
                            areaObjectArea += p2.Y * areaObject.Points[i].X;
                        }
                        break;
                    }
                    #endregion
                }
                #region Close Area, if not already closed
                if ((p2 != null) && (areaObject.Points[0].GetRelationship(p2, Common.EPSILON) == Relationship.Apart))
                {
                    areaObjectArea += p2.X * areaObject.Points[0].Y;
                    areaObjectArea -= p2.Y * areaObject.Points[0].X;
                }
                #endregion

                areaObjectArea = areaObjectArea / 2M;
                areaObjectArea = areaObjectArea.Absolute();
                #endregion

                #region Change Symbol

                string correctAreaNumber = (areaObjectArea.IsLessThan(MinLargeArea, Common.EPSILON)) ? smallAreaNumber : largeAreaNumber;
                if (areaObject.Symbol.Number == correctAreaNumber)
                {
                    continue;
                }

                areaObject.Symbol = ocadMap.GetSymbol(correctAreaNumber);
                #endregion
            }
        }

    }
}
