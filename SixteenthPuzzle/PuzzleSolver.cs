using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SixteenthPuzzle
{

    abstract class DanceMove
    {
        public abstract string Dance(string positions);
    }

    class SpinDanceMove : DanceMove
    {
        public SpinDanceMove(int numberOfPrograms)
        {
            NumberOfPrograms = numberOfPrograms;
        }

        public int NumberOfPrograms { get; }
        public override string Dance(string positions)
        {
            return string.Concat(positions.Substring(positions.Length - NumberOfPrograms, NumberOfPrograms),
                positions.Substring(0, positions.Length - NumberOfPrograms));
        }
    }

    class ExchangeDanceMove : DanceMove
    {
        public ExchangeDanceMove(int positionOfFirst, int positionOfSecond)
        {
            PositionOfFirst = positionOfFirst;
            PositionOfSecond = positionOfSecond;
        }

        public int PositionOfFirst { get; }
        public int PositionOfSecond { get; }
        public override string Dance(string positions)
        {
            var tempList = positions.ToCharArray();
            tempList[PositionOfFirst] = tempList[PositionOfSecond];
            tempList[PositionOfSecond] = positions[PositionOfFirst];
            return string.Join(String.Empty, tempList);
        }
    }

    class PartnerDanceMove : DanceMove
    {
        public PartnerDanceMove(char firstPartner, char secondPartner)
        {
            FirstPartner = firstPartner;
            SecondPartner = secondPartner;
        }

        public char FirstPartner { get; }
        public char SecondPartner { get; }
        public override string Dance(string positions)
        {
            var midResult = Regex.Replace(positions, FirstPartner.ToString(), "X");
            midResult = Regex.Replace(midResult, SecondPartner.ToString(), FirstPartner.ToString());
            return Regex.Replace(midResult, "X", SecondPartner.ToString());
        }
    }

    public class PuzzleSolver
    {
        private readonly IEnumerable<DanceMove> _danceMoves;
        private const string InitialPositions = "abcdefghijklmnop";
        private const int NumOfIterations = 1000000000;

        public PuzzleSolver(IEnumerable<string> danceMoves)
        {
            
            var danceMovesList = new List<DanceMove>();
            foreach (var danceMove in danceMoves)
            {
                if (danceMove[0] == 's')
                {
                    danceMovesList.Add(new SpinDanceMove(
                        Convert.ToInt32(Regex.Match(danceMove, "\\d+").Value)));
                }
                if (danceMove[0] == 'x')
                {
                    danceMovesList.Add(new ExchangeDanceMove(
                        Convert.ToInt32(Regex.Match(danceMove, "\\d+(?=/)").Value), 
                        Convert.ToInt32(Regex.Match(danceMove, "(?<=/)\\d+").Value)));
                }
                if (danceMove[0] == 'p')
                {
                    danceMovesList.Add(new PartnerDanceMove(
                        Regex.Match(danceMove, "(?<=p)\\w(?=/)").Value[0],
                        Regex.Match(danceMove,"(?<=/)\\w").Value[0]));
                }
            }
            _danceMoves = danceMovesList;
        }

        public string SolveFirst()
        {
            var positions = new string(InitialPositions.ToCharArray());
            foreach (var danceMove in _danceMoves)
            {
                positions = danceMove.Dance(positions);
            }
            return positions;
        }

        public string SolveSecond()
        {
            var periodList = new List<string>();
            var positions = new string(InitialPositions.ToCharArray());
            var counter = 0;
            do
            {
                periodList.Add(positions);
                counter++;
                foreach (var danceMove in _danceMoves)
                {
                    positions = danceMove.Dance(positions);
                }
            } while (positions != InitialPositions);

            return periodList[NumOfIterations % counter];
        }
    }
}
