using System;
using System.Collections.Generic;
using System.Text;

namespace Ocad.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field )]
    internal class VersionsSupported : System.Attribute
    {
        public Boolean V8;
        public Boolean V9;
        public Boolean V10;
        public Boolean V11;

        internal VersionsSupported()
        {
            V8 = false;
            V9 = false;
            V10 = false;
            V11 = false;
        }
    }
}
