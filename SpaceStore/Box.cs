using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceIndex
{
    public struct Box
    {
        public Point3d Min { get; }    
        public Point3d Max { get; }

        public Box(Point3d min, Point3d max)
        {
            var test = true;
            test = test && (min.X <= max.X ) && (min.Y <= max.Y ) && (min.Z <= max.Z );
            if(!test)
            {
                throw new ArgumentException("Min point dimensions must be smaller than those of the max point when constructing a box");
            }
            Min = min;
            Max = max;
        }

        public double Diagonal => Point3d.DistanceBetween(Min, Max);
    }
}
