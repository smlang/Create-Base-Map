using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MakeGeoRefBaseMap
{
    public partial class ProgressForm : Form
    {
        public string ErrorMessage { get; set; }

        private int west;
        private int south;
        private int eastWestLengthKm;
        private int northSouthLengthKm;
        private int ppkm;
        private string filePath;

        public ProgressForm(int west, int south, int eastWestLengthKm, int northSouthLengthKm, int ppkm, string filePath)
        {
            InitializeComponent();

            this.Icon = MakeGeoRefBaseMap.Properties.Resources.ApplicationIcon;

            downloadProgressBar.Maximum = 100;

            this.west = west;
            this.south = south;
            this.eastWestLengthKm = eastWestLengthKm;
            this.northSouthLengthKm = northSouthLengthKm;
            this.ppkm = ppkm;
            this.filePath = filePath;

            buildBackgroundWorker.RunWorkerAsync();
        }

        private void buildBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string errorMessage;
            GeoRefBaseMap.Build(west, south, eastWestLengthKm, northSouthLengthKm, ppkm, filePath, this.buildBackgroundWorker, out errorMessage);
            ErrorMessage = errorMessage;
        }

        private void buildBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            downloadProgressBar.Value = e.ProgressPercentage;
            messageLabel.Text = (string)e.UserState;
        }

        private void buildBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = String.IsNullOrEmpty(ErrorMessage) ? DialogResult.OK : DialogResult.Abort;
            this.Close();
        }

    }
}
