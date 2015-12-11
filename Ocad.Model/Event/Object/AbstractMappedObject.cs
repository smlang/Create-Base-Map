using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public class MappedObject : AbstractObject
    {
        private Type.EventObjectType _type;

        [VersionsSupported(V9 = true)]
        public override Type.EventObjectType Type { get { return _type; } }

        [VersionsSupported(V9 = true)]
        public Ocad.Model.AbstractObject ModelObject { get; set; }

        public MappedObject(String code, Type.EventObjectType type, Ocad.Model.AbstractObject modelObject)
            : base(code)
        {
            _type = type;
            ModelObject = modelObject;
        }
    }
}
