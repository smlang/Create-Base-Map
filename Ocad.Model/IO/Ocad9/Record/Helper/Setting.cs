using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Geometry;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        internal const Char DELIMITATOR = '\t';
        internal const String TRUE = "1";
        internal const String FALSE = "0";

        internal Type.SettingType SettingType { get; set; }
        internal Int32 ModelObjectIndex { get; set; }
        internal String ConcatenatedValues { get; set; }

        private String _mainValue;
        private String[,] _codeValue;

        internal void CopyToModel(Model.Map map)
        {
            String[] parts = ConcatenatedValues.Split(Setting.DELIMITATOR);

            _mainValue = parts[0];
            _codeValue = new String[parts.Length - 1, 2];
            for (Int32 i = 1; i < parts.Length; i++)
            {
                _codeValue[i - 1, 0] = parts[i][0].ToString();
                _codeValue[i - 1, 1] = parts[i].Substring(1);
            }

            switch (SettingType)
            {
                case Type.SettingType.EventObject:
                    CopyToEventObject(map);
                    break;
                case Type.SettingType.EventCourse:
                    CopyToEventCourse(map);
                    break;
                case Type.SettingType.EventClass:
                    CopyToEventClass(map);
                    break;
                case Type.SettingType.EventPreviewObject:
                    CopyToEventPreviewObject(map);
                    break;


                case Type.SettingType.EventControlDescriptionPrintParameter:
                    CopyToEventControlDescriptionPrintParameter(map);
                    break;
                case Type.SettingType.EventParameter:
                    CopyToEventParameter(map);
                    break;
                case Type.SettingType.EventExportCoursesTextParameter:
                    CopyToEventExportCoursesTextParameter(map);
                    break;
                case Type.SettingType.EventExportCoursesStatisticsParameter:
                    CopyToEventExportCoursesStatisticsParameter(map);
                    break;

                
                case Type.SettingType.DataSet:
                    CopyToModelDataSet(map);
                    break;
                case Type.SettingType.DatabaseObject:
                    CopyToModelDatabaseObject(map);
                    break;
                case Type.SettingType.OimFile:
                    CopyToModelOimFile(map);
                    break;
                case Type.SettingType.Template:
                    CopyToModelTemplate(map);
                    break;
                case Type.SettingType.Colour:
                    CopyToModelColour(map);
                    break;
                case Type.SettingType.SpotColour:
                    CopyToModelSpotColour(map);
                    break;
                case Type.SettingType.FileInfo:
                    CopyToModelFileInfo(map);
                    break;
                case Type.SettingType.Zoom:
                    CopyToModelZoom(map);
                    break;
                case Type.SettingType.ImportLayer:
                    CopyToModelImportLayer(map);
                    break;
                case Type.SettingType.OimFind:
                    CopyToModelOimFind(map);
                    break;
                case Type.SettingType.SymbolTree:
                    CopyToModelSymbolTree(map);
                    break;


                case Type.SettingType.DisplayParameter:
                    CopyToModelDisplayParameter(map);
                    break;
                case Type.SettingType.OimParameter:
                    CopyToModelOimParameter(map);
                    break;
                case Type.SettingType.PrintParameter:
                    CopyToModelPrintParameter(map);
                    break;
                case Type.SettingType.TemplateParameter:
                    CopyToModelTemplateParameter(map);
                    break;
                case Type.SettingType.EpsParameter:
                    CopyToModelEpsParameter(map);
                    break;
                case Type.SettingType.ViewParameter:
                    CopyToModelViewParameter(map);
                    break;
                case Type.SettingType.TiffParameter:
                    CopyToModelTiffParameter(map);
                    break;
                case Type.SettingType.TilesParameter:
                    CopyToModelTilesParameter(map);
                    break;
                case Type.SettingType.DatabaseParameter:
                    CopyToModelDatabaseParameter(map);
                    break;
                case Type.SettingType.ExportParameter:
                    CopyToModelExportParameter(map);
                    break;
                case Type.SettingType.ScaleParameter:
                    CopyToModelScaleParameter(map);
                    break;
                case Type.SettingType.DatabaseCreateObjectParameter:
                    CopyToModelDatabaseCreateObjectParameter(map);
                    break;
                case Type.SettingType.SelectedSpotColoursParameter:
                    CopyToModelSelectedSpotColoursParameter(map);
                    break;
                case Type.SettingType.XmlScriptParameter:
                    CopyToModelXmlScriptParameter(map);
                    break;
                default:
                    CopyToModelMiscellaneous(map);
                    break;
            }
        }

        #region Read
        internal Boolean GetBooleanValue(Int32 i)
        {
            return GetStringValue(i).Equals(TRUE);
        }

        internal Byte GetByteValue(Int32 i)
        {
            return Byte.Parse(GetStringValue(i));
        }

        internal Int16 GetInt16Value(Int32 i)
        {
            return Int16.Parse(GetStringValue(i));
        }

        internal Int32 GetInt32Value(Int32 i)
        {
            return Int32.Parse(GetStringValue(i));
        }

        internal Decimal GetDecimalValue(Int32 i)
        {
            return Decimal.Parse(GetStringValue(i));
        }

        internal Distance GetDistance(Int32 i, Distance.Unit unit, Scale scale)
        {
            return new Distance(Decimal.Parse(GetStringValue(i)), unit, scale);
        }

        internal String GetStringValue(Int32 i)
        {
            return _codeValue[i, 1];
        }
        #endregion

        internal static List<Setting> CopyFromModel(Model.Map map)
        {
            List<Setting> settings = new List<Setting>();

            if (map.Event != null)
            {
                CopyFromEventObjects(map, settings);
                CopyFromEventCourses(map, settings);
                CopyFromEventPreviewObjects(map, settings);

                CopyFromEventControlDescriptionPrintParameter(map, settings);
                CopyFromEventParameter(map, settings);
                CopyFromEventExportCoursesTextParameter(map, settings);
                CopyFromEventExportCoursesStatisticsParameter(map, settings);
            }

            CopyFromModelDataSets(map, settings);
            CopyFromModelDatabaseObjects(map, settings);
            CopyFromModelOimFiles(map, settings);
            CopyFromModelTemplates(map, settings);
            CopyFromModelColours(map, settings);
            CopyFromModelSpotColours(map, settings);
            CopyFromModelFileInfos(map, settings);
            CopyFromModelZooms(map, settings);
            CopyFromModelImportLayers(map, settings);
            CopyFromModelOimFinds(map, settings);
            CopyFromModelSymbolTrees(map, settings);

            CopyFromModelDisplayParameter(map, settings);
            CopyFromModelOimParameter(map, settings);
            CopyFromModelPrintParameter(map, settings);
            CopyFromModelTemplateParameter(map, settings);
            CopyFromModelEpsParameter(map, settings);
            CopyFromModelViewParameter(map, settings);
            CopyFromModelTiffParameter(map, settings);
            CopyFromModelTilesParameter(map, settings);
            CopyFromModelDatabaseParameter(map, settings);
            CopyFromModelExportParameter(map, settings);
            CopyFromModelScaleParameter(map, settings);
            CopyFromModelDatabaseCreateObjectParameter(map, settings);
            CopyFromModelSelectedSpotColoursParameter(map, settings);
            CopyFromModelXmlScriptParameter(map, settings);
            CopyFromModelMiscellaneousSettings(map, settings);

            return settings;
        }

        #region Write
        internal static void Write(StringBuilder b, String key)
        {
            b.AppendFormat("{0}{1}", DELIMITATOR, key);
        }

        internal static void Write(StringBuilder b, String key, Boolean? value)
        {
            if (value.HasValue)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value.Value ? TRUE : FALSE);
            }
        }

        internal static void Write(StringBuilder b, String key, Byte? value)
        {
            if (value.HasValue)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value.Value);
            }
        }

        internal static void Write(StringBuilder b, String key, Int16? value)
        {
            if (value.HasValue)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value.Value);
            }
        }

        internal static void Write(StringBuilder b, String key, Int32? value)
        {
            if (value.HasValue)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value.Value);
            }
        }

        internal static void Write(StringBuilder b, String key, Decimal? value)
        {
            if (value.HasValue)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value.Value);
            }
        }

        internal static void Write(StringBuilder b, String key, Decimal? value, String format)
        {
            if (value.HasValue)
            {
                String format2 = "{0}{1}{2:" + format + "}";
                b.AppendFormat(format2, DELIMITATOR, key, value.Value);
            }
        }

        internal static void Write(StringBuilder b, String key, String value)
        {
            if (value != null)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value);
            }
        }

        internal static void Write(StringBuilder b, String key, Ocad.Model.PointSymbol value)
        {
            if (value != null)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value.Number);
            }
        }

        internal static void Write(StringBuilder b, String key, Ocad.Event.AbstractObject value)
        {
            if (value != null)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value.Code);
            }
        }

        internal static void Write(StringBuilder b, String key, Ocad.Model.Colour value)
        {
            if (value != null)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value.Number);
            }
        }

        internal static void Write(StringBuilder b, String key, Ocad.Model.SymbolTreeNode value)
        {
            if (value != null)
            {
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, value.Id);
            }
        }

        internal static void Write<T, U>(StringBuilder b, String key, Nullable<T> value) 
            where T: struct
        {
            if (value.HasValue)
            {
                U u = (U)Convert.ChangeType(value.Value, typeof(U));
                b.AppendFormat("{0}{1}{2}", DELIMITATOR, key, u);
            }
        }
        #endregion

        private ApplicationSettingException CreateApplicationSettingException()
        {
            return new ApplicationSettingException(this);
        }

        private ApplicationSettingException CreateApplicationSettingException(Int32 i)
        {
            return new ApplicationSettingException(this, i);
        }

        private class ApplicationSettingException : ApplicationException
        {
            public ApplicationSettingException(Setting setting)
                : base(String.Format("Setting {0} has unexpected main parameter with value {1}", setting.SettingType.ToString(), setting._mainValue))
            {
            }

            public ApplicationSettingException(Setting setting, Int32 i)
                : base(String.Format("Setting {0} has unexpected parameter {1} with value {2}", setting.SettingType.ToString(), setting._codeValue[0, i], setting.GetStringValue(i)))
            {
            }
        }
    }
}
