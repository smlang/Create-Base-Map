using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CreateBaseMap
{
    internal partial class MainForm : Form
    {
        public static readonly Geometry.Distance MaxOcadSize = new Geometry.Distance(4, Geometry.Distance.Unit.Metre, Geometry.Scale.one);

        internal MapSettingsUserControl MapSettings;
        internal AddDataSourcesUserControl AddDataSources;
        internal FinishedUserControl Finished;

        private int _step = 0;
        
        // Managed in Step 1
        internal Ocad.Model.Map OcadMap { get; set; }
        internal TDPG.GeoCoordConversion.GridReference FurthestSWGridReference { get; set; }
        internal TDPG.GeoCoordConversion.GridReference FurthestNEGridReference { get; set; }

        internal MainForm()
        {
            InitializeComponent();

            this.MapSettings = new MapSettingsUserControl(this);
            this.AddDataSources = new AddDataSourcesUserControl(this);
            this.Finished = new FinishedUserControl(this);
            this.SuspendLayout();

            // 
            // Map Settings
            // 
            this.MapSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.MapSettings.Location = new System.Drawing.Point(infoPanel.Location.X - 4, infoPanel.Location.Y + infoPanel.Size.Height + 4);
            this.MapSettings.Name = "Map Settings";
            this.MapSettings.Size = new System.Drawing.Size(infoPanel.Size.Width + 8, Size.Height - MapSettings.Location.Y - 44);
            this.MapSettings.TabIndex = 20;
            this.MapSettings.Enabled = false;
            this.MapSettings.Visible = false;
            this.Controls.Add(this.MapSettings);
            // 
            // Add Data Sources
            // 
            this.AddDataSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AddDataSources.Location = this.MapSettings.Location;
            this.AddDataSources.Name = "Add Data Sources";
            this.AddDataSources.Size = this.MapSettings.Size;
            this.AddDataSources.TabIndex = 10;
            this.AddDataSources.Enabled = false;
            this.AddDataSources.Visible = false;
            this.Controls.Add(this.AddDataSources);
            // 
            // Finished
            // 
            this.Finished.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Finished.Location = this.MapSettings.Location;
            this.Finished.Name = "Finished";
            this.Finished.Size = this.MapSettings.Size;
            this.Finished.TabIndex = 10;
            this.Finished.Enabled = false;
            this.Finished.Visible = false;
            this.Controls.Add(this.Finished);

            this.ResumeLayout(false);

            NextStep(1);
        }

        internal void NextStep(int direction)
        {
            // Disable
            switch (_step)
            {
                case 1:
                    this.MapSettings.Visible = false;
                    this.MapSettings.Enabled = false;
                    break;
                case 2:
                    this.AddDataSources.Visible = false;
                    this.AddDataSources.Enabled = false;
                    break;
                case 3:
                    this.Finished.Visible = false;
                    this.Finished.Enabled = false;
                    break;
                default:
                    break;
            }
            //Enable
            _step += direction;
            switch (_step)
            {
                case 1:
                    // Enable previous step
                    this.MapSettings.Enabled = true;
                    this.MapSettings.Visible = true;
                    this.AcceptButton = this.MapSettings.nextButton;
                    this.CancelButton = null;
                    this.MapSettings.Start();
                    break;
                case 2:
                    this.AddDataSources.Enabled = true;
                    this.AddDataSources.Visible = true;
                    this.AcceptButton = this.AddDataSources.saveButton;
                    this.CancelButton = this.AddDataSources.previousButton;
                    this.AddDataSources.Start();
                    break;
                case 3:
                    this.Finished.Enabled = true;
                    this.Finished.Visible = true;
                    this.AcceptButton = this.Finished.closeButton;
                    this.CancelButton = null;
                    this.Finished.Start();
                    break;
                default:
                    break;
            }
        }
    }
}
