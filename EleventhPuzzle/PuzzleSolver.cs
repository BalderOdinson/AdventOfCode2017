using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EleventhPuzzle
{
    enum Direction
    {
        North = 'n',
        NorthEast = 'n' + 'e',
        SouthEast = 's' + 'e',
        South = 's',
        SouthWest = 's' + 'w',
        NorthWest = 'n' + 'w',
    }

    public class PuzzleSolver
    {
        private readonly IEnumerable<Direction> _input;

        private int _row = 0;
        private int _column = 0;

        public PuzzleSolver(IEnumerable<string> input)
        {
            _input = input.Select(s => (Direction)s.Sum(c => c));
        }

        public int SolveFirst()
        {
            foreach (var move in _input)
            {
                Move(move);
            }

            return FindShorthestPath(0, 0, _column, _row).Count();
        }

        public int SolveSecond()
        {
            _row = 0;
            _column = 0;
            int maxPath = 0;
            foreach (var move in _input)
            {
                Move(move);
                var path = FindShorthestPath(0, 0, _column, _row).Count();
                maxPath = path > maxPath ? path : maxPath;
            }

            return maxPath;

        }

        private void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    _row += 2;
                    break;
                case Direction.NorthEast:
                    _row++;
                    _column++;
                    break;
                case Direction.SouthEast:
                    _row--;
                    _column++;
                    break;
                case Direction.South:
                    _row -= 2;
                    break;
                case Direction.SouthWest:
                    _row--;
                    _column--;
                    break;
                case Direction.NorthWest:
                    _row++;
                    _column--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private static IEnumerable<Direction> FindShorthestPath(int startX, int startY, int endX, int endY)
        {
            endX -= startX;
            endY -= startY;
            startX = 0;
            startY = 0;
            while (startX != endX || startY != endY)
            {
                if (startX == endX)
                {
                    if (startY < endY)
                    {
                        startY += 2;
                        yield return Direction.North;
                    }
                    else
                    {
                        startY -= 2;
                        yield return Direction.South;
                    }
                }
                else if (startY == endY)
                {
                    if (startX < endX)
                    {
                        startX++;
                        startY++;
                        yield return Direction.NorthEast;
                    }
                    else
                    {
                        startX--;
                        startY++;
                        yield return Direction.NorthWest;
                    }
                }
                else if (startX > endX && startY > endY)
                {
                    startX--;
                    startY--;
                    yield return Direction.SouthWest;
                }
                else if (startX < endX && startY < endY)
                {
                    startX++;
                    startY++;
                    yield return Direction.NorthEast;
                }
                else if (startX < endX && startY > endY)
                {
                    startX++;
                    startY--;
                    yield return Direction.SouthEast;
                }
                else if (startX > endX && startY < endY)
                {
                    startX--;
                    startY++;
                    yield return Direction.NorthWest;
                }
            }
        }
    }
}
