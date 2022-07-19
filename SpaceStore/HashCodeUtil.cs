using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SpaceIndex
{
    internal static class HashCodeUtil
    {
        public static int Hash(params object[] values)
            => CustomHash(1009, 9176, values.Select(v => v.GetHashCode()).ToArray());

        // From answer https://stackoverflow.com/a/34006336/25338
        public static int CustomHash(int seed, int factor, params int[] vals)
        {
            unchecked
            {
                int hash = seed;
                foreach (int i in vals)
                {
                    hash = (hash * factor) + i;
                }
                return hash;
            }
            
        }
    }
}
