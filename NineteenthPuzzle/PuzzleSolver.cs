using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NineteenthPuzzle
{
    public class PuzzleSolver
    {
        private readonly IEnumerable<string> _labyrinth;

        public PuzzleSolver(IEnumerable<string> labyrinth)
        {
            _labyrinth = labyrinth;
        }

        public string SolveFirst()
        {
            var labyrinth = _labyrinth.ToList();
            var currentPositionX = labyrinth[0].IndexOf("|");
            var currentPositionY = 0;
            var currentChar = '|';
            var direction = 1;
            var result = string.Empty;
            while (true)
            {
                if (currentChar == '-')
                {
                    currentPositionX += direction;
                    if (char.IsWhiteSpace(labyrinth[currentPositionY][currentPositionX]))
                        return result;
                    while (!PossibleNextCharacters(currentChar).Contains(labyrinth[currentPositionY][currentPositionX]))
                    {
                        currentPositionX += direction;
                        if (char.IsWhiteSpace(labyrinth[currentPositionY][currentPositionX]))
                            return result;
                    }
                }

                if (currentChar == '|')
                {
                    currentPositionY += direction;
                    if (char.IsWhiteSpace(labyrinth[currentPositionY][currentPositionX]))
                        return result;
                    while (!PossibleNextCharacters(currentChar).Contains(labyrinth[currentPositionY][currentPositionX]))
                    {
                        currentPositionY += direction;
                        if (char.IsWhiteSpace(labyrinth[currentPositionY][currentPositionX]))
                            return result;
                    }
                }

                if (labyrinth[currentPositionY][currentPositionX] == '+')
                {
                    if (currentChar == '-')
                    {
                        if (labyrinth.Count - 1 > currentPositionY && 
                            PossibleNextCharacters('|').Contains(labyrinth[currentPositionY + 1][currentPositionX]))
                        {
                            direction = 1;    
                        }
                        else
                        {
                            direction = -1;
                        }
                        currentPositionY += direction;
                        currentChar = '|';
                    }
                    else if (currentChar == '|')
                    {
                        if (labyrinth[currentPositionY].Length - 1 > currentPositionX &&
                            PossibleNextCharacters('-').Contains(labyrinth[currentPositionY][currentPositionX + 1]))
                        {
                            direction = 1;
                        }
                        else
                        {
                            direction = -1;
                        }
                        currentPositionX += direction;
                        currentChar = '-';
                    }
                }

                if (char.IsLetter(labyrinth[currentPositionY][currentPositionX]))
                {
                    result += labyrinth[currentPositionY][currentPositionX];
                }
            }
        }

        public int SolveSecond()
        {
            var labyrinth = _labyrinth.ToList();
            var currentPositionX = labyrinth[0].IndexOf("|");
            var currentPositionY = 0;
            var currentChar = '|';
            var direction = 1;
            var result = 1;
            while (true)
            {
                if (currentChar == '-')
                {
                    currentPositionX += direction;
                    result++;
                    if (char.IsWhiteSpace(labyrinth[currentPositionY][currentPositionX]))
                    {
                        result--;
                        return result;
                    }
                    while (!PossibleNextCharacters(currentChar).Contains(labyrinth[currentPositionY][currentPositionX]))
                    {
                        currentPositionX += direction;
                        result++;
                        if (char.IsWhiteSpace(labyrinth[currentPositionY][currentPositionX]))
                        {
                            result--;
                            return result;
                        }
                    }
                }

                if (currentChar == '|')
                {
                    currentPositionY += direction;
                    result++;
                    if (char.IsWhiteSpace(labyrinth[currentPositionY][currentPositionX]))
                    {
                        result--;
                        return result;
                    }
                    while (!PossibleNextCharacters(currentChar).Contains(labyrinth[currentPositionY][currentPositionX]))
                    {
                        currentPositionY += direction;
                        result++;
                        if (char.IsWhiteSpace(labyrinth[currentPositionY][currentPositionX]))
                        {
                            result--;
                            return result;
                        }
                    }
                }

                if (labyrinth[currentPositionY][currentPositionX] == '+')
                {
                    if (currentChar == '-')
                    {
                        if (labyrinth.Count - 1 > currentPositionY &&
                            PossibleNextCharacters('|').Contains(labyrinth[currentPositionY + 1][currentPositionX]))
                        {
                            direction = 1;
                        }
                        else
                        {
                            direction = -1;
                        }
                        currentPositionY += direction;
                        result++;
                        currentChar = '|';
                    }
                    else if (currentChar == '|')
                    {
                        if (labyrinth[currentPositionY].Length - 1 > currentPositionX &&
                            PossibleNextCharacters('-').Contains(labyrinth[currentPositionY][currentPositionX + 1]))
                        {
                            direction = 1;
                        }
                        else
                        {
                            direction = -1;
                        }
                        currentPositionX += direction;
                        result++;
                        currentChar = '-';
                    }
                }
            }
        }

        private string PossibleNextCharacters(char currentChar, char previousChar = '\0')
        {
            var alpha = "ABCDEFGHIJKLMNOPQRSTUVWYXZ";
            switch (currentChar)
            {
                case '-':
                    return alpha + "-+";
                case '|':
                    return alpha + "|+";
                default:
                    if (alpha.Contains(currentChar))
                    {
                        return previousChar == '-' ? "-+" : "|+";
                    }
                    break;
            }

            return null;
        }

    }
}
