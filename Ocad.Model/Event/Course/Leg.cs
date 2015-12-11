using System;
using System.Collections.Generic;
using System.Text;
using Ocad.Attribute;

namespace Ocad.Event.Course
{
    [VersionsSupported(V9 = true)]
    public class Leg
    {
        public List<Ocad.Model.AbstractObject> ModelObjects { get; set; }

        [VersionsSupported(V9 = true)]
        public EventObjectDescription StartWayPoint { get; set; }
        [VersionsSupported(V9 = true)]
        public EventObjectDescription EndWayPoint { get; set; }

        public Leg(Ocad.Model.AbstractObject modelObject)
        {
            ModelObjects = new List<Model.AbstractObject>();
            if (modelObject != null)
            {
                ModelObjects.Add(modelObject);
            }
        }
    }
}
