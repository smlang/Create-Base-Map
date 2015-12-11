using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MakeGeoRefBaseMap
{
    internal class OSGridReference
    {
        private static readonly Regex osGridLettersRegex = new Regex(@"^[NOST][A-HJ-Z]$");
        private static readonly Regex osGridNumbersRegex = new Regex(@"^\d{2}$");

        internal static bool Validate(ErrorProvider error, TextBox osGrid1TextBox, TextBox osGrid2TextBox, TextBox osGrid3TextBox, out int Easting, out int Northing)
        {
            #region Validate Grid Reference
            bool valid = true;
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (!osGridLettersRegex.IsMatch(osGrid1TextBox.Text))
            {
                errors.Add("osGrid1TextBox", "First part of Grid Reference must be two letters.  First letter N, O, S or T.  Second letter A-Z, but not I.");
                valid = false;
            }

            if (!osGridNumbersRegex.IsMatch(osGrid2TextBox.Text))
            {
                errors.Add("osGrid2TextBox", "Second part of Grid Reference must be one or more digits.");
                valid = false;
            }

            if (!osGridNumbersRegex.IsMatch(osGrid3TextBox.Text))
            {
                errors.Add("osGrid3TextBox", "Third part of Grid Reference must be one or more digits.");
                valid = false;
            }

            if (!valid)
            {
                Easting = 0;
                Northing = 0;
                return false;
            }
            #endregion

            TDPG.GeoCoordConversion.GridReference osRef = new TDPG.GeoCoordConversion.GridReference(osGrid1TextBox.Text, osGrid2TextBox.Text, osGrid3TextBox.Text);
            Easting = (int)osRef.Easting;
            Northing = (int)osRef.Northing;

            return true;
        }
    }
}

