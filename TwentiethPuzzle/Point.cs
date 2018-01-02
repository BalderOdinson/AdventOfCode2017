using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentiethPuzzle
{
    public struct Point
    {
        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public static int ManhattanDistance(Point first, Point second)
        {
            return Math.Abs(first.X - second.X) +
                        Math.Abs(first.Y - second.Y) +
                            Math.Abs(first.Z - second.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is Point point))
                return false;
            return point.X.Equals(X) &&
                   point.Y.Equals(Y) &&
                   point.Z.Equals(Z);
        }
    }
}
