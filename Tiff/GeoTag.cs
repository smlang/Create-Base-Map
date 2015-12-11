using System;
using System.Collections.Generic;
using System.Text;
using Classic = BitMiracle.LibTiff.Classic;

namespace Tiff
{
    public class GeoTag
    {
        private const Classic.TiffTag TIFFTAG_GDAL_33550 = (Classic.TiffTag)33550;
        private const Classic.TiffTag TIFFTAG_GDAL_33922 = (Classic.TiffTag)33922;
        private const Classic.TiffTag TIFFTAG_GDAL_34735 = (Classic.TiffTag)34735;
        private const Classic.TiffTag TIFFTAG_GDAL_34737 = (Classic.TiffTag)34737;

        private static Classic.Tiff.TiffExtendProc m_parentExtender;

        internal static void TagExtender(Classic.Tiff tif)
        {
            Classic.TiffFieldInfo[] tiffFieldInfo = 
            {
                new Classic.TiffFieldInfo(TIFFTAG_GDAL_33550, -3, -3, Classic.TiffType.DOUBLE,
                    Classic.FieldBit.Custom, true, true, "Tag 33550"),
                new Classic.TiffFieldInfo(TIFFTAG_GDAL_33922, -3, -3, Classic.TiffType.DOUBLE,
                    Classic.FieldBit.Custom, true, true, "Tag 33922"),
                new Classic.TiffFieldInfo(TIFFTAG_GDAL_34735, -3, -3, Classic.TiffType.SHORT,
                    Classic.FieldBit.Custom, true, true, "Tag 34735"),
                new Classic.TiffFieldInfo(TIFFTAG_GDAL_34737, -3, -3, Classic.TiffType.ASCII,
                    Classic.FieldBit.Custom, true, true, "Tag 34737"),
            };

            tif.MergeFieldInfo(tiffFieldInfo, tiffFieldInfo.Length);

            if (m_parentExtender != null)
                m_parentExtender(tif);
        }

        internal static void Add(string tiffFilePath, double west, double north, double metersPerPixel)
        {
            m_parentExtender = Classic.Tiff.SetTagExtender(TagExtender);

            using (Classic.Tiff image = Classic.Tiff.Open(tiffFilePath, "a"))
            {
                // we should rewind to first directory (first image) because of append mode
                image.SetDirectory(0);

                // ModelPixelScaleTag
                byte[] buffer33550 = new byte[24];
                BitConverter.GetBytes(metersPerPixel).CopyTo(buffer33550, 0);
                BitConverter.GetBytes(metersPerPixel).CopyTo(buffer33550, 8);
                image.SetField(TIFFTAG_GDAL_33550, 3, buffer33550);

                // ModelTiepointTag
                byte[] buffer33922 = new byte[48];
                BitConverter.GetBytes(west).CopyTo(buffer33922, 24);
                BitConverter.GetBytes(north).CopyTo(buffer33922, 32);
                image.SetField(TIFFTAG_GDAL_33922, 6, buffer33922);

                // 34735  GeoKeyDirectoryTag SHORT x 32
                byte[] buffer34735 = new byte[64];
                BitConverter.GetBytes(((UInt16)1)).CopyTo(buffer34735, 0);
                BitConverter.GetBytes(((UInt16)1)).CopyTo(buffer34735, 2);
                BitConverter.GetBytes(((UInt16)0)).CopyTo(buffer34735, 4);
                BitConverter.GetBytes(((UInt16)7)).CopyTo(buffer34735, 6);

                BitConverter.GetBytes(((UInt16)1024)).CopyTo(buffer34735, 8);
                BitConverter.GetBytes(((UInt16)0)).CopyTo(buffer34735, 10);
                BitConverter.GetBytes(((UInt16)1)).CopyTo(buffer34735, 12);
                BitConverter.GetBytes(((UInt16)1)).CopyTo(buffer34735, 14);

                BitConverter.GetBytes(((UInt16)1025)).CopyTo(buffer34735, 16);
                BitConverter.GetBytes(((UInt16)0)).CopyTo(buffer34735, 18);
                BitConverter.GetBytes(((UInt16)1)).CopyTo(buffer34735, 20);
                BitConverter.GetBytes(((UInt16)1)).CopyTo(buffer34735, 22);

                BitConverter.GetBytes(((UInt16)1026)).CopyTo(buffer34735, 24);
                BitConverter.GetBytes(((UInt16)34737)).CopyTo(buffer34735, 26);
                BitConverter.GetBytes(((UInt16)34)).CopyTo(buffer34735, 28);
                BitConverter.GetBytes(((UInt16)0)).CopyTo(buffer34735, 30);

                BitConverter.GetBytes(((UInt16)2049)).CopyTo(buffer34735, 32);
                BitConverter.GetBytes(((UInt16)34737)).CopyTo(buffer34735, 34);
                BitConverter.GetBytes(((UInt16)10)).CopyTo(buffer34735, 36);
                BitConverter.GetBytes(((UInt16)34)).CopyTo(buffer34735, 38);

                BitConverter.GetBytes(((UInt16)2054)).CopyTo(buffer34735, 40);
                BitConverter.GetBytes(((UInt16)0)).CopyTo(buffer34735, 42);
                BitConverter.GetBytes(((UInt16)1)).CopyTo(buffer34735, 44);
                BitConverter.GetBytes(((UInt16)9102)).CopyTo(buffer34735, 46);

                BitConverter.GetBytes(((UInt16)3072)).CopyTo(buffer34735, 48);
                BitConverter.GetBytes(((UInt16)0)).CopyTo(buffer34735, 50);
                BitConverter.GetBytes(((UInt16)1)).CopyTo(buffer34735, 52);
                BitConverter.GetBytes(((UInt16)27700)).CopyTo(buffer34735, 54);

                BitConverter.GetBytes(((UInt16)3076)).CopyTo(buffer34735, 56);
                BitConverter.GetBytes(((UInt16)0)).CopyTo(buffer34735, 58);
                BitConverter.GetBytes(((UInt16)1)).CopyTo(buffer34735, 60);
                BitConverter.GetBytes(((UInt16)9001)).CopyTo(buffer34735, 62);
                image.SetField(TIFFTAG_GDAL_34735, 32, buffer34735);

                // GeoAsciiParamsTag
                byte[] buffer34737 = new byte[45];
                string grid = "OSGB 1936 / British National Grid|OSGB 1936|";
                System.Text.Encoding.ASCII.GetBytes(grid).CopyTo(buffer34737, 0);
                image.SetField(TIFFTAG_GDAL_34737, 45, buffer34737);

                // rewrites directory saving new tag
                image.CheckpointDirectory();
            }

            // restore previous tag extender
            Classic.Tiff.SetTagExtender(m_parentExtender);
        }

        public static void Read(string tiffFilePath, out int pixelWidth, out int pixelHeight, out double west, out double north, out double metersPerPixel)
        {
            using (Classic.Tiff image = Classic.Tiff.Open(tiffFilePath, "r"))
            {
                // ModelPixelScaleTag
                Classic.FieldValue[] value = image.GetField(TIFFTAG_GDAL_33550);
                byte[] buffer33550 = value[1].ToByteArray();
                metersPerPixel = BitConverter.ToDouble(buffer33550, 0);

                // ModelTiepointTag
                value = image.GetField(TIFFTAG_GDAL_33922);
                byte[] buffer33922 = value[1].ToByteArray();
                west = BitConverter.ToDouble(buffer33922, 24);
                north = BitConverter.ToDouble(buffer33922, 32);

                value = image.GetField(Classic.TiffTag.IMAGEWIDTH);
                pixelWidth = value[0].ToInt();

                value = image.GetField(Classic.TiffTag.IMAGELENGTH);
                pixelHeight = value[0].ToInt();
            }
        }
    }
}
