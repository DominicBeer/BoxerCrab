using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceIndex
{
    /// <summary>
    /// Basic point implementation for space store. Not to be confused with RhinoCommon Point3d
    /// </summary>
    public struct Point3d
    {
        public double X { get;}
        public double Y { get;}
        public double Z { get;}

        public Point3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
        public static double DistanceBetween(Point3d p1, Point3d p2)
        {
            var lx = p1.X - p2.X;
            var ly = p1.Y - p2.Y;
            var lz = p1.Z - p2.Z;
            return Math.Sqrt(lx * lx + ly * ly + lz * lz);
        }

        public static double DistanceSquaredBetween(Point3d p1, Point3d p2)
        {
            var lx = p1.X - p2.X;
            var ly = p1.Y - p2.Y;
            var lz = p1.Z - p2.Z;
            return lx * lx + ly * ly + lz * lz;
        }

        public static Point3d operator + (Point3d p, Vector3d v) 
            => new Point3d(p.X+v.X, p.Y+v.Y, p.Z+v.Z);

        public static Point3d operator - (Point3d p, Vector3d v)
            => new Point3d(p.X - v.X, p.Y - v.Y, p.Z - v.Z);

    }
}
