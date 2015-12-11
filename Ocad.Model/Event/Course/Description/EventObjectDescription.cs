using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event.Course
{
    [VersionsSupported(V9 = true)]
    public class EventObjectDescription : BasicDescription
    {
        [VersionsSupported(V9 = true)]
        public Ocad.Event.AbstractObject EventObject { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.EventCourseObjectType Type
        {
            get
            {
                return (Ocad.Event.Type.EventCourseObjectType)EventObject.Type;
            }
        }

        public EventObjectDescription(Ocad.Event.AbstractObject eventObject)
            : base((Ocad.Event.Type.EventCourseObjectType)eventObject.Type)
        {
            EventObject = eventObject;
        }
    }
}
