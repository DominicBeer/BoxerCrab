using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceIndex
{
    /// <summary>
    /// Basic vector implementation for space store. Not to be confused with RhinoCommon Vector3d
    /// </summary>
    public struct Vector3d
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
