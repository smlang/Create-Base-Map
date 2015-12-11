using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String PREVIEW_OBJECT_CONTROL_TEXT = "d";
        private const String PREVIEW_OBJECT_CONTROL_CIRCLE = "o";
        private const String PREVIEW_OBJECT_START_POINT_OF_LINE = "f";
        private const String PREVIEW_OBJECT_END_POINT_OF_LINE = "t";
        private const String PREVIEW_OBJECT_ALL_COURSES = "<All>";

        private void CopyToEventPreviewObject(Model.Map map)
        {
            if (map.Event == null)
            {
                map.Event = new Event.Event();
            }

            String textControlCode = null;
            String circleControlCode = null;
            String startControlCodeOfLeg = null;
            String endControlCodeOfLeg = null;

            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case PREVIEW_OBJECT_CONTROL_TEXT:
                        textControlCode = GetStringValue(i);
                        break;
                    case PREVIEW_OBJECT_CONTROL_CIRCLE:
                        circleControlCode = GetStringValue(i);
                        break;
                    case PREVIEW_OBJECT_START_POINT_OF_LINE:
                        startControlCodeOfLeg = GetStringValue(i);
                        break;
                    case PREVIEW_OBJECT_END_POINT_OF_LINE:
                        endControlCodeOfLeg = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }

            if (_mainValue.Equals(PREVIEW_OBJECT_ALL_COURSES, StringComparison.InvariantCultureIgnoreCase))
            {
                if (textControlCode != null)
                {
                    Ocad.Event.Control courseObject = (Ocad.Event.Control)MapExtension.GetOrCreateEventObject(map, textControlCode);
                    courseObject.ControlCodeObject = map.Objects[ModelObjectIndex - 1];
                }
                else if (circleControlCode != null)
                {
                    Ocad.Event.Control courseObject = (Ocad.Event.Control)MapExtension.GetOrCreateEventObject(map, circleControlCode);
                    courseObject.ControlCirleObject = map.Objects[ModelObjectIndex - 1];
                }
            }
            else if (textControlCode != null)
            {
                Ocad.Event.Course.ControlDescription courseControlObject = MapExtension.GetOrCreateEventControlDescription(map, _mainValue, textControlCode, true);
                courseControlObject.TextObject = map.Objects[ModelObjectIndex - 1];
            }
            else
            {
                Model.AbstractObject modelObject = map.Objects[ModelObjectIndex - 1];
                Ocad.Event.Course.Leg courseLeg = MapExtension.GetOrCreateEventCourseLeg(map, _mainValue, startControlCodeOfLeg, endControlCodeOfLeg, modelObject, true);
            }
        }

        private static void CopyFromEventPreviewObjects(Model.Map map, List<Setting> settings)
        {
            foreach (Event.AbstractObject o in map.Event.Objects)
            {
                if (!(o is Event.Control))
                {
                    continue;
                }
                Event.Control eventControl = (Event.Control)o;

                if (eventControl.ControlCodeObject != null)
                {
                    Setting setting = new Setting();
                    setting.SettingType = Type.SettingType.EventPreviewObject;
                    setting.ModelObjectIndex = eventControl.ControlCodeObject.Index;
                    settings.Add(setting);

                    StringBuilder b = new StringBuilder(PREVIEW_OBJECT_ALL_COURSES);
                    Write(b, PREVIEW_OBJECT_CONTROL_TEXT, eventControl);
                    setting.ConcatenatedValues = b.ToString();
                }

                if ((eventControl.ControlType == Event.Type.EventControlType.Control) && (eventControl.ControlCirleObject != null))
                {
                    Setting setting = new Setting();
                    setting.SettingType = Type.SettingType.EventPreviewObject;
                    setting.ModelObjectIndex = eventControl.ControlCirleObject.Index;
                    settings.Add(setting);

                    StringBuilder b = new StringBuilder(PREVIEW_OBJECT_ALL_COURSES);
                    Write(b, PREVIEW_OBJECT_CONTROL_CIRCLE, eventControl);
                    setting.ConcatenatedValues = b.ToString();
                }
            }

            foreach (Event.Course.Course course in map.Event.Courses)
            {
                foreach (Event.Course.ControlDescription courseControl in course.CourseControls)
                {
                    if (courseControl.Type == Event.Type.EventCourseObjectType.Control)
                    {
                        Setting setting = new Setting();
                        setting.SettingType = Type.SettingType.EventPreviewObject;
                        setting.ModelObjectIndex = courseControl.TextObject.Index;
                        settings.Add(setting);

                        StringBuilder b = new StringBuilder(course.Name);
                        Write(b, PREVIEW_OBJECT_CONTROL_TEXT, courseControl.EventObject);
                        setting.ConcatenatedValues = b.ToString();
                    }
                }
                foreach (Event.Course.Leg courseLeg in course.CourseLegs)
                {
                    foreach (Model.AbstractObject modelObject in courseLeg.ModelObjects)
                    {
                        Setting setting = new Setting();
                        setting.SettingType = Type.SettingType.EventPreviewObject;
                        setting.ModelObjectIndex = modelObject.Index;
                        settings.Add(setting);

                        StringBuilder b = new StringBuilder(course.Name);
                        Write(b, PREVIEW_OBJECT_START_POINT_OF_LINE, courseLeg.StartWayPoint.EventObject);
                        Write(b, PREVIEW_OBJECT_END_POINT_OF_LINE, courseLeg.EndWayPoint.EventObject);
                        setting.ConcatenatedValues = b.ToString();
                    }
                }
            }
        }
    }
}
