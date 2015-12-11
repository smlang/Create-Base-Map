using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public class TextBlock : AbstractObject
    {
        [VersionsSupported(V9 = true)]
        public String Text { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.EventObjectType Type { get { return Ocad.Event.Type.EventObjectType.TextBlock; } }

        public TextBlock(String code)
            : base(code)
        {
        }
    }
}
