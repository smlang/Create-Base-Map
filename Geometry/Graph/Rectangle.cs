using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Rectangle : Polygon
    {
        public Rectangle(Distance left, Distance right, Distance top, Distance bottom)
            : base()
        {
            Geometry.Point bottomLeftPoint = new Geometry.Point(left, bottom);
            Geometry.Point topLeftPoint = new Geometry.Point(left, top);
            Geometry.Point topRightPoint = new Geometry.Point(right, top);
            Geometry.Point bottomRightPoint = new Geometry.Point(right, bottom);

            _top = top;
            _bottom = bottom;
            _right = right;
            _left = left;

            Add2(new LinearSegment(bottomLeftPoint, topLeftPoint));
            Add2(new LinearSegment(topLeftPoint, topRightPoint));
            Add2(new LinearSegment(topRightPoint, bottomRightPoint));
            Add2(new LinearSegment(bottomRightPoint, bottomLeftPoint));
        }
    }
}
