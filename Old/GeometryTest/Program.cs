using System;
using System.Collections.Generic;
using System.Text;
using Geometry;

namespace GeometryTest
{
    class Program
    {
        private static Distance epsilon;

        static void Main(string[] args)
        {
            epsilon = new Distance(1, Distance.Unit.Metre, Scale.ten_minus_5);
            TestDistanceMethods();
            TestCoordinateMethods();
            TestPointMethods();
            TestLinearSegmentMethods();
        }

        #region Distance
        public static void TestDistanceMethods()
        {
            TestDistanceEquals();
        }

        public static void TestDistanceEquals()
        {
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                Decimal d1Value = r.Next(200) / 100M;

                Distance d1 = new Distance(d1Value, Distance.Unit.Metre, Scale.ten_minus_5);

                Distance d2 = new Distance(d1Value + 0.99M, Distance.Unit.Metre, Scale.ten_minus_5);
                if (!d1.Equals(d2, epsilon))
                {
                    throw (new Exception("Difference less than 1, should be same"));
                }

                Distance d3 = new Distance(d1Value + 1.00M, Distance.Unit.Metre, Scale.ten_minus_5);
                if (d1.Equals(d3, epsilon))
                {
                    throw (new Exception("Difference equals 1, should not be same"));
                }                
            }
        }
        #endregion

        #region Coordinate
        public static void TestCoordinateMethods()
        {
            TestCoordinateEquals();
        }

        public static void TestCoordinateEquals()
        {
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                Decimal d1Value = r.Next(200) / 100M;

                Distance d1 = new Distance(d1Value, Distance.Unit.Metre, Scale.ten_minus_5);

                Distance d2 = new Distance(d1Value + 0.99M, Distance.Unit.Metre, Scale.ten_minus_5);
                if (!d1.Equals(d2, epsilon))
                {
                    throw (new Exception("Difference less than 1, should be same"));
                }

                Distance d3 = new Distance(d1Value + 1.00M, Distance.Unit.Metre, Scale.ten_minus_5);
                if (d1.Equals(d3, epsilon))
                {
                    throw (new Exception("Difference equals 1, should not be same"));
                }
            }
        }
        #endregion

        #region Point
        public static void TestPointMethods()
        {
            TestPointEquals();
        }

        public static void TestPointEquals()
        {
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                Decimal x1Value = r.Next(200) / 100M;
                Distance x1 = new Distance(x1Value, Distance.Unit.Metre, Scale.ten_minus_5);
                Distance x2 = new Distance(x1Value + 0.99M, Distance.Unit.Metre, Scale.ten_minus_5);
                Distance x3 = new Distance(x1Value + 1.00M, Distance.Unit.Metre, Scale.ten_minus_5);

                Decimal y1Value = r.Next(200) / 100M;
                Distance y1 = new Distance(y1Value, Distance.Unit.Metre, Scale.ten_minus_5);
                Distance y2 = new Distance(y1Value + 0.99M, Distance.Unit.Metre, Scale.ten_minus_5);
                Distance y3 = new Distance(y1Value + 1.00M, Distance.Unit.Metre, Scale.ten_minus_5);

                Point p1 = new Point(x1, y1);
                Point p22 = new Point(x2, y2);
                if (!p1.Equals(p22, epsilon))
                {
                    throw (new Exception("Difference less than 1, should be same"));
                }

                Point p23 = new Point(x2, y3);
                if (p1.Equals(p23, epsilon))
                {
                    throw (new Exception("Difference equals 1, should not be same"));
                }

                Point p32 = new Point(x3, y2);
                if (p1.Equals(p32, epsilon))
                {
                    throw (new Exception("Difference equals 1, should not be same"));
                }

                Point p33 = new Point(x3, y3);
                if (p1.Equals(p33, epsilon))
                {
                    throw (new Exception("Difference equals 1, should not be same"));
                }
            }
        }
        #endregion

        #region Linear Segment
        public static void TestLinearSegmentMethods()
        {
            TestLinearSegmentDistanceIsZero();
            TestLinearSegmentSideOf();
            TestLinearIsOn();
        }

        public static void TestLinearSegmentDistanceIsZero()
        {
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                Decimal x1Value = r.Next(200) / 100M;
                Distance x1 = new Distance(x1Value, Distance.Unit.Metre, Scale.ten_minus_5);
                Distance x2 = new Distance(x1Value + 0.99M, Distance.Unit.Metre, Scale.ten_minus_5);
                Distance x3 = new Distance(x1Value + 1.00M, Distance.Unit.Metre, Scale.ten_minus_5);

                Decimal y1Value = r.Next(200) / 100M;
                Distance y1 = new Distance(y1Value, Distance.Unit.Metre, Scale.ten_minus_5);
                Distance y2 = new Distance(y1Value + 0.99M, Distance.Unit.Metre, Scale.ten_minus_5);
                Distance y3 = new Distance(y1Value + 1.00M, Distance.Unit.Metre, Scale.ten_minus_5);

                Point p1 = new Point(x1, y1);
                Point p22 = new Point(x2, y2);
                LinearSegment s1_22 = new LinearSegment(p1, p22);
                if (!s1_22.DistanceIsZero(epsilon))
                {
                    throw (new Exception("Difference less than 1, should be same"));
                }

                Point p23 = new Point(x2, y3);
                LinearSegment s1_23 = new LinearSegment(p1, p23);
                if (s1_23.DistanceIsZero(epsilon))
                {
                    throw (new Exception("Difference equals 1, should not be same"));
                }

                Point p32 = new Point(x3, y2);
                LinearSegment s1_32 = new LinearSegment(p1, p32);
                if (s1_32.DistanceIsZero(epsilon))
                {
                    throw (new Exception("Difference equals 1, should not be same"));
                }

                Point p33 = new Point(x3, y3);
                LinearSegment s1_33 = new LinearSegment(p1, p33);
                if (s1_33.DistanceIsZero(epsilon))
                {
                    throw (new Exception("Difference equals 1, should not be same"));
                }
            }
        }

        public static void TestLinearSegmentSideOf()
        {
            Decimal scale = 4M;

            Point p = new Point(
                        new Distance(scale, Distance.Unit.Metre, Scale.ten_minus_5),
                        new Distance(scale, Distance.Unit.Metre, Scale.ten_minus_5));

            for (int i = 0; i < 360; i++)
            {
                double degree = i * Math.PI / 180.0;
                double y = 2 * (double)scale * Math.Sin(degree);
                double x = 2 * (double)scale * Math.Cos(degree);

                LinearSegment s = new LinearSegment(
                    new Point(
                        new Distance(0M, Distance.Unit.Metre, Scale.ten_minus_5),
                        new Distance(0M, Distance.Unit.Metre, Scale.ten_minus_5)),
                    new Point(
                        new Distance((Decimal)x, Distance.Unit.Metre, Scale.ten_minus_5),
                        new Distance((Decimal)y, Distance.Unit.Metre, Scale.ten_minus_5)));

                DirectionSide d = s.SideOf(p, epsilon);
                Console.WriteLine("{0} {1}", i, d);
                if (((i < 44) || (i > 226)) && (d != DirectionSide.Leftside))
                {
                    throw (new Exception());
                }
                if ((((i >= 44) && (i <= 46)) || ((i >= 224) && (i <= 226))) && (d != DirectionSide.InLine))
                {
                    throw (new Exception());
                }
                if (((i > 46) && (i < 224)) && (d != DirectionSide.Rightside))
                {
                    throw (new Exception());
                }
            }
        }

        public static void TestLinearIsOn()
        {
            Decimal scale = 4M;

            Point p1 = new Point(
                        new Distance(scale, Distance.Unit.Metre, Scale.ten_minus_5),
                        new Distance(scale, Distance.Unit.Metre, Scale.ten_minus_5));

            double degree = 45 * Math.PI / 180.0;
            double y = 2 * (double)scale * Math.Sin(degree);
            double x = 2 * (double)scale * Math.Cos(degree);

            Point p2 = new Point(
                        new Distance((Decimal)x - 1M, Distance.Unit.Metre, Scale.ten_minus_5),
                        new Distance((Decimal)y - 1M, Distance.Unit.Metre, Scale.ten_minus_5));

            Point p3 = new Point(
                        new Distance((Decimal)x - 0.99M, Distance.Unit.Metre, Scale.ten_minus_5),
                        new Distance((Decimal)y - 0.99M, Distance.Unit.Metre, Scale.ten_minus_5));

            for (int i = 0; i < 360; i++)
            {
                degree = i * Math.PI / 180.0;
                y = 2 * (double)scale * Math.Sin(degree);
                x = 2 * (double)scale * Math.Cos(degree);

                LinearSegment s = new LinearSegment(
                    new Point(
                        new Distance(0M, Distance.Unit.Metre, Scale.ten_minus_5),
                        new Distance(0M, Distance.Unit.Metre, Scale.ten_minus_5)),
                    new Point(
                        new Distance((Decimal)x, Distance.Unit.Metre, Scale.ten_minus_5),
                        new Distance((Decimal)y, Distance.Unit.Metre, Scale.ten_minus_5)));

                Boolean on = s.IsOn(p1, epsilon);
                Console.WriteLine("{0} {1}", i, on);
                if (((i >= 44) && (i <= 46)) && !on)
                {
                    throw (new Exception());
                }
                if (((i < 44) || (i > 46)) && on)
                {
                    throw (new Exception());
                }

                on = s.IsOn(p2, epsilon);
                Console.WriteLine("{0} {1}", i, on);
                if ((i == 45) && !on)
                {
                    throw (new Exception());
                }
                if ((i != 45) && on)
                {
                    throw (new Exception());
                }

                on = s.IsOn(p3, epsilon);
                Console.WriteLine("{0} {1}", i, on);
                if (on)
                {
                    throw (new Exception());
                }
            }
        }
        #endregion
    }
}
