using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private void CopyToModelSelectedSpotColoursParameter(Model.Map map)
        {
            Model.SelectedSpotColoursParameter setting = new Model.SelectedSpotColoursParameter();
            map.SelectedSpotColoursParameter = setting;

            setting.Value = _mainValue;
            if (_codeValue.GetUpperBound(0) > 0)
            {
                CreateApplicationSettingException(1);
            }
        }

        private static void CopyFromModelSelectedSpotColoursParameter(Model.Map map, List<Setting> settings)
        {
            Model.SelectedSpotColoursParameter source = map.SelectedSpotColoursParameter;
            if (map.SelectedSpotColoursParameter != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.SelectedSpotColoursParameter;
                settings.Add(setting);
                setting.ConcatenatedValues = source.Value;
            }
        }
    }
}
