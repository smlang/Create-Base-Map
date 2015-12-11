using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private void CopyToModelFileInfo(Model.Map map)
        {
            Model.FileInfo setting = new Model.FileInfo();
            map.FileInfos.Add(setting);
            setting.Value = _mainValue;

            if (_codeValue.GetUpperBound(0) > 0)
            {
                CreateApplicationSettingException(1);
            }
        }

        private static void CopyFromModelFileInfos(Model.Map map, List<Setting> settings)
        {
            foreach (Model.FileInfo source in map.FileInfos)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.FileInfo;
                settings.Add(setting);
                setting.ConcatenatedValues = source.Value;
            }
        }
    }
}
