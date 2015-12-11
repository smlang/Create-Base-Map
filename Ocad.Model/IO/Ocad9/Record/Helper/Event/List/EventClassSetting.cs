using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String CLASS_COURSE_NAME = "c";
        private const String CLASS_NUMBER_OF_RUNNERS = "r";
        private const String CLASS_FROM_NUMBER = "f";
        private const String CLASS_TO_NUMBER = "t";

        private void CopyToEventClass(Model.Map map)
        {
            if (map.Event == null)
            {
                map.Event = new Event.Event();
            }
            Event.Class setting = new Event.Class();
            setting.Name = _mainValue;

            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case CLASS_COURSE_NAME:
                        Event.Course.Course course = MapExtension.GetOrCreateEventCourse(map, GetStringValue(i));
                        course.Classes.Add(setting);
                        break;
                    case CLASS_NUMBER_OF_RUNNERS:
                        setting.NumberOfRunners = GetInt32Value(i);
                        break;
                    case CLASS_FROM_NUMBER:
                        setting.FromBibNumber = GetInt32Value(i);
                        break;
                    case CLASS_TO_NUMBER:
                        setting.ToBibNumber = GetInt32Value(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromEventClasses(Event.Course.Course course, List<Setting> settings)
        {
            foreach (Event.Class source in course.Classes)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.EventClass;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.Name);
                Write(b, CLASS_COURSE_NAME, course.Name);
                Write(b, CLASS_NUMBER_OF_RUNNERS, source.NumberOfRunners);
                Write(b, CLASS_FROM_NUMBER, source.FromBibNumber);
                Write(b, CLASS_TO_NUMBER, source.ToBibNumber);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
