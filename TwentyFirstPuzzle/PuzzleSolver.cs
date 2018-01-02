using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwentyFirstPuzzle
{
    public class PuzzleSolver
    {
        private readonly IDictionary<PixelBlock, PixelBlock> _enhancementDictionary;
        private const string InitialBlock = ".#./..#/###";

        public PuzzleSolver(IEnumerable<string> input)
        {
            _enhancementDictionary = new Dictionary<PixelBlock, PixelBlock>();
            foreach (var line in input)
            {
                var splitResult = Regex.Split(line, " => ");
                var rotatingBlock = new PixelBlock(splitResult[0]);
                var valueBlock = new PixelBlock(splitResult[1]);
                _enhancementDictionary.Add(rotatingBlock, new PixelBlock(valueBlock));
                for (int i = 0; i < 3; i++)
                {
                    var flipingBlock = rotatingBlock.FlipVertical;
                    if (!_enhancementDictionary.ContainsKey(flipingBlock))
                        _enhancementDictionary.Add(flipingBlock, valueBlock);
                    flipingBlock = rotatingBlock.FlipHorizontal;
                    if (!_enhancementDictionary.ContainsKey(flipingBlock))
                        _enhancementDictionary.Add(flipingBlock, valueBlock);
                    rotatingBlock = rotatingBlock.RotateRight;
                    if (!_enhancementDictionary.ContainsKey(rotatingBlock))
                        _enhancementDictionary.Add(rotatingBlock, valueBlock);
                }
            }
        }

        public int SolveFirst()
        {
            var block = new PixelBlock(InitialBlock);
            for (int i = 0; i < 5; i++)
            {
                block = new PixelBlock(block.DivideBlocks().Select(pblock => _enhancementDictionary[pblock]));
            }
            return block.ToString().Count(c => c == '#');
        }

        public int SolveSecond()
        {
            var block = new PixelBlock(InitialBlock);
            for (int i = 0; i < 18; i++)
            {
                block = new PixelBlock(block.DivideBlocks().Select(pblock => _enhancementDictionary[pblock]));
            }
            return block.ToString().Count(c => c == '#');
        }
    }
}
