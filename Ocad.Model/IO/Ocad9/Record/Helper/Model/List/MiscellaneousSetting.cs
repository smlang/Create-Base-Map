using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private void CopyToModelMiscellaneous(Model.Map map)
        {
            Model.Miscellaneous setting = new Model.Miscellaneous()
            {
                TypeId = (Int32)SettingType,
                MainValue = _mainValue,
                CodeValue = _codeValue,
                Index = ModelObjectIndex
            };
            map.MiscellaneousSettings.Add(setting);
        }

        private static void CopyFromModelMiscellaneousSettings(Model.Map map, List<Setting> settings)
        {
            foreach (Model.Miscellaneous source in map.MiscellaneousSettings)
            {
                Setting setting = new Setting();
                setting.SettingType = (Type.SettingType)source.TypeId;
                setting.ModelObjectIndex = source.Index;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.MainValue);
                for (int i = 0; i < source.CodeValue.GetUpperBound(0); i++)
                {
                    Write(b, source.CodeValue[i, 0], source.CodeValue[i, 1]);
                }
                setting.ConcatenatedValues = b.ToString(); ;
            }
        }
    }
}
