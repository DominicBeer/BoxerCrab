using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SpaceIndex
{
    internal static class Functions
    {
        public static List<(T P1, T P2)> LowerTrianglePairs<T>(this IEnumerable<T>  @this)
        {
            var items = @this.ToArray();
            var n = items.Length;
            var lOut = new List<(T, T)>((n * (n - 1)) / 2);
            for (int i = 0; i < n - 1; i++)
                for (int j = i + 1; j < n; j++)
                    lOut.Add((items[i], items[j]));
            return lOut;
        }
        public static IEnumerable<T> TapList<T>(this IEnumerable<T> @this, Action<T> action)
        {
            var returnList = @this.ToList();
            foreach (var item in returnList)
            {
                action(item);
            }
            return returnList; ;
        }

        public static (int I, int J, int K)[] Range3d(int nI, int nJ, int nK)
        {
            var indices = new (int, int, int)[nI * nJ * nK];
            int count = 0;
            for (int i = 0; i < nI; i++)
            {
                for (int j = 0; j < nJ; j++)
                {
                    for (int k = 0; k < nK; k++)
                    {
                        indices[count] = (i, j, k);
                        count++;
                    }
                }
            }
            return indices;
        }

        internal static (int Index1, int Index2)[] GetSelfIntersectingPairs(IEnumerable<Box> boxes, double tolerance)
        {
            var boxesA = boxes.ToArray();
            
            var boxes_ids = boxes.Select((box, id) => (box, id));
            double targetShapesPerCell = 1.5; //tweak this variable to find something optimal, it will end up being hardcoded
            var cellLength = FindACellLength(boxesA, targetShapesPerCell);
            var spaceStore = BuildSpaceStore(boxes_ids, cellLength);
            var pairSet = spaceStore.GetPairs();

            return pairSet
                .Where(pair => BoxesClash(boxesA[pair.Int1], boxesA[pair.Int2], tolerance))
                .Select(p => (p.Int1,p.Int2))
                .ToArray();
        }

        internal static (int firstIndex,int secondIndex)[] GetIntersectingPairs(IEnumerable<Box> set1, IEnumerable<Box> set2, double tolerance)
        {
            var boxes_ids1 = set1.Select((b, i) => (b, i));
            var boxes_ids2 = set2.Select((b, i) => (b, i));
            var boxDict1 = boxes_ids1.ToDictionary(t => t.i, t => t.b);
            var boxDict2 = boxes_ids2.ToDictionary(t => t.i, t => t.b);


            int targetShapesPerCell = 3; //tweak this variable to find something optimal, it will end up being hardcoded
            var cellLength = FindACellLength(set1.Concat(set2), targetShapesPerCell);

            var spaceStore = BuildDoubleSpaceStore(boxes_ids1,boxes_ids2, cellLength);
            var pairSet = spaceStore.GetPairs();

            return pairSet.Where(pair => BoxesClash(boxDict1[pair.firstIndex], boxDict2[pair.secondIndex], tolerance)).ToArray();
        }

        private static SpaceStore BuildSpaceStore(IEnumerable<(Box box, int id)> boxes_ids, double cellLength)
        {
            
            var cells_ids = GetCells(boxes_ids, cellLength);
            int numberOfBoxes = cells_ids.Count();
            var spaceStore = new SpaceStore(2*numberOfBoxes);
            foreach((var cells, var id) in cells_ids)
            {
                foreach(var cell in cells)
                {
                    spaceStore.Add(cell, id);
                }
            }
            //cells_ids.Select(t => t.cells.TapList(cell => spaceStore.Add(cell, t.id)));
            return spaceStore;
        }

        private static DoubleSpaceStore BuildDoubleSpaceStore(IEnumerable<(Box box, int id)> boxes_ids1, IEnumerable<(Box box, int id)> boxes_ids2, double cellLength)
        {
            var cells_ids1 = GetCells(boxes_ids1, cellLength);
            var cells_ids2 = GetCells(boxes_ids2, cellLength);
            int numberOfBoxes = cells_ids1.Count()+cells_ids2.Count();
            var spaceStore = new DoubleSpaceStore(2 * numberOfBoxes);
            cells_ids1.Select(t => t.cells.TapList(cell => spaceStore.AddToFirstSet(cell, t.id)));
            cells_ids2.Select(t => t.cells.TapList(cell => spaceStore.AddToSecondSet(cell, t.id)));
            return spaceStore;
        }

        private static (SpaceCell[] cells, int id)[] GetCells(IEnumerable<(Box box, int id)> boxes_ids, double cellLength)
        {
            return boxes_ids.Select(t => (SpaceCellsForBox(t.box, cellLength), t.id)).ToArray();
        }

        private static SpaceCell[] SpaceCellsForBox(Box bBox, double cellLength)
        {
            var maxId = new SpaceCell(bBox.Max, cellLength);
            var minId = new SpaceCell(bBox.Min, cellLength);
            int nX = maxId.X - minId.X + 1;
            int nY = maxId.Y - minId.Y + 1;
            int nZ = maxId.Z - minId.Z + 1;
            return Range3d(nX, nY, nZ)
                .Select(t => new SpaceCell(minId.X + t.I, minId.Y + t.J, minId.Z + t.K, cellLength))
                .ToArray();
        }

        

        public static bool BoxesClash(Box boxA, Box boxB, double tol)
        {
            Vector3d tolPt = new Vector3d(tol, tol, tol);

            var xMinA = PointToArray(boxA.Min - tolPt);
            var xMaxA = PointToArray(boxA.Max + tolPt);
            var xMinB = PointToArray(boxB.Min - tolPt);
            var xMaxB = PointToArray(boxB.Max + tolPt);

            var test = true;
            for (int i = 0; i < 3; i++)
            {

                bool temp =
                    (xMinB[i] <= xMinA[i] && xMinA[i] <= xMaxB[i])
                    ||
                    (xMinA[i] <= xMinB[i] && xMinB[i] <= xMaxA[i]);
                test = test && temp;
            }
            return test;
        }

        private static double[] PointToArray(Point3d p)
        {
            var arr = new double[3];
            arr[0] = p.X;
            arr[1] = p.Y;
            arr[2] = p.Z;
            return arr;
        }

        private static double FindACellLength(IEnumerable<Box> boxes, double targetShapesPerCell)
        {
            double averageSideLength = boxes.Select(box => box.Diagonal)
                                            .Average();
            return averageSideLength * targetShapesPerCell;
        }
    }
}
