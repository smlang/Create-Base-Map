using System;
using System.Collections.Generic;
using System.Text;

namespace Ocad.IO.Ocad9
{
    internal static class Constant
    {
        internal const Int16 OCAD_MARK = 3245;
        internal const Int32 MAX_HEADERS = 256;

        internal const Int32 DATA_BLOCK_BYTE_SIZE = 8;
        internal const Int32 STRING_BLOCK_BYTE_SIZE = 64;
        internal const Int32 POINT_BYTE_SIZE = 8;

        internal const Int32 POINT_TYPE_BYTE_SIZE = 8;
        internal const Int32 POINT_TYPE_MASK = 255;

        internal const Int32 FILE_RECORD_BYTE_SIZE = 48;
        internal const Int32 SYMBOL_HEADER_BYTE_SIZE = 4;
        internal const Int32 SYMBOL_HEADER_BLOCK_BYTE_SIZE = 4 + (MAX_HEADERS * SYMBOL_HEADER_BYTE_SIZE);
        internal const Int32 OBJECT_HEADER_BYTE_SIZE = 40;
        internal const Int32 OBJECT_HEADER_BLOCK_BYTE_SIZE = 4 + (MAX_HEADERS * OBJECT_HEADER_BYTE_SIZE);
        internal const Int32 SETTING_HEADER_BYTE_BYTE_SIZE = 16;
        internal const Int32 SETTING_HEADER_BLOCK_SIZE = 4 + (MAX_HEADERS * SETTING_HEADER_BYTE_BYTE_SIZE);
    }
}
