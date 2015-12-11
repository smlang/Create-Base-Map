using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace Ocad.Import
{
    public static partial class Cleanse
    {
        private const double FULL_CIRCLE = 2 * Math.PI;
        private const double THIRD_CIRCLE = 2 * Math.PI / 3;
        private const double TWO_THIRD_CIRCLE = 4 * Math.PI / 3;

        public static void ConvertToCornerPoints(this Ocad.Model.Map ocadMap)
        {
            foreach (Ocad.Model.AbstractObject obj in ocadMap.Objects)
            {
                // Need at least three points to have two segments that form an angle
                if (obj.Points.Count < 3)
                {
                    continue;
                }

                #region Initialise Q and R
                Ocad.Model.Point p;
                Ocad.Model.Point q = obj.Points[0];
                Ocad.Model.Point r = obj.Points[1];

                double px;
                double py;
                double qx = (double)q.X[0, Distance.Unit.Metre, Scale.ten_minus_5];
                double qy = (double)q.Y[0, Distance.Unit.Metre, Scale.ten_minus_5];
                double rx = (double)r.X[0, Distance.Unit.Metre, Scale.ten_minus_5];
                double ry = (double)r.Y[0, Distance.Unit.Metre, Scale.ten_minus_5];

                double qpx;
                double qpy;
                double qrx = qx - rx;
                double qry = qy - ry;
                #endregion

                for (int i = 1; i <= obj.Points.Count - 2; i++)
                {
                    #region Get angle between PQ and QR
                    #region Shift Q->P, R->Q, and get new R
                    p = q;
                    q = r;
                    r = obj.Points[i + 1];
                    
                    px = qx;
                    py = qy;
                    qx = rx;
                    qy = ry;
                    rx = (double)r.X[0, Distance.Unit.Metre, Scale.ten_minus_5];
                    ry = (double)r.Y[0, Distance.Unit.Metre, Scale.ten_minus_5];

                    qpx = -qrx;
                    qpy = -qry;
                    qrx = qx - rx;
                    qry = qy - ry;
                    #endregion

                    if (q.MainPointFlag != Model.Type.PointFlag.BasicPoint)
                    {
                        // If not a basic point, point Q should not be convert
                        continue;
                    }

                    #region Calculate anit-clockwise angle PQR
                    double dot = (qpx * qrx) + (qpy * qry);
                    double cross = (qpx * qry) - (qpy * qrx);
                    double pqr = (Math.Atan2(cross, dot) + FULL_CIRCLE) % FULL_CIRCLE;
                    #endregion

                    #region Convert if PQR is acute
                    if ((pqr <= THIRD_CIRCLE) || (pqr >= TWO_THIRD_CIRCLE)) // Angle >120 Degrees or <240 Degrees
                    {
                        q.MainPointFlag |= Model.Type.PointFlag.CornerPoint;
                    }
                    #endregion
                    #endregion
                }
            }
        }
    }
}
