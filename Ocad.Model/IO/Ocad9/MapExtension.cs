using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Ocad.IO.Ocad9
{
    internal static class MapExtension
    {
        internal static Model.Colour GetOrCreateColour(Model.Map map, Int16 colourNumber, Boolean moveToEnd = false)
        {
            Model.Colour colour = null;
            foreach (Model.Colour s in map.ColourTable)
            {
                if (s.Number.Equals(colourNumber))
                {
                    if (!moveToEnd)
                    {
                        return s;
                    }
                    colour = s;
                    break;
                }
            }

            if (colour != null)
            {
                map.ColourTable.Remove(colour);
                map.ColourTable.Add(colour);
                return colour;
            }

            colour = new Model.Colour()
            {
                Number = colourNumber
            };
            map.ColourTable.Add(colour);
            return colour;
        }

        internal static Model.SpotColour GetOrCreateSpotColour(Model.Map map, String spotColourName, Boolean moveToEnd = false)
        {
            Model.SpotColour spotColour = null;
            foreach (Model.SpotColour s in map.SpotColours)
            {
                if (s.Name.Equals(spotColourName))
                {
                    if (!moveToEnd)
                    {
                        return s;
                    }
                    spotColour = s;
                    break;
                }
            }

            if (spotColour != null)
            {
                map.SpotColours.Remove(spotColour);
                map.SpotColours.Add(spotColour);
                return spotColour;
            }

            spotColour = new Model.SpotColour()
            {
                Name = spotColourName
            };
            map.SpotColours.Add(spotColour);
            return spotColour;
        }

        internal static Model.AbstractSymbol GetOrCreateSymbol(Model.Map map, String symbolNumber, Model.Type.FeatureType objectType)
        {
            foreach (Model.AbstractSymbol s in map.Symbols)
            {
                if (s.Number.Equals(symbolNumber))
                {
                    return s;
                }
            }

            // Should not really happen
            Model.AbstractSymbol symbol;
            switch (objectType)
            {
                case Model.Type.FeatureType.Area:
                    symbol = new Model.AreaSymbol();
                    break;
                case Model.Type.FeatureType.FormattedText:
                case Model.Type.FeatureType.UnformattedText:
                    symbol = new Model.BlockTextSymbol();
                    break;
                case Model.Type.FeatureType.LineText:
                    symbol = new Model.LineTextSymbol();
                    break;
                case Model.Type.FeatureType.Point:
                    symbol = new Model.PointSymbol();
                    break;
                case Model.Type.FeatureType.Rectangle:
                    symbol = new Model.RectangleSymbol();
                    break;
                case Model.Type.FeatureType.Line:
                    symbol = new Model.LineSymbol();
                    break;
                default:
                    return null; // throw
            }
            symbol.Number = symbolNumber;
            map.Symbols.Add(symbol);

            return symbol;
        }

        internal static Model.AbstractSymbol GetOrCreateSymbol(Model.Map map, String symbolNumber, Model.Type.SymbolType symbolType, Boolean moveToEnd = false)
        {
            Model.AbstractSymbol symbol = null;
            foreach (Model.AbstractSymbol s in map.Symbols)
            {
                if (s.Number.Equals(symbolNumber))
                {
                    if (!moveToEnd)
                    {
                        return s;
                    }
                    symbol = s;
                    break;
                }
            }

            if (symbol != null)
            {
                map.Symbols.Remove(symbol);
                map.Symbols.Add(symbol);
                return symbol;
            }

            switch (symbolType)
            {
                case Model.Type.SymbolType.Area:
                    symbol = new Model.AreaSymbol();
                    break;
                case Model.Type.SymbolType.BlockText:
                    symbol = new Model.BlockTextSymbol();
                    break;
                case Model.Type.SymbolType.LineText:
                    symbol = new Model.LineTextSymbol();
                    break;
                case Model.Type.SymbolType.Point:
                    symbol = new Model.PointSymbol();
                    break;
                case Model.Type.SymbolType.Rectangle:
                    symbol = new Model.RectangleSymbol();
                    break;
                case Model.Type.SymbolType.Line:
                    symbol = new Model.LineSymbol();
                    break;
                default:
                    return null; // throw
            }
            symbol.Number = symbolNumber;
            map.Symbols.Add(symbol);

            return symbol;
        }

        internal static Model.SymbolTreeNode GetOrCreateSymbolTree(Model.Map map, Int16 groupId, Boolean moveToEnd = false)
        {
            if (groupId == 0)
            {
                return null;
            }

            Model.SymbolTreeNode symbolTree = null;
            foreach (Model.SymbolTreeNode s in map.SymbolTrees)
            {
                if (s.Id.Equals(groupId))
                {
                    if (!moveToEnd)
                    {
                        return s;
                    }
                    symbolTree = s;
                    break;
                }
            }

            if (symbolTree != null)
            {
                map.SymbolTrees.Remove(symbolTree);
                map.SymbolTrees.Add(symbolTree);
                return symbolTree;
            }

            symbolTree = new Model.SymbolTreeNode()
            {
                Id = groupId
            };
            map.SymbolTrees.Add(symbolTree);
            return symbolTree;
        }

        internal static Model.ImportLayer GetOrCreateImportLayer(Model.Map map, Int16 layerNumber, Boolean moveToEnd = false)
        {
            if (layerNumber == 0)
            {
                return null;
            }

            Model.ImportLayer importLayer = null;
            foreach (Model.ImportLayer s in map.ImportLayers)
            {
                if (s.LayerNumber.Equals(layerNumber))
                {
                    if (!moveToEnd)
                    {
                        return s;
                    }
                    importLayer = s;
                    break;
                }
            }

            if (importLayer != null)
            {
                map.ImportLayers.Remove(importLayer);
                map.ImportLayers.Add(importLayer);
                return importLayer;
            }

            importLayer = new Model.ImportLayer()
            {
                LayerNumber = layerNumber
            };
            map.ImportLayers.Add(importLayer);
            return importLayer;
        }

        internal static Event.Course.Course GetOrCreateEventCourse(Model.Map map, String courseName, Boolean moveToEnd = false)
        {
            Event.Course.Course course = null;
            foreach (Event.Course.Course s in map.Event.Courses)
            {
                if (s.Name.Equals(courseName))
                {
                    if (!moveToEnd)
                    {
                        return s;
                    }
                    course = s;
                    break;
                }
            }

            if (course != null)
            {
                map.Event.Courses.Remove(course);
                map.Event.Courses.Add(course);
                return course;
            }

            course = new Event.Course.Course()
            {
                Name = courseName
            };
            map.Event.Courses.Add(course);
            return course;
        }

        internal static Event.AbstractObject GetOrCreateEventObject(Model.Map map, String controlCode, Event.Type.EventObjectType type = Event.Type.EventObjectType.Unassigned, Model.AbstractObject modelObject = null, Boolean moveToEnd = false)
        {
            Event.AbstractObject eventObject = null;

            foreach (Event.AbstractObject s in map.Event.Objects)
            {
                if (s.Code.Equals(controlCode))
                {
                    if ((type != Event.Type.EventObjectType.Unassigned) && (s is Event.Control))
                    {
                        Event.Control t = (Event.Control)s;
                        if (t.ControlType == Event.Type.EventControlType.Unassigned)
                        {
                            t.ControlType = (Event.Type.EventControlType)type;
                        }
                    }

                    if ((modelObject != null) && (s is Event.MappedObject))
                    {
                        Event.MappedObject t = (Event.MappedObject)s;
                        t.ModelObject = modelObject;
                    }

                    if (!moveToEnd)
                    {
                        return s;
                    }
                    eventObject = s;
                    break;
                }
            }

            if (eventObject != null)
            {
                map.Event.Objects.Remove(eventObject);
                map.Event.Objects.Add(eventObject);
                return eventObject;
            }

            switch (type)
            {
                case Event.Type.EventObjectType.Control:
                case Event.Type.EventObjectType.FinishControl:
                case Event.Type.EventObjectType.StartControl:
                case Event.Type.EventObjectType.Unassigned:
                    eventObject = new Event.Control(controlCode, (Event.Type.EventControlType)type, modelObject);
                    break;
                case Event.Type.EventObjectType.CourseTitle:
                case Event.Type.EventObjectType.RelayStartNumber:
                case Event.Type.EventObjectType.RelayVariation:
                    eventObject = new Event.MappedObject(controlCode, type, modelObject);
                    break;
                case Event.Type.EventObjectType.ControlDescriptionSheet:
                    eventObject = new Event.ControlDescriptionSheet(controlCode, modelObject);
                    break;
                case Event.Type.EventObjectType.MarkedRoute:
                    eventObject = new Event.MarkedRoute(controlCode, modelObject);
                    break;
                case Event.Type.EventObjectType.TextBlock:
                    eventObject = new Event.TextBlock(controlCode);
                    break;
            }

            map.Event.Objects.Add(eventObject);

            return eventObject;
        }

        internal static Event.Course.ControlDescription GetOrCreateEventControlDescription(Model.Map map, String courseName, String controlCode, Boolean moveToEnd = false)
        {
            Event.Course.Course course = GetOrCreateEventCourse(map, courseName, false);
            Event.Course.ControlDescription courseControlObject = null;

            foreach (Event.Course.ControlDescription s in course.CourseControls)
            {
                if (s.EventObject.Code.Equals(controlCode))
                {
                    if (!moveToEnd)
                    {
                        return s;
                    }
                    courseControlObject = s;
                    break;
                }
            }

            if (courseControlObject != null)
            {
                course.CourseControls.Remove(courseControlObject);
                course.CourseControls.Add(courseControlObject);
                return courseControlObject;
            }

            courseControlObject = new Event.Course.ControlDescription(GetOrCreateEventObject(map, controlCode, Event.Type.EventObjectType.Control));
            course.CourseControls.Add(courseControlObject);
            return courseControlObject;
        }

        internal static Event.Course.Leg GetOrCreateEventCourseLeg(Model.Map map, String courseName, String startControlCode, String endControlCode, Model.AbstractObject modelObject = null, Boolean moveToEnd = false)
        {
            Event.Course.Course course = GetOrCreateEventCourse(map, courseName, false);
            Event.Course.Leg eventLeg = null;

            foreach (Event.Course.Leg s in course.CourseLegs)
            {
                if ((s.StartWayPoint.EventObject.Code.Equals(startControlCode)) && (s.EndWayPoint.EventObject.Code.Equals(endControlCode)))
                {
                    if (modelObject != null)
                    {
                        s.ModelObjects.Add(modelObject);
                    }

                    if (!moveToEnd)
                    {
                        return s;
                    }
                    eventLeg = s;
                    break;
                }
            }

            if (eventLeg != null)
            {
                course.CourseLegs.Remove(eventLeg);
                course.CourseLegs.Add(eventLeg);
                return eventLeg;
            }

            eventLeg = new Event.Course.Leg(modelObject);
            eventLeg.StartWayPoint = MapExtension.GetOrCreateEventControlDescription(map, courseName, startControlCode);
            eventLeg.EndWayPoint = MapExtension.GetOrCreateEventControlDescription(map, courseName, endControlCode);
            course.CourseLegs.Add(eventLeg);

            return eventLeg;
        }
    }
}
