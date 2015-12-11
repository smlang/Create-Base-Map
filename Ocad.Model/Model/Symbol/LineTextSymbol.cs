using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class LineTextSymbol : AbstractTextSymbol
    {
        [VersionsSupported(V9 = true)]
        public override Type.SymbolType Type 
        {
            get
            {
                return Model.Type.SymbolType.LineText;
            }
        }
    }
}
