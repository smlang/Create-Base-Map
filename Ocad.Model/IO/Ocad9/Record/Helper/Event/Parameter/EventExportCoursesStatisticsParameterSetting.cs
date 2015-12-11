using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_COURSES_OR_CLASSES = "C";
        private const String EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_SEPARATOR_1 = "a";
        private const String EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_TAB_1 = "b";
        private const String EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_SEPARATOR_2 = "c";
        private const String EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_TAB_2 = "d";
        private const String EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_SEPARATOR_3 = "e";
        private const String EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_TAB_3 = "f";

        private void CopyToEventExportCoursesStatisticsParameter(Model.Map map)
        {
            if (map.Event == null)
            {
                map.Event = new Event.Event();
            }
            Event.ExportCoursesStatisticsParameter setting = new Event.ExportCoursesStatisticsParameter();
            map.Event.ExportCoursesStatisticsParameter = setting;

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
                    case EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_COURSES_OR_CLASSES:
                        setting.CoursesOrClasses = (Event.Type.CoursesOrClasses)GetInt32Value(i);
                        break;
                    case EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_SEPARATOR_1:
                        setting.Separator1 = GetStringValue(i);
                        break;
                    case EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_TAB_1:
                        setting.Tab1 = GetStringValue(i);
                        break;
                    case EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_SEPARATOR_2:
                        setting.Separator2 = GetStringValue(i);
                        break;
                    case EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_TAB_2:
                        setting.Tab2 = GetStringValue(i);
                        break;
                    case EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_SEPARATOR_3:
                        setting.Separator3 = GetStringValue(i);
                        break;
                    case EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_TAB_3:
                        setting.Tab3 = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromEventExportCoursesStatisticsParameter(Model.Map map, List<Setting> settings)
        {
            Event.ExportCoursesStatisticsParameter source = map.Event.ExportCoursesStatisticsParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.EventParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write<Event.Type.CoursesOrClasses, Int32>(b, EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_COURSES_OR_CLASSES, source.CoursesOrClasses);
                Write(b, EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_SEPARATOR_1, source.Separator1);
                Write(b, EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_TAB_1, source.Tab1);
                Write(b, EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_SEPARATOR_2, source.Separator2);
                Write(b, EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_TAB_2, source.Tab2);
                Write(b, EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_SEPARATOR_3, source.Separator3);
                Write(b, EVENT_EXPORT_COURSES_STATISTICS_PARAMETER_TAB_3, source.Tab3);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
