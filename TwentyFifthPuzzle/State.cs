using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyFifthPuzzle
{
    public struct State
    {
        private IDictionary<byte, TuringMachineAction> _actions;

        public char Value { get; }

        public State(char value, TuringMachineAction action0, TuringMachineAction action1)
        {
            Value = value;
            _actions = new Dictionary<byte, TuringMachineAction> { { action0.Value, action0 }, { action1.Value, action1 } };
        }

        public MoveDirection Move(byte value) => _actions[value].MoveDirections;

        public byte Write(byte value) => _actions[value].NewTapeValue;

        public char NextState(byte value) => _actions[value].NextState;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
