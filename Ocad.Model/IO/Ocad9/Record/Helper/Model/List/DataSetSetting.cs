using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String DATA_SET_DBASE_FILE = "e";
        private const String DATA_SET_ODBC_DATA_SOURCE = "d";
        private const String DATA_SET_TABLE = "t";
        private const String DATA_SET_KEY_FIELD = "k";
        private const String DATA_SET_SYMBOL_FIELD = "y";
        private const String DATA_SET_TEXT_FIELD = "x";
        private const String DATA_SET_SIZE_FIELD = "f";
        private const String DATA_SET_LENGTH_UNIT = "l";
        private const String DATA_SET_AREA_UNIT = "a";
        private const String DATA_SET_DECIMALS = "c";
        private const String DATA_SET_HORIZONTAL_COORDINATE = "h";
        private const String DATA_SET_VERTICAL_COORDINATE = "v";

        private void CopyToModelDataSet(Model.Map map)
        {
            Model.DataSet setting = new Model.DataSet();
            map.DataSets.Add(setting);
            setting.DataSetName = _mainValue;

            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case DATA_SET_DBASE_FILE:
                        setting.DBaseFile = GetStringValue(i);
                        break;
                    case DATA_SET_ODBC_DATA_SOURCE:
                        setting.OdbcDataSource = GetStringValue(i);
                        break;
                    case DATA_SET_TABLE:
                        setting.Table = GetStringValue(i);
                        break;
                    case DATA_SET_KEY_FIELD:
                        setting.KeyField = GetStringValue(i);
                        break;
                    case DATA_SET_SYMBOL_FIELD:
                        setting.SymbolField = GetStringValue(i);
                        break;
                    case DATA_SET_TEXT_FIELD:
                        setting.TextField = GetStringValue(i);
                        break;
                    case DATA_SET_SIZE_FIELD:
                        setting.SizeField = GetStringValue(i);
                        break;
                    case DATA_SET_LENGTH_UNIT:
                        setting.LengthUnit = GetStringValue(i);
                        break;
                    case DATA_SET_AREA_UNIT:
                        setting.AreaUnit = GetStringValue(i);
                        break;
                    case DATA_SET_DECIMALS:
                        setting.Decimals = GetStringValue(i);
                        break;
                    case DATA_SET_HORIZONTAL_COORDINATE:
                        setting.HorizontalCoordinate = GetStringValue(i);
                        break;
                    case DATA_SET_VERTICAL_COORDINATE:
                        setting.VerticalCoordinate = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelDataSets(Model.Map map, List<Setting> settings)
        {
            foreach (Model.DataSet source in map.DataSets)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.DataSet;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.DataSetName);
                Write(b, DATA_SET_DBASE_FILE, source.DBaseFile);
                Write(b, DATA_SET_ODBC_DATA_SOURCE, source.OdbcDataSource);
                Write(b, DATA_SET_TABLE, source.Table);
                Write(b, DATA_SET_KEY_FIELD, source.KeyField);
                Write(b, DATA_SET_SYMBOL_FIELD, source.SymbolField);
                Write(b, DATA_SET_TEXT_FIELD, source.TextField);
                Write(b, DATA_SET_SIZE_FIELD, source.SizeField);
                Write(b, DATA_SET_LENGTH_UNIT, source.LengthUnit);
                Write(b, DATA_SET_AREA_UNIT, source.AreaUnit);
                Write(b, DATA_SET_DECIMALS, source.Decimals);
                Write(b, DATA_SET_HORIZONTAL_COORDINATE, source.HorizontalCoordinate);
                Write(b, DATA_SET_VERTICAL_COORDINATE, source.VerticalCoordinate);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
