using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
               throw new ArgumentNullException(nameof(args));
            Console.WriteLine($"Rezultat prve polovice je: {new PuzzleSolver(args[0]?.Select(c => Int32.Parse(c.ToString()))).SolveFirstHalf()}");
            Console.WriteLine($"Rezultat druge polovice je: {new PuzzleSolver(args[0]?.Select(c => Int32.Parse(c.ToString()))).SolveSecondHalf()}");
            Console.ReadLine();
        }
    }
}
