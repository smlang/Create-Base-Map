using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public class ControlDescriptionSheet : MappedObject
    {
        [VersionsSupported(V9 = true)]
        public Boolean UseTextDescription { get; set; }

        public ControlDescriptionSheet(String code, Ocad.Model.AbstractObject modelObject)
            : base(code, Ocad.Event.Type.EventObjectType.ControlDescriptionSheet, modelObject)
        {
        }
    }
}
