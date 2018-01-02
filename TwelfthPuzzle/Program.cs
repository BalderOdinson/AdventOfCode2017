using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwelfthPuzzle
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a input file";
            openFileDialog.Filter = "Txt file|*.txt";
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            var input = File.ReadAllLines(openFileDialog.FileName);
            var puzzleSolver = new PuzzleSolver(input);
            Console.WriteLine(puzzleSolver.SolveFirst());
            Console.WriteLine(puzzleSolver.SolveSecond());
            Console.ReadLine();
        }
    }
}
