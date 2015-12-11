using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String TILES_PARAMETER_WIDTH = "w";
        private const String TILES_PARAMETER_HEIGHT = "h";

        private void CopyToModelTilesParameter(Model.Map map)
        {
            Model.TilesParameter setting = new Model.TilesParameter();
            map.TilesParameter = setting;

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
                    case TILES_PARAMETER_WIDTH:
                        setting.Width = GetInt32Value(i);
                        break;
                    case TILES_PARAMETER_HEIGHT:
                        setting.Height = GetInt32Value(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelTilesParameter(Model.Map map, List<Setting> settings)
        {
            Model.TilesParameter source = map.TilesParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.TilesParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, TILES_PARAMETER_WIDTH, source.Width);
                Write(b, TILES_PARAMETER_HEIGHT, source.Height);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
