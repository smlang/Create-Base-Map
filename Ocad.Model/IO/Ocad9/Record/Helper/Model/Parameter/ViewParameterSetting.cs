using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geometry;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String VIEW_PARAMETER_DRAFT_MODE_IGN_FOR_OCAD_MAP = "b";
        private const String VIEW_PARAMETER_DRAFT_MODE_IGN_FOR_BACKGROUND_MAPS = "c";
        private const String VIEW_PARAMETER_HIDE_BACKGROUND_MAPS = "d";
        private const String VIEW_PARAMETER_DRAFT_MODE_FOR_OCAD_MAP = "m";
        private const String VIEW_PARAMETER_DRAFT_MODE_FOR_BACKGROUND_MAPS = "t";
        private const String VIEW_PARAMETER_VIEW_MODE = "v";
        private const String VIEW_PARAMETER_OFFSET_CENTRE_X_MM = "x";
        private const String VIEW_PARAMETER_OFFSET_CENTRE_Y_MM = "y";
        private const String VIEW_PARAMETER_HATCHED = "h";
        private const String VIEW_PARAMETER_ZOOM = "z";

        private void CopyToModelViewParameter(Model.Map map)
        {
            Model.ViewParameter setting = new Model.ViewParameter();
            map.ViewParameter = setting;

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
                    case VIEW_PARAMETER_DRAFT_MODE_IGN_FOR_OCAD_MAP:
                        setting.DraftModeIgnForOcadMap = GetByteValue(i);
                        break;
                    case VIEW_PARAMETER_DRAFT_MODE_IGN_FOR_BACKGROUND_MAPS:
                        setting.DraftModeIgnForBackgroundMaps = GetByteValue(i);
                        break;
                    case VIEW_PARAMETER_HIDE_BACKGROUND_MAPS:
                        setting.HideBackgroundMaps = GetBooleanValue(i);
                        break;
                    case VIEW_PARAMETER_DRAFT_MODE_FOR_OCAD_MAP:
                        setting.DraftModeForOcadMap = GetByteValue(i);
                        break;
                    case VIEW_PARAMETER_DRAFT_MODE_FOR_BACKGROUND_MAPS:
                        setting.DraftModeForBackgroundMaps = GetByteValue(i);
                        break;
                    case VIEW_PARAMETER_VIEW_MODE:
                        setting.ViewMode = GetByteValue(i);
                        break;
                    case VIEW_PARAMETER_OFFSET_CENTRE_X_MM:
                        setting.OffsetCentreX = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case VIEW_PARAMETER_OFFSET_CENTRE_Y_MM:
                        setting.OffsetCentreY = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case VIEW_PARAMETER_HATCHED:
                        setting.Hatched = GetBooleanValue(i);
                        break;
                    case VIEW_PARAMETER_ZOOM:
                        setting.Zoom = GetDecimalValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelViewParameter(Model.Map map, List<Setting> settings)
        {
            Model.ViewParameter source = map.ViewParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.ViewParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, VIEW_PARAMETER_OFFSET_CENTRE_X_MM, source.OffsetCentreX[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, VIEW_PARAMETER_OFFSET_CENTRE_Y_MM, source.OffsetCentreY[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, VIEW_PARAMETER_ZOOM, source.Zoom, "0.000000");
                Write(b, VIEW_PARAMETER_VIEW_MODE, source.ViewMode);
                Write(b, VIEW_PARAMETER_DRAFT_MODE_FOR_OCAD_MAP, source.DraftModeForOcadMap);
                Write(b, VIEW_PARAMETER_DRAFT_MODE_FOR_BACKGROUND_MAPS, source.DraftModeForBackgroundMaps);
                Write(b, VIEW_PARAMETER_DRAFT_MODE_IGN_FOR_OCAD_MAP, source.DraftModeIgnForOcadMap);
                Write(b, VIEW_PARAMETER_DRAFT_MODE_IGN_FOR_BACKGROUND_MAPS, source.DraftModeIgnForBackgroundMaps);
                Write(b, VIEW_PARAMETER_HATCHED, source.Hatched);
                Write(b, VIEW_PARAMETER_HIDE_BACKGROUND_MAPS, source.HideBackgroundMaps);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
