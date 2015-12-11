using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String DISPLAY_PARAMETER_SHOW_SYMBOL_FAVOURITIES = "f";
        private const String DISPLAY_PARAMETER_SELECTED_SYMBOL_TREE = "g";
        private const String DISPLAY_PARAMETER_SELECTED_SYMBOL = "s";
        private const String DISPLAY_PARAMETER_SHOW_SYMBOL_TREE = "t";
        private const String DISPLAY_PARAMETER_SYMBOL_BOX_WIDTH_PIXEL = "x";
        private const String DISPLAY_PARAMETER_SYMBOL_BOX_HEIGHT_PIXEL = "y";
        private const String DISPLAY_PARAMETER_HORIZONTAL_SPLITTED_PIXELS_FROM_TOP = "h";
        private const String DISPLAY_PARAMETER_VERTICAL_SPLITTED_PIXELS_FROM_TOP = "v";

        private void CopyToModelDisplayParameter(Model.Map map)
        {
            Model.DisplayParameter setting = new Model.DisplayParameter();
            map.DisplayParameter = setting;

            if (!String.IsNullOrEmpty(_mainValue))
            {
                CreateApplicationSettingException(1);
            }

            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case DISPLAY_PARAMETER_SHOW_SYMBOL_FAVOURITIES:
                        setting.ShowSymbolFavourities = GetBooleanValue(i);
                        break;
                    case DISPLAY_PARAMETER_SELECTED_SYMBOL_TREE:
                        setting.SelectedSymbolTree = MapExtension.GetOrCreateSymbolTree(map, GetInt16Value(i));
                        break;
                    case DISPLAY_PARAMETER_SELECTED_SYMBOL:
                        setting.SelectedSymbol = GetInt16Value(i);
                        break;
                    case DISPLAY_PARAMETER_SHOW_SYMBOL_TREE:
                        setting.ShowSymbolTree = GetBooleanValue(i);
                        break;
                    case DISPLAY_PARAMETER_SYMBOL_BOX_WIDTH_PIXEL:
                        setting.SymbolBoxWidthPixel = GetInt16Value(i);
                        break;
                    case DISPLAY_PARAMETER_SYMBOL_BOX_HEIGHT_PIXEL:
                        setting.SymbolBoxHeightPixel = GetInt16Value(i);
                        break;
                    case DISPLAY_PARAMETER_HORIZONTAL_SPLITTED_PIXELS_FROM_TOP:
                        setting.HorizontalSplittedPixelsFromTop = GetInt16Value(i);
                        break;
                    case DISPLAY_PARAMETER_VERTICAL_SPLITTED_PIXELS_FROM_TOP:
                        setting.VerticalSplittedPixelsFromRight = GetInt16Value(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelDisplayParameter(Model.Map map, List<Setting> settings)
        {
            Model.DisplayParameter source = map.DisplayParameter;
            if (source != null)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.DisplayParameter;
                settings.Add(setting);

                StringBuilder b = new StringBuilder();
                Write(b, DISPLAY_PARAMETER_HORIZONTAL_SPLITTED_PIXELS_FROM_TOP, source.HorizontalSplittedPixelsFromTop);
                Write(b, DISPLAY_PARAMETER_VERTICAL_SPLITTED_PIXELS_FROM_TOP, source.VerticalSplittedPixelsFromRight);
                Write(b, DISPLAY_PARAMETER_SELECTED_SYMBOL, source.SelectedSymbol);
                Write(b, DISPLAY_PARAMETER_SELECTED_SYMBOL_TREE, source.SelectedSymbolTree);
                Write(b, DISPLAY_PARAMETER_SHOW_SYMBOL_FAVOURITIES, source.ShowSymbolFavourities);
                Write(b, DISPLAY_PARAMETER_SHOW_SYMBOL_TREE, source.ShowSymbolTree);
                Write(b, DISPLAY_PARAMETER_SYMBOL_BOX_WIDTH_PIXEL, source.SymbolBoxWidthPixel);
                Write(b, DISPLAY_PARAMETER_SYMBOL_BOX_HEIGHT_PIXEL, source.SymbolBoxHeightPixel);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}