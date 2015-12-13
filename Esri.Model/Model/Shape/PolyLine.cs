using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esri.Model.Shape
{
    public class PolyLine : AbstractShape
    {
        public Double XMin { get; set; }
        public Double YMin { get; set; }
        public Double XMax { get; set; }
        public Double YMax { get; set; }
        public int PartCount { get; set; }
        public int PointCount { get; set; }
        public List<int> PartStartPointIndexes { get; set; }
        public List<Point> Points { get; set; }

        public PolyLine()
        {
            PartStartPointIndexes = new List<int>();
            Points = new List<Point>();
        }
    }
}
