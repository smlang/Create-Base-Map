using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;
using System.IO;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class Map
    {
        [VersionsSupported(V9 = true)]
        public List<AbstractSymbol> Symbols { get; set; }
        [VersionsSupported(V9 = true)]
        public List<AbstractObject> Objects { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Miscellaneous> MiscellaneousSettings { get; set; }

        [VersionsSupported(V9 = true)]
        public Event.Event Event { get; set; }

        [VersionsSupported(V9 = true)]
        public List<Colour> ColourTable { get; set; }
        [VersionsSupported(V9 = true)]
        public List<DatabaseObject> DatabaseObjects { get; set; }
        [VersionsSupported(V9 = true)]
        public List<DataSet> DataSets { get; set; }
        [VersionsSupported(V9 = true)]
        public List<FileInfo> FileInfos { get; set; }
        [VersionsSupported(V9 = true)]
        public List<ImportLayer> ImportLayers { get; set; }
        [VersionsSupported(V9 = true)]
        public List<OimFile> OimFiles { get; set; }
        [VersionsSupported(V9 = true)]
        public List<OimFind> OimFinds { get; set; }
        [VersionsSupported(V9 = true)]
        public List<SpotColour> SpotColours { get; set; }
        [VersionsSupported(V9 = true)]
        public List<SymbolTreeNode> SymbolTrees { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Template> Templates { get; set; }
        [VersionsSupported(V9 = true)]
        public List<Zoom> Zooms { get; set; }

        [VersionsSupported(V9 = true)]
        public DatabaseCreateObjectParameter DatabaseCreateObjectParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public DatabaseParameter DatabaseParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public DisplayParameter DisplayParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public EpsParameter EpsParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public ExportParameter ExportParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public FileName FileName { get; set; }
        [VersionsSupported(V9 = true)]
        public FileType FileType { get; set; }
        [VersionsSupported(V9 = true)]
        public FileVersion FileVersion { get; set; }
        [VersionsSupported(V9 = true)]
        public OimParameter OimParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public PrintParameter PrintParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public ScaleParameter ScaleParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public TemplateParameter TemplateParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public TiffParameter TiffParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public TilesParameter TilesParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public ViewParameter ViewParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public SelectedSpotColoursParameter SelectedSpotColoursParameter { get; set; }
        [VersionsSupported(V9 = true)]
        public XmlScriptParameter XmlScriptParameter { get; set; }

        internal Map()
        {
            Symbols = new List<AbstractSymbol>();
            Objects = new List<AbstractObject>();

            //Event = new Event.Event();

            ColourTable = new List<Colour>();
            DatabaseObjects = new List<DatabaseObject>();
            DataSets = new List<DataSet>();
            FileInfos = new List<FileInfo>();
            ImportLayers = new List<ImportLayer>();
            OimFiles = new List<OimFile>();
            OimFinds = new List<OimFind>();
            SpotColours = new List<SpotColour>();
            SymbolTrees = new List<SymbolTreeNode>();
            Templates = new List<Template>();
            Zooms = new List<Zoom>();

            MiscellaneousSettings = new List<Miscellaneous>();
        }

        public AbstractSymbol GetSymbol(string symbolNumber)
        {
            if (System.String.IsNullOrEmpty(symbolNumber))
            {
                return null;
            }

            foreach (Ocad.Model.AbstractSymbol symbol in Symbols)
            {
                if (symbol.Number.Equals(symbolNumber))
                {
                    return symbol;
                }
            }
            return null;
        }

        public static Map Import(String ocadDataFilePath)
        {
            using (FileStream ocadDataStream = new FileStream(ocadDataFilePath, FileMode.Open, FileAccess.Read))
            {
                return Import(ocadDataStream);
            }
        }

        public static Map Import(Stream ocadDataStream)
        {
            using (BufferedStream buffer = new BufferedStream(ocadDataStream))
            {
                using (Ocad.IO.Ocad9.Reader reader = new Ocad.IO.Ocad9.Reader(buffer))
                {
                    return reader.ReadContent();
                }
            }
        }

        public void Export(String ocadDataFilePath)
        {
            FileName.Value = ocadDataFilePath;
            using (FileStream ocadDataStream = new FileStream(FileName.Value, FileMode.Create, FileAccess.Write))
            {
                Export(ocadDataStream);
            }
        }

        public void Export(Stream ocadDataStream)
        {
            using (BufferedStream buffer = new BufferedStream(ocadDataStream))
            {
                using (Ocad.IO.Ocad9.Writer writer = new Ocad.IO.Ocad9.Writer(buffer))
                {
                    writer.Write(this);
                    writer.Flush();
                }
            }
        }
    }
}
