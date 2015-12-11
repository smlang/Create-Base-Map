using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String COURSE_START_CONTROL = "s";
        private const String COURSE_CONTROL = "c";
        private const String COURSE_MARKED_ROUTE = "m";
        private const String COURSE_MANDATORY_CROSSING_POINT = "k";
        private const String COURSE_MANDATORY_PASSAGE = "w";
        private const String COURSE_MAP_EXCHANGE = "g";
        private const String COURSE_TEXT_BLOCK = "t";
        private const String COURSE_FINISH_CONTROL = "f";
        private const String COURSE_CONTROL_DESCRIPTION = "d";
        private const String COURSE_TITLE = "n";
        private const String COURSE_RELAY_START_NUMBER = "u";
        private const String COURSE_OTHER = "o";

        private const String COURSE_LEG_VARITION_START = "l";
        private const String COURSE_LEG_VARITION_BRANCH = "b";
        private const String COURSE_LEG_VARITION_END = "p";
        private const String COURSE_RELAY_VARITION_START = "r";
        private const String COURSE_RELAY_VARITION_BRANCH = "v";
        private const String COURSE_RELAY_VARITION_END = "q";

        private const String COURSE_CLIMB = "C";
        private const String COURSE_EXTRA_LENGTH = "E";
        private const String COURSE_RELAY_LEGS = "L";
        private const String COURSE_COURSE_TYPE = "Y";
        private const String COURSE_EXPORT_FILE_NAME = "M";
        private const String COURSE_MAP_SCALE = "S";

        private const String COURSE_COURSE_TYPE_RELAY = "s";
        private const String COURSE_COURSE_TYPE_ONE_MAN_RELAY = "o";
        private const String COURSE_COURSE_TYPE_NORMAL = "n";

        private void CopyToEventCourse(Model.Map map)
        {
            if (map.Event == null)
            {
                map.Event = new Event.Event();
            }
            Event.Course.Course setting = MapExtension.GetOrCreateEventCourse(map, _mainValue, true);
            
            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                if (CopyToEventCourse(map, setting.Name, setting.ControlDescription, ref i))
                {
                    continue;
                }

                string code = _codeValue[i, 0];
                switch (code)
                {
                    case COURSE_CLIMB:
                        setting.ClimbMetre = GetInt32Value(i);
                        break;
                    case COURSE_EXTRA_LENGTH:
                        setting.ExtraLengthKm = GetDecimalValue(i);
                        break;
                    case COURSE_RELAY_LEGS:
                        setting.RelayLegs = GetInt32Value(i);
                        break;
                    case COURSE_COURSE_TYPE:
                        if (GetStringValue(i).Equals(COURSE_COURSE_TYPE_RELAY))
                        {
                                setting.Type = Event.Type.EventCourseType.Relay;
                        }
                        else if (GetStringValue(i).Equals(COURSE_COURSE_TYPE_ONE_MAN_RELAY))
                        {
                            setting.Type = Event.Type.EventCourseType.OneManRelay;
                        }
                        else
                        {
                            setting.Type = Event.Type.EventCourseType.Normal;
                        }
                        break;
                    case COURSE_EXPORT_FILE_NAME:
                        setting.ExportFileName = GetStringValue(i);
                        break;
                    case COURSE_MAP_SCALE:
                        setting.MapScale = GetInt32Value(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }

            BuildLegs(map, setting.CourseLegs, new List<Event.Course.EventObjectDescription>(), setting.ControlDescription);
        }

        private bool CopyToEventCourse(Model.Map map, String courseName, List<Event.Course.BasicDescription> listOfCourseObjects, ref Int32 i)
        {
            string typeCode = _codeValue[i, 0];
            switch (typeCode)
            {
                case COURSE_START_CONTROL:
                    listOfCourseObjects.Add(
                        new Event.Course.EventObjectDescription(
                            MapExtension.GetOrCreateEventObject(map, GetStringValue(i), Event.Type.EventObjectType.StartControl)));
                    i++;
                    return true;
                case COURSE_FINISH_CONTROL:
                    listOfCourseObjects.Add(
                        new Event.Course.EventObjectDescription(
                            MapExtension.GetOrCreateEventObject(map, GetStringValue(i), Event.Type.EventObjectType.FinishControl)));
                    i++;
                    return true;
                case COURSE_CONTROL:
                    listOfCourseObjects.Add(MapExtension.GetOrCreateEventControlDescription(map, courseName, GetStringValue(i)));
                    i++;
                    return true;
                case COURSE_MARKED_ROUTE:                    
                    listOfCourseObjects.Add(
                        new Event.Course.EventObjectDescription(
                            MapExtension.GetOrCreateEventObject(map, GetStringValue(i), Event.Type.EventObjectType.MarkedRoute)));
                    i++;
                    return true;
                case COURSE_CONTROL_DESCRIPTION:
                    listOfCourseObjects.Add(
                        new Event.Course.EventObjectDescription(
                            MapExtension.GetOrCreateEventObject(map, GetStringValue(i), Event.Type.EventObjectType.ControlDescriptionSheet)));
                    i++;
                    return true;
                case COURSE_MANDATORY_CROSSING_POINT:
                    listOfCourseObjects.Add(new Event.Course.BasicDescription(Event.Type.EventCourseObjectType.MandatoryCrossingPoint));
                    i++;
                    return true;
                case COURSE_MANDATORY_PASSAGE:
                    listOfCourseObjects.Add(new Event.Course.BasicDescription(Event.Type.EventCourseObjectType.MandatoryPassage));
                    i++;
                    return true;
                case COURSE_MAP_EXCHANGE:
                    listOfCourseObjects.Add(new Event.Course.BasicDescription(Event.Type.EventCourseObjectType.MapExchange));
                    i++;
                    return true;
                case COURSE_TEXT_BLOCK:
                    listOfCourseObjects.Add(
                        new Event.Course.EventObjectDescription(
                            MapExtension.GetOrCreateEventObject(map, GetStringValue(i), Event.Type.EventObjectType.TextBlock)));
                    i++;
                    return true;
                case COURSE_TITLE:
                    listOfCourseObjects.Add(
                        new Event.Course.EventObjectDescription(
                            MapExtension.GetOrCreateEventObject(map, GetStringValue(i), Event.Type.EventObjectType.CourseTitle)));
                    i++;
                    return true;
                case COURSE_RELAY_START_NUMBER:
                    listOfCourseObjects.Add(
                        new Event.Course.EventObjectDescription(
                            MapExtension.GetOrCreateEventObject(map, GetStringValue(i), Event.Type.EventObjectType.RelayStartNumber)));
                    i++;
                    return true;
                case COURSE_OTHER:
                    throw (new Exception());
                case COURSE_RELAY_VARITION_BRANCH:
                    if (String.IsNullOrEmpty(GetStringValue(i)))
                    {
                        listOfCourseObjects.Add(new Event.Course.BasicDescription(Event.Type.EventCourseObjectType.RelayVariation));
                    }
                    else
                    {
                        listOfCourseObjects.Add(
                            new Event.Course.EventObjectDescription(
                                MapExtension.GetOrCreateEventObject(map, GetStringValue(i), Event.Type.EventObjectType.RelayVariation)));
                    }
                    i++;
                    return true;
                case COURSE_LEG_VARITION_BRANCH:
                    listOfCourseObjects.Add(new Event.Course.LegVariation() { Code = GetStringValue(i) });
                    i++;
                    return true;
                case COURSE_LEG_VARITION_START:
                    Event.Course.LegVariations legVariations = new Event.Course.LegVariations();
                    listOfCourseObjects.Add(legVariations);
                    i++;
                    CopyToEventCourse(map, courseName, legVariations, ref i);
                    return true;
                case COURSE_RELAY_VARITION_START:
                    Event.Course.RelayVariations relayVariations = new Event.Course.RelayVariations() { Code = GetStringValue(i) };
                    listOfCourseObjects.Add(relayVariations);
                    i++;
                    CopyToEventCourse(map, courseName, relayVariations, ref i);
                    return true;
                default:
                    return false;
            }
        }

        private void CopyToEventCourse(Model.Map map, String courseName, Event.Course.LegVariations variations, ref Int32 i)
        {
            Event.Course.DescriptionSegment branch = null;

            while (i <= _codeValue.GetUpperBound(0))
            {
                if ((branch != null) && (CopyToEventCourse(map, courseName, branch, ref i)))
                {
                    continue;
                }

                string code = _codeValue[i, 0];
                switch (code)
                {
                    case COURSE_LEG_VARITION_BRANCH:
                    case COURSE_RELAY_VARITION_BRANCH:
                        branch = new Event.Course.DescriptionSegment();
                        variations.ControlDescriptionSegments.Add(branch);
                        break;
                    case COURSE_LEG_VARITION_END:
                    case COURSE_RELAY_VARITION_END:
                        i++;
                        return;
                    default:
                        // Read other objects
                        return;
                }
            }
        }

        private List<Event.Course.EventObjectDescription> BuildLegs(Model.Map map, List<Event.Course.Leg> legs, List<Event.Course.EventObjectDescription> precedingControls, Ocad.Event.Course.DescriptionSegment branch)
        {
            foreach (Event.Course.BasicDescription courseObject in branch)
            {
                if (courseObject is Event.Course.EventObjectDescription)
                {
                    Event.Course.EventObjectDescription nextControl = (Event.Course.EventObjectDescription)courseObject;
                    if (nextControl.EventObject.IsWayPoint)
                    {
                        foreach (Event.Course.EventObjectDescription precedingControl in precedingControls)
                        {
                            bool found = false;
                            foreach (Event.Course.Leg leg in legs)
                            {
                                if (leg.StartWayPoint.Equals(precedingControl) && leg.EndWayPoint.Equals(nextControl))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                MapExtension.GetOrCreateEventCourseLeg(map, _mainValue, precedingControl.EventObject.Code, nextControl.EventObject.Code);
                            }
                        }
                        precedingControls = new List<Event.Course.EventObjectDescription>();
                        precedingControls.Add(nextControl);
                    }
                }
                else if (courseObject is Event.Course.LegVariations)
                {
                    Event.Course.LegVariations variation = (Event.Course.LegVariations)courseObject;
                    List<Event.Course.EventObjectDescription> nextControls = new List<Event.Course.EventObjectDescription>();
                    foreach (Event.Course.DescriptionSegment subBranch in variation.ControlDescriptionSegments)
                    {
                        nextControls.AddRange(BuildLegs(map, legs, precedingControls, subBranch));
                    }
                    precedingControls = nextControls;
                }
            }
            return precedingControls;
        }

        private static void CopyFromEventCourses(Model.Map map, List<Setting> settings)
        {
            foreach (Event.Course.Course source in map.Event.Courses)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.EventCourse;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.Name);

                CopyFromCourseObject(source.ControlDescription, b);

                if (source.Type.HasValue)
                {
                    switch (source.Type)
                    {
                        case Event.Type.EventCourseType.OneManRelay:
                            Write(b, COURSE_COURSE_TYPE, COURSE_COURSE_TYPE_ONE_MAN_RELAY);
                            break;
                        case Event.Type.EventCourseType.Relay:
                            Write(b, COURSE_COURSE_TYPE, COURSE_COURSE_TYPE_RELAY);
                            break;
                        default:
                            Write(b, COURSE_COURSE_TYPE, COURSE_COURSE_TYPE_NORMAL);
                            break;
                    }
                }
                Write(b, COURSE_CLIMB, source.ClimbMetre);
                Write(b, COURSE_EXTRA_LENGTH, source.ExtraLengthKm, "0.00");
                Write(b, COURSE_RELAY_LEGS, source.RelayLegs);
                Write(b, COURSE_EXPORT_FILE_NAME, source.ExportFileName);
                Write(b, COURSE_MAP_SCALE, source.MapScale);
                setting.ConcatenatedValues = b.ToString();

                CopyFromEventClasses(source, settings);
            }
        }

        private static void CopyFromCourseObject(List<Event.Course.BasicDescription> courseObjects, StringBuilder b)
        {
            foreach (Event.Course.BasicDescription courseObject in courseObjects)
            {
                switch (courseObject.Type)
                {
                    case Event.Type.EventCourseObjectType.Control:
                        Write(b, COURSE_CONTROL, ((Event.Course.ControlDescription)courseObject).EventObject);
                        break;
                    case Event.Type.EventCourseObjectType.StartControl:
                        Write(b, COURSE_START_CONTROL, ((Event.Course.EventObjectDescription)courseObject).EventObject);
                        break;
                    case Event.Type.EventCourseObjectType.FinishControl:
                        Write(b, COURSE_FINISH_CONTROL, ((Event.Course.EventObjectDescription)courseObject).EventObject);
                        break;
                    case Event.Type.EventCourseObjectType.ControlDescriptionSheet:
                        Write(b, COURSE_CONTROL_DESCRIPTION, ((Event.Course.EventObjectDescription)courseObject).EventObject);
                        break;
                    case Event.Type.EventCourseObjectType.MarkedRoute:
                        Write(b, COURSE_MARKED_ROUTE, ((Event.Course.EventObjectDescription)courseObject).EventObject);
                        break;
                    case Event.Type.EventCourseObjectType.MandatoryCrossingPoint:
                        Write(b, COURSE_MANDATORY_CROSSING_POINT);
                        break;
                    case Event.Type.EventCourseObjectType.MandatoryPassage:
                        Write(b, COURSE_MANDATORY_PASSAGE);
                        break;
                    case Event.Type.EventCourseObjectType.MapExchange:
                        Write(b, COURSE_MAP_EXCHANGE);
                        break;
                    case Event.Type.EventCourseObjectType.RelayStartNumber:
                        Write(b, COURSE_RELAY_START_NUMBER, ((Event.Course.EventObjectDescription)courseObject).EventObject);
                        break;
                    case Event.Type.EventCourseObjectType.CourseTitle:
                        Write(b, COURSE_TITLE, ((Event.Course.EventObjectDescription)courseObject).EventObject);
                        break;
                    case Event.Type.EventCourseObjectType.Other:
                        Write(b, COURSE_OTHER, ((Event.Course.EventObjectDescription)courseObject).EventObject);
                        break;
                    case Event.Type.EventCourseObjectType.LegVariation:
                        Write(b, COURSE_LEG_VARITION_BRANCH, ((Event.Course.LegVariation)courseObject).Code);
                        break;
                    case Event.Type.EventCourseObjectType.RelayVariation:
                        if (courseObject is Event.Course.EventObjectDescription)
                        {
                            Write(b, COURSE_RELAY_VARITION_BRANCH, ((Event.Course.EventObjectDescription)courseObject).EventObject);
                        }
                        else
                        {
                            Write(b, COURSE_RELAY_VARITION_BRANCH);
                        }
                        break;
                    case Event.Type.EventCourseObjectType.LegVariations:
                        Event.Course.LegVariations legVariations = (Event.Course.LegVariations)courseObject;
                        Write(b, COURSE_LEG_VARITION_START);
                        foreach (Event.Course.DescriptionSegment branch in legVariations.ControlDescriptionSegments)
                        {
                            CopyFromCourseObject(branch, b);
                        }
                        Write(b, COURSE_LEG_VARITION_END);
                        break;
                    case Event.Type.EventCourseObjectType.RelayVariations:
                        Event.Course.RelayVariations relayVariations = (Event.Course.RelayVariations)courseObject;
                        Write(b, COURSE_RELAY_VARITION_START, relayVariations.Code);
                        foreach (Event.Course.DescriptionSegment branch in relayVariations.ControlDescriptionSegments)
                        {
                            CopyFromCourseObject(branch, b);
                        }
                        Write(b, COURSE_RELAY_VARITION_END);
                        break;
                    case Event.Type.EventCourseObjectType.TextBlock:
                        Write(b, COURSE_TEXT_BLOCK, ((Event.Course.EventObjectDescription)courseObject).EventObject);
                        break;
                }
            }
        }
    }
}
