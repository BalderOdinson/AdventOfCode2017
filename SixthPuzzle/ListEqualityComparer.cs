using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixthPuzzle
{
    public class ListEqualityComparer : IEqualityComparer<List<int>>
    {
        public bool Equals(List<int> x, List<int> y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return true;
                }

                return false;
            }
            return x.SequenceEqual(y);
        }

        public int GetHashCode(List<int> obj)
        {
            var sb = new StringBuilder();
            foreach (var element in obj)
            {
                sb.Append(element.ToString());
            }
            return sb.ToString().GetHashCode();
        }
    }
}
