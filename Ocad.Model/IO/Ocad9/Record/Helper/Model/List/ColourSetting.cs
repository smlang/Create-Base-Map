using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String COLOUR_NUMBER_CODE = "n";
        private const String COLOUR_CYAN_CODE = "c";
        private const String COLOUR_MAGENTA_CODE = "m";
        private const String COLOUR_YELLOW_CODE = "y";
        private const String COLOUR_BLACK_CODE = "k";
        private const String COLOUR_OVERPRINT_CODE = "o";
        private const String COLOUR_TRANSPARENCY_CODE = "t";
        private const String COLOUR_SPOT_COLOUR_NAME_CODE = "s";
        private const String COLOUR_SPOT_COLOUR_PERCENTAGE = "p";

        private void CopyToModelColour(Model.Map map)
        {
            Int16 number = 0;
            int i = 0;
            int numberIndex = -1;
            while ((i <= _codeValue.GetUpperBound(0)) && (numberIndex < 0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case COLOUR_NUMBER_CODE:
                        number = GetInt16Value(i);
                        numberIndex = i;
                        break;
                }
                i++;
            }

            Model.Colour setting = MapExtension.GetOrCreateColour(map, number, true);
            setting.Name = _mainValue;

            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case COLOUR_CYAN_CODE:
                        setting.Cyan = GetDecimalValue(i);
                        break;
                    case COLOUR_MAGENTA_CODE:
                        setting.Magenta = GetDecimalValue(i);
                        break;
                    case COLOUR_YELLOW_CODE:
                        setting.Yellow = GetDecimalValue(i);
                        break;
                    case COLOUR_BLACK_CODE:
                        setting.Black = GetDecimalValue(i);
                        break;
                    case COLOUR_OVERPRINT_CODE:
                        setting.Overprint = GetBooleanValue(i);
                        break;
                    case COLOUR_TRANSPARENCY_CODE:
                        setting.Transparency = GetDecimalValue(i);
                        break;
                    case COLOUR_SPOT_COLOUR_NAME_CODE:
                        Model.SpotColour spotColour = MapExtension.GetOrCreateSpotColour(map, GetStringValue(i));
                        i++;
                        setting.SpotColours.Add(spotColour, GetDecimalValue(i));
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelColours(Model.Map map, List<Setting> settings)
        {
            foreach (Model.Colour source in map.ColourTable)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.Colour;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.Name);
                Write(b, COLOUR_NUMBER_CODE, source.Number);
                Write(b, COLOUR_CYAN_CODE, source.Cyan, "0.0");
                Write(b, COLOUR_MAGENTA_CODE, source.Magenta, "0.0");
                Write(b, COLOUR_YELLOW_CODE, source.Yellow, "0.0");
                Write(b, COLOUR_BLACK_CODE, source.Black, "0.0");
                Write(b, COLOUR_OVERPRINT_CODE, source.Overprint);
                Write(b, COLOUR_TRANSPARENCY_CODE, source.Transparency, "0.0");
                foreach (Model.SpotColour s in source.SpotColours.Keys)
                {
                    Write(b, COLOUR_SPOT_COLOUR_NAME_CODE, s.Name);
                    Write(b, COLOUR_SPOT_COLOUR_PERCENTAGE, source.SpotColours[s], "0.0");
                }
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}