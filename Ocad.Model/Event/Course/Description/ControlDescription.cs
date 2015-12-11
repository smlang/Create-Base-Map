using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event.Course
{
    [VersionsSupported(V9 = true)]
    public class ControlDescription : EventObjectDescription
    {
        [VersionsSupported(V9 = true)]
        public Ocad.Model.AbstractObject TextObject { get; set; }

        public ControlDescription(Ocad.Event.AbstractObject eventObject)
            : base(eventObject)
        {
        }
    }
}
