using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class SymbolTreeNode
    {
        [VersionsSupported(V9 = true)]
        public String Name { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16 Id { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean Expand { get; set; }

        [VersionsSupported(V9 = true)]
        public Boolean FirstNodeInSubgroup { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean LastNodeInSubgroup { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte Depth { get; set; }

        public List<SymbolTreeNode> Children { get; set; }

        internal static void Add(List<SymbolTreeNode> children, Int32 depth, SymbolTreeNode newNode)
        {
        }
    }
}
