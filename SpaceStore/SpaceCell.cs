using System;

namespace SpaceIndex
{
    internal struct SpaceCell
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public double CellLength { get; }

        private static int UnitIndex(double x, double cellLength)
        {
            return (int)Math.Round(x / cellLength);
        }

        public SpaceCell(double x, double y, double z, double cellLength)
        {
            X = UnitIndex(x, cellLength);
            Y = UnitIndex(y, cellLength);
            Z = UnitIndex(z, cellLength);
            CellLength = cellLength;
        }

        public SpaceCell(int unitX, int unitY, int unitZ, double cellLength)
        {
            X = unitX;
            Y = unitY;
            Z = unitZ;
            CellLength = cellLength;
        }

        public SpaceCell(Point3d point, double cellLength)
        {
            X = UnitIndex(point.X, cellLength);
            Y = UnitIndex(point.Y, cellLength);
            Z = UnitIndex(point.Z, cellLength);
            CellLength = cellLength;
        }

        public override bool Equals(object obj) =>
            obj is SpaceCell spaceID && 
            spaceID.X == X && 
            spaceID.Y == Y && 
            spaceID.Z == Z && 
            spaceID.CellLength == CellLength;

        public override int GetHashCode() => HashCodeUtil.Hash(X, Y, Z, CellLength);

    }
}
