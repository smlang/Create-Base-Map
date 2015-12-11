using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Model
{
    [VersionsSupported(V9 = true)]
    public class ImageObject : AbstractObject
    {
        [VersionsSupported(V9 = true)]
        public Byte Cyan { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte Yellow { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte Magenta { get; set; }
        [VersionsSupported(V9 = true)]
        public Byte Black { get; set; }

        [VersionsSupported(V9 = true)]
        public override Type.ObjectType Type
        {
            get
            {
                return Model.Type.ObjectType.Image;
            }
        }

        internal ImageObject(Model.Map map, Model.Type.FeatureType featureType)
            : base(map, featureType)
        {
        }
    }
}
