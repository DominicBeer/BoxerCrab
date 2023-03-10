using System;

namespace SpaceIndex
{
    internal struct SpaceChunk
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        private static int UnitIndex(double x, double cellLength)
        {
            return (int)Math.Floor(x / cellLength);
        }

        public SpaceChunk(double x, double y, double z, double cellLength)
        {
            X = UnitIndex(x, cellLength);
            Y = UnitIndex(y, cellLength);
            Z = UnitIndex(z, cellLength);
        }

        public SpaceChunk(int unitX, int unitY, int unitZ)
        {
            X = unitX;
            Y = unitY;
            Z = unitZ;
        }

        public SpaceChunk(Point3d point, double cellLength)
        {
            X = UnitIndex(point.X, cellLength);
            Y = UnitIndex(point.Y, cellLength);
            Z = UnitIndex(point.Z, cellLength);

        }

        public override bool Equals(object obj) =>
            obj is SpaceChunk spaceID &&
            spaceID.X == X &&
            spaceID.Y == Y &&
            spaceID.Z == Z;

        public override int GetHashCode() => HashCodeUtil.Hash(X, Y, Z);

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }

        public SpaceChunk Parent => new SpaceChunk(
            X >> 1,
            Y >> 1,
            Z >> 1);

        public SpaceChunk[] Children()
        {
            var c = new SpaceChunk[8];
            int x = 2 * X;
            int y = 2 * Y;
            int z = 2 * Z;
            int x1 = x + 1;
            int y1 = y + 1;
            int z1 = z + 1;
            c[0] = new SpaceChunk(x, y, z);
            c[1] = new SpaceChunk(x, y, z1);
            c[2] = new SpaceChunk(x, y1, z);
            c[3] = new SpaceChunk(x, y1, z1);
            c[4] = new SpaceChunk(x1, y, z);
            c[5] = new SpaceChunk(x1, y, z1);
            c[6] = new SpaceChunk(x1, y1, z);
            c[7] = new SpaceChunk(x1, y1, z1);
            return c;
        }
    }
}
