using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Tiff
{
    public class GeoTagBaseMap
    {
        private const int km = 1000;

        public static void Download(string urlPath, int osLeftEasting, int osTopNorthing, int imagePixelPerKm, string destinationFilePath)
        {
            string tempfilename = Path.GetTempFileName();

            using (Net.Resource resource = new Net.Resource(urlPath))
            {
                using (Bitmap allBmp = new Bitmap(imagePixelPerKm, imagePixelPerKm))
                {
                    using (Graphics allGraphics = Graphics.FromImage(allBmp))
                    {
                        using (Image gif = Image.FromStream(resource.Stream, false, true))
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
                                    allGraphics.DrawImage(jpg, 0, 0);
                                }
                            }
                        }
                    }
                    allBmp.Save(tempfilename, ImageFormat.Jpeg);
                }
            }

            ConvertImageToTiff(tempfilename, destinationFilePath);

            Double metersPerPixel = ((Double)km) / ((Double)imagePixelPerKm);
            Tiff.GeoTag.Add(destinationFilePath, osLeftEasting, osTopNorthing, metersPerPixel);
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
