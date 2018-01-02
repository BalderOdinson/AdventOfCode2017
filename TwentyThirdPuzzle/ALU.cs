using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public class ALU
    {
        public void Add(long a, long b)
        {
            AluResult = a + b;
        }

        public void Sub(long a, long b)
        {
            AluResult = a - b;
        }

        public void Multiply(long a, long b)
        {
            AluResult = a * b;
        }

        public void Modulo(long a, long b)
        {
            AluResult = a % b;
        }

        public void Compare(long a, long b)
        {
            AluResult = a.CompareTo(b);
        }

        public long AluResult { get; private set; }
    }

}
