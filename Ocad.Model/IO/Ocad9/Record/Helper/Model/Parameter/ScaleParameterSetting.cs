using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geometry;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String SCALE_PARAMETER_REAL_WORLD_ANGLE_CODE = "a";
        private const String SCALE_PARAMETER_REAL_WORLD_GRID_DISTANCE_M_CODE = "d";
        private const String SCALE_PARAMETER_PAPER_GRID_DISTANCE_MM_CODE = "g";
        private const String SCALE_PARAMETER_REAL_WORLD_COORDINATE_SYSTEM_CODE = "i";
        private const String SCALE_PARAMETER_MAP_SCALE_CODE = "m";
        private const String SCALE_PARAMETER_USE_REAL_WORLD_GRID_CODE = "r";
        private const String SCALE_PARAMETER_REAL_WORLD_OFFSET_X_CODE = "x";
        private const String SCALE_PARAMETER_REAL_WORLD_OFFSET_Y_CODE = "y";

        private void CopyToModelScaleParameter(Model.Map map)
        {
            Model.ScaleParameter setting = new Model.ScaleParameter();
            map.ScaleParameter = setting;

            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case SCALE_PARAMETER_MAP_SCALE_CODE:
                        setting.MapScale = GetDecimalValue(i);
                        break;
                    case SCALE_PARAMETER_PAPER_GRID_DISTANCE_MM_CODE:
                        setting.PaperGridDistance = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case SCALE_PARAMETER_USE_REAL_WORLD_GRID_CODE:
                        setting.UseRealWorldGrid = GetBooleanValue(i);
                        break;
                    case SCALE_PARAMETER_REAL_WORLD_OFFSET_X_CODE:
                        setting.RealWorldOffsetX = GetDistance(i, Distance.Unit.Metre, Scale.one);
                        break;
                    case SCALE_PARAMETER_REAL_WORLD_OFFSET_Y_CODE:
                        setting.RealWorldOffsetY = GetDistance(i, Distance.Unit.Metre, Scale.one);
                        break;
                    case SCALE_PARAMETER_REAL_WORLD_ANGLE_CODE:
                        setting.RealWorldAngle = GetDecimalValue(i);
                        break;
                    case SCALE_PARAMETER_REAL_WORLD_GRID_DISTANCE_M_CODE:
                        setting.RealWorldGridDistance = GetDistance(i, Distance.Unit.Metre, Scale.one);
                        break;
                    case SCALE_PARAMETER_REAL_WORLD_COORDINATE_SYSTEM_CODE:
                        setting.RealWorldCoordinateSystem = (Model.Type.CoordinateSystemType)GetInt32Value(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelScaleParameter(Model.Map map, List<Setting> settings)
        {
            Model.ScaleParameter source = map.ScaleParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.ScaleParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, SCALE_PARAMETER_MAP_SCALE_CODE, source.MapScale, "0.00");
                Write(b, SCALE_PARAMETER_PAPER_GRID_DISTANCE_MM_CODE, source.PaperGridDistance[4, Distance.Unit.Metre, Scale.milli], "0.0000");
                Write(b, SCALE_PARAMETER_USE_REAL_WORLD_GRID_CODE, source.UseRealWorldGrid);
                Write(b, SCALE_PARAMETER_REAL_WORLD_OFFSET_X_CODE, source.RealWorldOffsetX[0, Distance.Unit.Metre, Scale.one], "0");
                Write(b, SCALE_PARAMETER_REAL_WORLD_OFFSET_Y_CODE, source.RealWorldOffsetY[0, Distance.Unit.Metre, Scale.one], "0");
                Write(b, SCALE_PARAMETER_REAL_WORLD_ANGLE_CODE, source.RealWorldAngle, "0.00");
                Write(b, SCALE_PARAMETER_REAL_WORLD_GRID_DISTANCE_M_CODE, source.RealWorldGridDistance[0, Distance.Unit.Metre, Scale.one], "0");
                Write<Model.Type.CoordinateSystemType, Int32>(b, SCALE_PARAMETER_REAL_WORLD_COORDINATE_SYSTEM_CODE, source.RealWorldCoordinateSystem);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}