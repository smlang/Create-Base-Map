using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String OIM_PARAMETER_ANTI_ALIASING = "a";
        private const String OIM_PARAMETER_BORDER_WIDTH = "b";
        private const String OIM_PARAMETER_COMPRESSED_SVG = "c";
        private const String OIM_PARAMETER_EXTERNAL_SCRIPTING = "e";
        private const String OIM_PARAMETER_FIND_LABEL = "f";
        private const String OIM_PARAMETER_HEIGHT = "h";
        private const String OIM_PARAMETER_OVERVIEW_HEIGHT = "i";
        private const String OIM_PARAMETER_DO_NOT_CREATE_TILES = "m";
        private const String OIM_PARAMETER_ZOOM_RANGE = "r";
        private const String OIM_PARAMETER_SELECT_LABEL = "s";
        private const String OIM_PARAMETER_OVERVIEW_WIDTH = "v";
        private const String OIM_PARAMETER_WIDTH = "w";
        private const String OIM_PARAMETER_ZOOM_LEVELS = "z";
        private const String OIM_PARAMETER_FILL_COLOUR_RED = "R";
        private const String OIM_PARAMETER_FILL_COLOUR_GREEN = "G";
        private const String OIM_PARAMETER_FILL_COLOUR_BLUE = "B";
        private const String OIM_PARAMETER_OVERVIEW_MAP_NUMBER = "o";

        private void CopyToModelOimParameter(Model.Map map)
        {
            Model.OimParameter setting = new Model.OimParameter();
            map.OimParameter = setting;

            setting.FileName = _mainValue;

            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case OIM_PARAMETER_ANTI_ALIASING:
                        setting.AntiAliasing = GetBooleanValue(i);
                        break;
                    case OIM_PARAMETER_BORDER_WIDTH:
                        setting.BorderWidth = GetInt32Value(i);
                        break;
                    case OIM_PARAMETER_COMPRESSED_SVG:
                        setting.CompressedSvg = GetBooleanValue(i);
                        break;
                    case OIM_PARAMETER_EXTERNAL_SCRIPTING:
                        setting.ExternalScripting = GetBooleanValue(i);
                        break;
                    case OIM_PARAMETER_FIND_LABEL:
                        setting.FindLabel = GetStringValue(i);
                        break;
                    case OIM_PARAMETER_HEIGHT:
                        setting.Height = GetInt32Value(i);
                        break;
                    case OIM_PARAMETER_OVERVIEW_HEIGHT:
                        setting.OverviewHeight = GetInt32Value(i);
                        break;
                    case OIM_PARAMETER_DO_NOT_CREATE_TILES:
                        setting.DoNotCreateFiles = GetBooleanValue(i);
                        break;
                    case OIM_PARAMETER_ZOOM_RANGE:
                        setting.ZoomRange = GetDecimalValue(i);
                        break;
                    case OIM_PARAMETER_SELECT_LABEL:
                        setting.SelectLabel = GetStringValue(i);
                        break;
                    case OIM_PARAMETER_OVERVIEW_WIDTH:
                        setting.OverviewWidth = GetInt32Value(i);
                        break;
                    case OIM_PARAMETER_WIDTH:
                        setting.Width = GetInt32Value(i);
                        break;
                    case OIM_PARAMETER_ZOOM_LEVELS:
                        setting.ZoomLevels = GetByteValue(i);
                        break;
                    case OIM_PARAMETER_FILL_COLOUR_RED:
                        setting.FillColourRed = GetByteValue(i);
                        break;
                    case OIM_PARAMETER_FILL_COLOUR_GREEN:
                        setting.FillColourGreen = GetByteValue(i);
                        break;
                    case OIM_PARAMETER_FILL_COLOUR_BLUE:
                        setting.FillColourBlue = GetByteValue(i);
                        break;
                    case OIM_PARAMETER_OVERVIEW_MAP_NUMBER:
                        setting.OverviewMapNumber = GetByteValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelOimParameter(Model.Map map, List<Setting> settings)
        {
            Model.OimParameter source = map.OimParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.OimParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.FileName);
                Write(b, OIM_PARAMETER_ZOOM_RANGE, source.ZoomRange, "0.00");
                Write(b, OIM_PARAMETER_ZOOM_LEVELS, source.ZoomLevels);
                Write(b, OIM_PARAMETER_DO_NOT_CREATE_TILES, source.DoNotCreateFiles);
                Write(b, OIM_PARAMETER_WIDTH, source.Width);
                Write(b, OIM_PARAMETER_HEIGHT, source.Height);
                Write(b, OIM_PARAMETER_OVERVIEW_WIDTH, source.OverviewWidth);
                Write(b, OIM_PARAMETER_OVERVIEW_HEIGHT, source.OverviewHeight);
                Write(b, OIM_PARAMETER_FILL_COLOUR_RED, source.FillColourRed);
                Write(b, OIM_PARAMETER_FILL_COLOUR_GREEN, source.FillColourGreen);
                Write(b, OIM_PARAMETER_FILL_COLOUR_BLUE, source.FillColourBlue);
                Write(b, OIM_PARAMETER_EXTERNAL_SCRIPTING, source.ExternalScripting);
                Write(b, OIM_PARAMETER_SELECT_LABEL, source.SelectLabel);
                Write(b, OIM_PARAMETER_FIND_LABEL, source.FindLabel);
                Write(b, OIM_PARAMETER_BORDER_WIDTH, source.BorderWidth);
                Write(b, OIM_PARAMETER_COMPRESSED_SVG, source.CompressedSvg);
                Write(b, OIM_PARAMETER_OVERVIEW_MAP_NUMBER, source.OverviewMapNumber);
                Write(b, OIM_PARAMETER_ANTI_ALIASING, source.AntiAliasing);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
