using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    public class Relationship2
    {
        public enum Exist
        {
            None = 1,
            Some = 0,
            All = 2,
            Unknown = 3
        }

        public class _Vector
        {
            public class _BVector
            {
                public Exist Apart;
                public Exist Identical;

                public _BVector()
                {
                    Apart = Exist.Unknown;
                    Identical = Exist.Unknown;
                }
            }

            public class _BEdge
            {
                public Exist Apart;
                public Exist Intersect_0D;

                public _BEdge()
                {
                    Apart = Exist.Unknown;
                    Intersect_0D = Exist.Unknown;
                }
            }

            public class _BPlane
            {
                public Exist Apart;
                public Exist Intersect_0D;

                public _BPlane()
                {
                    Apart = Exist.Unknown;
                    Intersect_0D = Exist.Unknown;
                }
            }

            public _BVector Vector;
            public _BEdge Edge;
            public _BPlane Plane;

            public _Vector()
            {
                this.Vector = new _BVector();
            }
        }

        public class _Edge
        {
            public class _BVector
            {
                public Exist Apart;
                public Exist Superset;

                public _BVector()
                {
                    Apart = Exist.Unknown;
                    Superset = Exist.Unknown;
                }
            }

            public class _BEdge
            {
                public Exist Apart;
                public Exist Intersect_0D;
                public Exist Intersect_1D;
                public Exist Subset;
                public Exist Identical;
                public Exist Superset;

                public _BEdge()
                {
                    Apart = Exist.Unknown;
                    Intersect_0D = Exist.Unknown;
                    Intersect_1D = Exist.Unknown;
                    Subset = Exist.Unknown;
                    Identical = Exist.Unknown;
                    Superset = Exist.Unknown;
                }
            }

            public class _BPlane
            {
                public Exist Apart;
                public Exist Intersect_1D;
                public Exist Subset;

                public _BPlane()
                {
                    Apart = Exist.Unknown;
                    Intersect_1D = Exist.Unknown;
                    Subset = Exist.Unknown;
                }
            }

            public _BVector Vector;
            public _BEdge Edge;
            public _BPlane Plane;

            public _Edge()
            {
                this.Vector = new _BVector();
            }
        }

        public class _Plane
        {
            public class _BVector
            {
                public Exist Apart;
                public Exist Superset;

                public _BVector()
                {
                    Apart = Exist.Unknown;
                    Superset = Exist.Unknown;
                }
            }

            public class _BEdge
            {
                public Exist Apart;
                public Exist Intersect_1D;
                public Exist Superset;

                public _BEdge()
                {
                    Apart = Exist.Unknown;
                    Intersect_1D = Exist.Unknown;
                    Superset = Exist.Unknown;
                }
            }

            public class _BPlane
            {
                public Exist Apart;
                public Exist Intersect_2D;
                public Exist Subset;
                public Exist Identical;
                public Exist Superset;

                public _BPlane()
                {
                    Apart = Exist.Unknown;
                    Intersect_2D = Exist.Unknown;
                    Subset = Exist.Unknown;
                    Identical = Exist.Unknown;
                    Superset = Exist.Unknown;
                }
            }

            public _BVector Vector;
            public _BEdge Edge;
            public _BPlane Plane;

            public _Plane()
            {
                this.Vector = new _BVector();
            }
        }

        public _Vector Vector;
        public _Edge Edge;
        public _Plane Plane;

        public bool Apart
        {
            get
            {
                if (this.Vector.Vector.Apart != Exist.All) { return false; }
                if ((this.Vector.Edge != null) && (this.Vector.Edge.Apart != Exist.All)) { return false; }
                if ((this.Vector.Plane != null) && (this.Vector.Plane.Apart != Exist.All)) { return false; }
                if (this.Edge != null)
                {
                    if (this.Edge.Vector.Apart != Exist.All) { return false; }
                    if ((this.Edge.Edge != null) && (this.Edge.Edge.Apart != Exist.All)) { return false; }
                    if ((this.Edge.Plane != null) && (this.Edge.Plane.Apart != Exist.All)) { return false; }
                }
                if (this.Plane != null)
                {
                    if (this.Plane.Vector.Apart != Exist.All) { return false; }
                    if ((this.Plane.Edge != null) && (this.Plane.Edge.Apart != Exist.All)) { return false; }
                    if ((this.Plane.Plane != null) && (this.Plane.Plane.Apart != Exist.All)) { return false; }
                }
                return true;
            }
        }

        public bool Intersect_0D
        {
            get
            {
                if ((this.Vector.Edge != null) && (this.Vector.Edge.Intersect_0D != Exist.None)) { return true; }
                if ((this.Vector.Plane != null) && (this.Vector.Plane.Intersect_0D != Exist.None)) { return true; }
                if (this.Edge != null)
                {
                    if ((this.Edge.Edge != null) && (this.Edge.Edge.Intersect_0D != Exist.None)) { return true; }
                }
                return false;
            }
        }

        public bool Intersect_1D
        {
            get
            {
                if (this.Edge != null)
                {
                    if ((this.Edge.Edge != null) && (this.Edge.Edge.Intersect_1D != Exist.None)) { return true; }
                    if ((this.Edge.Plane != null) && (this.Edge.Plane.Intersect_1D != Exist.None)) { return true; }
                }
                if (this.Plane != null)
                {
                    if ((this.Plane.Edge != null) && (this.Plane.Edge.Intersect_1D != Exist.None)) { return true; }
                }
                return false;
            }
        }

        public bool Intersect_2D
        {
            get
            {
                if (this.Plane != null)
                {
                    if ((this.Plane.Plane != null) && (this.Plane.Plane.Intersect_2D != Exist.None)) { return true; }
                }
                return false;
            }
        }

        public bool Subset
        {
            get
            {
                // If this has planes
                if (this.Plane != null)
                {
                    // then must subset all/some of the other planes  
                    if (this.Plane.Plane != null)
                    {
                        if (this.Plane.Plane.Identical == Exist.All) { return false; }
                        if (this.Plane.Plane.Superset == Exist.All) { return false; }
                        if (this.Plane.Plane.Apart == Exist.All) { return false; }
                        return true;
                    }
                    return false;
                }

                if (this.Edge != null)
                {
                    if (this.Edge.Plane != null)
                    {
                        if (this.Edge.Plane.Apart == Exist.All) { return false; }
                        return true;
                    }

                    if (this.Edge.Edge != null)
                    {
                        if (this.Edge.Edge.Identical == Exist.All) { return false; }
                        if (this.Edge.Edge.Superset == Exist.All) { return false; }
                        if (this.Edge.Edge.Apart == Exist.All) { return false; }
                        return true;
                    }
                    return false;
                }

                if (this.Vector.Plane != null)
                {
                    if (this.Vector.Plane.Apart == Exist.All) { return false; }
                    return true;
                }

                if (this.Vector.Edge != null)
                {
                    if (this.Vector.Edge.Apart == Exist.All) { return false; }
                    return true;
                }

                return false;
            }
        }

        public bool Identical
        {
            get
            {
                // If this has planes
                if (this.Plane != null)
                {
                    // then the other must have identical planes
                    if ((this.Plane.Plane != null) && (this.Plane.Plane.Identical == Exist.All)) { return true; }
                    return false;
                }
                // Otherwise, if this has edges
                if (this.Edge != null)
                {
                    // then the other must have identical edges
                    if ((this.Edge.Edge != null) && (this.Edge.Edge.Identical == Exist.All)) { return true; }
                    return false;
                }
                // Otherwise, all vectors must be idential
                if (this.Vector.Vector.Identical == Exist.All) { return true; }
                return false;
            }
        }

        public bool Superset
        {
            get
            {
                // If this has planes
                if (this.Plane != null)
                {
                    // then must not have any subsets, intersections, apart  
                    if (this.Plane.Plane != null)
                    {
                        if (this.Plane.Plane.Subset != Exist.None) { return false; }
                        if (this.Plane.Plane.Intersect_2D != Exist.None) { return false; }
                        if (this.Plane.Plane.Apart != Exist.None) { return false; }
                        // At least one plane must be a superset
                        if (this.Plane.Plane.Superset != Exist.None) { return true; }
                    }
                    return false;
                }

                // If this has edges
                if (this.Edge != null)
                {
                    if (this.Edge.Edge != null)
                    {
                        // then must not have any subsets, intersections, apart
                        if (this.Edge.Edge.Subset != Exist.None) { return false; }
                        if (this.Edge.Edge.Intersect_0D != Exist.None) { return false; }
                        if (this.Edge.Edge.Intersect_1D != Exist.None) { return false; }
                        if (this.Edge.Edge.Apart != Exist.None) { return false; }
                        // At least one edge must be a superset
                        if (this.Plane.Edge.Superset != Exist.None) { return true; }
                    }
                    return false;
                }

                return false;
            }
        }

        public bool Neigbouring
        {
            get
            {
                return false;
            }
        }

        public Relationship2()
        {
            this.Vector = new _Vector();
        }

        public Relationship2 Inverse()
        {
            Relationship2 inverse = new Relationship2();

            inverse.Vector = new _Vector();
            inverse.Vector.Vector = new _Vector._BVector();
            inverse.Vector.Vector.Apart = this.Vector.Vector.Apart;
            inverse.Vector.Vector.Identical = this.Vector.Vector.Identical;
            if (this.Vector.Edge != null)
            {
                inverse.Edge = new _Edge();
                inverse.Edge.Vector = new _Edge._BVector();
                inverse.Edge.Vector.Apart = this.Vector.Edge.Apart;
                inverse.Edge.Vector.Superset = this.Vector.Edge.Intersect_0D;
            }
            if (this.Vector.Plane != null)
            {
                inverse.Plane = new _Plane();
                inverse.Plane.Vector = new _Plane._BVector();
                inverse.Plane.Vector.Apart = this.Vector.Plane.Apart;
                inverse.Plane.Vector.Superset = this.Vector.Plane.Intersect_0D;
            }
            if (this.Edge != null)
            {
                inverse.Vector.Edge = new _Vector._BEdge();
                inverse.Vector.Edge.Apart = this.Edge.Vector.Apart;
                inverse.Vector.Edge.Intersect_0D = this.Edge.Vector.Superset;
                if (this.Edge.Edge != null)
                {
                    if (inverse.Edge == null)
                    {
                        inverse.Edge = new _Edge();
                    }
                    inverse.Edge.Edge = new _Edge._BEdge();
                    inverse.Edge.Edge.Apart = this.Edge.Edge.Apart;
                    inverse.Edge.Edge.Intersect_0D = this.Edge.Edge.Intersect_0D;
                    inverse.Edge.Edge.Intersect_1D = this.Edge.Edge.Intersect_1D;
                    inverse.Edge.Edge.Subset = this.Edge.Edge.Superset;
                    inverse.Edge.Edge.Identical = this.Edge.Edge.Identical;
                    inverse.Edge.Edge.Superset = this.Edge.Edge.Subset;
                }
                if (this.Edge.Plane != null)
                {
                    if (inverse.Plane == null)
                    {
                        inverse.Plane = new _Plane();
                    }
                    inverse.Plane.Edge = new _Plane._BEdge();
                    inverse.Plane.Edge.Apart = this.Edge.Plane.Apart;
                    inverse.Plane.Edge.Intersect_1D = this.Edge.Plane.Intersect_1D;
                    inverse.Plane.Edge.Superset = this.Edge.Plane.Subset;
                }
            }
            if (this.Plane != null)
            {
                inverse.Vector.Plane = new _Vector._BPlane();
                inverse.Vector.Plane.Apart = this.Plane.Vector.Apart;
                inverse.Vector.Plane.Intersect_0D = this.Plane.Vector.Superset;
                if (this.Plane.Edge != null)
                {
                    if (inverse.Edge == null)
                    {
                        inverse.Edge = new _Edge();
                    }
                    inverse.Edge.Plane = new _Edge._BPlane();
                    inverse.Edge.Plane.Apart = this.Plane.Edge.Apart;
                    inverse.Edge.Plane.Intersect_1D = this.Plane.Edge.Intersect_1D;
                    inverse.Edge.Plane.Subset = this.Plane.Edge.Superset;
                }
                if (this.Plane.Plane != null)
                {
                    if (inverse.Plane == null)
                    {
                        inverse.Plane = new _Plane();
                    }
                    inverse.Plane.Plane = new _Plane._BPlane();
                    inverse.Plane.Plane.Apart = this.Plane.Plane.Apart;
                    inverse.Plane.Plane.Intersect_2D = this.Plane.Plane.Intersect_2D;
                    inverse.Plane.Plane.Subset = this.Plane.Plane.Superset;
                    inverse.Plane.Plane.Identical = this.Plane.Plane.Identical;
                    inverse.Plane.Plane.Superset = this.Plane.Plane.Subset;
                }
            }

            return inverse;
        }

        public void Include(Relationship2 component)
        {
            this.Vector.Vector.Apart &= component.Vector.Vector.Apart;
            this.Vector.Vector.Identical &= component.Vector.Vector.Identical;
            if (component.Vector.Edge != null)
            {
                if (this.Vector.Edge == null)
                {
                    this.Vector.Edge = new _Vector._BEdge();
                }
                this.Vector.Edge.Apart &= component.Vector.Edge.Apart;
                this.Vector.Edge.Intersect_0D &= component.Vector.Edge.Intersect_0D;
            }
            if (component.Vector.Plane != null)
            {
                if (this.Vector.Plane == null)
                {
                    this.Vector.Plane = new _Vector._BPlane();
                }
                this.Vector.Plane.Apart &= component.Vector.Plane.Apart;
                this.Vector.Plane.Intersect_0D &= component.Vector.Plane.Intersect_0D;
            }
            if (component.Edge != null)
            {
                if (this.Edge == null)
                {
                    this.Edge = new _Edge();
                }
                this.Edge.Vector.Apart &= component.Edge.Vector.Apart;
                this.Edge.Vector.Superset &= component.Edge.Vector.Superset;
                if (component.Edge.Edge != null)
                {
                    if (this.Edge.Edge == null)
                    {
                        this.Edge.Edge = new _Edge._BEdge();
                    }
                    this.Edge.Edge.Apart &= component.Edge.Edge.Apart;
                    this.Edge.Edge.Intersect_0D &= component.Edge.Edge.Intersect_0D;
                    this.Edge.Edge.Intersect_1D &= component.Edge.Edge.Intersect_1D;
                    this.Edge.Edge.Subset &= component.Edge.Edge.Superset;
                    this.Edge.Edge.Identical &= component.Edge.Edge.Identical;
                    this.Edge.Edge.Superset &= component.Edge.Edge.Subset;
                }
                if (component.Edge.Plane != null)
                {
                    if (this.Edge.Plane == null)
                    {
                        this.Edge.Plane = new _Edge._BPlane();
                    }
                    this.Edge.Plane.Apart &= component.Edge.Plane.Apart;
                    this.Edge.Plane.Intersect_1D &= component.Edge.Plane.Intersect_1D;
                    this.Edge.Plane.Subset &= component.Edge.Plane.Subset;
                }
            }
            if (component.Plane != null)
            {
                if (this.Plane == null)
                {
                    this.Plane = new _Plane();
                }
                this.Plane.Vector.Apart &= component.Plane.Vector.Apart;
                this.Plane.Vector.Superset &= component.Plane.Vector.Superset;
                if (component.Plane.Edge != null)
                {
                    if (this.Plane.Edge == null)
                    {
                        this.Plane.Edge = new _Plane._BEdge();
                    }
                    this.Plane.Edge.Apart &= component.Plane.Edge.Apart;
                    this.Plane.Edge.Intersect_1D &= component.Plane.Edge.Intersect_1D;
                    this.Plane.Edge.Superset &= component.Plane.Edge.Superset;
                }
                if (component.Plane.Plane != null)
                {
                    if (this.Plane.Plane == null)
                    {
                        this.Plane.Plane = new _Plane._BPlane();
                    }
                    this.Plane.Plane.Apart &= component.Plane.Plane.Apart;
                    this.Plane.Plane.Intersect_2D &= component.Plane.Plane.Intersect_2D;
                    this.Plane.Plane.Subset &= component.Plane.Plane.Superset;
                    this.Plane.Plane.Identical &= component.Plane.Plane.Identical;
                    this.Plane.Plane.Superset &= component.Plane.Plane.Subset;
                }
            }
        }
    }


    /*
                             Meet/ |         | Vertex   Vertex  Vertex  |  Edge     Edge    Edge   |  Face     Face    Face   | Overlap Sub Identical Super
    Object       Subject     Apart | Crosses | Touches  Touches Touches | Touches  Touches Touches | Touches  Touches Touches |   Set   Set    Set     Set
                                   |         | Vertex    Edge    Face   | Vertex    Edge    Face   | Vertex    Edge    Face   |
    -------------------------------+---------+--------------------------+--------------------------+--------------------------+----------------------------
    0-Dimension  0-Dimension   X   |    -    |    X        -       -    |    -        -       -    |    -        -       -    |    -     -      X       -
    0-Dimension  1-Dimension   X   |    -    |    X        X       -    |    -        -       -    |    -        -       -    |    -     -      -       - <---+
    0-Dimension  2-Dimension   X   |    -    |    X        X       X    |    -        -       -    |    -        -       -    |    -     -      -       - <---|-+
    -------------------------------+---------+--------------------------+--------------------------+--------------------------+----------------------------   | |
    1-Dimension  0-Dimension   X   |    -    |    X        -       -    |    X        -       -    |    -        -       -    |    -     -      -       - <---+ |
    1-Dimension  1-Dimension   X   |    X    |    X        X       -    |    X        X       -    |    -        -       -    |    X     X      X       X       |
    1-Dimension  2-Dimension   X   |    X    |    X        X       .    |    X        X       X    |    -        -       -    |    -     -      -       - <---+ |
    -------------------------------+---------+--------------------------+--------------------------+--------------------------+----------------------------   | |
    2-Dimension  0-Dimension   X   |    -    |    X        -       -    |    X        -       -    |    X        -       -    |    -     -      -       - <---|-+
    2-Dimension  1-Dimension   X   |    X    |    X        X       -    |    X        X       -    |    .        X       -    |    -     -      -       - <---+
    2-Dimension  2-Dimension   X   |    X    |    X        X       .    |    X        X       .    |    .        .       X    |    X     X      X       X
    */
    public enum Relationship
    {
        Apart = 0,
        Meet = 1,

        Cross = 1 + 2,
        EdgeOverlapsEdge,
        EdgeOverlapsFace,
        FaceOverlapsEdge,
        FaceOverlapsFace,

        VertexTouchesVertex = 1 + 4 + 32,
        VertexTouchesEdge = 1 + 4 + 64,
        VertexTouchesFace = 1 + 4 + 128,

        EdgeTouchesVertex = 1 + 8 + 256,
        EdgeTouchesEdge = 1 + 8 + 512,
        EdgeTouchesFace = 1 + 8 + 1024,

        FaceTouchesVertex = 1 + 16 + 2048,
        FaceTouchesEdge = 1 + 16 + 4096,
        FaceTouchesFace = 1 + 16 + 8192,

        OverlapSet = 1 + 16384,
        Subset = 1 + 32768,     //IsCoveredBy
        IdenticalSet = 1 + 65536,
        Superset = 1 + 131072,     //Covers
    }

    public enum DirectionSide
    {
        Rightside = -1,
        InLine = 0,
        Crosses,
        Leftside = 1
    }

    public enum Rotation
    {
        Anticlockwise = -1,
        Point = 0,
        Clockwise = 1
    }

    public enum EnclosedSide
    {
        Outside = -1,
        OnEdge = 0,
        Inside = 1
    }
}
