using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwentyFirstPuzzle
{
    public struct PixelBlock
    {
        public int Size => _block.Count;
        private readonly List<BitArray> _block;

        private PixelBlock(IEnumerable<BitArray> block)
        {
            var listBlock = block.ToList();
            _block = listBlock;
        }

        public PixelBlock(IEnumerable<PixelBlock> blocks)
        {
            _block = JoinBlocks(blocks.Select(p => p._block)).ToList();
        }

        public PixelBlock(string block)
        {
            _block = Regex.Split(block, "/").Select(s => new BitArray(s.Select(c => c == '#').ToArray())).ToList();
        }

        public PixelBlock(PixelBlock pixelBlock)
        {
            _block = pixelBlock._block.Select(b => new BitArray(b)).ToList();
        }

        private static IEnumerable<BitArray> JoinBlocks(IEnumerable<IEnumerable<BitArray>> blocks)
        {
            var blocksList = blocks.Select(b => b.ToList()).ToList();
            var numOfBlocks = blocksList.Count;
            var blockSize = blocksList[0].Count;
            var newSize = (int)Math.Sqrt(blockSize * blockSize * numOfBlocks);
            var currentPosition = 0;
            var buffers = new List<bool[]>();
            for (int i = 0; i < newSize; i++)
            {
                buffers.Add(new bool[newSize]);
            }
            foreach (var block in blocksList)
            {
                for (int i = 0; i < blockSize; i++)
                {
                    block[i].CopyTo(buffers[i], currentPosition);
                }

                currentPosition += blockSize;

                if (currentPosition == newSize)
                {
                    for (int i = 0; i < blockSize; i++)
                    {
                        yield return new BitArray(buffers[i]);
                        buffers[i] = new bool[newSize];
                    }

                    currentPosition = 0;
                }
            }
        }

        public IEnumerable<PixelBlock> DivideBlocks()
        {

            if (Size % 2 == 0)
            {
                for (int i = 0; i < Size; i += 2)
                {
                    for (int j = 0; j < Size; j += 2)
                    {
                        var newBlock = new List<BitArray>
                        {
                            new BitArray(new[]
                            {
                                _block[i][j], _block[i][j + 1]
                            }),
                            new BitArray(new[]
                            {
                                _block[i + 1][j], _block[i + 1][j + 1]
                            })
                        };

                        yield return new PixelBlock(newBlock);
                    }
                }
            }

            else if (Size % 3 == 0)
            {
                for (int i = 0; i < Size; i += 3)
                {
                    for (int j = 0; j < Size; j += 3)
                    {
                        var newBlock = new List<BitArray>
                        {
                            new BitArray(new[]
                            {
                                _block[i][j], _block[i][j + 1], _block[i][j + 2]
                            }),
                            new BitArray(new[]
                            {
                                _block[i + 1][j], _block[i + 1][j + 1], _block[i + 1][j + 2]
                            }),
                            new BitArray(new []
                            {
                                _block[i + 2][j], _block[i + 2][j + 1], _block[i + 2][j + 2]
                            })
                        };

                        yield return new PixelBlock(newBlock);
                    }
                }
            }
        }

        public PixelBlock RotateLeft => new PixelBlock(_rotateLeft());

        public PixelBlock RotateRight => new PixelBlock(_rotateRight());

        public PixelBlock FlipVertical => new PixelBlock(_flipVertical());

        public PixelBlock FlipHorizontal => new PixelBlock(_flipHorizontal());

        private IEnumerable<BitArray> _rotateLeft()
        {
            for (int i = Size - 1; i >= 0; i--)
            {
                var array = new bool[Size];
                for (int j = 0; j < Size; j++)
                {
                    array[j] = _block[j][i];
                }
                yield return new BitArray(array);
            }
        }

        private IEnumerable<BitArray> _rotateRight()
        {
            for (int i = 0; i < Size; i++)
            {
                var array = new bool[Size];
                for (int j = Size - 1; j >= 0; j--)
                {
                    array[Size - 1 - j] = _block[j][i];
                }
                yield return new BitArray(array);
            }
        }

        private IEnumerable<BitArray> _flipHorizontal()
        {
            for (int i = 0; i < Size; i++)
            {
                var array = new bool[Size];
                for (int j = 0; j < Size; j++)
                {
                    array[j] = _block[i][Size - 1 - j];
                }
                yield return new BitArray(array);
            }
        }

        private IEnumerable<BitArray> _flipVertical()
        {
            for (int i = Size - 1; i >= 0; i--)
            {
                yield return new BitArray(_block[i]);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var bitArray in _block)
            {
                foreach (var bit in bitArray)
                {
                    if ((bool)bit)
                        sb.Append("#");
                    else
                        sb.Append(".");
                }

                sb.Append("/");
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public string ToNewLineString()
        {
            var sb = new StringBuilder();
            foreach (var bitArray in _block)
            {
                foreach (var bit in bitArray)
                {
                    if ((bool)bit)
                        sb.Append("#");
                    else
                        sb.Append(".");
                }

                sb.AppendLine();
            }

            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is PixelBlock pixelBlock))
                return false;
            if (pixelBlock.Size != Size)
                return false;
            return pixelBlock.ToString().Equals(ToString());
        }

        public bool Equals(PixelBlock other)
        {
            return Equals(_block, other._block);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
