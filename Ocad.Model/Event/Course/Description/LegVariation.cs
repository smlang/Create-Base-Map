using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event.Course
{
    [VersionsSupported(V9 = true)]
    public class LegVariation : BasicDescription
    {
        [VersionsSupported(V9 = true)]
        public String Code { get; set; }

        public LegVariation()
            : base(Ocad.Event.Type.EventCourseObjectType.LegVariation)
        {
        }
    }
}
