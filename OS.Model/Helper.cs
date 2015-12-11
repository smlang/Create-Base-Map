using System;
using System.Collections.Generic;
using System.Text;

namespace OS.Model
{
    internal static class Helper
    {
        #region Double
        internal static Decimal[] StringToDecimalArray(String value)
        {
            string[] parts = value.Split(' ');
            Decimal[] result = new Decimal[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                result[i] = Decimal.Parse(parts[i]);
            }
            return result;
        }

        internal static String DecimalArrayToString(Decimal[] value)
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            bool first = true;
            foreach (Decimal part in value)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    result.Append(" ");
                }
                result.Append(part);
            }
            return result.ToString();
        }
        #endregion
    }
}
