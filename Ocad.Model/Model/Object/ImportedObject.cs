using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class ImportedObject : AbstractObject
    {
        [VersionsSupported(V9 = true)]
        public override Type.ObjectType Type
        {
            get
            {
                return Model.Type.ObjectType.Imported;
            }
        }

        internal ImportedObject(Model.Map map, Model.Type.FeatureType featureType)
            : base(map, featureType)
        {
        }
    }
}
