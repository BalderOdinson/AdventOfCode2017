using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SixteenthPuzzle
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
            var puzzleSolver = new PuzzleSolver(Regex.Split(input[0], ","));
            Console.WriteLine(puzzleSolver.SolveFirst());
            Console.WriteLine(puzzleSolver.SolveSecond());
            Console.ReadLine();
        }
    }
}
