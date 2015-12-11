using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public class Parameter
    {
        [VersionsSupported(V9 = true)]
        public Boolean? CreateClassesAutomatically { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? WhiteBackgroundForControlDescription { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.EventNumbering? Numbering { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? AddControlDescriptionsForAllControls { get; set; }
        [VersionsSupported(V9 = true)]
        public String EventTitle { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.ControlDescriptionHoriztonalThickerLine? ControlDescriptionHoriztonalThickerLine { get; set; }
        [VersionsSupported(V9 = true)]
        public Int32? ControlDescriptionMaximumRows { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance DistanceFromCircleToNumber { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance DistanceFromCircleToConnectionLine { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance ControlDescriptionBoxSize { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.CourseTitle? CourseTitle { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? ExportRelayCombinationInXmlFile { get; set; }
    }
}
