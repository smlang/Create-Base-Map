using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String IMPORT_LAYER_NUMBER = "n";

        private void CopyToModelImportLayer(Model.Map map)
        {
            Int16 number = 0;
            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case IMPORT_LAYER_NUMBER:
                        number = GetInt16Value(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }

            Model.ImportLayer setting = MapExtension.GetOrCreateImportLayer(map, number, true);
            setting.LayerName = _mainValue;
        }

        private static void CopyFromModelImportLayers(Model.Map map, List<Setting> settings)
        {
            foreach (Model.ImportLayer source in map.ImportLayers)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.ImportLayer;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.LayerName);
                Write(b, IMPORT_LAYER_NUMBER, source.LayerNumber);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
