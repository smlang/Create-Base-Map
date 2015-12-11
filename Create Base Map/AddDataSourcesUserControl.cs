using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using Ocad.Import;

namespace CreateBaseMap
{
    internal partial class AddDataSourcesUserControl : UserControl
    {
        private readonly MainForm _parent;
        private ProgressForm _progress;

        internal AddDataSourcesUserControl(MainForm parent)
        {
            InitializeComponent();

            _parent = parent;
        }

        #region Enter User Control
        internal void Start()
        {
            _parent.infoLabel.Text = "(1) Add OSM maps, (2) Add OS maps, (3) Add OCAD maps, (4) Add GeoTiff images, (5) Download and add OSM maps, and (6) Download and add Roadworks GeoTiff images.\n";
            FileListBox.Focus();
        }
        #endregion

        #region Manage Control's UI
        internal class SelectedFile
        {
            internal string FilePath { get; private set; }

            internal SelectedFile(string filePath)
            {
                FilePath = filePath;
            }

            public override string ToString()
            {
                string fileType;
                string filename = Path.GetFileNameWithoutExtension(FilePath);
                string directoryPath = Path.GetDirectoryName(FilePath);
                switch (Path.GetExtension(FilePath))
                {
                    case ".xml":
                        fileType = "OS";
                        break;
                    case ".ocd":
                        fileType = "OCAD";
                        break;
                    case ".osm":
                        fileType = "OSM";
                        break;
                    case ".tif":
                        fileType = "GeoTiff";
                        break;
                    default:
                        fileType = "?";
                        break;
                }

                return String.Format("{0}: {1} ({2})", fileType, filename, directoryPath);
            }
        }

        private void addLocalFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string selectedFileName in selectFileDialog.FileNames)
                {
                    AddFileListBoxItem(selectedFileName);
                }
            }
        }

        private void downloadAddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (DownloadForm downloadForm = new DownloadForm(_parent))
            {
                if (downloadForm.ShowDialog() == DialogResult.OK)
                {
                    foreach (string selectedFileName in downloadForm.FileList)
                    {
                        AddFileListBoxItem(selectedFileName);
                    }
                }
            }
        }

        private void AddFileListBoxItem(string selectedFileName)
        {
            bool found = false;
            foreach (object item in FileListBox.Items)
            {
                if (selectedFileName.Equals(((SelectedFile)item).FilePath))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                saveButton.Enabled = true;
                FileListBox.Items.Add(new SelectedFile(selectedFileName));
            }
        }

        private void removeFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<object> removeItems = new List<object>();
            foreach (object selectedItem in FileListBox.SelectedItems)
            {
                removeItems.Add(selectedItem);
            }
            foreach (object removeItem in removeItems)
            {
                FileListBox.Items.Remove(removeItem);
            }
            if (FileListBox.Items.Count == 0)
            {
                saveButton.Enabled = false;
            }
        }

        private void fileListContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            removeFilesToolStripMenuItem.Enabled = (FileListBox.SelectedItems.Count != 0);
        }
        #endregion

        #region Go Backwards
        private void previousButton_MouseClick(object sender, MouseEventArgs e)
        {
            errorProvider.Clear();
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

            if (saveOcadFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (_progress = new ProgressForm(NextProcess))
            {
                if (_progress.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            _parent.NextStep(1);
        }

        private bool CheckForIssues()
        {
            errorProvider.Clear();
            bool error = false;

            // Do checks here

            if (error)
            {
                return false;
            }

            return true;
        }

        private void NextProcess()
        {
            int count = 1;
            int maxCount = FileListBox.Items.Count + 14;

            foreach (object item in FileListBox.Items)
            {
                string dataSourceFilePath = ((SelectedFile)item).FilePath;

                switch (Path.GetExtension(dataSourceFilePath))
                {
                    case ".tif":

                        _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, String.Format("Adding GeoTiff image {0}...", Path.GetFileNameWithoutExtension(dataSourceFilePath)));

                        int pixelWidth, pixelHeight;
                        double west, north, metersPerPixel;
                        Tiff.GeoTag.Read(dataSourceFilePath, out pixelWidth, out pixelHeight, out west, out north, out metersPerPixel);

                        Ocad.Model.Template template = new Ocad.Model.Template();
                        template.FileName = dataSourceFilePath;

                        template.VisibleInNormalMode = true;
                        template.VisibleInDraftMode = true;

                        Geometry.Distance templateRealWorldPixelLength = new Geometry.Distance((decimal)metersPerPixel, Geometry.Distance.Unit.Metre, Geometry.Scale.one);
                        template.PixelSizeX = templateRealWorldPixelLength / _parent.OcadMap.ScaleParameter.MapScale;
                        template.PixelSizeY = template.PixelSizeX;

                        // Calculate Centre from NW corner
                        Geometry.Distance templateRealWorldOffsetX = new Geometry.Distance((decimal)(west + (pixelWidth * metersPerPixel / 2)), Geometry.Distance.Unit.Metre, Geometry.Scale.one);
                        template.OffsetCentreX = (templateRealWorldOffsetX - _parent.OcadMap.ScaleParameter.RealWorldOffsetX) / _parent.OcadMap.ScaleParameter.MapScale;

                        Geometry.Distance templateRealWorldOffsetY = new Geometry.Distance((decimal)(north - (pixelWidth * metersPerPixel / 2)), Geometry.Distance.Unit.Metre, Geometry.Scale.one);
                        template.OffsetCentreY = (templateRealWorldOffsetY - _parent.OcadMap.ScaleParameter.RealWorldOffsetY) / _parent.OcadMap.ScaleParameter.MapScale;

                        _parent.OcadMap.Templates.Add(template);
                        break;

                    case ".osm":

                        _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, String.Format("Adding OSM map {0}...", Path.GetFileNameWithoutExtension(dataSourceFilePath)));

                        Osm.Model.osm osmModel = Osm.Model.osm.Import(dataSourceFilePath);
                        osmModel.CopyTo(_parent.OcadMap);
                        break;

                    case ".xml":

                        _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, String.Format("Adding OS map {0}...", Path.GetFileNameWithoutExtension(dataSourceFilePath)));

                        OS.Model.FeatureCollectionType osModel = OS.Model.FeatureCollectionType.Import(dataSourceFilePath);
                        osModel.CopyTo(_parent.OcadMap);
                        break;

                    case ".ocd":

                        _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, String.Format("Adding OCAD map {0}...", Path.GetFileNameWithoutExtension(dataSourceFilePath)));

                        Ocad.Model.Map ocadModel = Ocad.Model.Map.Import(dataSourceFilePath);

                        decimal scaleRatio = (ocadModel.ScaleParameter.MapScale / _parent.OcadMap.ScaleParameter.MapScale);
                        Geometry.Distance offsetX = (_parent.OcadMap.ScaleParameter.RealWorldOffsetX - ocadModel.ScaleParameter.RealWorldOffsetX) / _parent.OcadMap.ScaleParameter.MapScale;
                        Geometry.Distance offsetY = (_parent.OcadMap.ScaleParameter.RealWorldOffsetY - ocadModel.ScaleParameter.RealWorldOffsetY) / _parent.OcadMap.ScaleParameter.MapScale;

                        foreach (Ocad.Model.AbstractObject obj in ocadModel.Objects)
                        {
                            foreach (Ocad.Model.Point p in obj.Points)
                            {
                                Geometry.Distance mapX = (p.X * scaleRatio) - offsetX;
                                Geometry.Distance mapY = (p.Y * scaleRatio) - offsetY;
                                p.X = mapX;
                                p.Y = mapY;

                                if (p.FirstBezier != null)
                                {
                                    mapX = (p.FirstBezier.X * scaleRatio) - offsetX;
                                    mapY = (p.FirstBezier.Y * scaleRatio) - offsetY;
                                    p.FirstBezier = new Geometry.Point(mapX, mapY);
                                }

                                if (p.SecondBezier != null)
                                {
                                    mapX = (p.SecondBezier.X * scaleRatio) - offsetX;
                                    mapY = (p.SecondBezier.Y * scaleRatio) - offsetY;
                                    p.SecondBezier = new Geometry.Point(mapX, mapY);
                                }
                            }
                            _parent.OcadMap.Objects.Add(obj);
                        }
                        break;
                }
                count++;
            }

            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Removing very short line segments...");
            _parent.OcadMap.RemoveShortSegments();
            count++;

            // Done early to ensure no two segments cross each other - helps later tasks
            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Adding points where segments touch...");
            _parent.OcadMap.AddIntersectPoints();
            count++;

            // Remove river/stream under lakes/ponds
            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Removing line objects from area objects...");
            _parent.OcadMap.RemoveBorders();
            count++;

            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Joining neighbouring line objects...");
            _parent.OcadMap.JoinLineObjects();
            count++;

            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Joining neighbouring area objects...");
            //_parent.OcadMap.JoinAreaObjects();
            count++;
            //-*
            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Removing duplicate bridges...");
            _parent.OcadMap.RemoveDuplicateBridges();
            count++;

            // Dependent on Removing duplicate bridges
            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Removing objects below bridges...");
            //_parent.OcadMap.RemoveObjectsBelowBridges();
            count++;

            // Dependent on Adding points where segments touch
            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Converting points at junctions to dash points...");
            _parent.OcadMap.ConvertToDashPoints();
            count++;

            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Converting points at acute angle to corner points...");
            _parent.OcadMap.ConvertToCornerPoints();
            count++;

            // Dependent on Converting points at junctions and at acute angle
            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Converting line objects to use appropriate complexity symbol...");
            _parent.OcadMap.ConvertLineObjects();
            count++;

            // Dependent on Joining neighbouring area objects
            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Converting area objects to use appropriate small/large symbol...");
            _parent.OcadMap.ConvertAreaObjects();
            count++;

            // Dependent on Converting area objects
            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, "Adding borders to area objects...");
            _parent.OcadMap.AddBorders();
            count++;
            //*/
            _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount, String.Format("Saving OCAD9 File {0}...", Path.GetFileNameWithoutExtension(saveOcadFileDialog.FileName)));
            _parent.OcadMap.Export(saveOcadFileDialog.FileName);
        }
        #endregion
    }
}
