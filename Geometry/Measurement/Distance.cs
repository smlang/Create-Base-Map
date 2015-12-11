using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Distance
    {
        public readonly static Distance Zero = new Distance(0, Unit.Metre, Scale.ten_minus_5); 

        public enum Unit
        {
            Metre
        }

        private Decimal _value;
        private Unit _unit;
        private Scale _scale;

        public Distance(Decimal value, Unit unit, Scale scale)
        {
            _value = value;
            _unit = unit;
            _scale = scale;
        }

        #region Unary
        public static Distance operator +(Distance a)
        {
            return new Distance(+a._value, a._unit, a._scale);
        }

        public static Distance operator -(Distance a)
        {
            return new Distance(-a._value, a._unit, a._scale);
        }
        #endregion

        #region Binary
        public static Distance operator +(Distance a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return new Distance(aValue + bValue, unit, scale);
        }

        public static Distance operator -(Distance a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return new Distance(aValue - bValue, unit, scale);
        }

        public static Area operator *(Distance a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return new Area(aValue * bValue, unitSquared, scale);
        }

        public static Distance operator *(Distance a, Decimal b)
        {
            return new Distance(a._value * b, a._unit, a._scale);
        }

        public static Distance operator *(Decimal a, Distance b)
        {
            return new Distance(a * b._value, b._unit, b._scale);
        }

        public static Decimal operator /(Distance a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue / bValue;
        }

        public static Distance operator /(Distance a, Decimal b)
        {
            return new Distance(a._value / b, a._unit, a._scale);
        }

        public static Distance operator %(Distance a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return new Distance(aValue % bValue, unit, scale);
        }
        #endregion

        #region Comparison
        public static bool operator ==(Distance a, Distance b)
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
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return (aValue == bValue);
        }

        public static bool operator !=(Distance a, Distance b)
        {
            return !(a == b);
        }

        public static bool operator <(Distance a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue < bValue;
        }

        public static bool operator >(Distance a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue > bValue;
        }

        public static bool operator <=(Distance a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue <= bValue;
        }

        public static bool operator >=(Distance a, Distance b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue >= bValue;
        }
        #endregion

        #region Index
        public Decimal this[Unit unit]
        {
            get
            {
                if (unit != _unit)
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
                        value = value / 10M;
                    }
                }
                else if (change > 0)
                {
                    for (int i = 0; i != change; i++)
                    {
                        value = value * 10M;
                    }
                }

                return value;
            }
        }

        public Decimal this[Unit unit, Scale scale]
        {
            get
            {
                if (unit != _unit)
                {
                    throw (new NotImplementedException());
                }

                return this[scale];
            }
        }

        public Decimal this[Int32 precision, Unit unit, Scale scale]
        {
            get
            {
                return Math.Round(this[unit, scale], precision, MidpointRounding.AwayFromZero);
            }
        }

        internal protected static void Convert(Distance a, Distance b, out Decimal aValue, out Decimal bValue, out Unit unit, out Scale scale)
        {
            if (a._unit != b._unit)
            {
                throw (new NotImplementedException());
            }

            if (a._scale == b._scale)
            {
                aValue = a._value;
                bValue = b._value;
                unit = a._unit;
                scale = a._scale;
            }
            else if (a._scale <= b._scale)
            {
                aValue = a._value;
                bValue = b[a._unit, a._scale];
                unit = a._unit;
                scale = a._scale;
            }
            else
            {
                aValue = a[b._unit, b._scale];
                bValue = b._value;
                unit = b._unit;
                scale = b._scale;
            }
        }

        public void Convert(Scale scale)
        {
            _value = this[_unit, scale];
            _scale = scale;
        }

        public void Convert(Unit unit, Scale scale)
        {
            _value = this[unit, scale];
            _unit = unit;
            _scale = scale;
        }
        #endregion

        #region Near Comparison 
        public Boolean IsZero(Distance epsilon)
        {
            Distance x = this.Absolute() - epsilon;
            return (x._value < 0);
        }

        public Boolean IsPositive(Distance epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return false;
            }
            return (this._value > 0);
        }

        public Boolean IsNegative(Distance epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return false;
            }
            return (this._value < 0);
        }

        // Positive or Zero
        public Boolean IsNotNegative(Distance epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return true;
            }
            return (this._value > 0);
        }

        // Negative or Zero
        public Boolean IsNotPositive(Distance epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return true;
            }
            return (this._value < 0);
        }

        public Boolean IsEqualTo(Distance a, Distance epsilon)
        {
            return (this - a).IsZero(epsilon);
        }

        public Boolean IsLessThan(Distance a, Distance epsilon)
        {
            Distance difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return false;
            }
            return (difference._value < 0);
        }

        public Boolean IsGreaterThan(Distance a, Distance epsilon)
        {
            Distance difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return false;
            }
            return (difference._value > 0);
        }

        // Less Than or Equal
        public Boolean IsNotGreaterThan(Distance a, Distance epsilon)
        {
            Distance difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return true;
            }
            return (difference._value <= 0);
        }

        // Greater Than or Equal
        public Boolean IsNotLessThan(Distance a, Distance epsilon)
        {
            Distance difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return true;
            }
            return (difference._value >= 0);
        }
        #endregion

        public Distance Absolute()
        {
            return new Distance(Math.Abs(_value), _unit, _scale);
        }

        public int Sign
        {
            get
            {
                return Math.Sign(_value);
            }
        }
    }
}
