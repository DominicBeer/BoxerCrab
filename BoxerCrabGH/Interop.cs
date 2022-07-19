using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
namespace BoxerCrab.GH
{
    internal static class Interop
    {
        public static SpaceIndex.Point3d ToSI(this Point3d pt) =>
                new SpaceIndex.Point3d(pt.X, pt.Y, pt.Z);

        public static SpaceIndex.Box ToSI(this BoundingBox box) =>
            new SpaceIndex.Box(box.Min.ToSI(), box.Max.ToSI());

        public static Point3d ToRhino(this SpaceIndex.Point3d pt) =>
            new Point3d(pt.X, pt.Y, pt.Z);

        public static BoundingBox ToRhino(this SpaceIndex.Box box)=>
            new BoundingBox(box.Min.ToRhino(), box.Max.ToRhino());
    }
}
