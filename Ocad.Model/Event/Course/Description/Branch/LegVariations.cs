using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event.Course
{
    [VersionsSupported(V9 = true)]
    public class LegVariations : BasicDescription
    {
        [VersionsSupported(V9 = true)]
        public List<DescriptionSegment> ControlDescriptionSegments { get; set; }

        internal protected LegVariations(Type.EventCourseObjectType type)
            : base(type)
        {
            ControlDescriptionSegments = new List<DescriptionSegment>();
        }

        public LegVariations()
            : base(Ocad.Event.Type.EventCourseObjectType.LegVariations)
        {
            ControlDescriptionSegments = new List<DescriptionSegment>();
        }
    }
}
