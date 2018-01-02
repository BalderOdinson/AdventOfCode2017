using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenthPuzzle
{
    public class PuzzleSolver
    {
        private readonly IEnumerable<int> _lengths;
        private readonly IEnumerable<char> _charLengths;
        private readonly IEnumerable<int> _list;
        private readonly IEnumerable<char> _charList;

        public PuzzleSolver(string lengths)
        {
            _lengths = lengths.Split(',').Select(int.Parse);
            var charLengthsTemp = lengths.Trim().ToCharArray().ToList();
            charLengthsTemp.AddRange(new[] { (char)17, (char)31, (char)73, (char)47, (char)23 });
            _charLengths = charLengthsTemp;
            var temp = new List<int>();
            var charTemp = new List<char>();
            for (int i = 0; i < 256; i++)
            {
                temp.Add(i);
                charTemp.Add((char)i);
            }

            _charList = charTemp;
            _list = temp;
        }

        public int SolveFirst()
        {
            var list = _list.ToList();
            var currentPosition = 0;
            var skipSize = 0;
            foreach (var length in _lengths)
            {
                for (int i = length - 1; i >= (int)Math.Round(length / 2.0); i--)
                {
                    var offset = (i + currentPosition) % list.Count;
                    var replaceOffset = (length - 1 - i + currentPosition) % list.Count;
                    var helper = list[offset];
                    list[offset] = list[replaceOffset];
                    list[replaceOffset] = helper;
                }
                currentPosition = (currentPosition + skipSize + length) % list.Count;
                skipSize++;
            }

            return list[0] * list[1];
        }

        public string SolveSecond()
        {
            var list = _charList.ToList();
            var currentPosition = 0;
            var skipSize = 0;
            for (int j = 0; j < 64; j++)
            {
                foreach (var length in _charLengths)
                {
                    for (int i = length - 1; i >= (int)Math.Round(length / 2.0); i--)
                    {
                        var offset = (i + currentPosition) % list.Count;
                        var replaceOffset = (length - 1 - i + currentPosition) % list.Count;
                        var helper = list[offset];
                        list[offset] = list[replaceOffset];
                        list[replaceOffset] = helper;
                    }
                    currentPosition = (currentPosition + skipSize + length) % list.Count;
                    skipSize++;
                }
            }

            var hash = new List<char>();

            for (int i = 0; i < 256; i+=16)
            {
                var temp = list.Skip(i).Take(16);
                var result = (char) 0;
                foreach (var value in temp)
                {
                    result ^= value;
                }
                hash.Add(result);
            }

            var sb = new StringBuilder();

            foreach (var value in hash)
            {
                sb.Append(($"{Convert.ToInt32(value):x}").PadLeft(2,'0'));
            }

            return sb.ToString();
        }
    }
}
