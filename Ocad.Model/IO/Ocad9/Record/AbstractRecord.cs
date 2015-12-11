using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record
{
    internal abstract class AbstractRecord
    {
        internal protected Int32 BodyPointer { get; set; }
        internal protected Int32 BodyByteSize { get; set; }

        internal abstract void ReadHeader(Reader reader);
        internal abstract void ReadBody(Reader reader);

        internal abstract Int32 SizeBody(Writer writer, Int32 offset, object o);

        internal abstract void WriteHeader(Writer reader);
        internal abstract void WriteBody(Writer reader);
    }
}
