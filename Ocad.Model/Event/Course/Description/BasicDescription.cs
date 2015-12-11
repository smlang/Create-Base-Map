using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event.Course
{
    [VersionsSupported(V9 = true)]
    public class BasicDescription
    {
        private Type.EventCourseObjectType _type;

        [VersionsSupported(V9 = true)]
        public virtual Type.EventCourseObjectType Type
        {
            get
            {
                return _type;
            }
        }

        public BasicDescription(Type.EventCourseObjectType type)
        {
            _type = type;
        }
    }
}
