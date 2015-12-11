using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CreateBaseMap
{
    internal partial class MapSettingsUserControl : UserControl
    {
        private readonly MainForm _parent;
        private ProgressForm _progress;
        private string previousOcadFile = null;

        internal MapSettingsUserControl(MainForm parent)
        {
            osGridReferenceUserControl = new OSGridReferenceUserControl();

            InitializeComponent();

            _parent = parent;
        }

        #region Enter User Control
        internal void Start()
        {
            _parent.infoLabel.Text = "(1) Specify the centre of the new master map\n(2) Select a OCAD9 file containing the desired map symbols.";
            this.selectOcadFileButton.Focus();

#if DEBUG
            selectOcadFileDialog.FileName = @"..\..\..\MDOC Street Symbols 15000.ocd";
            LoadOcad(selectOcadFileDialog.FileName);
#endif
        }
        #endregion

        #region Manage Control's UI
        private void loadOcadButton_Click(object sender, EventArgs e)
        {
            if (selectOcadFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadOcad(selectOcadFileDialog.FileName);
            }
        }

        private void LoadOcad(string fileName)
        {
            if (!fileName.Equals(previousOcadFile))
            {
                selectOcadFileTextBox.Text = Path.GetFileName(fileName);

                using (_progress = new ProgressForm(LoadOcadProcess))
                {
                    if (_progress.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }

                if ((_parent.OcadMap.ScaleParameter != null) &&
                    (_parent.OcadMap.ScaleParameter.RealWorldOffsetX != null) &&
                    (_parent.OcadMap.ScaleParameter.RealWorldOffsetY != null) &&
                    ((!_parent.OcadMap.ScaleParameter.RealWorldCoordinateSystem.HasValue) || (_parent.OcadMap.ScaleParameter.RealWorldCoordinateSystem.Value == Ocad.Model.Type.CoordinateSystemType.UK_NationalGrid)))
                {
                    osGridReferenceUserControl.Value = new TDPG.GeoCoordConversion.GridReference(
                        (long)_parent.OcadMap.ScaleParameter.RealWorldOffsetX[0, Geometry.Distance.Unit.Metre, Geometry.Scale.one],
                        (long)_parent.OcadMap.ScaleParameter.RealWorldOffsetY[0, Geometry.Distance.Unit.Metre, Geometry.Scale.one]);
                }

                previousOcadFile = fileName;
            }
        }

        private void LoadOcadProcess()
        {
            _progress.buildBackgroundWorker.ReportProgress(0, "Loading OCAD9 File...");
            _parent.OcadMap = Ocad.Model.Map.Import(selectOcadFileDialog.FileName);
            _parent.OcadMap.Templates.Clear();
            _parent.OcadMap.Objects.Clear();
            _parent.OcadMap.FileInfos.Clear();
            _parent.OcadMap.FileInfos.Add(new Ocad.Model.FileInfo() { Value = String.Format("Created by Steve Lang's Make Master Map at {0}", DateTime.Now) });
        }
        #endregion

        #region Go Backwards
        private void previousButton_Click(object sender, EventArgs e)
        {
            _parent.NextStep(-1);
        }
        #endregion

        #region Go Forwards
        private void nextButton_Click(object sender, EventArgs e)
        {
            if (!CheckForIssues())
            {
                return;
            }

            _parent.OcadMap.ScaleParameter.RealWorldCoordinateSystem = Ocad.Model.Type.CoordinateSystemType.UK_NationalGrid;
            _parent.OcadMap.ScaleParameter.UseRealWorldGrid = true;

            TDPG.GeoCoordConversion.GridReference centre = osGridReferenceUserControl.Value;
            _parent.OcadMap.ScaleParameter.RealWorldOffsetX = new Geometry.Distance(centre.Easting, Geometry.Distance.Unit.Metre, Geometry.Scale.one);
            _parent.OcadMap.ScaleParameter.RealWorldOffsetY = new Geometry.Distance(centre.Northing, Geometry.Distance.Unit.Metre, Geometry.Scale.one);
            _parent.OcadMap.ScaleParameter.RealWorldGridDistance = new Geometry.Distance(1000, Geometry.Distance.Unit.Metre, Geometry.Scale.one);

            long offsetMetre = (long)(MainForm.MaxOcadSize[0, Geometry.Distance.Unit.Metre, Geometry.Scale.one] * (_parent.OcadMap.ScaleParameter.MapScale / 2M));
            _parent.FurthestSWGridReference = new TDPG.GeoCoordConversion.GridReference(centre.Easting - offsetMetre, centre.Northing - offsetMetre);
            _parent.FurthestNEGridReference = new TDPG.GeoCoordConversion.GridReference(centre.Easting + offsetMetre, centre.Northing + offsetMetre);

            _parent.NextStep(1);
        }

        private bool CheckForIssues()
        {
            errorProvider.Clear();
            bool error = false;
            error = !osGridReferenceUserControl.Validate();
            if (String.IsNullOrEmpty(selectOcadFileDialog.FileName))
            {
                errorProvider.SetError(selectOcadFileButton, "A OCAD9 file must be selected.");
                error = true;
            }
            else if (!File.Exists(selectOcadFileDialog.FileName))
            {
                errorProvider.SetError(selectOcadFileButton, "The selected OCAD9 file does not exist.");
                error = true;
            }
            if (error)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
