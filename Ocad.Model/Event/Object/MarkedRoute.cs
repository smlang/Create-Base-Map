using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public class MarkedRoute : MappedObject
    {
        [VersionsSupported(V9 = true)]
        public Boolean Funnel { get; set; }
        [VersionsSupported(V9 = true)]
        public String Text { get; set; }

        public override Boolean IsWayPoint { get { return true; } }

        public MarkedRoute(String code, Ocad.Model.AbstractObject modelObject)
            : base(code, Ocad.Event.Type.EventObjectType.MarkedRoute, modelObject)
        {
        }
    }
}