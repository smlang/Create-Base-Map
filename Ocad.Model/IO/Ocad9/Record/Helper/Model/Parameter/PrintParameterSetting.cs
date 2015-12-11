using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geometry;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String PRINT_PARAMETER_PRINT_SCALE = "a";
        private const String PRINT_PARAMETER_LANDSCAPE = "l";
        private const String PRINT_PARAMETER_PRINT_SPOT_SEPARATION = "c";
        private const String PRINT_PARAMETER_PRINT_GRID = "g";
        private const String PRINT_PARAMETER_GRID_COLOUR = "d";
        private const String PRINT_PARAMETER_INTENSITY = "i";
        private const String PRINT_PARAMETER_WIDTH_FOR_LINES_AND_DOTS_PERCENTAGE = "w";
        private const String PRINT_PARAMETER_RANGE = "r";
        private const String PRINT_PARAMETER_PARTIAL_MAP_LEFT_MM = "L";
        private const String PRINT_PARAMETER_PARTIAL_MAP_BOTTOM_MM = "B";
        private const String PRINT_PARAMETER_PARTIAL_MAP_RIGHT_MM = "R";
        private const String PRINT_PARAMETER_PARTIAL_MAP_TOP_MM = "T";
        private const String PRINT_PARAMETER_HORIZONTAL_OVERLAP = "x";
        private const String PRINT_PARAMETER_VERTICAL_OVERLAP = "y";
        private const String PRINT_PARAMETER_HORIZONTAL_SCALE = "s";
        private const String PRINT_PARAMETER_VERTICAL_SCALE = "t";

        private void CopyToModelPrintParameter(Model.Map map)
        {
            Model.PrintParameter setting = new Model.PrintParameter();
            map.PrintParameter = setting;

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
                    case PRINT_PARAMETER_PRINT_SCALE:
                        setting.PrintScale = GetDecimalValue(i);
                        break;
                    case PRINT_PARAMETER_LANDSCAPE:
                        setting.Landscape = GetBooleanValue(i);
                        break;
                    case PRINT_PARAMETER_PRINT_SPOT_SEPARATION:
                        setting.PrintSpotSeparation = GetBooleanValue(i);
                        break;
                    case PRINT_PARAMETER_PRINT_GRID:
                        setting.PrintGrid = GetBooleanValue(i);
                        break;
                    case PRINT_PARAMETER_GRID_COLOUR:
                        setting.GridColour = MapExtension.GetOrCreateColour(map, GetInt16Value(i));
                        break;
                    case PRINT_PARAMETER_INTENSITY:
                        setting.Intensity = GetByteValue(i);
                        break;
                    case PRINT_PARAMETER_WIDTH_FOR_LINES_AND_DOTS_PERCENTAGE:
                        setting.WidthForLinesAndDotsPercentage = GetByteValue(i);
                        break;
                    case PRINT_PARAMETER_RANGE:
                        setting.Range = GetByteValue(i);
                        break;
                    case PRINT_PARAMETER_PARTIAL_MAP_LEFT_MM:
                        setting.PartialMapLeft = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case PRINT_PARAMETER_PARTIAL_MAP_BOTTOM_MM:
                        setting.PartialMapBottom = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case PRINT_PARAMETER_PARTIAL_MAP_RIGHT_MM:
                        setting.PartialMapRight = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case PRINT_PARAMETER_PARTIAL_MAP_TOP_MM:
                        setting.PartialMapTop = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case PRINT_PARAMETER_HORIZONTAL_OVERLAP:
                        setting.HorizontalOverlap = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case PRINT_PARAMETER_VERTICAL_OVERLAP:
                        setting.VerticalOverlap = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case PRINT_PARAMETER_HORIZONTAL_SCALE:
                        setting.HorizontalScale = GetDecimalValue(i);
                        break;
                    case PRINT_PARAMETER_VERTICAL_SCALE:
                        setting.VerticalScale = GetDecimalValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelPrintParameter(Model.Map map, List<Setting> settings)
        {
            Model.PrintParameter source = map.PrintParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.PrintParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, PRINT_PARAMETER_PRINT_SCALE, source.PrintScale, "0.00");
                Write(b, PRINT_PARAMETER_PARTIAL_MAP_LEFT_MM, source.PartialMapLeft[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, PRINT_PARAMETER_PARTIAL_MAP_BOTTOM_MM, source.PartialMapBottom[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, PRINT_PARAMETER_PARTIAL_MAP_RIGHT_MM, source.PartialMapRight[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, PRINT_PARAMETER_PARTIAL_MAP_TOP_MM, source.PartialMapTop[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, PRINT_PARAMETER_HORIZONTAL_OVERLAP, source.HorizontalOverlap[2, Distance.Unit.Metre, Scale.milli], "0.00");
                Write(b, PRINT_PARAMETER_VERTICAL_OVERLAP, source.VerticalOverlap[2, Distance.Unit.Metre, Scale.milli], "0.00");
                Write(b, PRINT_PARAMETER_PRINT_GRID, source.PrintGrid);
                Write(b, PRINT_PARAMETER_GRID_COLOUR, source.GridColour);
                Write(b, PRINT_PARAMETER_INTENSITY, source.Intensity);
                Write(b, PRINT_PARAMETER_WIDTH_FOR_LINES_AND_DOTS_PERCENTAGE, source.WidthForLinesAndDotsPercentage);
                Write(b, PRINT_PARAMETER_LANDSCAPE, source.Landscape);
                Write(b, PRINT_PARAMETER_PRINT_SPOT_SEPARATION, source.PrintSpotSeparation);
                Write(b, PRINT_PARAMETER_RANGE, source.Range);
                Write(b, PRINT_PARAMETER_HORIZONTAL_SCALE, source.HorizontalScale, "0.0");
                Write(b, PRINT_PARAMETER_VERTICAL_SCALE, source.VerticalScale, "0.0");
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}