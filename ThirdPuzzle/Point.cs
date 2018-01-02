using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPuzzle
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Point point))
                return false;
            return point.X.Equals(X) && point.Y.Equals(Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public static Point operator *(Point first, Point second)
        {
            return new Point(first.X * second.X, first.Y * second.Y);
        }

        public static Point operator +(Point first, Point second)
        {
            return new Point(first.X + second.X, first.Y + second.Y);
        }

        public static Point operator -(Point first, Point second)
        {
            return new Point(first.X - second.X, first.Y - second.Y);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
