using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String SYMBOL_TREE_GROUP_ID = "g";
        private const String SYMBOL_TREE_FIRST_NODE_IN_SUBGROUP = "f";
        private const String SYMBOL_TREE_LAST_NODE_IN_SUBGROUP = "l";
        private const String SYMBOL_TREE_DEPTH = "i";
        private const String SYMBOL_TREE_EXPAND = "e";

        private void CopyToModelSymbolTree(Model.Map map)
        {
            Int16 groupId = 0;
            int i = 0;
            int groupIdIndex = -1;
            while ((i <= _codeValue.GetUpperBound(0)) && (groupIdIndex < 0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case SYMBOL_TREE_GROUP_ID:
                        groupId = GetInt16Value(i);
                        groupIdIndex = i;
                        break;
                }
                i++;
            }

            Model.SymbolTreeNode setting = MapExtension.GetOrCreateSymbolTree(map, groupId, true);
            if (setting == null)
            {
                return;
            }

            setting.Name = _mainValue;

            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case SYMBOL_TREE_EXPAND:
                        setting.Expand = GetBooleanValue(i);
                        break;
                    case SYMBOL_TREE_FIRST_NODE_IN_SUBGROUP:
                        setting.FirstNodeInSubgroup = GetBooleanValue(i);
                        break;
                    case SYMBOL_TREE_LAST_NODE_IN_SUBGROUP:
                        setting.LastNodeInSubgroup = GetBooleanValue(i);
                        break;
                    case SYMBOL_TREE_DEPTH:
                        setting.Depth = GetByteValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }
        }

        private static void CopyFromModelSymbolTrees(Model.Map map, List<Setting> settings)
        {
            foreach (Model.SymbolTreeNode source in map.SymbolTrees)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.SymbolTree;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.Name);
                Write(b, SYMBOL_TREE_GROUP_ID, source.Id);
                Write(b, SYMBOL_TREE_EXPAND, source.Expand);
                Write(b, SYMBOL_TREE_FIRST_NODE_IN_SUBGROUP, source.FirstNodeInSubgroup);
                Write(b, SYMBOL_TREE_LAST_NODE_IN_SUBGROUP, source.LastNodeInSubgroup);
                Write(b, SYMBOL_TREE_DEPTH, source.Depth);
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}
