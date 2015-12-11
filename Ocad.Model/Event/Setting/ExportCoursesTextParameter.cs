using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public class ExportCoursesTextParameter
    {
        [VersionsSupported(V9 = true)]
        public Type.CoursesOrClasses? CoursesOrClasses { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? IncludeClimb { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? IncludeNumberOfControls { get; set; }
    }
}
