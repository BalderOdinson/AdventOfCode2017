using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyFifthPuzzle
{
    public struct TuringMachineAction
    {
        public TuringMachineAction(byte value, char nextStates, MoveDirection moveDirections, byte newTapeValue)
        {
            Value = value;
            NextState = nextStates;
            MoveDirections = moveDirections;
            NewTapeValue = newTapeValue;
        }

        public byte Value { get; }
        public char NextState { get; }
        public MoveDirection MoveDirections { get; }
        public byte NewTapeValue { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is TuringMachineAction turingMachineAction))
                return false;
            return turingMachineAction.Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
