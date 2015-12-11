using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String DATABASE_OBJECT_DATA_SET = "d";

        private void CopyToModelDatabaseObject(Model.Map map)
        {
            Model.DatabaseObject setting = new Model.DatabaseObject();
            map.DatabaseObjects.Add(setting);

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
                    case DATABASE_OBJECT_DATA_SET:
                        setting.DataSet = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelDatabaseObjects(Model.Map map, List<Setting> settings)
        {
            foreach (Model.DatabaseObject source in map.DatabaseObjects)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.DatabaseObject;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, DATABASE_OBJECT_DATA_SET, source.DataSet);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
