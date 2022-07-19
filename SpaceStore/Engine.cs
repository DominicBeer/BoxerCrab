using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceIndex
{
    /// <summary>
    /// Class exposing SpaceIndexes functionality
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// For a sequence of boxes find all the intersecting pairs of boxes.
        /// </summary>
        /// <param name="boxes"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static (int Index1, int Index2)[] GetSelfIntersectingPairs(IEnumerable<Box> boxes, double tolerance)
            => Functions.GetSelfIntersectingPairs( boxes , tolerance)
                .ToArray();
        public static (int Index1, int Index2)[] GetIntersectingPairs(IEnumerable<Box> boxSet1, IEnumerable<Box> boxSet2, double tolerance)
            => Functions.GetIntersectingPairs(boxSet1, boxSet2, tolerance);
                
    }
}
