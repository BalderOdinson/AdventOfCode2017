using System;
using System.Collections.Generic;
using System.Linq;

namespace TwentyFourthPuzzle
{
    public struct Component
    {
        private readonly List<int> _ends;

        public Component(IEnumerable<int> ends)
        {
            if(ends == null)
                throw new ArgumentException("Component must have two ends.");
            _ends = ends.ToList();
            if(_ends.Count != 2)
                throw new ArgumentException("Component must have two ends.");
        }

        public IEnumerable<int> Ends => _ends;

        public int Strength => Ends.Sum();

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is Component component))
                return false;
            return component.Ends.OrderBy(e => e).SequenceEqual(_ends.OrderBy(es => es));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + _ends.Min().GetHashCode();
                hash = hash * 23 + _ends.Max().GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{_ends[0]}/{_ends[1]}";
        }
    }
}
