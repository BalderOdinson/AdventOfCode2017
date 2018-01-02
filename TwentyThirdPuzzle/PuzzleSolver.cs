using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwentyThirdPuzzle
{
    public class PuzzleSolver
    {
        private readonly IEnumerable<char> _registers;
        private readonly IEnumerable<Instruction> _instructions;

        public PuzzleSolver(IEnumerable<string> instructions)
        {
            var setOfRegisters = new HashSet<char>();
            var program = new List<Instruction>();
            foreach (var instruction in instructions)
            {
                var trimedInstruction = instruction.Trim();
                foreach (var match in Regex.Matches(trimedInstruction, "(?<= )[a-z]").OfType<Match>())
                {
                    setOfRegisters.Add(match.Value[0]);
                }
                var instructionId = Regex.Match(trimedInstruction, "(?<=^)[a-z]+").Value;
                var firstOperand = Regex.IsMatch(trimedInstruction, $"(?<={instructionId} )[a-z]")
                    ? (dynamic)Regex.Match(trimedInstruction, $"(?<={instructionId} )[a-z]").Value[0]
                    : Convert.ToInt64(Regex.Match(trimedInstruction, $"(?<={instructionId} )(-\\d+|\\d+)(?!$)").Value);
                var secondOperand = Regex.IsMatch(trimedInstruction, "(?<=([a-z] |(-\\d+|\\d+) ))(.+)(?=$)")
                    ? (Regex.IsMatch(trimedInstruction, "(?<=([a-z] |(-\\d+|\\d+) ))[a-z](?=$)")
                        ? (dynamic)Regex.Match(trimedInstruction, "(?<=([a-z] |(-\\d+|\\d+) ))[a-z](?=$)").Value[0]
                        : Convert.ToInt64(Regex.Match(trimedInstruction, "(?<=([a-z] |(-\\d+|\\d+) ))(-\\d+|\\d+)(?=$)").Value))
                    : null;
                program.Add(secondOperand != null
                    ? new Instruction(instructionId.GetInstructionId(), firstOperand, secondOperand)
                    : new Instruction(instructionId.GetInstructionId(), firstOperand));
            }

            _registers = setOfRegisters;
            _instructions = program;
        }

        public long SolveFirst()
        {
            var cpu = new CPU(0);
            cpu.Initialize(_registers);
            cpu.LoadProgram(_instructions);
            return cpu.Execute().Count(state => state.CurrentInstruction.InstructionId == InstructionId.Mul);
        }
        public long SolveSecond()
        {
            var h = 0;
            GetBAndCRegister(out var b, out var c, out var bIncremeant);
            for (var i = b; i <= c; i += bIncremeant)
            {
                if (!IsPrime(i))
                    h++;
            }

            return h;
        }

        private static bool IsPrime(long number)
        {
            if (number == 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
    
        private void GetBAndCRegister(out long b, out long c, out long bIncremeant)
        {
            b = 0;
            c = 0;
            bIncremeant = 0;
            var skipRest = false;
            foreach (var instruction in _instructions)
            {
                if (instruction.InstructionId == InstructionId.Jnz && instruction.FirstConstant.HasValue
                                                                   && instruction.FirstConstant < 0)
                {
                    skipRest = true;
                    continue;
                }

                if (skipRest)
                {
                    if (instruction.InstructionId == InstructionId.Sub && instruction.FirstRegister == 'b' && instruction.FirstConstant.HasValue)
                    {
                        bIncremeant = -instruction.FirstConstant.Value;
                        continue;
                    }
                }

                switch (instruction.InstructionId)
                {
                    case InstructionId.Set:
                        switch (instruction.FirstRegister)
                        {
                            case 'b':
                                if (instruction.SecondRegister == 'c')
                                {
                                    b = c;
                                }
                                else if (instruction.FirstConstant.HasValue)
                                {
                                    b = instruction.FirstConstant.Value;
                                }

                                break;
                            case 'c':
                                if (instruction.SecondRegister == 'b')
                                {
                                    c = b;
                                }
                                else if (instruction.FirstConstant.HasValue)
                                {
                                    c = instruction.FirstConstant.Value;
                                }

                                break;
                        }
                        break;
                    case InstructionId.Sub:
                        switch (instruction.FirstRegister)
                        {
                            case 'b':
                                if (instruction.SecondRegister == 'c')
                                {
                                    b -= c;
                                }
                                else if (instruction.FirstConstant.HasValue)
                                {
                                    b -= instruction.FirstConstant.Value;
                                }

                                break;
                            case 'c':
                                if (instruction.SecondRegister == 'b')
                                {
                                    c -= b;
                                }
                                else if (instruction.FirstConstant.HasValue)
                                {
                                    c -= instruction.FirstConstant.Value;
                                }

                                break;
                        }
                        break;
                    case InstructionId.Mul:
                        switch (instruction.FirstRegister)
                        {
                            case 'b':
                                if (instruction.SecondRegister == 'c')
                                {
                                    b *= c;
                                }
                                else if (instruction.FirstConstant.HasValue)
                                {
                                    b *= instruction.FirstConstant.Value;
                                }

                                break;
                            case 'c':
                                if (instruction.SecondRegister == 'b')
                                {
                                    c *= b;
                                }
                                else if (instruction.FirstConstant.HasValue)
                                {
                                    c *= instruction.FirstConstant.Value;
                                }

                                break;
                        }
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}
