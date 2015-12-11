using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String OIM_FIND_CONDITION = "c";
        private const String OIM_FIND_DATA_SET = "d";
        private const String OIM_FIND_FROM_ZOOM = "f";
        private const String OIM_FIND_HINT_FIELD = "h";
        private const String OIM_FIND_NAME_FIELD = "n";
        private const String OIM_FIND_LIST_NAMES = "l";
        private const String OIM_FIND_HOTSPOT_TYPE = "o";
        private const String OIM_FIND_POINTER_TYPE = "p";
        private const String OIM_FIND_SHOW_HOTSPOTS = "s";
        private const String OIM_FIND_TO_ZOOM = "t";
        private const String OIM_FIND_URL_FIELD = "u";
        private const String OIM_FIND_PREFIX = "x";
        private const String OIM_FIND_POSTFIX = "y";
        private const String OIM_FIND_TARGET = "z";
        private const String OIM_FIND_POINTER_COLOUR_RED = "r";
        private const String OIM_FIND_POINTER_COLOUR_GREEN = "g";
        private const String OIM_FIND_POINTER_COLOUR_BLUE = "b";
        private const String OIM_FIND_HOTSPOT_COLOUR_RED = "R";
        private const String OIM_FIND_HOTSPOT_COLOUR_GREEN = "G";
        private const String OIM_FIND_HOTSPOT_COLOUR_BLUE = "B";

        private void CopyToModelOimFind(Model.Map map)
        {
            Model.OimFind setting = new Model.OimFind();
            map.OimFinds.Add(setting);

            setting.Name = _mainValue;

            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case OIM_FIND_CONDITION:
                        setting.Condition = GetStringValue(i);
                        break;
                    case OIM_FIND_DATA_SET:
                        setting.DataSet = GetStringValue(i);
                        break;
                    case OIM_FIND_FROM_ZOOM:
                        setting.FromZoom = GetStringValue(i);
                        break;
                    case OIM_FIND_HINT_FIELD:
                        setting.HintField = GetStringValue(i);
                        break;
                    case OIM_FIND_NAME_FIELD:
                        setting.NameField = GetStringValue(i);
                        break;
                    case OIM_FIND_LIST_NAMES:
                        setting.ListNames = GetStringValue(i);
                        break;
                    case OIM_FIND_HOTSPOT_TYPE:
                        setting.HotspotType = GetStringValue(i);
                        break;
                    case OIM_FIND_POINTER_TYPE:
                        setting.PointerType = GetStringValue(i);
                        break;
                    case OIM_FIND_SHOW_HOTSPOTS:
                        setting.ShowHotspots = GetStringValue(i);
                        break;
                    case OIM_FIND_TO_ZOOM:
                        setting.ToZoom = GetStringValue(i);
                        break;
                    case OIM_FIND_URL_FIELD:
                        setting.UrlField = GetStringValue(i);
                        break;
                    case OIM_FIND_PREFIX:
                        setting.Prefix = GetStringValue(i);
                        break;
                    case OIM_FIND_POSTFIX:
                        setting.Postfix = GetStringValue(i);
                        break;
                    case OIM_FIND_TARGET:
                        setting.Target = GetStringValue(i);
                        break;
                    case OIM_FIND_POINTER_COLOUR_RED:
                        setting.PointerColourRed = GetStringValue(i);
                        break;
                    case OIM_FIND_POINTER_COLOUR_GREEN:
                        setting.PointerColourBlue = GetStringValue(i);
                        break;
                    case OIM_FIND_POINTER_COLOUR_BLUE:
                        setting.PointerColourGreen = GetStringValue(i);
                        break;
                    case OIM_FIND_HOTSPOT_COLOUR_RED:
                        setting.HotspotColourRed = GetStringValue(i);
                        break;
                    case OIM_FIND_HOTSPOT_COLOUR_GREEN:
                        setting.HotspotColourGreen = GetStringValue(i);
                        break;
                    case OIM_FIND_HOTSPOT_COLOUR_BLUE:
                        setting.HotspotColourBlue = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelOimFinds(Model.Map map, List<Setting> settings)
        {
            foreach (Model.OimFind source in map.OimFinds)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.OimFind;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.Name);
                Write(b, OIM_FIND_CONDITION, source.Condition);
                Write(b, OIM_FIND_DATA_SET, source.DataSet);
                Write(b, OIM_FIND_FROM_ZOOM, source.FromZoom);
                Write(b, OIM_FIND_HINT_FIELD, source.HintField);
                Write(b, OIM_FIND_NAME_FIELD, source.NameField);
                Write(b, OIM_FIND_LIST_NAMES, source.ListNames);
                Write(b, OIM_FIND_HOTSPOT_TYPE, source.HotspotType);
                Write(b, OIM_FIND_POINTER_TYPE, source.PointerType);
                Write(b, OIM_FIND_SHOW_HOTSPOTS, source.ShowHotspots);
                Write(b, OIM_FIND_TO_ZOOM, source.ToZoom);
                Write(b, OIM_FIND_URL_FIELD, source.UrlField);
                Write(b, OIM_FIND_PREFIX, source.Prefix);
                Write(b, OIM_FIND_POSTFIX, source.Postfix);
                Write(b, OIM_FIND_TARGET, source.Target);
                Write(b, OIM_FIND_POINTER_COLOUR_RED, source.PointerColourRed);
                Write(b, OIM_FIND_POINTER_COLOUR_GREEN, source.PointerColourGreen);
                Write(b, OIM_FIND_POINTER_COLOUR_BLUE, source.PointerColourBlue);
                Write(b, OIM_FIND_HOTSPOT_COLOUR_RED, source.HotspotColourRed);
                Write(b, OIM_FIND_HOTSPOT_COLOUR_GREEN, source.HotspotColourGreen);
                Write(b, OIM_FIND_HOTSPOT_COLOUR_BLUE, source.HotspotColourBlue);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
