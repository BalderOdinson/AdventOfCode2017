using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourteenthPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input: ");
            var input = Console.ReadLine();
            var puzzleSolver = new PuzzleSolver(input);
            Console.WriteLine(puzzleSolver.SolveFirst());
            Console.WriteLine(puzzleSolver.SolveSecond());
            Console.ReadLine();
        }
    }
}
