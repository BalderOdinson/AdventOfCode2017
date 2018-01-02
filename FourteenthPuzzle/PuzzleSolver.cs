using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourteenthPuzzle
{
    struct Square
    {
        public Square(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; }
        public int Column { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;
            return ((Square)obj).Column.Equals(Column)
                && ((Square)obj).Row.Equals(Row);
        }

        public override int GetHashCode()
        {
            return (Row * (Row % 27) + Column * (Column % 29)).GetHashCode();
        }
    }

    public class PuzzleSolver
    {
        private readonly string _stringKey;

        public PuzzleSolver(string stringKey)
        {
            this._stringKey = stringKey;
        }

        public int SolveFirst()
        {
            var numOfUsedSpaces = 0;
            for (int i = 0; i < 128; i++)
            {
                var tempHash = $"{_stringKey}-{i}";
                var hash = new TenthPuzzle.PuzzleSolver(tempHash).SolveSecond();
                var binarystring = string.Join(string.Empty,
                    hash.Select(
                        c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                    )
                );
                numOfUsedSpaces += binarystring.Count(c => c == '1');
            }

            return numOfUsedSpaces;
        }

        public int SolveSecond()
        {
            var hashs = new List<string>();
            var squareSet = new HashSet<Square>();
            for (int i = 0; i < 128; i++)
            {
                var tempHash = $"{_stringKey}-{i}";
                var hash = new TenthPuzzle.PuzzleSolver(tempHash).SolveSecond();
                var binarystring = string.Join(string.Empty,
                    hash.Select(
                        c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                    )
                );
                hashs.Add(binarystring);
            }

            var numOfGroups = 0;

            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    if (IsInGroup(squareSet, new Square(j, i), hashs))
                        numOfGroups++;
                }
            }

            return numOfGroups;
        }

        private bool IsInGroup(ISet<Square> squares, Square square, IEnumerable<string> hashs)
        {
            var hashList = hashs.ToList();
            if (hashList[square.Column][square.Row] == '0' || !squares.Add(square))
                return false;
            foreach (var adjacentSquare in GetUsedAdjacentSquares(square, hashList))
            {
                IsInGroup(squares, adjacentSquare, hashList);
            }

            return true;
        }

        private IEnumerable<Square> GetUsedAdjacentSquares(Square square, IEnumerable<string> hashs)
        {
            var listHashes = hashs.ToList();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j != 0)
                    {
                        if(j == 1 && square.Row == 127
                           || j == -1 && square.Row == 0)
                            continue;
                        if (listHashes[square.Column][square.Row + j] == '1')
                            yield return new Square(square.Row + j, square.Column);
                    }

                    if (j == 0 && i != 0)
                    {
                        if (i == 1 && square.Column == 127
                            || i == -1 && square.Column == 0)
                            continue;
                        if (listHashes[square.Column + i][square.Row] == '1')
                            yield return new Square(square.Row, square.Column + i);
                    }
                }
            }
        }
    }
}
