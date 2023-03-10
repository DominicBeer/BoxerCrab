using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("SpaceStoreTests")]
[assembly: InternalsVisibleTo("BoxerCrabGH")]
namespace SpaceIndex
{
    public struct Box
    {
        public Point3d Min { get; private set; }
        public Point3d Max { get; private set; }

        public static Box CreateBox(Point3d min, Point3d max)
        {
            var test = true;
            test = test && (min.X <= max.X) && (min.Y <= max.Y) && (min.Z <= max.Z);
            if (!test)
            {
                throw new ArgumentException("Min point dimensions must be smaller than those of the max point when constructing a box");
            }
            return new Box(min, max);
        }

        internal Box(Point3d min, Point3d max)
        {
            Min = min;
            Max = max;
        }
        public double Diagonal => Point3d.DistanceBetween(Min, Max);

        public double Lx => Max.X - Min.X;  
        public double Ly => Max.Y - Min.Y;
        public double Lz => Max.Z - Min.Z; 
    }
}
