using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceIndex
{
    internal class SpaceStore
    {
        private Dictionary<SpaceCell, HashSet<int>> Store;

        public SpaceStore()
        {
            Store = new Dictionary<SpaceCell, HashSet<int>>();
        }

        public SpaceStore(int size)
        {
            Store = new Dictionary<SpaceCell, HashSet<int>>(size);
        }

        public void Add(SpaceCell cell, int id)
        {
            if (Store.ContainsKey(cell))
            {
                Store[cell].Add(id);
            }
            else
            {
                var set = new HashSet<int> { id };
                Store[cell] = set;
            }
        }

        public HashSet<IntegerPair> GetPairs()
        {
            var pairSet = new HashSet<IntegerPair>();
            foreach (var kvp in Store)
            {
                var ids = kvp.Value;
                var pairs = ids.LowerTrianglePairs().Select(pair => new IntegerPair(pair));
                foreach (var pair in pairs)
                {
                    pairSet.Add(pair);
                }
            }
            return pairSet;
        }
    }

    
}
