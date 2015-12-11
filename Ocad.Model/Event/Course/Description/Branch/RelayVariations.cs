using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event.Course
{
    [VersionsSupported(V9 = true)]
    public class RelayVariations : LegVariations
    {
        [VersionsSupported(V9 = true)]
        public String Code { get; set; }

        public RelayVariations()
            : base(Ocad.Event.Type.EventCourseObjectType.RelayVariations)
        {
        }
    }
}