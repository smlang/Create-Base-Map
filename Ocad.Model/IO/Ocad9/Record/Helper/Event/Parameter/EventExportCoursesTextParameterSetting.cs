using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String EVENT_EXPORT_COURSES_TEXT_PARAMETER_COURSES_OR_CLASSES = "C";
        private const String EVENT_EXPORT_COURSES_TEXT_PARAMETER_INCLUDE_CLIMB = "L";
        private const String EVENT_EXPORT_COURSES_TEXT_PARAMETER_INCLUDE_NUMBER_OF_CONTROLS = "N";

        private void CopyToEventExportCoursesTextParameter(Model.Map map)
        {
            if (map.Event == null)
            {
                map.Event = new Event.Event();
            }
            Event.ExportCoursesTextParameter setting = new Event.ExportCoursesTextParameter();
            map.Event.ExportCoursesTextParameter = setting;

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
                    case EVENT_EXPORT_COURSES_TEXT_PARAMETER_COURSES_OR_CLASSES:
                        setting.CoursesOrClasses = (Event.Type.CoursesOrClasses)GetInt32Value(i);
                        break;
                    case EVENT_EXPORT_COURSES_TEXT_PARAMETER_INCLUDE_CLIMB:
                        setting.IncludeClimb = GetBooleanValue(i);
                        break;
                    case EVENT_EXPORT_COURSES_TEXT_PARAMETER_INCLUDE_NUMBER_OF_CONTROLS:
                        setting.IncludeNumberOfControls = GetBooleanValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromEventExportCoursesTextParameter(Model.Map map, List<Setting> settings)
        {
            Event.ExportCoursesTextParameter source = map.Event.ExportCoursesTextParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.EventParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write<Event.Type.CoursesOrClasses, Int32>(b, EVENT_EXPORT_COURSES_TEXT_PARAMETER_COURSES_OR_CLASSES, source.CoursesOrClasses);
                Write(b, EVENT_EXPORT_COURSES_TEXT_PARAMETER_INCLUDE_CLIMB, source.IncludeClimb);
                Write(b, EVENT_EXPORT_COURSES_TEXT_PARAMETER_INCLUDE_NUMBER_OF_CONTROLS, source.IncludeNumberOfControls);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}