using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FiftheenthPuzzle
{
    public class PuzzleSolver
    {
        private readonly long _generatorAStart;
        private readonly long _generatorBStart;
        private const int GeneratorAFactor = 16807;
        private const int GeneratorBFactor = 48271;
        private const int DividingFactor = 2147483647;
        private const int NumberOfIterationsFirst = 40000000;
        private const int NumberOfIterationsSecond = 5000000;
        private const int GeneratorAMultiplicator = 4;
        private const int GeneratorBMultiplicator = 8;

        public PuzzleSolver(IReadOnlyList<string> input)
        {
            _generatorAStart = Convert.ToInt64(Regex.Match(input[0], "(\\d)+").Value);
            _generatorBStart = Convert.ToInt64(Regex.Match(input[1], "(\\d)+").Value);
        }

        public int SolveFirst()
        {
            var generatorAValue = _generatorAStart;
            var generatorBValue = _generatorBStart;
            var numOfMatch = 0;
            for (int i = 0; i < NumberOfIterationsFirst; i++)
            {
                generatorAValue = generatorAValue * GeneratorAFactor % DividingFactor;
                generatorBValue = generatorBValue * GeneratorBFactor % DividingFactor;
                var binaryALowest16 = Convert.ToString(generatorAValue, 2).PadLeft(16, '0');
                binaryALowest16 = binaryALowest16.Substring(binaryALowest16.Length - 16, 16);
                var binaryBLowest16 = Convert.ToString(generatorBValue, 2).PadLeft(16, '0');
                binaryBLowest16 = binaryBLowest16.Substring(binaryBLowest16.Length - 16, 16);
                if (binaryALowest16.Equals(binaryBLowest16))
                    numOfMatch++;
            }

            return numOfMatch;
        }

        public int SolveSecond()
        {
            var generatorAValue = _generatorAStart;
            var generatorBValue = _generatorBStart;
            var numOfMatch = 0;
            var i = 0;
            var generatorAQue = new Queue<string>();
            var generatorBQue = new Queue<string>();
            while (i != NumberOfIterationsSecond)
            {
                generatorAValue = generatorAValue * GeneratorAFactor % DividingFactor;
                generatorBValue = generatorBValue * GeneratorBFactor % DividingFactor;
                if (generatorAValue % GeneratorAMultiplicator == 0)
                {
                    var binaryALowest16 = Convert.ToString(generatorAValue, 2).PadLeft(16, '0');
                    binaryALowest16 = binaryALowest16.Substring(binaryALowest16.Length - 16, 16);
                    generatorAQue.Enqueue(binaryALowest16);
                }

                if (generatorBValue % GeneratorBMultiplicator == 0)
                {
                    var binaryBLowest16 = Convert.ToString(generatorBValue, 2).PadLeft(16, '0');
                    binaryBLowest16 = binaryBLowest16.Substring(binaryBLowest16.Length - 16, 16);
                    generatorBQue.Enqueue(binaryBLowest16);
                }

                if (generatorAQue.Count > 0 && generatorBQue.Count > 0)
                {
                    i++;
                    if (generatorAQue.Dequeue().Equals(generatorBQue.Dequeue()))
                        numOfMatch++;
                }
            }

            return numOfMatch;
        }
    }
}
