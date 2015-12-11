using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String TIFF_PARAMETER_COMPRESSION = "c";
        private const String TIFF_PARAMETER_GEOTIFF = "g";
        private const String TIFF_PARAMETER_PIXEL_SIZE = "s";
        private const String TIFF_PARAMETER_TFW_FILE = "w";

        private void CopyToModelTiffParameter(Model.Map map)
        {
            Model.TiffParameter setting = new Model.TiffParameter();
            map.TiffParameter = setting;

            if (!String.IsNullOrEmpty(_mainValue))
            {
                CreateApplicationSettingException(1);
            }

            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case TIFF_PARAMETER_COMPRESSION:
                        setting.Compression = (Model.Type.TiffCompression)GetByteValue(i);
                        break;
                    case TIFF_PARAMETER_GEOTIFF:
                        setting.GeoTiff = GetBooleanValue(i);
                        break;
                    case TIFF_PARAMETER_PIXEL_SIZE:
                        setting.PixelSize = GetDecimalValue(i);
                        break;
                    case TIFF_PARAMETER_TFW_FILE:
                        setting.TfwFile = GetBooleanValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelTiffParameter(Model.Map map, List<Setting> settings)
        {
            Model.TiffParameter source = map.TiffParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.TiffParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, TIFF_PARAMETER_PIXEL_SIZE, source.PixelSize, "0.00");
                Write(b, TIFF_PARAMETER_GEOTIFF, source.GeoTiff);
                Write<Model.Type.TiffCompression, Byte>(b, TIFF_PARAMETER_COMPRESSION, source.Compression);
                Write(b, TIFF_PARAMETER_TFW_FILE, source.TfwFile);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
