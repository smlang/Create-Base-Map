using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class GraphicObject : AbstractObject
    {
        [VersionsSupported(V9 = true)]
        public Colour Colour { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.ObjectType Type
        {
            get
            {
                return Model.Type.ObjectType.Graphic;
            }
        }

        internal GraphicObject(Model.Map map, Model.Type.FeatureType featureType)
            : base(map, featureType)
        {
        }
    }
}