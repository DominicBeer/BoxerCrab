using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace BoxerCrab.GH
{
    internal static class Util
    {
        public static BoundingBox GetBestBoundingBox(this IGH_GeometricGoo goo, Plane plane)
        {
            if (plane == Plane.WorldXY)
            {
                return goo.Boundingbox;
            }
            var xform = Transform.ChangeBasis(plane, Plane.WorldXY);
            return goo.GetBoundingBox(xform);
        }
    }
}
