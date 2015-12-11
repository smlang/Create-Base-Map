using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9=true)]
    public class Event
    {
        [VersionsSupported(V9 = true)]
        public List<AbstractObject> Objects { get; set; }

        [VersionsSupported(V9 = true)]
        public List<Course.Course> Courses { get; set; }

        [VersionsSupported(V9 = true)]
        public ControlDescriptionPrintParameter ControlDescriptionPrintParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public Parameter Parameter { get; set; }
        [VersionsSupported(V9 = true)]
        public ExportCoursesStatisticsParameter ExportCoursesStatisticsParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public ExportCoursesTextParameter ExportCoursesTextParameter { get; set; }

        public Event()
        {
            Objects = new List<AbstractObject>();
            Courses = new List<Course.Course>();
        }
    }
}
