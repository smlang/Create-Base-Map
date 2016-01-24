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

        public decimal Value;
        public Unit _unit;
        public Scale _scale;

        public Distance(decimal value, Unit unit, Scale scale)
        {
            Value = value;
            _unit = unit;
            _scale = scale;
        }

        public Distance(Distance source)
        {
            Value = source.Value;
            _unit = source._unit;
            _scale = source._scale;
        }

        #region Unary
        public static Distance operator +(Distance a)
        {
            return new Distance(+a.Value, a._unit, a._scale);
        }

        public static Distance operator -(Distance a)
        {
            return new Distance(-a.Value, a._unit, a._scale);
        }
        #endregion

        #region Binary
        public static Distance operator +(Distance a, Distance b)
        {
            decimal aValue;
            decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return new Distance(aValue + bValue, unit, scale);
        }

        public static Distance operator -(Distance a, Distance b)
        {
            decimal aValue;
            decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return new Distance(aValue - bValue, unit, scale);
        }

        public static Area operator *(Distance a, Distance b)
        {
            decimal aValue;
            decimal bValue;
            Unit unitSquared;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unitSquared, out scale);
            return new Area(aValue * bValue, unitSquared, scale);
        }

        public static Distance operator *(Distance a, decimal b)
        {
            return new Distance(a.Value * b, a._unit, a._scale);
        }

        public static Distance operator *(decimal a, Distance b)
        {
            return new Distance(a * b.Value, b._unit, b._scale);
        }

        public static decimal operator /(Distance a, Distance b)
        {
            decimal aValue;
            decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue / bValue;
        }

        public static Distance operator /(Distance a, decimal b)
        {
            return new Distance(a.Value / b, a._unit, a._scale);
        }

        public static Distance operator %(Distance a, Distance b)
        {
            decimal aValue;
            decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return new Distance(aValue % bValue, unit, scale);
        }

        public static Distance operator %(Distance a, decimal b)
        {
            return new Distance(a.Value % b, a._unit, a._scale);
        }
        #endregion

        #region Comparison
        public static bool operator ==(Distance a, Distance b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            decimal aValue;
            decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return (aValue == bValue);
        }

        public static bool operator !=(Distance a, Distance b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Distance)
            {
                return (this == ((Distance)obj));
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if ((_unit == Unit.Metre) && (_scale == Scale.ten_minus_5))
            {
                return Value.GetHashCode();
            }
            return this[Unit.Metre, Scale.ten_minus_5].GetHashCode();
        }

        public static bool operator <(Distance a, Distance b)
        {
            if ((a == null) || (b == null))
            {
                return false;
            }

            decimal aValue;
            decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue < bValue;
        }

        public static bool operator >(Distance a, Distance b)
        {
            if ((a == null) || (b == null))
            {
                return false;
            }

            decimal aValue;
            decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue > bValue;
        }

        public static bool operator <=(Distance a, Distance b)
        {
            if ((a == null) || (b == null))
            {
                return false;
            }

            decimal aValue;
            decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue <= bValue;
        }

        public static bool operator >=(Distance a, Distance b)
        {
            if ((a == null) || (b == null))
            {
                return false;
            }

            decimal aValue;
            decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return aValue >= bValue;
        }
        #endregion

        #region Index
        public decimal this[Unit unit]
        {
            get
            {
                if (unit != _unit)
                {
                    throw (new NotImplementedException());
                }
                return Value;
            }
        }

        public decimal this[Scale scale]
        {
            get
            {
                int change = (int)_scale - (int)scale;

                decimal value = Value;
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

        public decimal this[Unit unit, Scale scale]
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

        public decimal this[Int32 precision, Unit unit, Scale scale]
        {
            get
            {
                return Math.Round(this[unit, scale], precision, MidpointRounding.AwayFromZero);
            }
        }

        internal protected static void Convert(Distance a, Distance b, out decimal aValue, out decimal bValue, out Unit unit, out Scale scale)
        {
            if (a._unit != b._unit)
            {
                throw (new NotImplementedException());
            }

            if (a._scale == b._scale)
            {
                aValue = a.Value;
                bValue = b.Value;
                unit = a._unit;
                scale = a._scale;
            }
            else if (a._scale <= b._scale)
            {
                aValue = a.Value;
                bValue = b[a._unit, a._scale];
                unit = a._unit;
                scale = a._scale;
            }
            else
            {
                aValue = a[b._unit, b._scale];
                bValue = b.Value;
                unit = b._unit;
                scale = b._scale;
            }
        }

        public void Convert(Scale scale)
        {
            Value = this[scale];
            _scale = scale;
        }

        public void Convert(Unit unit, Scale scale)
        {
            Value = this[unit, scale];
            _unit = unit;
            _scale = scale;
        }
        #endregion

        #region Near Comparison 
        public Boolean IsZero(Distance epsilon)
        {
            Distance x = this.Absolute() - epsilon;
            return (x.Value < 0);
        }

        public Boolean IsPositive(Distance epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return false;
            }
            return (this.Value > 0);
        }

        public Boolean IsNegative(Distance epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return false;
            }
            return (this.Value < 0);
        }

        // Positive or Zero
        public Boolean IsNotNegative(Distance epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return true;
            }
            return (this.Value > 0);
        }

        // Negative or Zero
        public Boolean IsNotPositive(Distance epsilon)
        {
            if (this.IsZero(epsilon))
            {
                return true;
            }
            return (this.Value < 0);
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
            return (difference.Value < 0);
        }

        public Boolean IsGreaterThan(Distance a, Distance epsilon)
        {
            Distance difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return false;
            }
            return (difference.Value > 0);
        }

        // Less Than or Equal
        public Boolean IsNotGreaterThan(Distance a, Distance epsilon)
        {
            Distance difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return true;
            }
            return (difference.Value <= 0);
        }

        // Greater Than or Equal
        public Boolean IsNotLessThan(Distance a, Distance epsilon)
        {
            Distance difference = this - a;
            if (difference.IsZero(epsilon))
            {
                return true;
            }
            return (difference.Value >= 0);
        }
        #endregion

        public Distance Absolute()
        {
            return new Distance(Math.Abs(Value), _unit, _scale);
        }

        public int Sign
        {
            get
            {
                return Math.Sign(Value);
            }
        }

        public static Distance Max(Distance a, params Distance[] list)
        {
            Distance max = a;
            foreach (Distance b in list)
            {
                if (b > max)
                {
                    max = b;
                }
            }
            return max;
        }

        public static Distance Min(Distance a, params Distance[] list)
        {
            Distance min = a;
            foreach (Distance b in list)
            {
                if (b < min)
                {
                    min = b;
                }
            }
            return min;
        }
    }
}
