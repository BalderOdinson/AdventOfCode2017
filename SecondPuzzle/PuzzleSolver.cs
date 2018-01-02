using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecondPuzzle
{
    public class PuzzleSolver
    {

        private readonly List<List<int>> _input = new List<List<int>>();

        public PuzzleSolver(IEnumerable<string> input)
        {
            foreach (var line in input)
            {
                _input.Add(Regex.Split(line, @"[\s\t]+").Select(c => Int32.Parse(c.ToString())).OrderByDescending(i => i).ToList());
            }
        }

        public int SolveFirst()
        {
            var sum = 0;
            foreach (var row in _input)
            {
                sum += row.First() - row.Last();
            }
            return sum;
        }

        public int SolveSecond()
        {
            var sum = 0;
            foreach (var row in _input)
            {
                sum += SumOfDivisible(row);
            }
            return sum;
        }

        private static int SumOfDivisible(IReadOnlyList<int> row)
        {
            for (int i = row.Count - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (row[j] % row[i] == 0)
                        return row[j] / row[i];
                }
            }
            return 0;
        }
    }
}
