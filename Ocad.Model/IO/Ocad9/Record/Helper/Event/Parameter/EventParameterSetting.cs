using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geometry;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String EVENT_PARAMETER_CREATE_CLASSES_AUTOMATICALLY = "a";
        private const String EVENT_PARAMETER_WHITE_BACKGROUND_FOR_CONTROL_DESCRIPTION = "b";
        private const String EVENT_PARAMETER_NUMBERING = "c";
        private const String EVENT_PARAMETER_ADD_CONTROL_DESCRIPTIONS_FOR_ALL_CONTROLS = "d";
        private const String EVENT_PARAMETER_EVENT_TITLE = "e";
        private const String EVENT_PARAMETER_CONTROL_DESCRIPTION_HORIZTONAL_THICKER_LINE = "h";
        private const String EVENT_PARAMETER_CONTROL_DESCRIPTION_MAXIMUM_ROWS = "i";
        private const String EVENT_PARAMETER_DISTANCE_FROM_CIRCLE_TO_NUMBER_MM = "l";
        private const String EVENT_PARAMETER_DISTANCE_FROM_CIRCLE_TO_CONTROL_DESCRIPTION_MM = "n";
        private const String EVENT_PARAMETER_CONTROL_DESCRIPTION_BOX_SIZE_MM = "s";
        private const String EVENT_PARAMETER_COURSE_TITLE = "t";
        private const String EVENT_PARAMETER_EXPORT_RELAY_COMBINATION_IN_XML_FILE = "r";

        private void CopyToEventParameter(Model.Map map)
        {
            if (map.Event == null)
            {
                map.Event = new Event.Event();
            }
            Event.Parameter setting = new Event.Parameter();
            map.Event.Parameter = setting;

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
                    case EVENT_PARAMETER_CREATE_CLASSES_AUTOMATICALLY:
                        setting.CreateClassesAutomatically = GetBooleanValue(i);
                        break;
                    case EVENT_PARAMETER_WHITE_BACKGROUND_FOR_CONTROL_DESCRIPTION:
                        setting.WhiteBackgroundForControlDescription = GetBooleanValue(i);
                        break;
                    case EVENT_PARAMETER_NUMBERING:
                        setting.Numbering = (Event.Type.EventNumbering)GetInt32Value(i);
                        break;
                    case EVENT_PARAMETER_ADD_CONTROL_DESCRIPTIONS_FOR_ALL_CONTROLS:
                        setting.AddControlDescriptionsForAllControls = GetBooleanValue(i);
                        break;
                    case EVENT_PARAMETER_EVENT_TITLE:
                        setting.EventTitle = GetStringValue(i);
                        break;
                    case EVENT_PARAMETER_CONTROL_DESCRIPTION_HORIZTONAL_THICKER_LINE:
                        setting.ControlDescriptionHoriztonalThickerLine = (Event.Type.ControlDescriptionHoriztonalThickerLine)GetInt32Value(i);
                        break;
                    case EVENT_PARAMETER_CONTROL_DESCRIPTION_MAXIMUM_ROWS:
                        setting.ControlDescriptionMaximumRows = GetInt32Value(i);
                        break;
                    case EVENT_PARAMETER_DISTANCE_FROM_CIRCLE_TO_NUMBER_MM:
                        setting.DistanceFromCircleToNumber = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case EVENT_PARAMETER_DISTANCE_FROM_CIRCLE_TO_CONTROL_DESCRIPTION_MM:
                        setting.DistanceFromCircleToConnectionLine = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case EVENT_PARAMETER_CONTROL_DESCRIPTION_BOX_SIZE_MM:
                        setting.ControlDescriptionBoxSize = GetDistance(i, Distance.Unit.Metre, Scale.milli);
                        break;
                    case EVENT_PARAMETER_COURSE_TITLE:
                        setting.CourseTitle = (Event.Type.CourseTitle)GetInt32Value(i);
                        break;
                    case EVENT_PARAMETER_EXPORT_RELAY_COMBINATION_IN_XML_FILE:
                        setting.ExportRelayCombinationInXmlFile = GetBooleanValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromEventParameter(Model.Map map, List<Setting> settings)
        {
            Event.Parameter source = map.Event.Parameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.EventParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write<Event.Type.CourseTitle, Int32>(b, EVENT_PARAMETER_COURSE_TITLE, source.CourseTitle);
                Write<Event.Type.EventNumbering, Int32>(b, EVENT_PARAMETER_NUMBERING, source.Numbering);
                Write<Event.Type.ControlDescriptionHoriztonalThickerLine, Int32>(b, EVENT_PARAMETER_CONTROL_DESCRIPTION_HORIZTONAL_THICKER_LINE, source.ControlDescriptionHoriztonalThickerLine);
                Write(b, EVENT_PARAMETER_CONTROL_DESCRIPTION_MAXIMUM_ROWS, source.ControlDescriptionMaximumRows);
                Write(b, EVENT_PARAMETER_DISTANCE_FROM_CIRCLE_TO_CONTROL_DESCRIPTION_MM, source.DistanceFromCircleToConnectionLine[2, Distance.Unit.Metre, Scale.milli], "0.00");
                Write(b, EVENT_PARAMETER_DISTANCE_FROM_CIRCLE_TO_NUMBER_MM, source.DistanceFromCircleToNumber[2, Distance.Unit.Metre, Scale.milli], "0.00");
                Write(b, EVENT_PARAMETER_CONTROL_DESCRIPTION_BOX_SIZE_MM, source.ControlDescriptionBoxSize[2, Distance.Unit.Metre, Scale.milli], "0.00");
                Write(b, EVENT_PARAMETER_WHITE_BACKGROUND_FOR_CONTROL_DESCRIPTION, source.WhiteBackgroundForControlDescription);
                Write(b, EVENT_PARAMETER_ADD_CONTROL_DESCRIPTIONS_FOR_ALL_CONTROLS, source.AddControlDescriptionsForAllControls);
                Write(b, EVENT_PARAMETER_EXPORT_RELAY_COMBINATION_IN_XML_FILE, source.ExportRelayCombinationInXmlFile);
                Write(b, EVENT_PARAMETER_CREATE_CLASSES_AUTOMATICALLY, source.CreateClassesAutomatically);
                Write(b, EVENT_PARAMETER_EVENT_TITLE, source.EventTitle);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}