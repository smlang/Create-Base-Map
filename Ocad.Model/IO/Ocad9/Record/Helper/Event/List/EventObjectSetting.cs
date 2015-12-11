using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ocad.IO.Ocad9.Record.Helper
{
    internal partial class Setting
    {
        private const String EVENT_OBJECT_TYPE = "Y";
        private const String EVENT_OBJECT_SYMBOL_C = "c";
        private const String EVENT_OBJECT_SYMBOL_D = "d";
        private const String EVENT_OBJECT_SYMBOL_E = "e";
        private const String EVENT_OBJECT_SYMBOL_F = "f";
        private const String EVENT_OBJECT_SYMBOL_G = "g";
        private const String EVENT_OBJECT_SYMBOL_H = "h";
        private const String EVENT_OBJECT_FUNNEL = "m";
        private const String EVENT_OBJECT_FUNNEL_VALUE = "f";
        private const String EVENT_OBJECT_USE_TEXT_DESCRIPTION = "o";
        private const String EVENT_OBJECT_USE_TEXT_DESCRIPTION_VALUE = "t";
        private const String EVENT_OBJECT_SIZE = "s";
        private const String EVENT_OBJECT_TEXT = "t";

        private const String EVENT_OBJECT_TYPE_START_CONTROL = "s";
        private const String EVENT_OBJECT_TYPE_CONTROL = "c";
        private const String EVENT_OBJECT_TYPE_MARKED_ROUTE = "m";
        private const String EVENT_OBJECT_TYPE_FINISH_CONTROL = "f";
        private const String EVENT_OBJECT_TYPE_CONTROL_DESCRIPTION = "d";
        private const String EVENT_OBJECT_TYPE_COURSE_TITLE = "n";
        private const String EVENT_OBJECT_TYPE_RELAY_START_NUMBER = "u";
        private const String EVENT_OBJECT_TYPE_RELAY_VARIATION = "v";
        private const String EVENT_OBJECT_TYPE_TEXT_BLOCK = "t";

        private void CopyToEventObject(Model.Map map)
        {
            if (map.Event == null)
            {
                map.Event = new Event.Event();
            }

            String eventObjectType = null;
            Model.PointSymbol symbolC = null;
            Model.PointSymbol symbolD = null;
            Model.PointSymbol symbolE = null;
            Model.PointSymbol symbolF = null;
            Model.PointSymbol symbolG = null;
            Model.PointSymbol symbolH = null;
            Boolean funnel = true;
            Boolean useTextDescription = true;
            String size = null;
            String text = null;

            // index
            int i = 0;
            while (i <= _codeValue.GetUpperBound(0))
            {
                string code = _codeValue[i, 0];
                switch (code)
                {
                    case EVENT_OBJECT_TYPE:
                        eventObjectType = GetStringValue(i);
                        break;
                    case EVENT_OBJECT_SYMBOL_C:
                        symbolC = (Model.PointSymbol)MapExtension.GetOrCreateSymbol(map, GetStringValue(i), Model.Type.FeatureType.Point);
                        break;
                    case EVENT_OBJECT_SYMBOL_D:
                        symbolD = (Model.PointSymbol)MapExtension.GetOrCreateSymbol(map, GetStringValue(i), Model.Type.FeatureType.Point);
                        break;
                    case EVENT_OBJECT_SYMBOL_E:
                        symbolE = (Model.PointSymbol)MapExtension.GetOrCreateSymbol(map, GetStringValue(i), Model.Type.FeatureType.Point);
                        break;
                    case EVENT_OBJECT_SYMBOL_F:
                        symbolF = (Model.PointSymbol)MapExtension.GetOrCreateSymbol(map, GetStringValue(i), Model.Type.FeatureType.Point);
                        break;
                    case EVENT_OBJECT_SYMBOL_G:
                        symbolG = (Model.PointSymbol)MapExtension.GetOrCreateSymbol(map, GetStringValue(i), Model.Type.FeatureType.Point);
                        break;
                    case EVENT_OBJECT_SYMBOL_H:
                        symbolH = (Model.PointSymbol)MapExtension.GetOrCreateSymbol(map, GetStringValue(i), Model.Type.FeatureType.Point);
                        break;
                    case EVENT_OBJECT_FUNNEL:
                        funnel = true;
                        break;
                    case EVENT_OBJECT_USE_TEXT_DESCRIPTION:
                        useTextDescription = true;
                        break;
                    case EVENT_OBJECT_SIZE:
                        size = GetStringValue(i); // ; or /
                        break;
                    case EVENT_OBJECT_TEXT:
                        text = GetStringValue(i);
                        break;
                    default:
                        throw CreateApplicationSettingException(i);
                }
                i++;
            }

            Model.AbstractObject modelObject = null;
            if (ModelObjectIndex != 0)
            {
                modelObject = map.Objects[ModelObjectIndex - 1];
            }

            switch (eventObjectType)
            {
                case EVENT_OBJECT_TYPE_START_CONTROL:
                    Event.Control start = (Event.Control)MapExtension.GetOrCreateEventObject(map, _mainValue, Event.Type.EventObjectType.StartControl, modelObject, true);
                    start.SymbolC = symbolC;
                    start.SymbolD = symbolD;
                    start.SymbolE = symbolE;
                    start.SymbolF = symbolF;
                    start.SymbolG = symbolG;
                    start.SymbolH = symbolH;
                    start.Size = size;
                    start.Text = text;
                    break;
                case EVENT_OBJECT_TYPE_CONTROL:
                    Event.Control control = (Event.Control)MapExtension.GetOrCreateEventObject(map, _mainValue, Event.Type.EventObjectType.Control, modelObject, true);
                    control.SymbolC = symbolC;
                    control.SymbolD = symbolD;
                    control.SymbolE = symbolE;
                    control.SymbolF = symbolF;
                    control.SymbolG = symbolG;
                    control.SymbolH = symbolH;
                    control.Size = size;
                    control.Text = text;
                    break;
                case EVENT_OBJECT_TYPE_MARKED_ROUTE:
                    Event.MarkedRoute route = (Event.MarkedRoute)MapExtension.GetOrCreateEventObject(map, _mainValue, Event.Type.EventObjectType.MarkedRoute, modelObject, true);
                    route.Funnel = funnel;
                    route.Text = text;
                    break;
                case EVENT_OBJECT_TYPE_FINISH_CONTROL:
                    MapExtension.GetOrCreateEventObject(map, _mainValue, Event.Type.EventObjectType.FinishControl, modelObject, true);
                    break;
                case EVENT_OBJECT_TYPE_CONTROL_DESCRIPTION:
                    Event.ControlDescriptionSheet cd = (Event.ControlDescriptionSheet)MapExtension.GetOrCreateEventObject(map, _mainValue, Event.Type.EventObjectType.ControlDescriptionSheet, modelObject, true);
                    cd.UseTextDescription = useTextDescription;
                    break;
                case EVENT_OBJECT_TYPE_COURSE_TITLE:
                    MapExtension.GetOrCreateEventObject(map, _mainValue, Event.Type.EventObjectType.CourseTitle, modelObject, true);
                    break;
                case EVENT_OBJECT_TYPE_RELAY_START_NUMBER:
                    MapExtension.GetOrCreateEventObject(map, _mainValue, Event.Type.EventObjectType.RelayStartNumber, modelObject, true);
                    break;
                case EVENT_OBJECT_TYPE_RELAY_VARIATION:
                    MapExtension.GetOrCreateEventObject(map, _mainValue, Event.Type.EventObjectType.RelayVariation, modelObject, true);
                    break;
                case EVENT_OBJECT_TYPE_TEXT_BLOCK:
                    Event.TextBlock textBlock = (Event.TextBlock)MapExtension.GetOrCreateEventObject(map, _mainValue, Event.Type.EventObjectType.TextBlock, modelObject, true);
                    textBlock.Text = text;
                    break;
            }
        }

        private static void CopyFromEventObjects(Model.Map map, List<Setting> settings)
        {
            foreach (Event.AbstractObject source in map.Event.Objects)
            {
                Setting setting = new Setting();
                setting.SettingType = Type.SettingType.EventObject;
                settings.Add(setting);

                StringBuilder b = new StringBuilder(source.Code);
                switch (source.Type)
                {
                    case Event.Type.EventObjectType.Control:
                        Event.Control control = (Event.Control)source;
                        setting.ModelObjectIndex = control.ModelObject.Index;
                        Write(b, EVENT_OBJECT_TYPE, EVENT_OBJECT_TYPE_CONTROL);
                        Write(b, EVENT_OBJECT_SYMBOL_C, control.SymbolC);
                        Write(b, EVENT_OBJECT_SYMBOL_D, control.SymbolD);
                        Write(b, EVENT_OBJECT_SYMBOL_E, control.SymbolE);
                        Write(b, EVENT_OBJECT_SYMBOL_F, control.SymbolF);
                        Write(b, EVENT_OBJECT_SYMBOL_G, control.SymbolG);
                        Write(b, EVENT_OBJECT_SYMBOL_H, control.SymbolH);
                        Write(b, EVENT_OBJECT_SIZE, control.Size);
                        Write(b, EVENT_OBJECT_TEXT, control.Text);
                        break;
                    case Event.Type.EventObjectType.ControlDescriptionSheet:
                        Event.ControlDescriptionSheet controlDescription = (Event.ControlDescriptionSheet)source;
                        setting.ModelObjectIndex = controlDescription.ModelObject.Index;
                        Write(b, EVENT_OBJECT_TYPE, EVENT_OBJECT_TYPE_CONTROL_DESCRIPTION);
                        if (controlDescription.UseTextDescription)
                        {
                            Write(b, EVENT_OBJECT_USE_TEXT_DESCRIPTION, EVENT_OBJECT_USE_TEXT_DESCRIPTION_VALUE);
                        }
                        break;
                    case Event.Type.EventObjectType.CourseTitle:
                        Event.MappedObject courseTitle = (Event.MappedObject)source;
                        setting.ModelObjectIndex = courseTitle.ModelObject.Index;
                        Write(b, EVENT_OBJECT_TYPE, EVENT_OBJECT_TYPE_COURSE_TITLE);
                        break;
                    case Event.Type.EventObjectType.FinishControl:
                        Event.Control finishControl = (Event.Control)source;
                        setting.ModelObjectIndex = finishControl.ModelObject.Index;
                        Write(b, EVENT_OBJECT_TYPE, EVENT_OBJECT_TYPE_FINISH_CONTROL);
                        break;
                    case Event.Type.EventObjectType.MarkedRoute:
                        Event.MarkedRoute markedRoute = (Event.MarkedRoute)source;
                        setting.ModelObjectIndex = markedRoute.ModelObject.Index;
                        Write(b, EVENT_OBJECT_TYPE, EVENT_OBJECT_TYPE_MARKED_ROUTE);
                        Write(b, EVENT_OBJECT_TEXT, markedRoute.Text);
                        if (markedRoute.Funnel)
                        {
                            Write(b, EVENT_OBJECT_FUNNEL, EVENT_OBJECT_FUNNEL_VALUE);
                        }
                        break;
                    case Event.Type.EventObjectType.RelayStartNumber:
                        Event.MappedObject relayStartNumber = (Event.MappedObject)source;
                        setting.ModelObjectIndex = relayStartNumber.ModelObject.Index;
                        Write(b, EVENT_OBJECT_TYPE, EVENT_OBJECT_TYPE_RELAY_START_NUMBER);
                        break;
                    case Event.Type.EventObjectType.RelayVariation:
                        Event.MappedObject relayVariation = (Event.MappedObject)source;
                        setting.ModelObjectIndex = relayVariation.ModelObject.Index;
                        Write(b, EVENT_OBJECT_TYPE, EVENT_OBJECT_TYPE_RELAY_VARIATION);
                        break;
                    case Event.Type.EventObjectType.StartControl:
                        Event.Control startControl = (Event.Control)source;
                        setting.ModelObjectIndex = startControl.ModelObject.Index;
                        Write(b, EVENT_OBJECT_TYPE, EVENT_OBJECT_TYPE_START_CONTROL);
                        Write(b, EVENT_OBJECT_SYMBOL_C, startControl.SymbolC);
                        Write(b, EVENT_OBJECT_SYMBOL_D, startControl.SymbolD);
                        Write(b, EVENT_OBJECT_SYMBOL_E, startControl.SymbolE);
                        Write(b, EVENT_OBJECT_SYMBOL_F, startControl.SymbolF);
                        Write(b, EVENT_OBJECT_SYMBOL_G, startControl.SymbolG);
                        Write(b, EVENT_OBJECT_SYMBOL_H, startControl.SymbolH);
                        Write(b, EVENT_OBJECT_SIZE, startControl.Size);
                        Write(b, EVENT_OBJECT_TEXT, startControl.Text);
                        break;
                    case Event.Type.EventObjectType.TextBlock:
                        Event.TextBlock textBlock = (Event.TextBlock)source;
                        Write(b, EVENT_OBJECT_TYPE, EVENT_OBJECT_TYPE_TEXT_BLOCK);
                        Write(b, EVENT_OBJECT_TEXT, textBlock.Text);
                        break;
                }
                setting.ConcatenatedValues = b.ToString();
            }
        }
    }
}