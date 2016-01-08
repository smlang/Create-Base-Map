using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace CreateBaseMap
{
    internal partial class DownloadForm : Form
    {
        private static readonly Regex distanceRegex = new Regex(@"^\d{1,2}$");
        private const string TIFF_URL_PATH_FORMAT = "http://d2.roadworks.org/g.1.dynamic.live/wms?LAYERS=MasterMap_Area-ORA_Line-SHP&FORMAT=image%2Fgif&TILED=false&SRS=EPSG%3A81989&SERVICE=WMS&VERSION=1.1.1&REQUEST=GetMap&STYLES=&EXCEPTIONS=application%2Fvnd.ogc.se_inimage&BBOX={0},{1},{2},{3}&WIDTH={4}&HEIGHT={5}";
        private const string OSM_URL_PATH_FORMAT = "http://www.overpass-api.de/api/xapi_meta?*[bbox={0},{1},{2},{3}]";

        internal System.Collections.Specialized.StringCollection FileList { get; private set; }
        
        private MainForm _parent;
        private ProgressForm _progress;
        private TDPG.GeoCoordConversion.GridReference _mapCentre;
        private int _widthKm;
        private int _maxWidthKm;
        private int _heightKm;
        private int _maxHeightKm;

        internal DownloadForm(MainForm parent)
        {
            FileList = new System.Collections.Specialized.StringCollection();
            centreOSGridReferenceUserControl = new OSGridReferenceUserControl();

            InitializeComponent();

            _parent = parent;
            _mapCentre = new TDPG.GeoCoordConversion.GridReference(
                 (long)_parent.OcadMap.ScaleParameter.RealWorldOffsetX[0, Geometry.Distance.Unit.Metre, Geometry.Scale.one],
                 (long)_parent.OcadMap.ScaleParameter.RealWorldOffsetY[0, Geometry.Distance.Unit.Metre, Geometry.Scale.one]);

            centreOSGridReferenceUserControl.Value = _mapCentre;
            centreOSGridReferenceUserControl.MinValue = _parent.FurthestSWGridReference;
            centreOSGridReferenceUserControl.MaxValue = _parent.FurthestNEGridReference;

            _maxWidthKm = (int)((_parent.FurthestNEGridReference.Easting - _parent.FurthestSWGridReference.Easting) / 1000);
            _maxHeightKm = (int)((_parent.FurthestNEGridReference.Northing - _parent.FurthestSWGridReference.Northing) / 1000);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (!CheckForIssues())
            {
                return;
            }

            if (destinationfolderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (_progress = new ProgressForm(DoDownload))
            {
                if (_progress.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private bool CheckForIssues()
        {
            errorProvider.Clear();
            bool error = false;
            TDPG.GeoCoordConversion.GridReference imageCenter = null;
            if (centreOSGridReferenceUserControl.Validate())
            {
                imageCenter = centreOSGridReferenceUserControl.Value;
            }
            else
            {
                error = true;
            }

            #region Check Dimensions
            _widthKm = 0;
            if (!distanceRegex.IsMatch(widthTextBox.Text))
            {
                errorProvider.SetError(widthTextBox, String.Format("Width must be between 1 and {0}.", _maxWidthKm));
                error = true;
            }
            else
            {
                _widthKm = Int32.Parse(widthTextBox.Text);
                if ((_widthKm < 0) || (_widthKm > _maxWidthKm))
                {
                    errorProvider.SetError(widthTextBox, String.Format("Width must be between 1 and {0}.", _maxWidthKm));
                    error = true;
                }
            }

            _heightKm = 0;
            if (!distanceRegex.IsMatch(heightTextBox.Text))
            {
                errorProvider.SetError(heightTextBox, String.Format("Height must be between 1 and {0}.", _maxHeightKm));
                error = true;
            }
            else
            {
                _heightKm = Int32.Parse(heightTextBox.Text);
                if ((_heightKm < 0) || (_heightKm > _maxHeightKm))
                {
                    errorProvider.SetError(heightTextBox, String.Format("Height must be between 1 and {0}.", _maxHeightKm));
                    error = true;
                }
            }
            #endregion

            if (imageCenter != null)
            {
                if (_widthKm != 0)
                {
                    long maxWidthDiff = (long)_maxWidthKm / 2;

                    if (maxWidthDiff > Math.Abs((imageCenter.Easting - (_widthKm * 500)) - _mapCentre.Easting))
                    {
                        errorProvider.SetError(centreOSGridReferenceUserControl, String.Format("Eastern edge of download area will not fit within final OCAD9 file."));
                        error = true;
                    }
                    if (maxWidthDiff > Math.Abs((imageCenter.Easting + (_widthKm * 500)) - _mapCentre.Easting))
                    {
                        errorProvider.SetError(centreOSGridReferenceUserControl, String.Format("Western edge of download area will not fit within final OCAD9 file."));
                        error = true;
                    }
                }

                if (_heightKm != 0)
                {
                    long maxHeightDiff = (long)_maxHeightKm / 2;

                    if (maxHeightDiff > Math.Abs((imageCenter.Northing - (_heightKm * 500)) - _mapCentre.Northing))
                    {
                        errorProvider.SetError(centreOSGridReferenceUserControl, String.Format("Southern edge of download area will not fit within final OCAD9 file."));
                        error = true;
                    }
                    if (maxHeightDiff > Math.Abs((imageCenter.Northing + (_heightKm * 500)) - _mapCentre.Northing))
                    {
                        errorProvider.SetError(centreOSGridReferenceUserControl, String.Format("Northern edge of download area will not fit within final OCAD9 file."));
                        error = true;
                    }
                }
            }

            if (error)
            {
                return false;
            }

            if (((_widthKm * _heightKm) > 25) &&
                (MessageBox.Show(String.Format("Downloading {0} km squares will take awhile. Do you wish to continue?", _widthKm * _heightKm), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No))
            {
                return false;
            }

            return true;
        }

        private void DoDownload()
        {
            string destinationFolderPath = destinationfolderBrowserDialog.SelectedPath;
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            int maxCount = 0;
            if (roadworksCheckBox.Checked)
            {
                maxCount += _widthKm * _heightKm;
            }
            if (osmCheckBox.Checked)
            {
                maxCount += 1;
            }

            TDPG.GeoCoordConversion.GridReference imageCenter = centreOSGridReferenceUserControl.Value;
            TDPG.GeoCoordConversion.GridReference swCorner = new TDPG.GeoCoordConversion.GridReference(imageCenter.Easting - (_widthKm * 500), imageCenter.Northing - (_heightKm * 500));

            #region Download Roadworks
            if (roadworksCheckBox.Checked)
            {
                int count = 1;
                for (int x = 0; x < _widthKm; x++)
                {
                    for (int y = 0; y < _heightKm; y++)
                    {
                        _progress.buildBackgroundWorker.ReportProgress(100 * count / maxCount,
                            String.Format("Downloading Roadworks Image {0}/{1}...", count, _widthKm * _heightKm));
                        count++;

                        TDPG.GeoCoordConversion.GridReference origin = new TDPG.GeoCoordConversion.GridReference(swCorner.Easting + (x * 1000), swCorner.Northing + (y * 1000));
                        string letters, eastNumber, northNumber;
                        origin.GetGridReference(out letters, out eastNumber, out northNumber);
                        string tiffFileName = String.Format("Roadworks_{0}_{1}_{2}.tif", letters, eastNumber, northNumber, _widthKm, _heightKm);
                        string tiffFilePath = Path.Combine(destinationFolderPath, tiffFileName);

                        if (overwriteCheckBox.Checked || !File.Exists(tiffFilePath))
                        {
                            string urlPath = String.Format(TIFF_URL_PATH_FORMAT, (int)(origin.Easting), (int)(origin.Northing), (int)(origin.Easting) + 1000, (int)(origin.Northing) + 1000, 1000, 1000);

                            string tempfilename = Path.GetTempFileName();

                            Tiff.GeoTagBaseMap.Download(
                                urlPath,
                                (int)(origin.Easting),
                                (int)(origin.Northing) + 1000,
                                1000,
                                tiffFilePath);
                        }
                        FileList.Add(tiffFilePath);
                    }
                }
            }
            #endregion

            #region Download OSM
            if (osmCheckBox.Checked)
            {
                string letters, eastNumber, northNumber;
                swCorner.GetGridReference(out letters, out eastNumber, out northNumber);
                string osmFileName = String.Format("{0}_{1}_{2}_{3}_{4}.osm", letters, eastNumber, northNumber, _widthKm, _heightKm);
                string osmFilePath = Path.Combine(destinationFolderPath, osmFileName);

                _progress.buildBackgroundWorker.ReportProgress(100, "Downloading OSM Data...");

                if (overwriteCheckBox.Checked || !File.Exists(osmFilePath))
                {

                    TDPG.GeoCoordConversion.PolarGeoCoordinate swWgs84Point = TDPG.GeoCoordConversion.GridReference.ChangeToPolarGeo(swCorner);
                    double minlat = swWgs84Point.Lat;
                    double minlon = swWgs84Point.Lon;

                    TDPG.GeoCoordConversion.GridReference neCorner = new TDPG.GeoCoordConversion.GridReference(swCorner.Easting + (_widthKm * 1000), swCorner.Northing + (_heightKm * 1000));
                    TDPG.GeoCoordConversion.PolarGeoCoordinate neWgs84Point = TDPG.GeoCoordConversion.GridReference.ChangeToPolarGeo(neCorner);
                    double maxlat = neWgs84Point.Lat;
                    double maxlon = neWgs84Point.Lon;

                    string osmUrl = String.Format(OSM_URL_PATH_FORMAT, minlon, minlat, maxlon, maxlat);
                    using (Net.Resource osmResource = new Net.Resource(osmUrl))
                    {
                        using (BufferedStream inputBufferStream = new BufferedStream(osmResource.Stream))
                        {
                            using (FileStream osmFileStream = new FileStream(osmFilePath, FileMode.Create, FileAccess.Write))
                            {
                                using (BufferedStream outputBufferSteam = new BufferedStream(osmFileStream))
                                {
                                    int size = (inputBufferStream.CanSeek) ? Math.Min((int)(inputBufferStream.Length - inputBufferStream.Position), 0x2000) : 0x2000;
                                    byte[] buffer = new byte[size];
                                    int n;
                                    int totalN = 0;
                                    do
                                    {
                                        n = inputBufferStream.Read(buffer, 0, buffer.Length);
                                        totalN += n;
                                        outputBufferSteam.Write(buffer, 0, n);
                                        _progress.buildBackgroundWorker.ReportProgress(100, String.Format("Downloading OSM Data {0:#,##0} KB...", totalN / 1024));
                                    } while (n != 0);
                                }
                            }
                        }
                    }
                }
                FileList.Add(osmFilePath);
            }
            #endregion
        }

        private void roadworksOrOsmCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool enableDownload = (roadworksCheckBox.Checked || osmCheckBox.Checked);
            if (downloadButton.Enabled != enableDownload)
            {
                downloadButton.Enabled = enableDownload;
            }
        }
    }
}
