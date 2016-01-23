using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Angle
    {
        public readonly static Angle Zero = new Angle(0M, Unit.Radian);
        public readonly static Angle QuarterCircle = new Angle(0.5M * (decimal)Math.PI, Unit.Radian);
        public readonly static Angle HalfCircle = new Angle((decimal)Math.PI, Unit.Radian);
        public readonly static Angle FullCircle = new Angle(2M * (decimal)Math.PI, Unit.Radian);
        
        public enum Unit
        {
            Radian
        }

        internal protected Decimal _value;
        internal protected Unit _unit;

        public Angle(Decimal value, Unit unit)
        {
            _value = value;
            _unit = unit;
        }

        public Angle(Double value, Unit unit)
        {
            _value = (decimal)value;
            _unit = unit;
        }

        public Angle(Point origin, Point other, Unit unit)
        {
            Distance x = (other.X - origin.X);
            Distance y = (other.Y - origin.Y);

            _value = (decimal)Math.Atan2((double)y[0, Distance.Unit.Metre, Scale.ten_minus_5], (double)x[0, Distance.Unit.Metre, Scale.ten_minus_5]);
            if (_value < 0)
            {
                _value += (decimal)(2 * Math.PI);
            }
            _unit = Unit.Radian;

            if (_unit != unit)
            {
                _value = this[unit];
            }
        }

        #region Unary
        public static Angle operator +(Angle a)
        {
            return new Angle(+a._value, a._unit);
        }

        public static Angle operator -(Angle a)
        {
            return new Angle(-a._value, a._unit);
        }
        #endregion

        #region Binary
        public static Angle operator +(Angle a, Angle b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Convert(a, b, out aValue, out bValue, out unit);
            return new Angle(aValue + bValue, unit);
        }

        public static Angle operator -(Angle a, Angle b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Convert(a, b, out aValue, out bValue, out unit);
            return new Angle(aValue - bValue, unit);
        }

        public static Angle operator *(Angle a, Decimal b)
        {
            return new Angle(a._value * b, a._unit);
        }

        public static Angle operator *(Decimal a, Angle b)
        {
            return new Angle(a * b._value, b._unit);
        }

        public static Decimal operator /(Angle a, Angle b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Convert(a, b, out aValue, out bValue, out unit);
            return aValue / bValue;
        }

        public static Angle operator /(Angle a, Decimal b)
        {
            return new Angle(a._value / b, a._unit);
        }

        public static Angle operator %(Angle a, Angle b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Convert(a, b, out aValue, out bValue, out unit);
            return new Angle(a._value % bValue, unit);
        }
        #endregion

        #region Comparison
        public static bool operator ==(Angle a, Angle b)
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
            Convert(a, b, out aValue, out bValue, out unit);
            return (aValue == bValue);
        }

        public static bool operator !=(Angle a, Angle b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Angle)
            {
                return (this == ((Angle)obj));
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if (_unit == Unit.Radian)
            {
                return _value.GetHashCode();
            }
            return this[Unit.Radian].GetHashCode();
        }

        public static bool operator <(Angle a, Angle b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Convert(a, b, out aValue, out bValue, out unit);
            return aValue < bValue;
        }

        public static bool operator >(Angle a, Angle b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Convert(a, b, out aValue, out bValue, out unit);
            return aValue > bValue;
        }

        public static bool operator <=(Angle a, Angle b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Convert(a, b, out aValue, out bValue, out unit);
            return aValue <= bValue;
        }

        public static bool operator >=(Angle a, Angle b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Convert(a, b, out aValue, out bValue, out unit);
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

        public Decimal this[Int32 precision, Unit unit]
        {
            get
            {
                return Math.Round(this[unit], precision, MidpointRounding.AwayFromZero);
            }
        }

        internal protected static void Convert(Angle a, Angle b, out Decimal aValue, out Decimal bValue, out Unit unit)
        {
            if (a._unit != b._unit)
            {
                throw (new NotImplementedException());
            }

            aValue = a._value;
            bValue = b._value;
            unit = a._unit;
        }

        public void Convert(Unit unit)
        {
            _value = this[unit];
            _unit = unit;
        }
        #endregion

        public Angle Absolute()
        {
            return new Angle(Math.Abs(_value), _unit);
        }

        public int Sign
        {
            get
            {
                return Math.Sign(_value);
            }
        }

        public Angle Reverse()
        {
            return (this + Angle.HalfCircle).Modulus();
        }

        public decimal Cos()
        {
            return (decimal)Math.Cos((double)this[Unit.Radian]);
        }

        public decimal Sin()
        {
            return (decimal)Math.Sin((double)this[Unit.Radian]);
        }

        public Angle Modulus()
        {
            return ((this + Angle.FullCircle) % Angle.FullCircle);
        }
    }
}
