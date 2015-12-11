using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event
{
    [VersionsSupported(V9 = true)]
    public class Control : MappedObject
    {
        #region Descriptions
        [VersionsSupported(V9 = true)]
        public Model.PointSymbol SymbolC { get; set; }
        [VersionsSupported(V9 = true)]
        public Model.PointSymbol SymbolD { get; set; }
        [VersionsSupported(V9 = true)]
        public Model.PointSymbol SymbolE { get; set; }
        [VersionsSupported(V9 = true)]
        public Model.PointSymbol SymbolF { get; set; }
        [VersionsSupported(V9 = true)]
        public Model.PointSymbol SymbolG { get; set; }
        [VersionsSupported(V9 = true)]
        public Model.PointSymbol SymbolH { get; set; }
        [VersionsSupported(V9 = true)]
        public String Size { get; set; }
        [VersionsSupported(V9 = true)]
        public String Text { get; set; }
        #endregion

        public Type.EventControlType ControlType { get; set; }

        public Ocad.Model.AbstractObject ControlCodeObject { get; set; }
        public Ocad.Model.AbstractObject ControlCirleObject { get; set; }
        public override Boolean IsWayPoint { get { return true; } }

        [VersionsSupported(V9 = true)]
        public override Type.EventObjectType Type { get { return (Ocad.Event.Type.EventObjectType)ControlType; } }

        public Control(String code, Type.EventControlType controlType, Model.AbstractObject modelObject)
            : base(code, (Ocad.Event.Type.EventObjectType)controlType, modelObject)
        {
            ControlType = controlType;
        }
    }
}
