using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String TEMPLATE_PARAMETER_DEFAULT_SCALE = "s";

        private void CopyToModelTemplateParameter(Model.Map map)
        {
            Model.TemplateParameter setting = new Model.TemplateParameter();
            map.TemplateParameter = setting;

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
                    case TEMPLATE_PARAMETER_DEFAULT_SCALE:
                        setting.DefaultScale = GetInt32Value(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelTemplateParameter(Model.Map map, List<Setting> settings)
        {
            Model.TemplateParameter source = map.TemplateParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.TemplateParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, TEMPLATE_PARAMETER_DEFAULT_SCALE, source.DefaultScale);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
