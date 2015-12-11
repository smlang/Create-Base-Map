using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class Miscellaneous
    {
        [VersionsSupported(V9 = true)]
        public Int32 TypeId;
        [VersionsSupported(V9 = true)]
        public String MainValue;
        [VersionsSupported(V9 = true)]
        public String[,] CodeValue;
        [VersionsSupported(V9 = true)]
        public Int32 Index;
    }
}
