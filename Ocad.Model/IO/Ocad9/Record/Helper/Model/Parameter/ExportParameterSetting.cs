using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String EXPORT_PARAMETER_ANTI_ALIASING = "a";
        private const String EXPORT_PARAMETER_SPOT_COLOURS_COMBINED = "b";
        private const String EXPORT_PARAMETER_COLOUR_FORMAT = "c";
        private const String EXPORT_PARAMETER_COLOUR_CORRECTION = "l";
        private const String EXPORT_PARAMETER_SPOT_COLOURS = "o";
        private const String EXPORT_PARAMETER_PARTIAL_MAP = "p";
        private const String EXPORT_PARAMETER_RESOLUTION = "r";
        private const String EXPORT_PARAMETER_SCALE = "s";
        private const String EXPORT_PARAMETER_TILES = "t";
        private const String EXPORT_PARAMETER_Z = "z";

        private void CopyToModelExportParameter(Model.Map map)
        {
            Model.ExportParameter setting = new Model.ExportParameter();
            map.ExportParameter = setting;

            int i = 0;
            setting.Format = _mainValue;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case EXPORT_PARAMETER_ANTI_ALIASING:
                        setting.AntiAliasing = GetBooleanValue(i);
                        break;
                    case EXPORT_PARAMETER_SPOT_COLOURS_COMBINED:
                        setting.SpotColoursCombined = GetBooleanValue(i);
                        break;
                    case EXPORT_PARAMETER_COLOUR_FORMAT:
                        setting.ColourFormat = (Model.Type.ColourFormat)GetByteValue(i);
                        break;
                    case EXPORT_PARAMETER_COLOUR_CORRECTION:
                        setting.ColourCorrection = GetBooleanValue(i);
                        break;
                    case EXPORT_PARAMETER_SPOT_COLOURS:
                        setting.SpotColours = GetBooleanValue(i);
                        break;
                    case EXPORT_PARAMETER_PARTIAL_MAP:
                        setting.PartialMap = GetBooleanValue(i);
                        break;
                    case EXPORT_PARAMETER_RESOLUTION:
                        setting.Resolution = GetInt16Value(i);
                        break;
                    case EXPORT_PARAMETER_SCALE:
                        setting.Scale = GetStringValue(i);
                        break;
                    case EXPORT_PARAMETER_TILES:
                        setting.Tiles = GetBooleanValue(i);
                        break;
                    case EXPORT_PARAMETER_Z:
                        setting.ZParameter = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelExportParameter(Model.Map map, List<Setting> settings)
        {
            Model.ExportParameter source = map.ExportParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.ExportParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.Format);
                Write(b, EXPORT_PARAMETER_ANTI_ALIASING, source.AntiAliasing);
                Write(b, EXPORT_PARAMETER_SPOT_COLOURS_COMBINED, source.SpotColoursCombined);
                Write(b, EXPORT_PARAMETER_COLOUR_CORRECTION, source.ColourCorrection);
                Write<Model.Type.ColourFormat, Byte>(b, EXPORT_PARAMETER_COLOUR_FORMAT, source.ColourFormat);
                Write(b, EXPORT_PARAMETER_SPOT_COLOURS, source.SpotColours);
                Write(b, EXPORT_PARAMETER_PARTIAL_MAP, source.PartialMap);
                Write(b, EXPORT_PARAMETER_RESOLUTION, source.Resolution);
                Write(b, EXPORT_PARAMETER_TILES, source.Tiles);
                Write(b, EXPORT_PARAMETER_Z, source.ZParameter);
                Write(b, EXPORT_PARAMETER_SCALE, source.Scale);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}