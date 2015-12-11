using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geometry;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String ZOOM_OFFSET_CENTRE_X_MM = "x";
        private const String ZOOM_OFFSET_CENTRE_Y_MM = "y";
        private const String ZOOM_DEPTH = "z";

        private void CopyToModelZoom(Model.Map map)
        {
            Model.Zoom setting = new Model.Zoom();
            map.Zooms.Add(setting);

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
                    case ZOOM_OFFSET_CENTRE_X_MM:
                        setting.OffsetCentreX = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case ZOOM_OFFSET_CENTRE_Y_MM:
                        setting.OffsetCentreY = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case ZOOM_DEPTH:
                        setting.Depth = GetDecimalValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelZooms(Model.Map map, List<Setting> settings)
        {
            foreach (Model.Zoom source in map.Zooms)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.Zoom;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, ZOOM_OFFSET_CENTRE_X_MM, source.OffsetCentreX[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, ZOOM_OFFSET_CENTRE_Y_MM, source.OffsetCentreY[6, Distance.Unit.Metre, Scale.milli], "0.000000");
                Write(b, ZOOM_DEPTH, source.Depth, "0.000000");
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
