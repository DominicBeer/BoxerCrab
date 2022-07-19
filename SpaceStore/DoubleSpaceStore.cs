using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceIndex
{
    internal class DoubleSpaceStore
    {
        private Dictionary<SpaceCell, (HashSet<int> FirstSet, HashSet<int> SecondSet)> Store;

        public DoubleSpaceStore()
        {
            Store = new Dictionary<SpaceCell, (HashSet<int>, HashSet<int>)>();
        }

        public DoubleSpaceStore(int size)
        {
            Store = new Dictionary<SpaceCell, (HashSet<int>, HashSet<int>)>(size);
        }

        public void AddToFirstSet(SpaceCell cell, int id)
        {
            if (Store.ContainsKey(cell))
            {
                Store[cell].FirstSet.Add(id);
            }
            else
            {
                var set1 = new HashSet<int> { id };
                var set2 = new HashSet<int>();
                Store[cell] = (set1, set2);
            }
        }

        public void AddToSecondSet(SpaceCell cell, int id)
        {
            if (Store.ContainsKey(cell))
            {
                Store[cell].SecondSet.Add(id);
            }
            else
            {
                var set2 = new HashSet<int> { id };
                var set1 = new HashSet<int>();
                Store[cell] = (set1, set2);
            }
        }

        public HashSet<(int firstIndex, int secondIndex)> GetPairs()
        {
            var pairSet = new HashSet<(int, int)>();
            foreach (var kvp in Store)
            {
                var ids = kvp.Value;
                var set1 = kvp.Value.FirstSet;
                var set2 = kvp.Value.SecondSet;
                foreach (var id1 in set1)
                {
                    foreach (var id2 in set2)
                    {
                        pairSet.Add((id1, id2));
                    }
                }
            }
            return pairSet;
        }
    }
}
