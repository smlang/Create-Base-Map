using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CreateBaseMap
{
    internal partial class OSGridReferenceUserControl : UserControl
    {
        internal TDPG.GeoCoordConversion.GridReference MinValue { get; set; }
        internal TDPG.GeoCoordConversion.GridReference MaxValue { get; set; }

        internal OSGridReferenceUserControl()
        {
            InitializeComponent();
        }

        internal TDPG.GeoCoordConversion.GridReference Value
        {
            get
            {
                TDPG.GeoCoordConversion.GridReference value;
                Validate(out value);
                return value;
            }
            set
            {
                string letters, eastNumber, northNumber;
                value.GetGridReference(out letters, out eastNumber, out northNumber);
                osGrid1TextBox.Text = letters;
                osGrid2TextBox.Text = eastNumber;
                osGrid3TextBox.Text = northNumber;
            }
        }

        public new bool Validate()
        {
            TDPG.GeoCoordConversion.GridReference value;
            return Validate(out value);
        }

        private bool Validate(out TDPG.GeoCoordConversion.GridReference value)
        {
            bool valid = true;
            value = null;
            errorProvider.Clear();

            if (!TDPG.GeoCoordConversion.GridReference.OSGridLettersRegex.IsMatch(osGrid1TextBox.Text))
            {
                errorProvider.SetError(osGrid1TextBox, "Must be two letters. The first letter H, N, O, S or T. The Second letter A-Z, but not I.");
                valid = false;
            }
            if (!TDPG.GeoCoordConversion.GridReference.OSGridNumbersRegex.IsMatch(osGrid2TextBox.Text))
            {
                errorProvider.SetError(osGrid2TextBox, "Must be between 1 and 5 digits.");
                valid = false;
            }
            if (!TDPG.GeoCoordConversion.GridReference.OSGridNumbersRegex.IsMatch(osGrid3TextBox.Text))
            {
                errorProvider.SetError(osGrid3TextBox, "Must be between 1 and 5 digits.");
                valid = false;
            }
            if (!valid)
            {
                return false;
            }

            value = new TDPG.GeoCoordConversion.GridReference(osGrid1TextBox.Text, osGrid2TextBox.Text, osGrid3TextBox.Text);
            if (MinValue != null)
            {
                if (value.Easting < MinValue.Easting)
                {
                    errorProvider.SetError(this, "The grid reference is too far West.");
                    valid = false;
                }
                if (value.Northing < MinValue.Northing)
                {
                    errorProvider.SetError(this, "The grid reference is too far South.");
                    valid = false;
                }
            }
            if (MaxValue != null)
            {
                if (value.Easting > MaxValue.Easting)
                {
                    errorProvider.SetError(this, "The grid reference is too far East.");
                    valid = false;
                }
                if (value.Northing > MaxValue.Northing)
                {
                    errorProvider.SetError(this, "The grid reference is too far North.");
                    valid = false;
                }
            }

            return valid;
        }
    }
}
