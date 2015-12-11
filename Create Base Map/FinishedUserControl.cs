using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace CreateBaseMap
{
    internal partial class FinishedUserControl : UserControl
    {
        private readonly MainForm _parent;

        internal FinishedUserControl(MainForm parent)
        {
            InitializeComponent();

            _parent = parent;
        }

        #region Enter User Control
        internal void Start()
        {
            _parent.infoLabel.Text = String.Format("The new OCAD9 file '{0}' has been created.\nClick on link below to open the new file.", _parent.OcadMap.FileName.Value);
            linkLabel.Text = Path.GetFileName(_parent.OcadMap.FileName.Value);
            linkLabel.Focus();
        }
        #endregion

        #region Manage Control's UI
        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_parent.OcadMap.FileName.Value);
        }
        #endregion

        #region Go Backwards
        #endregion

        #region Go Forwards
        private void completedButton_Click(object sender, EventArgs e)
        {
            _parent.Close();
        }
        #endregion
    }
}
