using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentySecondPuzzle
{
    public class PuzzleSolver
    {
        private readonly IDictionary<Point, bool> _gridDictionary;
        private readonly int _centerX;
        private readonly int _centerY;
        private const int IterationFirst = 10000;
        private const int IterationSecond = 10000000;

        public PuzzleSolver(IEnumerable<string> input)
        {
            _gridDictionary = new Dictionary<Point, bool>();
            var y = 0;
            foreach (var line in input)
            {
                var x = 0;
                foreach (var character in line)
                {
                    if (character != '#' && character != '.') continue;
                    _gridDictionary.Add(new Point(x, line.Length - 1 - y), character == '#');
                    x++;
                }

                if (_centerX == 0)
                    _centerX = x / 2;
                y++;
            }

            _centerY = y / 2;
        }

        public int SolveFirst()
        {
            var currentPoint = new Point(_centerX, _centerY);
            var direction = new Point(0,1);
            var grid = _gridDictionary.ToDictionary(k => k.Key, v => v.Value);
            var burstinfected = 0;
            for (int i = 0; i < IterationFirst; i++)
            {
                if (!grid.ContainsKey(currentPoint))
                {
                    grid.Add(currentPoint, false);
                }
                direction = grid[currentPoint] ? new Point(direction.Y, direction.X * -1) : new Point(direction.Y * -1, direction.X);
                grid[currentPoint] = !grid[currentPoint];
                burstinfected = grid[currentPoint] ? ++burstinfected : burstinfected;
                currentPoint = currentPoint + direction;
            }

            return burstinfected;
        }

        public int SolveSecond()
        {
            var currentPoint = new Point(_centerX, _centerY);
            var direction = new Point(0, 1);
            var grid = _gridDictionary.ToDictionary(k => k.Key, v => v.Value ? InfectionState.Infected : InfectionState.Clean);
            var burstinfected = 0;
            for (int i = 0; i < IterationSecond; i++)
            {
                if (!grid.ContainsKey(currentPoint))
                {
                    grid.Add(currentPoint, InfectionState.Clean);
                }

                switch (grid[currentPoint])
                {
                    case InfectionState.Clean:
                        direction = new Point(direction.Y * -1, direction.X);
                        grid[currentPoint] = InfectionState.Weakened;
                        break;
                    case InfectionState.Weakened:
                        grid[currentPoint] = InfectionState.Infected;
                        break;
                    case InfectionState.Infected:
                        direction = new Point(direction.Y, direction.X * -1);
                        grid[currentPoint] = InfectionState.Flagged;
                        break;
                    case InfectionState.Flagged:
                        direction = direction * new Point(-1, -1);
                        grid[currentPoint] = InfectionState.Clean;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                burstinfected = grid[currentPoint] == InfectionState.Infected ? ++burstinfected : burstinfected;
                currentPoint = currentPoint + direction;
            }

            return burstinfected;
        }
    }
}
