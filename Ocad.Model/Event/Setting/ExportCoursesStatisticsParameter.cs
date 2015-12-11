using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public class ExportCoursesStatisticsParameter
    {
        [VersionsSupported(V9 = true)]
        public Type.CoursesOrClasses? CoursesOrClasses { get; set; }
        [VersionsSupported(V9 = true)]
        public String Separator1 { get; set; }
        [VersionsSupported(V9 = true)]
        public String Tab1 { get; set; }
        [VersionsSupported(V9 = true)]
        public String Separator2 { get; set; }
        [VersionsSupported(V9 = true)]
        public String Tab2 { get; set; }
        [VersionsSupported(V9 = true)]
        public String Separator3 { get; set; }
        [VersionsSupported(V9 = true)]
        public String Tab3 { get; set; }
    }
}
