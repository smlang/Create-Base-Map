using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MakeGeoRefBaseMap
{
    public partial class MakeGeoRefBaseMapForm : Form
    {
        private static readonly Regex lengthRegex = new Regex("^[1-6]$");
        private static readonly Regex ppkmRegex = new Regex(@"^(1\d{3})|([1-9]\d{2})$");

        public MakeGeoRefBaseMapForm()
        {
            InitializeComponent();

            this.Icon = MakeGeoRefBaseMap.Properties.Resources.ApplicationIcon;
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            int west;
            int south;
            bool valid = OSGridReference.Validate(this.errorProvider1, osGrid1TextBox, osGrid2TextBox, osGrid3TextBox, out west, out south);

            string eastWestLengthKmString = lengthComboBox.Text == null ? (string)lengthComboBox.SelectedItem : lengthComboBox.Text;
            if ((eastWestLengthKmString == null) || (!lengthRegex.IsMatch(eastWestLengthKmString)))
            {
                errorProvider1.SetError(lengthComboBox, "Selected length must be between 1 and 6 km.");
                valid = false;
            }

            string northSouthLengthKmString = widthComboBox.Text == null ? (string)widthComboBox.SelectedItem : widthComboBox.Text;
            if ((northSouthLengthKmString == null) || (!lengthRegex.IsMatch(northSouthLengthKmString)))
            {
                errorProvider1.SetError(widthComboBox, "Selected width must be between 1 and 6 km.");
                valid = false;
            }

            string ppkmString = ppkmTextBox.Text;
            if (!ppkmRegex.IsMatch(ppkmString))
            {
                errorProvider1.SetError(ppkmTextBox, "Pixels per km must be between 100 and 1000");
                valid = false;
            }

            if (!valid)
            {
                return;
            }

            int eastWestLengthKm = Int32.Parse(eastWestLengthKmString);
            int northSouthLengthKm = Int32.Parse(northSouthLengthKmString);
            int ppkm = Int32.Parse(ppkmString);

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (ProgressForm progress = new ProgressForm(west, south, eastWestLengthKm, northSouthLengthKm, ppkm, saveFileDialog.FileName))
                {
                    if (progress.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show(progress.ErrorMessage, "Problem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show(String.Format("The base map has been successfully created at {0}.\n\nClick on Yes to view.", saveFileDialog.FileName), "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            Process.Start(saveFileDialog.FileName);
                        }
                    }
                }
            }
        }

        private void ClearError(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            using (AboutBox about = new AboutBox())
            {
                about.ShowDialog();
            }
        }
    }
}
