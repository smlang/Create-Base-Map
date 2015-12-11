using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record
{
    internal class Setting : AbstractRecord
    {
        private Helper.Setting setting;

        internal override void ReadHeader(Reader reader)
        {
            setting = new Helper.Setting();

            BodyPointer = reader.ReadInt32();
            BodyByteSize = reader.ReadInt32();
            setting.SettingType = (Type.SettingType)reader.ReadInt32();
            setting.ModelObjectIndex = reader.ReadInt32();
        }

        internal override void ReadBody(Reader reader)
        {
            setting.ConcatenatedValues = reader.ReadAsciiString(BodyPointer, BodyByteSize);
            setting.CopyToModel(reader.Map);
        }

        internal override Int32 SizeBody(Writer writer, Int32 offset, object o)
        {
            setting = (Helper.Setting)o;
            BodyPointer = offset;
            BodyByteSize = writer.SizeAsciiString(setting.ConcatenatedValues);
            return BodyPointer + BodyByteSize;
        }

        internal override void WriteHeader(Writer writer)
        {
            writer.Write(BodyPointer);
            writer.Write(BodyByteSize);
            writer.Write((Int32)setting.SettingType);
            writer.Write((Int32)setting.ModelObjectIndex);
        }

        internal override void WriteBody(Writer writer)
        {
            writer.WriteAsciiString(setting.ConcatenatedValues);
        }
    }
}
