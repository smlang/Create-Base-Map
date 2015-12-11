using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String DATABASE_PARAMETER_DATA_SET = "d";
        private const String DATABASE_PARAMETER_LAST_CODE = "l";
        private const String DATABASE_PARAMETER_CREATE_NEW_RECORD = "n";

        private void CopyToModelDatabaseParameter(Model.Map map)
        {
            Model.DatabaseParameter setting = new Model.DatabaseParameter();
            map.DatabaseParameter = setting;

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
                    case DATABASE_PARAMETER_DATA_SET:
                        setting.DataSet = GetStringValue(i);
                        break;
                    case DATABASE_PARAMETER_LAST_CODE:
                        setting.LastCode = GetStringValue(i);
                        break;
                    case DATABASE_PARAMETER_CREATE_NEW_RECORD:
                        setting.CreateNewRecord = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelDatabaseParameter(Model.Map map, List<Setting> settings)
        {
            Model.DatabaseParameter source = map.DatabaseParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.DatabaseParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, DATABASE_PARAMETER_DATA_SET, source.DataSet);
                Write(b, DATABASE_PARAMETER_LAST_CODE, source.LastCode);
                Write(b, DATABASE_PARAMETER_CREATE_NEW_RECORD, source.CreateNewRecord);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
