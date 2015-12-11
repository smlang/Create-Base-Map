using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class TemplateParameter
    {
        [VersionsSupported(V9 = true)]
        public Int32 DefaultScale { get; set; }
    }
}
