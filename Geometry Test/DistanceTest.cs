using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Geometry;

namespace Geometry_Test
{
    [TestClass]
    public class DistanceTest
    {
        private static Scale[] _scales = new Scale[] { Scale.ten_minus_5, Scale.ten_minus_4, Scale.milli, Scale.centi, Scale.deci, Scale.one, Scale.deca, Scale.hecto, Scale.kilo };

        #region Scale Operations
        [TestMethod]
        public void SameScale()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Scale.one];

            // Assert
            decimal expected = x;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void DecreaseScaleByOneMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Scale.deci];

            // Assert
            decimal expected = x * 10;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void DecreaseScaleByTwoMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Scale.centi];

            // Assert
            decimal expected = x * 100;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void DecreaseScaleByThreeMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Scale.milli];

            // Assert
            decimal expected = x * 1000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void DecreaseScaleByFourMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Scale.ten_minus_4];

            // Assert
            decimal expected = x * 10000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void DecreaseScaleByFiveMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Scale.ten_minus_5];

            // Assert
            decimal expected = x * 100000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void DecreaseScaleBySixMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.deca);

            // Act
            decimal actual = d[Scale.ten_minus_5];

            // Assert
            decimal expected = x * 1000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void DecreaseScaleBySevenMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.hecto);

            // Act
            decimal actual = d[Scale.ten_minus_5];

            // Assert
            decimal expected = x * 10000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void DecreaseScaleByEightMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.kilo);

            // Act
            decimal actual = d[Scale.ten_minus_5];

            // Assert
            decimal expected = x * 100000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void IncreaseScaleByOneMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Scale.deca];

            // Assert
            decimal expected = x / 10;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void IncreaseScaleByTwoMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Scale.hecto];

            // Assert
            decimal expected = x / 100;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void IncreaseScaleByThreeMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Scale.kilo];

            // Assert
            decimal expected = x / 1000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void IncreaseScaleByFourMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.deci);

            // Act
            decimal actual = d[Scale.kilo];

            // Assert
            decimal expected = x / 10000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void IncreaseScaleByFiveMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.centi);

            // Act
            decimal actual = d[Scale.kilo];

            // Assert
            decimal expected = x / 100000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void IncreaseScaleBySixMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.milli);

            // Act
            decimal actual = d[Scale.kilo];

            // Assert
            decimal expected = x / 1000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void IncreaseScaleBySevenMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.ten_minus_4);

            // Act
            decimal actual = d[Scale.kilo];

            // Assert
            decimal expected = x / 10000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void IncreaseScaleByEightMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.ten_minus_5);

            // Act
            decimal actual = d[Scale.kilo];

            // Assert
            decimal expected = x / 100000000;
            Assert.AreEqual(actual, expected);
        }
        #endregion

        #region Unit Operations
        [TestMethod]
        public void SameUnit()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre];

            // Assert
            decimal expected = x;
            Assert.AreEqual(actual, expected);
        }
        #endregion

        #region Unt and Scale Operations
        [TestMethod]
        public void SameUnitSameScale()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.one];

            // Assert
            decimal expected = x;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitDecreaseScaleByOneMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.deci];

            // Assert
            decimal expected = x * 10;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitDecreaseScaleByTwoMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.centi];

            // Assert
            decimal expected = x * 100;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitDecreaseScaleByThreeMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.milli];

            // Assert
            decimal expected = x * 1000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitDecreaseScaleByFourMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.ten_minus_4];

            // Assert
            decimal expected = x * 10000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitDecreaseScaleByFiveMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.ten_minus_5];

            // Assert
            decimal expected = x * 100000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitDecreaseScaleBySixMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.deca);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.ten_minus_5];

            // Assert
            decimal expected = x * 1000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitDecreaseScaleBySevenMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.hecto);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.ten_minus_5];

            // Assert
            decimal expected = x * 10000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitDecreaseScaleByEightMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.kilo);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.ten_minus_5];

            // Assert
            decimal expected = x * 100000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitIncreaseScaleByOneMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.deca];

            // Assert
            decimal expected = x / 10;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitIncreaseScaleByTwoMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.hecto];

            // Assert
            decimal expected = x / 100;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitIncreaseScaleByThreeMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.one);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.kilo];

            // Assert
            decimal expected = x / 1000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitIncreaseScaleByFourMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.deci);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.kilo];

            // Assert
            decimal expected = x / 10000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitIncreaseScaleByFiveMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.centi);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.kilo];

            // Assert
            decimal expected = x / 100000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitIncreaseScaleBySixMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.milli);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.kilo];

            // Assert
            decimal expected = x / 1000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitIncreaseScaleBySevenMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.ten_minus_4);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.kilo];

            // Assert
            decimal expected = x / 10000000;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void SameUnitIncreaseScaleByEightMagitude()
        {
            // Arrange
            decimal x = 10;
            Distance d = new Distance(10, Distance.Unit.Metre, Scale.ten_minus_5);

            // Act
            decimal actual = d[Distance.Unit.Metre, Scale.kilo];

            // Assert
            decimal expected = x / 100000000;
            Assert.AreEqual(actual, expected);
        }
        #endregion

        [TestMethod]
        public void ChangePrecisionSameUnitChangeScale()
        {
            // Arrange
            decimal x = 0.0123456789M;
            Distance smallD = new Distance(x, Distance.Unit.Metre, Scale.ten_minus_5);
            Distance largeD = new Distance(x, Distance.Unit.Metre, Scale.kilo);

            // Act
            for (int scale = 0; scale <= _scales.GetUpperBound(0); scale++)
            {
                for (int precision = 0; precision <= 10; precision++)
                {
                    decimal actual = smallD[precision, Distance.Unit.Metre, _scales[scale]];

                    // Assert
                    decimal expected = Math.Round(x / (decimal)Math.Pow(10.0, scale), precision, MidpointRounding.AwayFromZero);
                    Assert.AreEqual(actual, expected);
                }
            }

            for (int scale = _scales.GetUpperBound(0); scale >= 0; scale--)
            {
                for (int precision = 0; precision <= 10; precision++)
                {
                    decimal actual = largeD[precision, Distance.Unit.Metre, _scales[scale]];

                    // Assert
                    decimal expected = Math.Round(x * (decimal)Math.Pow(10.0, _scales.GetUpperBound(0) - scale), precision, MidpointRounding.AwayFromZero);
                    Assert.AreEqual(actual, expected);
                }
            }
        }

        [TestMethod]
        public void ConvertScale()
        {
            // Arrange
            decimal x = 0.0123456789M;

            // Act
            for (int scale = 0; scale <= _scales.GetUpperBound(0); scale++)
            {
                Distance d = new Distance(x, Distance.Unit.Metre, Scale.ten_minus_5);
                d.Convert(_scales[scale]);
                decimal actual = d.Value;

                // Assert
                decimal expected = x / (decimal)Math.Pow(10.0, scale);
                Assert.AreEqual(actual, expected);
            }

            for (int scale = _scales.GetUpperBound(0); scale >= 0; scale--)
            {
                Distance d = new Distance(x, Distance.Unit.Metre, Scale.kilo);
                d.Convert(_scales[scale]);
                decimal actual = d[_scales[scale]];

                // Assert
                decimal expected = x * (decimal)Math.Pow(10.0, _scales.GetUpperBound(0) - scale);
                Assert.AreEqual(actual, expected);
            }
        }

        [TestMethod]
        public void ConvertUnitConvertScale()
        {
            // Arrange
            decimal x = 0.0123456789M;

            // Act
            for (int scale = 0; scale <= _scales.GetUpperBound(0); scale++)
            {
                Distance d = new Distance(x, Distance.Unit.Metre, Scale.ten_minus_5);
                d.Convert(Distance.Unit.Metre, _scales[scale]);
                decimal actual = d[_scales[scale]];

                // Assert
                decimal expected = x / (decimal)Math.Pow(10.0, scale);
                Assert.AreEqual(actual, expected);
            }

            for (int scale = _scales.GetUpperBound(0); scale >= 0; scale--)
            {
                Distance d = new Distance(x, Distance.Unit.Metre, Scale.kilo);
                d.Convert(Distance.Unit.Metre, _scales[scale]);
                decimal actual = d[_scales[scale]];

                // Assert
                decimal expected = x * (decimal)Math.Pow(10.0, _scales.GetUpperBound(0) - scale);
                Assert.AreEqual(actual, expected);
            }
        }

        [TestMethod]
        public void UniaryAdd()
        {
            for (int i = -1; i <= 1; i++)
            {
                // Arrange
                Distance d = new Distance(i, Distance.Unit.Metre, Scale.ten_minus_5);

                // Act
                Distance actual = (+d);

                // Assert
                Distance expected = new Distance(i, Distance.Unit.Metre, Scale.ten_minus_5);
                Assert.AreEqual(actual, expected);
            }
        }

        [TestMethod]
        public void UniaryMinus()
        {
            for (int i = -1; i <= 1; i++)
            {
                // Arrange
                Distance d = new Distance(i, Distance.Unit.Metre, Scale.ten_minus_5);

                // Act
                Distance actual = (-d);

                // Assert
                Distance expected = new Distance(-i, Distance.Unit.Metre, Scale.ten_minus_5);
                Assert.AreEqual(actual, expected);
            }
        }

        [TestMethod]
        public void BinaryAdd()
        {
            // Arrange
            decimal x = 0.0123456789M;

            for (int scaleA = 0; scaleA <= _scales.GetUpperBound(0); scaleA++)
            {
                for (int scaleB = 0; scaleB <= _scales.GetUpperBound(0); scaleB++)
                {
                    // Act
                    Distance a = new Distance(x, Distance.Unit.Metre, _scales[scaleA]);
                    Distance b = new Distance(x, Distance.Unit.Metre, _scales[scaleB]);
                    Distance c = a + b;
                    decimal actual = c.Value;


                    // Assert
                    // Convert to lowest scale
                    if (scaleA < scaleB)
                    {
                        b.Convert(Distance.Unit.Metre, _scales[scaleA]);
                    }
                    else if (scaleA > scaleB)
                    {
                        a.Convert(Distance.Unit.Metre, _scales[scaleB]);
                    }
                    decimal expected = a.Value + b.Value;
                    Assert.AreEqual(actual, expected);
                }
            }
        }

        [TestMethod]
        public void BinaryMinus()
        {
            // Arrange
            decimal x = 0.0123456789M;

            for (int scaleA = 0; scaleA <= _scales.GetUpperBound(0); scaleA++)
            {
                for (int scaleB = 0; scaleB <= _scales.GetUpperBound(0); scaleB++)
                {
                    // Act
                    Distance a = new Distance(x, Distance.Unit.Metre, _scales[scaleA]);
                    Distance b = new Distance(x, Distance.Unit.Metre, _scales[scaleB]);
                    Distance c = a - b;
                    decimal actual = c.Value;


                    // Assert
                    // Convert to lowest scale
                    if (scaleA < scaleB)
                    {
                        b.Convert(Distance.Unit.Metre, _scales[scaleA]);
                    }
                    else if (scaleA > scaleB)
                    {
                        a.Convert(Distance.Unit.Metre, _scales[scaleB]);
                    }
                    decimal expected = a.Value - b.Value;
                    Assert.AreEqual(actual, expected);
                }
            }
        }

        [TestMethod]
        public void BinaryTimesDistanceByDistance()
        {
            // Arrange
            decimal x = 0.0123456789M;

            for (int scaleA = 0; scaleA <= _scales.GetUpperBound(0); scaleA++)
            {
                for (int scaleB = 0; scaleB <= _scales.GetUpperBound(0); scaleB++)
                {
                    // Act
                    Distance a = new Distance(x, Distance.Unit.Metre, _scales[scaleA]);
                    Distance b = new Distance(x, Distance.Unit.Metre, _scales[scaleB]);
                    Area c = a * b;
                    decimal actual = c.Value;


                    // Assert
                    // Convert to lowest scale
                    if (scaleA < scaleB)
                    {
                        b.Convert(Distance.Unit.Metre, _scales[scaleA]);
                    }
                    else if (scaleA > scaleB)
                    {
                        a.Convert(Distance.Unit.Metre, _scales[scaleB]);
                    }
                    decimal expected = a.Value * b.Value;
                    Assert.AreEqual(actual, expected);
                }
            }
        }

        [TestMethod]
        public void BinaryTimesDistanceByDecimal()
        {
            // Arrange
            decimal x = 0.0123456789M;

            for (int scaleA = 0; scaleA <= _scales.GetUpperBound(0); scaleA++)
            {
                // Act
                Distance a = new Distance(x, Distance.Unit.Metre, _scales[scaleA]);
                Distance c = a * x;
                decimal actual = c.Value;

                // Assert
                decimal expected = a.Value * x;
                Assert.AreEqual(actual, expected);
            }
        }

        [TestMethod]
        public void BinaryTimesDecimalByDistance()
        {
            // Arrange
            decimal x = 0.0123456789M;

            for (int scaleA = 0; scaleA <= _scales.GetUpperBound(0); scaleA++)
            {
                // Act
                Distance a = new Distance(x, Distance.Unit.Metre, _scales[scaleA]);
                Distance c = x * a;
                decimal actual = c.Value;

                // Assert
                decimal expected = x * a.Value;
                Assert.AreEqual(actual, expected);
            }
        }

        [TestMethod]
        public void BinaryDivideDistanceByDistance()
        {
            // Arrange
            decimal x = 0.0123456789M;

            for (int scaleA = 0; scaleA <= _scales.GetUpperBound(0); scaleA++)
            {
                for (int scaleB = 0; scaleB <= _scales.GetUpperBound(0); scaleB++)
                {
                    // Act
                    Distance a = new Distance(x, Distance.Unit.Metre, _scales[scaleA]);
                    Distance b = new Distance(x, Distance.Unit.Metre, _scales[scaleB]);
                    decimal actual = a / b;

                    // Assert
                    // Convert to lowest scale
                    if (scaleA < scaleB)
                    {
                        b.Convert(Distance.Unit.Metre, _scales[scaleA]);
                    }
                    else if (scaleA > scaleB)
                    {
                        a.Convert(Distance.Unit.Metre, _scales[scaleB]);
                    }
                    decimal expected = a.Value / b.Value;
                    Assert.AreEqual(actual, expected);
                }
            }
        }

        [TestMethod]
        public void BinaryDivideDistanceByDecimal()
        {
            // Arrange
            decimal x = 0.0123456789M;

            for (int scaleA = 0; scaleA <= _scales.GetUpperBound(0); scaleA++)
            {
                // Act
                Distance a = new Distance(x, Distance.Unit.Metre, _scales[scaleA]);
                Distance c = a / x;
                decimal actual = c.Value;

                // Assert
                decimal expected = a.Value / x;
                Assert.AreEqual(actual, expected);
            }
        }
    }
}
