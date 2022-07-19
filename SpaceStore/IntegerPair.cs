using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceIndex
{
    internal struct IntegerPair
    {
        public int Int1 { get; }
        public int Int2 { get; }

        public IntegerPair(int int1, int int2)
        {
            if (int1 < int2)
            {
                Int1 = int1;
                Int2 = int2;
            }
            else
            {
                Int1 = int2;
                Int2 = int1;
            }
        }

        public IntegerPair((int int1, int int2) t) : this(t.int1, t.int2)
        {
        }

        public override bool Equals(object obj) => 
            obj is IntegerPair other && 
            Int1 == other.Int1 && 
            Int2 == other.Int2;

        public override int GetHashCode() => HashCodeUtil.Hash(Int1, Int2);
        
    }
}
