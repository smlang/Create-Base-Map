using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event.Type
{
    [VersionsSupported(V9 = true)]
    public enum EventControlType
    {
        [VersionsSupported(V9 = true)]
        Unassigned = -1,
        [VersionsSupported(V9 = true)]
        Control = 0,
        [VersionsSupported(V9 = true)]
        FinishControl = 1,
        [VersionsSupported(V9 = true)]
        StartControl = 2,
    }

    [VersionsSupported(V9 = true)]
    public enum EventObjectType
    {
        [VersionsSupported(V9 = true)]
        Unassigned = EventControlType.Unassigned,
        [VersionsSupported(V9 = true)]
        Control = EventControlType.Control,
        [VersionsSupported(V9 = true)]
        FinishControl = EventControlType.FinishControl,
        [VersionsSupported(V9 = true)]
        StartControl = EventControlType.StartControl,

        [VersionsSupported(V9 = true)]
        ControlDescriptionSheet = 512,
        [VersionsSupported(V9 = true)]
        CourseTitle = 513,
        [VersionsSupported(V9 = true)]
        MarkedRoute = 514,
        [VersionsSupported(V9 = true)]
        RelayStartNumber = 515,
        [VersionsSupported(V9 = true)]
        RelayVariation = 516,
        [VersionsSupported(V9 = true)]
        TextBlock = 517
    }

    [VersionsSupported(V9 = true)]
    public enum EventCourseObjectType
    {
        [VersionsSupported(V9 = true)]
        Unassigned = EventObjectType.Unassigned,
        [VersionsSupported(V9 = true)]
        Control = EventObjectType.Control,
        [VersionsSupported(V9 = true)]
        FinishControl = EventObjectType.FinishControl,
        [VersionsSupported(V9 = true)]
        StartControl = EventObjectType.StartControl,

        [VersionsSupported(V9 = true)]
        ControlDescriptionSheet = EventObjectType.ControlDescriptionSheet,
        [VersionsSupported(V9 = true)]
        CourseTitle = EventObjectType.CourseTitle,
        [VersionsSupported(V9 = true)]
        MarkedRoute = EventObjectType.MarkedRoute,
        [VersionsSupported(V9 = true)]
        RelayStartNumber = EventObjectType.RelayStartNumber,
        [VersionsSupported(V9 = true)]
        RelayVariation = EventObjectType.RelayVariation,
        [VersionsSupported(V9 = true)]
        TextBlock = EventObjectType.TextBlock,

        [VersionsSupported(V9 = true)]
        LegVariation = 1024,
        [VersionsSupported(V9 = true)]
        LegVariations = 1025,
        [VersionsSupported(V9 = true)]
        MandatoryCrossingPoint = 1026,
        [VersionsSupported(V9 = true)]
        MandatoryPassage = 1027,
        [VersionsSupported(V9 = true)]
        MapExchange = 1028,
        [VersionsSupported(V9 = true)]
        Other = 1029,
        [VersionsSupported(V9 = true)]
        RelayVariations = 1030
    }

    [VersionsSupported(V9 = true)]
    public enum EventCourseType
    {
        [VersionsSupported(V9 = true)]
        Relay,
        [VersionsSupported(V9 = true)]
        OneManRelay,
        [VersionsSupported(V9 = true)]
        Normal
    }

    [VersionsSupported(V9 = true)]
    public enum EventNumbering
    {
        [VersionsSupported(V9 = true)]
        ControlNumber = 0,
        [VersionsSupported(V9 = true)]
        ControlNumberAndCode = 1,
        [VersionsSupported(V9 = true)]
        ControlCode = 2
    }

    [VersionsSupported(V9 = true)]
    public enum ControlDescriptionHoriztonalThickerLine
    {
        [VersionsSupported(V9 = true)]
        None = 0,
        [VersionsSupported(V9 = true)]
        EveryThirdLine = 1,
        [VersionsSupported(V9 = true)]
        EveryFourthLine = 2
    }

    [VersionsSupported(V9 = true)]
    public enum CourseTitle
    {
        [VersionsSupported(V9 = true)]
        Class = 0,
        [VersionsSupported(V9 = true)]
        CourseNameAndClass = 1,
        [VersionsSupported(V9 = true)]
        CourseName = 2
    }

    [VersionsSupported(V9 = true)]
    public enum CoursesOrClasses
    {
        [VersionsSupported(V9 = true)]
        Courses = 0,
        [VersionsSupported(V9 = true)]
        Classes = 1,
    }
}
