using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ThirteenthPuzzle
{
    class Field
    {
        public Field(int depth, int range)
        {
            Depth = depth;
            Range = range;
            ActiveField = 0;
            _isUpDirection = false;
        }

        public int Depth { get; }
        public int Range { get; }
        public int ActiveField { get; set; }
        private bool _isUpDirection;

        public void Move()
        {
            if (ActiveField == Range - 1)
                _isUpDirection = true;
            if (ActiveField == 0 && _isUpDirection)
                _isUpDirection = false;
            if (_isUpDirection)
                ActiveField--;
            else
                ActiveField++;
        }

        public bool IsFieldActive(int picosecond, int activeField)
        {
            return activeField == picosecond % ((Range - 1) * 2);
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType())
                return false;
            return ((Field)obj).Depth.Equals(Depth);
        }

        public override int GetHashCode()
        {
            return Depth.GetHashCode();
        }
    }
    public class PuzzleSolver
    {
        private readonly IDictionary<int, Field> _fieldsState;

        public PuzzleSolver(IEnumerable<string> input)
        {
            _fieldsState = new Dictionary<int, Field>();
            foreach (var depthInfo in input)
            {
                var depth = int.Parse(Regex.Match(depthInfo, "(.*)(?=\\:)").Value);
                var range = int.Parse(Regex.Match(depthInfo, "(?<=(\\: ))(.*)").Value);
                _fieldsState.Add(depth, new Field(depth, range));
            }
        }

        public int SolveFirst()
        {
            var fieldsState = new Dictionary<int,Field>(_fieldsState);
            var maxDepth = fieldsState.Max(f => f.Key);
            var sum = 0;
            for (int i = 0; i <= maxDepth; i++)
            {
                if (fieldsState.ContainsKey(i) && fieldsState[i].ActiveField == 0)
                    sum += fieldsState[i].Depth * fieldsState[i].Range;

                foreach (var field in fieldsState)
                {
                    field.Value.Move();
                }
            }

            return sum;
        }

        public int SolveSecond()
        {
            var fieldsState = new Dictionary<int, Field>(_fieldsState);
            return FindFreeSpacePicosecond(fieldsState);
        }

        private int FindFreeSpacePicosecond(IDictionary<int,Field> fieldsState)
        {
            var picosecond = 0; 
            while (true)
            {
                var isFound = true;
                foreach (var field in fieldsState)
                {
                    if (field.Value.IsFieldActive(picosecond + field.Key, 0))
                        isFound = false;
                }

                if (isFound)
                    return picosecond;

                picosecond++;
            }
        }
    }
}
