using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPuzzle
{
    public class PuzzleSolver
    {
        private readonly List<int> _digits;

        public PuzzleSolver(IEnumerable<int> digits)
        {
            _digits = digits.ToList();
        }

        public int SolveFirstHalf()
        {
            var previousDigit = _digits[_digits.Count - 1];
            var sum = 0;
            foreach (var digit in _digits)
            {
                if (digit == previousDigit)
                    sum += digit;
                previousDigit = digit;
            }
            return sum;
        }

        public int SolveSecondHalf()
        {
            var sum = 0;
            var count = _digits.Count;
            var halfway = count / 2;
            for (int i = 0; i < count; i++)
            {
                if (_digits[(halfway + i) % count] == _digits[i])
                    sum += _digits[i];
            }
            return sum;
        }

    }
}
