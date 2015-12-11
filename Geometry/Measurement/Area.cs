using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Area
    {
        public readonly static Area Zero = new Area(0, Distance.Unit.Metre, Scale.ten_minus_5);

        private Decimal _value;
        private Distance.Unit _unitSquared;
        private Scale _scale;

        public Area(Decimal value, Distance.Unit unitSquared, Scale scale)
        {
            _value = value;
            _unitSquared = unitSquared;
            _scale = scale;
        }

        #region Unary
        public static Area operator +(Area a)
        {
            return new Area(+a._value, a._unitSquared, a._scale);
        }

        public static Area operator -(Area a)
        {
            return new Area(-a._value, a._unitSquared, a._scale);
        }
        #endregion

        #region Binary
        public static Area operator +(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Distance.Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return new Area(aValue + bValue, unitSquared, scale);
        }

        public static Area operator -(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Distance.Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return new Area(aValue - bValue, unitSquared, scale);
        }

        public static Area operator *(Area a, Decimal b)
        {
            return new Area(a._value * b, a._unitSquared, a._scale);
        }

        public static Area operator *(Decimal a, Area b)
        {
            return new Area(a * b._value, b._unitSquared, b._scale);
        }

        public static Decimal operator /(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Distance.Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return aValue / bValue;
        }

        public static Distance operator /(Area a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Distance.Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return new Distance(aValue / bValue, unit, scale);
        }

        public static Area operator /(Area a, Decimal b)
        {
            return new Area(a._value / b, a._unitSquared, a._scale);
        }

        public static Area operator %(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Distance.Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return new Area(aValue % bValue, unitSquared, scale);
        }
        #endregion

        #region Comparison
        public static bool operator ==(Area a, Area b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            Decimal aValue;
            Decimal bValue;
            Distance.Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return (aValue == bValue);
        }

        public static bool operator !=(Area a, Area b)
        {
            return !(a == b);
        }

        public static bool operator <(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Distance.Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return aValue < bValue;
        }

        public static bool operator >(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Distance.Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return aValue > bValue;
        }

        public static bool operator <=(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Distance.Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return aValue <= bValue;
        }

        public static bool operator >=(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Distance.Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return aValue >= bValue;
        }
        #endregion

        #region Index
        public Decimal this[Distance.Unit unitSquared]
        {
            get
            {
                if (unitSquared != _unitSquared)
                {
                    throw (new NotImplementedException());
                }
                return _value;
            }
        }

        public Decimal this[Scale scale]
        {
            get
            {
                int change = (int)_scale - (int)scale;

                decimal value = _value;
                if (change < 0)
                {
                    for (int i = 0; i != change; i--)
                    {
                        value = value / 100M;
                    }
                }
                else if (change > 0)
                {
                    for (int i = 0; i != change; i++)
                    {
                        value = value * 100M;
                    }
                }

                return value;
            }
        }

        public Decimal this[Distance.Unit unitSquared, Scale scale]
        {
            get
            {
                if (unitSquared != _unitSquared)
                {
                    throw (new NotImplementedException());
                }

                return this[scale];
            }
        }

        public Decimal this[Int32 precision, Distance.Unit unitSquared, Scale scale]
        {
            get
            {
                return Math.Round(this[unitSquared, scale], precision, MidpointRounding.AwayFromZero);
            }
        }

        internal protected static void Convert(Area a, Area b, out Decimal aValue, out Decimal bValue, out Distance.Unit unitSquared, out Scale scale)
        {
            if (a._unitSquared != b._unitSquared)
            {
                throw (new NotImplementedException());
            }

            if (a._scale == b._scale)
            {
                aValue = a._value;
                bValue = b._value;
                unitSquared = a._unitSquared;
                scale = a._scale;
            }
            else if (a._scale <= b._scale)
            {
                aValue = a._value;
                bValue = b[a._unitSquared, a._scale];
                unitSquared = a._unitSquared;
                scale = a._scale;
            }
            else
            {
                aValue = a[b._unitSquared, b._scale];
                bValue = b._value;
                unitSquared = b._unitSquared;
                scale = b._scale;
            }
        }

        internal protected static void Convert(Area a, Distance b, out Decimal aValue, out Decimal bValue, out Distance.Unit unitSquared, out Scale scale)
        {
            aValue = a._value;
            bValue = b[a._unitSquared, a._scale];
            unitSquared = a._unitSquared;
            scale = a._scale;
        }

        public void Convert(Scale scale)
        {
            _value = this[_unitSquared, scale];
            _scale = scale;
        }

        public void Convert(Distance.Unit unitSquared, Scale scale)
        {
            _value = this[unitSquared, scale];
            _unitSquared = unitSquared;
            _scale = scale;
        }
        #endregion

        #region Near Comparison
        public Boolean IsZero(Distance epsilon)
        {
            return IsZero(epsilon * epsilon);
        }

        public Boolean IsPositive(Distance epsilon)
        {
            return IsPositive(epsilon * epsilon);
        }

        public Boolean IsNegative(Distance epsilon)
        {
            return IsNegative(epsilon * epsilon);
        }

        public Boolean IsNotNegative(Distance epsilon)
        {
            return IsNotNegative(epsilon * epsilon);
        }

        public Boolean IsNotPositive(Distance epsilon)
        {
            return IsNotPositive(epsilon * epsilon);
        }

        public Boolean IsEqualTo(Area a, Distance epsilon)
        {
            return (this - a).IsZero(epsilon);
        }

        public Boolean IsLessThan(Area a, Distance epsilon)
        {
            return IsLessThan(a, epsilon * epsilon);
        }

        public Boolean IsGreaterThan(Area a, Distance epsilon)
        {
            return IsGreaterThan(a, epsilon * epsilon);
        }

        // Less Than or Equal
        public Boolean IsNotGreaterThan(Area a, Distance epsilon)
        {
            return IsNotGreaterThan(a, epsilon * epsilon);
        }

        // Greater Than or Equal
        public Boolean IsNotLessThan(Area a, Distance epsilon)
        {
            return IsNotLessThan(a, epsilon * epsilon);
        }


        private Boolean IsZero(Area epsilon)
        {
            Area x = this.Absolute() - epsilon;
            return (x._value < 0);
        }

        private Boolean IsPositive(Area epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return false;
            }
            return (this._value > 0);
        }

        private Boolean IsNegative(Area epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return false;
            }
            return (this._value < 0);
        }

        // Positive or Zero
        private Boolean IsNotNegative(Area epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return true;
            }
            return (this._value > 0);
        }

        // Negative or Zero
        private Boolean IsNotPositive(Area epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return true;
            }
            return (this._value < 0);
        }

        private Boolean IsEqualTo(Area a, Area epsilon)
        {
            return (this - a).IsZero(epsilon);
        }

        private Boolean IsLessThan(Area a, Area epsilon)
        {
            Area difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return false;
            }
            return (difference._value < 0);
        }

        private Boolean IsGreaterThan(Area a, Area epsilon)
        {
            Area difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return false;
            }
            return (difference._value > 0);
        }

        // Less Than or Equal
        private Boolean IsNotGreaterThan(Area a, Area epsilon)
        {
            Area difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return true;
            }
            return (difference._value <= 0);
        }

        // Greater Than or Equal
        private Boolean IsNotLessThan(Area a, Area epsilon)
        {
            Area difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return true;
            }
            return (difference._value >= 0);
        }
        #endregion

        public Area Absolute()
        {
            return new Area(Math.Abs(_value), _unitSquared, _scale);
        }

        public int Sign
        {
            get
            {
                return Math.Sign(_value);
            }
        }

        public Distance SquareRoot()
        {
            return new Distance((Decimal)Math.Sqrt((double)_value), _unitSquared, _scale);
        }
    }
}
