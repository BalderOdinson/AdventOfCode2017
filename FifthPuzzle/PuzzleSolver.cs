using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifthPuzzle
{
    public class PuzzleSolver
    {
        private readonly List<int> _listOfJumpOffests;

        public PuzzleSolver(IEnumerable<int> listOfJumpOffests)
        {
            _listOfJumpOffests = listOfJumpOffests.ToList();
        }

        public int SolveFirst()
        {
            var tempList = new List<int>(_listOfJumpOffests);
            var numberOfJumps = 0;
            var currentField = 0;
            try
            {
                while (true)
                {
                    var newCurrentField = currentField;
                    newCurrentField += tempList[currentField];
                    tempList[currentField]++;
                    currentField = newCurrentField;
                    numberOfJumps++;
                }

            }
            catch (ArgumentOutOfRangeException e)
            {
                return numberOfJumps;
            }
        }

        public int SolveSecond()
        {
            var tempList = new List<int>(_listOfJumpOffests);
            var numberOfJumps = 0;
            var currentField = 0;
            try
            {
                while (true)
                {
                    var newCurrentField = currentField;
                    newCurrentField += tempList[currentField];
                    if (tempList[currentField] > 2)
                        tempList[currentField]--;
                    else
                        tempList[currentField]++;
                    currentField = newCurrentField;
                    numberOfJumps++;
                }

            }
            catch (ArgumentOutOfRangeException e)
            {
                return numberOfJumps;
            }
        }
    }
}
