using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceIndex
{
    internal class SpaceChunkPointTree
    {
        readonly Point3d[] m_points;
        readonly Box m_box;
        readonly double m_tol;
        readonly double side_l;
        readonly int m_depth;

        readonly Dictionary<SpaceChunk, List<int>> baseStore;
        readonly HashSet<SpaceChunk>[] upperLevels;

        public SpaceChunkPointTree(Point3d[] points, double tol, int depth) 
        {
            if(depth<=0)
            {
                throw new ArgumentException("Depth of tree must be positive, and less than 9");
            }

            if (depth > 8)
            {
                throw new ArgumentException("Depth tree of tree must be less than or equal to 8");
            }
            m_box = TransformToPositive(points, tol);
            m_points = points.ToArray();
            var np = points.Length;
            m_tol = tol;
            m_depth = depth;
            side_l = GetSideLength(m_box, depth);
            upperLevels = new HashSet<SpaceChunk>[depth-1];
            
            for(int i = 0; i < depth - 1; i++)
            {
                var d = i + 1;
                var size = (int)(
                    d <= 3 
                    ? Math.Pow(8, d) 
                    : (1 - d/depth)*(Math.Pow(8, d) + (d/depth)*(np / Math.Pow(8, depth - d))));
                Console.WriteLine(size);
                upperLevels[i] = new HashSet<SpaceChunk>(size);
            }

            baseStore = new Dictionary<SpaceChunk, List<int>>(np);
            for(int i = 0; i < np; i++)
            {
                var pt = points[i];
                var chunk = new SpaceChunk(pt, side_l);
                if(baseStore.ContainsKey(chunk))
                {
                    baseStore[chunk].Add(i);
                }
                else
                {
                    baseStore[chunk] = new List<int>() { i };
                }

                //Go up the heirarchy adding in the higher level chunks
                int d = depth - 1;
                var nl = chunk;
                while (d > 0)
                {
                    nl = nl.Parent;
                    if (upperLevels[d].Contains(nl)) break; //if there is already a chunk here, break, we are all good...
                    upperLevels[d].Add(nl);
                    d--;
                }
            }

        }


        private class DistanceResult
        {

        }

        private Point3d FindClosestPointWithinCells(
            Point3d target,
            SpaceChunk targetChunk,
            List<SpaceChunk> cellsToExplore,
            int depth,
            DistanceResult currentResult)
        {
            throw new NotImplementedException();
        }

        private static double GetSideLength(Box box, int depth)
        {
            var maxL = Math.Max(Math.Max(box.Lx, box.Ly), box.Lz);
            return maxL/Math.Pow(2,depth);
        }

        private static Box TransformToPositive(Point3d[] points, double tol)
        {
            var bbox = Functions.GetBoundingBox(points);
            var tol_v = new Vector3d(tol, tol, tol);
            var min = bbox.Min - tol_v;
            var max = bbox.Max + tol_v;
            return new Box(min, max); 
        }
    }
}
