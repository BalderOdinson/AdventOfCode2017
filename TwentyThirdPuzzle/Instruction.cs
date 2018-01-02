using EighteenthPuzzle;
using Microsoft.Build.Utilities;

namespace TwentyThirdPuzzle
{
    public struct Instruction
    {
        public Instruction(InstructionId instructionId)
        {
            InstructionId = instructionId;
            FirstRegister = null;
            SecondRegister = null;
            FirstConstant = null;
            SecondConstant = null;
        }

        public Instruction(InstructionId instructionId, char firstRegister)
        {
            InstructionId = instructionId;
            FirstRegister = firstRegister;
            SecondRegister = null;
            FirstConstant = null;
            SecondConstant = null;
        }

        public Instruction(InstructionId instructionId, char firstRegister,
            char secondRegister)
        {
            InstructionId = instructionId;
            FirstRegister = firstRegister;
            SecondRegister = secondRegister;
            FirstConstant = null;
            SecondConstant = null;
        }

        public Instruction(InstructionId instructionId, char firstRegister,
            long firstConstant)
        {
            InstructionId = instructionId;
            FirstRegister = firstRegister;
            SecondRegister = null;
            FirstConstant = firstConstant;
            SecondConstant = null;
        }

        public Instruction(InstructionId instructionId,
            long firstConstant,
            char secondRegister)
        {
            InstructionId = instructionId;
            FirstRegister = null;
            SecondRegister = secondRegister;
            FirstConstant = firstConstant;
            SecondConstant = null;
        }

        public Instruction(InstructionId instructionId,
            long firstConstant, long secondConstant)
        {
            InstructionId = instructionId;
            FirstRegister = null;
            SecondRegister = null;
            FirstConstant = firstConstant;
            SecondConstant = secondConstant;
        }

        public InstructionId InstructionId { get; }
        public char? FirstRegister { get; }
        public char? SecondRegister { get; }
        public long? FirstConstant { get; }
        public long? SecondConstant { get; }

        public override string ToString()
        {
            return InstructionId.GetDescription() + " " +
                (FirstRegister ?? FirstConstant.Value) + " " +
                (FirstRegister.HasValue ? (FirstConstant ?? SecondRegister.Value) : (SecondRegister ?? SecondConstant.Value));
        }
    }
}
