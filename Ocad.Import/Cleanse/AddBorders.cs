using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class Cleanse
    {
        /// <summary>
        /// Favour Endpoints and work inwards, to prevent drifting if the object consists only of short segments.
        /// </summary>
        /// <param name="ocadMap"></param>
        public static void AddBorders(this Ocad.Model.Map ocadMap)
        {
            #region Get Lakes
            List<Ocad.Model.SymbolObject> areaObjectList = new List<Model.SymbolObject>();
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

                switch (areaObject.Symbol.Number)
                {
                    case "301.0" :
                        areaObjectList.Add(areaObject);
                        break;
                    default:
                        continue;
                }
            }
            #endregion

            #region Add Lake Edge
            foreach (Ocad.Model.SymbolObject areaObject in areaObjectList)
            {
                Ocad.Model.AbstractSymbol borderSymbol = null;
                switch (areaObject.Symbol.Number)
                {
                    case "301.0":
                        borderSymbol = ocadMap.GetSymbol("301.1");
                        break;
                    default:
                        continue;
                }

                Ocad.Model.SymbolObject borderObject = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, borderSymbol);
                foreach (Ocad.Model.Point p in areaObject.Points)
                {
                    if ((p.MainPointFlag & Model.Type.PointFlag.HolePoint) == Model.Type.PointFlag.HolePoint)
                    {
                        // Start new border
                        borderObject = new Ocad.Model.SymbolObject(ocadMap, Ocad.Model.Type.FeatureType.Line, borderSymbol);
                    }
                    borderObject.Points.Add(new Model.Point(p));
                }
                // Close border
                if (borderObject.Points[0].GetRelationship(borderObject.Points[borderObject.Points.Count - 1], Common.EPSILON) == Relationship.Apart)
                {
                    borderObject.Points.Add(new Model.Point(areaObject.Points[0]));
                }
            }
            #endregion
        }

    }
}
