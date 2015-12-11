using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String EPS_PARAMETER_RESOLUTION = "r";

        private void CopyToModelEpsParameter(Model.Map map)
        {
            Model.EpsParameter setting = new Model.EpsParameter();
            map.EpsParameter = setting;

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
                    case EPS_PARAMETER_RESOLUTION:
                        setting.Resolution = GetDecimalValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelEpsParameter(Model.Map map, List<Setting> settings)
        {
            Model.EpsParameter source = map.EpsParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.EpsParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, EPS_PARAMETER_RESOLUTION, source.Resolution, "0.0#");
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
