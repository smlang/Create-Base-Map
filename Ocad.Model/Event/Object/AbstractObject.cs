using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public abstract class AbstractObject
    {
        [VersionsSupported(V9 = true)]
        public String Code { get; set; }

        [VersionsSupported(V9 = true)]
        public abstract Type.EventObjectType Type { get; }

        public virtual Boolean IsWayPoint { get { return false; } }

        public AbstractObject(String code)
        {
            Code = code;
        }
    }
}
