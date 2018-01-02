using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourthPuzzle
{
    public class AnagramEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            if (x == null)
                return y == null;
            return new string(x.OrderBy(c => c).ToArray()).Equals(new string(y?.OrderBy(c => c).ToArray()));
        }

        public int GetHashCode(string obj)
        {
            return new string(obj.OrderBy(c => c).ToArray()).GetHashCode();
        }
    }
}
