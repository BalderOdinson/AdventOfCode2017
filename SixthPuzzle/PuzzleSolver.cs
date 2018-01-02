using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace SixthPuzzle
{
    public class PuzzleSolver
    {
        private readonly HashSet<List<int>> _listOfmemoryloops;

        public PuzzleSolver(IEnumerable<int> memeoryBlocks)
        {
            var memoryBlocks = memeoryBlocks.ToList();
            _listOfmemoryloops = new HashSet<List<int>>(new[] { memoryBlocks }, new ListEqualityComparer());
        }

        public int SolveFirst()
        {
            var numberOfLoops = 0;
            var hasNext = true;
            while (hasNext)
            {
                numberOfLoops++;
                var newLoopList = new List<int>(_listOfmemoryloops.Last());
                var largestBlockValue = newLoopList.Max();
                var largestBlockField = newLoopList.IndexOf(largestBlockValue);
                newLoopList[largestBlockField] = 0;
                largestBlockField++;
                largestBlockField %= newLoopList.Count;
                while (largestBlockValue != 0)
                {
                    newLoopList[largestBlockField]++;
                    largestBlockValue--;
                    largestBlockField++;
                    largestBlockField %= newLoopList.Count;
                }

                hasNext = _listOfmemoryloops.Add(newLoopList);
            }

            return numberOfLoops;
        }

        public int SolveSecond()
        {
            var numberOfLoops = 0;
            var eqComp = new ListEqualityComparer();
            var nextCycle = new List<int>(_listOfmemoryloops.Last());
            while (true)
            {
                numberOfLoops++;
                var largestBlockValue = nextCycle.Max();
                var largestBlockField = nextCycle.IndexOf(largestBlockValue);
                nextCycle[largestBlockField] = 0;
                largestBlockField++;
                largestBlockField %= nextCycle.Count;
                while (largestBlockValue != 0)
                {
                    nextCycle[largestBlockField]++;
                    largestBlockValue--;
                    largestBlockField++;
                    largestBlockField %= nextCycle.Count;
                }

                if (eqComp.Equals(_listOfmemoryloops.Last(), nextCycle))
                    break;
            }

            return numberOfLoops;
        }
    }
}
