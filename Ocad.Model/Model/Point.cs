using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class Point : Geometry.Point
    {
        [VersionsSupported(V9 = true)]
        private Geometry.Point Main { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.PointFlag MainPointFlag { get; set; }

        [VersionsSupported(V9 = true)]
        public Geometry.Point FirstBezier { get; set; }
        [VersionsSupported(V9 = true)]
        public Geometry.Point SecondBezier { get; set; }

        public Point(Geometry.Point p)
        {
            p.X.Convert(Scale.ten_minus_5);
            p.Y.Convert(Scale.ten_minus_5);
            Main = p;

            X = Main.X;
            Y = Main.Y;

            MainPointFlag = Type.PointFlag.BasicPoint;
        }

        public Point(Geometry.Point realworldPoint, Geometry.Point offset, decimal scale)
        {
            Distance x = realworldPoint.X - offset.X;
            x = x / scale;
            x.Convert(Scale.ten_minus_5);

            Distance y = realworldPoint.Y - offset.Y;
            y = y / scale;
            y.Convert(Scale.ten_minus_5);

            Main = new Geometry.Point(x, y);

            X = Main.X;
            Y = Main.Y;
            
            MainPointFlag = Type.PointFlag.BasicPoint;
        }

        public Point(Geometry.Distance x, Geometry.Distance y)
        {
            x.Convert(Scale.ten_minus_5);
            y.Convert(Scale.ten_minus_5);
            Main = new Geometry.Point(x, y);
            
            X = Main.X;
            Y = Main.Y;

            MainPointFlag = Type.PointFlag.BasicPoint;
        }
    }
}
