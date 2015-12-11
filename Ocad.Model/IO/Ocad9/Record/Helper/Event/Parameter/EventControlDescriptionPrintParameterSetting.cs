using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String EVENT_CONTROL_DESCRIPTION_PRINT_PARAMETER_SIZE = "s";

        private void CopyToEventControlDescriptionPrintParameter(Model.Map map)
        {
            if (map.Event == null)
            {
                map.Event = new Event.Event();
            }
            Event.ControlDescriptionPrintParameter setting = new Event.ControlDescriptionPrintParameter();
            map.Event.ControlDescriptionPrintParameter = setting;

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
                    case EVENT_CONTROL_DESCRIPTION_PRINT_PARAMETER_SIZE:
                        setting.Size = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromEventControlDescriptionPrintParameter(Model.Map map, List<Setting> settings)
        {
            Event.ControlDescriptionPrintParameter source = map.Event.ControlDescriptionPrintParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.EventParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, EVENT_CONTROL_DESCRIPTION_PRINT_PARAMETER_SIZE, source.Size);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
