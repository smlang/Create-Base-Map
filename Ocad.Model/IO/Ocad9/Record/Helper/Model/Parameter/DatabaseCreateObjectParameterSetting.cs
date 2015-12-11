using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String DATABASE_CREATE_OBJECT_PARAMETER_CONDITION = "c";
        private const String DATABASE_CREATE_OBJECT_PARAMETER_DATA_SET = "d";
        private const String DATABASE_CREATE_OBJECT_PARAMETER_TEXT_FIELD = "t";
        private const String DATABASE_CREATE_OBJECT_PARAMETER_UNIT_OF_MEASURE = "m";
        private const String DATABASE_CREATE_OBJECT_PARAMETER_HORIZONTAL_OFFSET = "u";
        private const String DATABASE_CREATE_OBJECT_PARAMETER_VERTICAL_OFFSET = "v";
        private const String DATABASE_CREATE_OBJECT_PARAMETER_HORIZONTAL_FIELD = "x";
        private const String DATABASE_CREATE_OBJECT_PARAMETER_VERTICAL_FIELD = "y";

        private void CopyToModelDatabaseCreateObjectParameter(Model.Map map)
        {
            Model.DatabaseCreateObjectParameter setting = new Model.DatabaseCreateObjectParameter();
            map.DatabaseCreateObjectParameter = setting;

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
                    case DATABASE_CREATE_OBJECT_PARAMETER_CONDITION:
                        setting.Condition = GetStringValue(i);
                        break;
                    case DATABASE_CREATE_OBJECT_PARAMETER_DATA_SET:
                        setting.DataSet = GetStringValue(i);
                        break;
                    case DATABASE_CREATE_OBJECT_PARAMETER_TEXT_FIELD:
                        setting.TextField = GetStringValue(i);
                        break;
                    case DATABASE_CREATE_OBJECT_PARAMETER_UNIT_OF_MEASURE:
                        setting.UnitOfMeasure = GetStringValue(i);
                        break;
                    case DATABASE_CREATE_OBJECT_PARAMETER_HORIZONTAL_OFFSET:
                        setting.HorizontalOffset = GetStringValue(i);
                        break;
                    case DATABASE_CREATE_OBJECT_PARAMETER_VERTICAL_OFFSET:
                        setting.VerticalOffset = GetStringValue(i);
                        break;
                    case DATABASE_CREATE_OBJECT_PARAMETER_HORIZONTAL_FIELD:
                        setting.HorizontalField = GetStringValue(i);
                        break;
                    case DATABASE_CREATE_OBJECT_PARAMETER_VERTICAL_FIELD:
                        setting.VerticalField = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelDatabaseCreateObjectParameter(Model.Map map, List<Setting> settings)
        {
            Model.DatabaseCreateObjectParameter source = map.DatabaseCreateObjectParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.DatabaseCreateObjectParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, DATABASE_CREATE_OBJECT_PARAMETER_CONDITION, source.Condition);
                Write(b, DATABASE_CREATE_OBJECT_PARAMETER_DATA_SET, source.DataSet);
                Write(b, DATABASE_CREATE_OBJECT_PARAMETER_TEXT_FIELD, source.TextField);
                Write(b, DATABASE_CREATE_OBJECT_PARAMETER_UNIT_OF_MEASURE, source.UnitOfMeasure);
                Write(b, DATABASE_CREATE_OBJECT_PARAMETER_HORIZONTAL_OFFSET, source.HorizontalOffset);
                Write(b, DATABASE_CREATE_OBJECT_PARAMETER_VERTICAL_OFFSET, source.VerticalOffset);
                Write(b, DATABASE_CREATE_OBJECT_PARAMETER_HORIZONTAL_FIELD, source.HorizontalField);
                Write(b, DATABASE_CREATE_OBJECT_PARAMETER_VERTICAL_FIELD, source.VerticalField);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
