using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;
using Geometry;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public abstract class AbstractSymbol
    {
        [VersionsSupported(V9 = true)]
        public String Number { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.SymbolFlag Flag { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean Selected { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.SymbolStatus Status { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.DefaultDrawingMode DefaultDrawingMode { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.EventMode EventMode { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.ControlObjectType CourseObjectType { get; set; }
        [VersionsSupported(V9 = true)]
        public Type.CourseDescriptionFlag CourseDescriptionFlag { get; set; }
        [VersionsSupported(V9 = true)]
        public Distance Extent { get; set; }
        [VersionsSupported(V9 = true)]
        public SymbolTreeNode SymbolTree1 { get; set; }
        [VersionsSupported(V9 = true)]
        public SymbolTreeNode SymbolTree2 { get; set; }
        [VersionsSupported(V9 = true)]
        public abstract List<Colour> Colours { get; }
        [VersionsSupported(V9 = true)]
        public String Description { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte[] IconBits { get; set; }

        [VersionsSupported(V9 = true)]
        public abstract Type.SymbolType Type { get; }

        internal AbstractSymbol()
        {
        }
    }
}
