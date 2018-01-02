using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPuzzle
{
    public class PuzzleSolver
    {
        private readonly long _input;

        public PuzzleSolver(string input)
        {
            _input = long.Parse(input);
        }

        public long SolveFirst()
        {
            var sideSize = (long)Math.Ceiling(Math.Sqrt(_input));
            var lastNumberInSquare = sideSize * sideSize;
            var halfOfSize = sideSize / 2;
            for (int i = 0; i < 4; i++)
            {
                var center = lastNumberInSquare - halfOfSize - i * (sideSize - 1);
                var distanceFromCenter = Math.Abs(center - _input);
                if (distanceFromCenter < sideSize)
                    return distanceFromCenter + halfOfSize;
            }

            return 0;
        }

        public long SolveSecond()
        {
            var memory = new Dictionary<Point, long> { { new Point(0, 0), 1 } };
            var pointer = new Point(1, 0);
            var square = 1;
            var quadrant = 0;
            while (true)
            {
                var newValue = 0L;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;
                        var point = new Point(i, j) + pointer;
                        if (memory.ContainsKey(point))
                            newValue += memory[point];
                    }
                }

                if (newValue > _input)
                {
                    return newValue;
                }

                memory.Add(pointer, newValue);

                if (quadrant % 2 == 0)
                {
                    if (Math.Abs(pointer.Y) < square)
                        pointer = pointer + new Point(0, 1 - quadrant);
                    else
                    {
                        pointer = pointer + new Point(quadrant - 1, 0);
                        quadrant++;
                    }
                }
                else
                {
                    if (Math.Abs(pointer.X) < square)
                        pointer = pointer + new Point(quadrant - 2, 0);
                    else
                    {
                        if (quadrant == 1)
                        {
                            pointer = pointer + new Point(0, -1);
                            quadrant++;
                        }
                        else
                        {
                            quadrant = 0;
                            square++;
                            pointer = pointer + new Point(1, 0);
                        }
                    }
                }

                
            }
        }
    }
}
