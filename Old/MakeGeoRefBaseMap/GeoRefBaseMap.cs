using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;

namespace MakeGeoRefBaseMap
{
    internal class GeoRefBaseMap
    {
        const int km = 1000;
        //const int dpi = 96;
        //const int scale = 3795;
        const string urlPathFormat = "http://d2.roadworks.org/g.1.dynamic.live/wms?LAYERS=MasterMap_Area-ORA_Line-SHP&FORMAT=image%2Fgif&TILED=false&SRS=EPSG%3A81989&SERVICE=WMS&VERSION=1.1.1&REQUEST=GetMap&STYLES=&EXCEPTIONS=application%2Fvnd.ogc.se_inimage&BBOX={0},{1},{2},{3}&WIDTH={4}&HEIGHT={5}";

        internal static void Build(int westStart, int southStart, int eastWestLengthKm, int northSouthLengthKm, int imagePixelPerKm, string destinationFilePath, BackgroundWorker bw, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                int eastFinish = westStart + (eastWestLengthKm * km);
                int northFinish = southStart + (northSouthLengthKm * km);

                int imageTopX = eastWestLengthKm * imagePixelPerKm;
                int imageTopY = northSouthLengthKm * imagePixelPerKm;

                int countKmSquare = 0;
                int totalKmSquare = eastWestLengthKm * northSouthLengthKm;

                int progressStep = 0;
                int progressMaximum = totalKmSquare + 1;

                string tempfilename = Path.GetTempFileName();

                using (Bitmap allBmp = new Bitmap(imageTopX, imageTopY))
                {
                    //allBmp.SetResolution(dpi, dpi);

                    using (Graphics allGraphics = Graphics.FromImage(allBmp))
                    {
                        int imageOffsetY = imageTopY;
                        for (int south = southStart; south < northFinish; south += km)
                        {
                            imageOffsetY -= imagePixelPerKm;

                            int imageOffsetX = 0;
                            for (int west = westStart; west < eastFinish; west += km)
                            {
                                countKmSquare++;
                                string urlPath = String.Format(urlPathFormat, west, south, west + km, south + km, imagePixelPerKm, imagePixelPerKm);
                                bw.ReportProgress(100 * progressStep / progressMaximum, String.Format("Downloading km square {0} of {1}...", countKmSquare, totalKmSquare));
                                progressStep++;

                                using (Image gif = NetResource.DownloadImage(urlPath, out errorMessage))
                                {
                                    if (gif == null)
                                    {
                                        return;
                                    }

                                    using (MemoryStream jpgStream = new MemoryStream())
                                    {
                                        gif.Save(jpgStream, ImageFormat.Jpeg);
                                        using (Image jpg = Image.FromStream(jpgStream))
                                        {
                                            allGraphics.DrawImage(jpg, imageOffsetX, imageOffsetY);
                                        }
                                    }

                                    imageOffsetX += imagePixelPerKm;
                                }
                            }
                        }
                    }

                    bw.ReportProgress(100 * progressStep / progressMaximum, "Save as TIFF image...");
                    progressStep++;
                    allBmp.Save(tempfilename, ImageFormat.Jpeg);
                }

                ConvertImageToTiff(tempfilename, destinationFilePath);

                bw.ReportProgress(100 * progressStep / progressMaximum, "Add Georef Tags to TIFF image...");
                progressStep++;

                Double metersPerPixel = ((Double)km) / ((Double)imagePixelPerKm);
                GeoTiff.AddTags(destinationFilePath, westStart, northFinish, metersPerPixel);
            }
            catch (Exception e)
            {
                Exception e2 = e;
                errorMessage = "Unexpected Problem";
                while (e2 != null)
                {
                    errorMessage += "\n" + e.Message + "\n" + e.StackTrace;
                    e2 = e2.InnerException;
                }
            }
        }

        private static void ConvertImageToTiff(string imageFilePath, string tiffFilePath)
        {
            // get the tiff codec info
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/tiff");

            // create encode parameters
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(
                Encoder.Compression, 
                (long)EncoderValue.CompressionNone);

            using (Image tempImage = Image.FromFile(imageFilePath))
            {
                tempImage.Save(tiffFilePath, myImageCodecInfo, myEncoderParameters);
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            foreach (ImageCodecInfo encoder in ImageCodecInfo.GetImageEncoders())
            {
                if (encoder.MimeType == mimeType)
                {
                    return encoder;
                }
            }
            return null;
        }
    }
}
