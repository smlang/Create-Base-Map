using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String SPOT_COLOUR_NUMBER_CODE = "n";
        private const String SPOT_COLOUR_VISIBLE_CODE = "v";
        private const String SPOT_COLOUR_CYAN_CODE = "c";
        private const String SPOT_COLOUR_MAGENTA_CODE = "m";
        private const String SPOT_COLOUR_YELLOW_CODE = "y";
        private const String SPOT_COLOUR_BLACK_CODE = "k";
        private const String SPOT_COLOUR_FREQUENCY_CODE = "f";
        private const String SPOT_COLOUR_HALFTONE_ANGLE_CODE = "a";

        private void CopyToModelSpotColour(Model.Map map)
        {
            Model.SpotColour setting = MapExtension.GetOrCreateSpotColour(map, _mainValue, true);
            
            setting.Name = _mainValue;
            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case SPOT_COLOUR_VISIBLE_CODE:
                        setting.Visible = GetBooleanValue(i);
                        break;
                    case SPOT_COLOUR_NUMBER_CODE:
                        setting.Number = GetInt16Value(i);
                        break;
                    case SPOT_COLOUR_FREQUENCY_CODE:
                        setting.FrequencyLpi = Decimal.Divide(GetDecimalValue(i), 10);
                        break;
                    case SPOT_COLOUR_HALFTONE_ANGLE_CODE:
                        setting.HalftoneAngle = GetDecimalValue(i);
                        break;
                    case SPOT_COLOUR_CYAN_CODE:
                        setting.Cyan = GetDecimalValue(i);
                        break;
                    case SPOT_COLOUR_MAGENTA_CODE:
                        setting.Magenta = GetDecimalValue(i);
                        break;
                    case SPOT_COLOUR_YELLOW_CODE:
                        setting.Yellow = GetDecimalValue(i);
                        break;
                    case SPOT_COLOUR_BLACK_CODE:
                        setting.Black = GetDecimalValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelSpotColours(Model.Map map, List<Setting> settings)
        {
            foreach (Model.SpotColour source in map.SpotColours)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.SpotColour;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.Name);
                Write(b, SPOT_COLOUR_NUMBER_CODE, source.Number);
                Write(b, SPOT_COLOUR_VISIBLE_CODE, source.Visible);
                Write(b, SPOT_COLOUR_CYAN_CODE, source.Cyan, "0.0");
                Write(b, SPOT_COLOUR_MAGENTA_CODE, source.Magenta, "0.0");
                Write(b, SPOT_COLOUR_YELLOW_CODE, source.Yellow, "0.0");
                Write(b, SPOT_COLOUR_BLACK_CODE, source.Black, "0.0");
                Write(b, SPOT_COLOUR_FREQUENCY_CODE, source.FrequencyLpi * 10, "0.0");
                Write(b, SPOT_COLOUR_HALFTONE_ANGLE_CODE, source.HalftoneAngle, "0.0");
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
