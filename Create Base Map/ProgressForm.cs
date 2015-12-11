using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CreateBaseMap
{
    internal partial class ProgressForm : Form
    {
        internal delegate void BackgroundTask();

        private BackgroundTask _task;

        internal ProgressForm(BackgroundTask task)
        {
            InitializeComponent();

            _task = task;
            downloadProgressBar.Maximum = 100;

            buildBackgroundWorker.RunWorkerAsync();
        }

        private void buildBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _task();
        }

        private void buildBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            downloadProgressBar.Value = e.ProgressPercentage;
            messageLabel.Text = (string)e.UserState;
        }

        private void buildBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = e.Cancelled ? DialogResult.Cancel : DialogResult.OK;
            this.Close();
        }

    }
}
