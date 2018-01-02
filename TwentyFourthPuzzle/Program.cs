using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwentyFourthPuzzle
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select a input file",
                Filter = "Txt file|*.txt"
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            var input = File.ReadAllLines(openFileDialog.FileName);
            var puzzleSolver = new PuzzleSolver(input);
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine(puzzleSolver.SolveFirst());
            Console.WriteLine(puzzleSolver.SolveSecond());
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed {stopwatch.Elapsed.TotalSeconds} s");
            Console.ReadLine();
        }
    }
}
