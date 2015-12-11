using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event.Course
{
    [VersionsSupported(V9 = true)]
    public class Course
    {
        [VersionsSupported(V9 = true)]
        public String Name { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal? ClimbMetre { get; set; }
        [VersionsSupported(V9 = true)]
        public Decimal? ExtraLengthKm { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32? RelayLegs { get; set; }
        [VersionsSupported(V9 = true)]
        public String ExportFileName { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32? MapScale { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.EventCourseType? Type { get; set; }

        [VersionsSupported(V9 = true)]
        public List<Class> Classes { get; set; }
        [VersionsSupported(V9 = true)]
        public DescriptionSegment ControlDescription { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Leg> CourseLegs { get; set; }
        [VersionsSupported(V9 = true)]
        public List<ControlDescription> CourseControls { get; set; }

        public Course()
        {
            Classes = new List<Class>();
            ControlDescription = new DescriptionSegment();
            CourseControls = new List<ControlDescription>();
            CourseLegs = new List<Leg>();
        }
    }
}
