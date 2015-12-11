using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private void CopyToModelOimFile(Model.Map map)
        {
            Model.OimFile setting = new Model.OimFile();
            map.OimFiles.Add(setting);
            setting.FileName = _mainValue;

            if (_codeValue.GetUpperBound(0) > 0)
            {
                CreateApplicationSettingException(1);
            }
        }

        private static void CopyFromModelOimFiles(Model.Map map, List<Setting> settings)
        {
            foreach (Model.OimFile source in map.OimFiles)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.OimFile;
                settings.Add(setting);
                setting.ConcatenatedValues = source.FileName;
            }
        }
    }
}
