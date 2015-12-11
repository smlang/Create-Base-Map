using System;
using System.Collections.Generic;
using System.Text;

namespace Measurement
{
    public class Area
    {
        private Decimal _value;
        private Unit _unit;
        private Scale _scale;

        public Decimal Value { get { return this._value; } }

        public enum Unit
        {
            SquareMetre
        }

        public Decimal this[Int32 precision, Unit unit, Scale scale]
        {
            get
            {
                if (unit == _unit)
                {
                    throw (new NotImplementedException());
                }
                if (scale == _scale)
                {
                    return Math.Round(_value, precision, MidpointRounding.AwayFromZero);
                }
                return Math.Round(Convert(unit, scale), precision, MidpointRounding.AwayFromZero);
            }
        }

        public Area(Decimal value, Unit unit, Scale scale)
        {
            _value = value;
            _unit = unit;
            _scale = scale;
        }

        private static void Convert(Area a, Area b, out Decimal aValue, out Decimal bValue, out Unit unit, out Scale scale)
        {
            unit = a._unit;

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
                bValue = b.Convert(unit, a._scale);
                unit = a._unit;
                scale = a._scale;
            }
            else
            {
                aValue = a.Convert(unit, b._scale);
                bValue = b._value;
                unit = b._unit;
                scale = b._scale;
            }
        }

        private Decimal Convert(Unit unit, Scale scale)
        {
            if (unit != _unit) { throw (new NotImplementedException()); }

            int change = (int)scale - (int)_scale;
            if (change == 0) { return _value; }
            Decimal value = _value;
            if (change < 0)
            {
                for (int i = change; change != 0; i++)
                {
                    value = value / 10M;
                }
            }
            else
            {
                for (int i = change; change != 0; i--)
                {
                    value = value * 10M;
                }
            }
            return value;
        }

        public static Area operator +(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            Decimal value = a._value + b._value;
            return new Area(value, unit, scale);
        }

        public static Area operator -(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            Decimal value = a._value - b._value;
            return new Area(value, unit, scale);
        }

        public static Area operator -(Area a)
        {
            return new Area(-a._value, a._unit, a._scale);
        }

        public static Area operator *(Area a, Decimal b)
        {
            return new Area(a._value * b, a._unit, a._scale);
        }

        public static Area operator *(Decimal a, Area b)
        {
            return new Area(a * b._value, b._unit, b._scale);
        }

        public static Decimal operator /(Area a, Area b)
        {
            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return a._value / b._value;
        }

        public static Boolean operator ==(Area a, Area b)
        {
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
            return (a._value == b._value);
        }

        public static Boolean operator !=(Area a, Area b)
        {
            return !(a == b);
        }

        public static Boolean operator <(Area a, Area b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return false;
            }

            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return (a._value < b._value);
        }

        public static Boolean operator <=(Area a, Area b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return (a._value <= b._value);
        }

        public static Boolean operator >(Area a, Area b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return false;
            }

            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return (a._value > b._value);
        }

        public static Boolean operator >=(Area a, Area b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            Decimal aValue;
            Decimal bValue;
            Unit unit;
            Scale scale;
            Convert(a, b, out aValue, out bValue, out unit, out scale);
            return (a._value >= b._value);
        }
    }
}
