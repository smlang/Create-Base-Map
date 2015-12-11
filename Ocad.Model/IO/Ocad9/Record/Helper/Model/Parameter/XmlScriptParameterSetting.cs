using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String XML_SCRIPT_PARAMETER_LAST_FILE_USED = "f";

        private void CopyToModelXmlScriptParameter(Model.Map map)
        {
            Model.XmlScriptParameter setting = new Model.XmlScriptParameter();
            map.XmlScriptParameter = setting;

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
                    case XML_SCRIPT_PARAMETER_LAST_FILE_USED:
                        setting.LastFileUsed = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelXmlScriptParameter(Model.Map map, List<Setting> settings)
        {
            Model.XmlScriptParameter source = map.XmlScriptParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.XmlScriptParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, XML_SCRIPT_PARAMETER_LAST_FILE_USED, source.LastFileUsed);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
