using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FourthPuzzle
{
    public class PuzzleSolver
    {
        private readonly IEnumerable<string> _passphrases;

        public PuzzleSolver(IEnumerable<string> passphrases)
        {
            _passphrases = passphrases;
        }

        public int SolveFirst()
        {
            var numOfValid = 0;
            foreach (var passphrase in _passphrases)
            {
                var phrase = Regex.Split(passphrase, " ");
                if (phrase.Length == phrase.Distinct().Count())
                    numOfValid++;
            }

            return numOfValid;
        }

        public int SolveSecond()
        {
            var numOfValid = 0;
            foreach (var passphrase in _passphrases)
            {
                var phrase = Regex.Split(passphrase, " ");
                if (phrase.Length == phrase.Distinct(new AnagramEqualityComparer()).Count())
                    numOfValid++;
            }

            return numOfValid;
        }
    }
}
