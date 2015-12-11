using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class DisplayParameter
    {
        [VersionsSupported(V9 = true)]
        public Boolean? ShowSymbolFavourities { get; set; }
        [VersionsSupported(V9 = true)]
        public SymbolTreeNode SelectedSymbolTree { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16? SelectedSymbol { get; set; }
        [VersionsSupported(V9 = true)]
        public Boolean? ShowSymbolTree { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16? SymbolBoxWidthPixel { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16? SymbolBoxHeightPixel { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16? HorizontalSplittedPixelsFromTop { get; set; }
        [VersionsSupported(V9 = true)]
        public Int16? VerticalSplittedPixelsFromRight { get; set; }
    }
}
