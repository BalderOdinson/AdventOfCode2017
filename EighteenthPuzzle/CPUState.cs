using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public struct CPUState
    {
        public CPUState(IDictionary<char, long> registerStates, Instruction currentInstruction, long programCounter, bool executed = true)
        {
            RegisterStates = registerStates.ToDictionary(pair => pair.Key, pair => pair.Value);
            CurrentInstruction = currentInstruction;
            ProgramCounter = programCounter;
            Executed = executed;
        }

        public IDictionary<char, long> RegisterStates { get; }
        public Instruction CurrentInstruction { get; }
        public bool Executed { get; }
        public long ProgramCounter { get; }
    }
}
