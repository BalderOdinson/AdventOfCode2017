using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NinthPuzzle
{
    public class PuzzleSolver
    {
        private readonly List<string> _garbage;
        private readonly string _inputWithoutGarbageAndNegators;

        public PuzzleSolver(string input)
        {
            input = Regex.Replace(input, "(!(.{1}))+?", "");
            _garbage = Regex.Matches(input, "(\\<(.*?)\\>)+").OfType<Match>().Select(m => m.Value).ToList();
            _inputWithoutGarbageAndNegators = Regex.Replace(input, "((?<!\\!)\\<(.*?)(?<!\\!)\\>)+", "");
        }

        public int SolveFirst()
        {
            var stack = new Stack<char>();
            var sum = 0;
            foreach (var character in _inputWithoutGarbageAndNegators)
            {
                if(character == '{')
                    stack.Push('{');
                if (character == '}')
                {
                    sum += stack.Count;
                    stack.Pop();
                }
            }

            return sum;
        }

        public int SolveSecond()
        {
            return _garbage.Sum(s => s.Length) - _garbage.Count * 2;
        }
    }
}
