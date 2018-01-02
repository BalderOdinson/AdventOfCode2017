using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EighteenthPuzzle
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
            using (var router = new Router())
            {
                var cpu = new CPU(0, CPUMode.Disconnected, router);
                cpu.Initialize(_registers).Wait();
                cpu.LoadProgram(_instructions);
                return (from cpuState in cpu.Execute()
                        where cpuState.Executed && cpuState.CurrentInstruction.InstructionId == InstructionId.Rcv
                        select cpuState.RegisterStates[cpuState.CurrentInstruction.FirstRegister.Value]).FirstOrDefault();
            }

        }

        public async Task<long> SolveSecond()
        {
            using (var router = new Router())
            {
                var firstCpu = new CPU(0, CPUMode.InLocalNetwork, router);
                await firstCpu.Initialize(_registers);
                firstCpu.LoadProgram(_instructions);
                var secondCpu = new CPU(1, CPUMode.InLocalNetwork, router);
                await secondCpu.Initialize(_registers);
                secondCpu.LoadProgram(_instructions);
                var firstCpuTask = firstCpu.ExecuteAsync();
                var secondCpuTask = secondCpu.ExecuteAsync();
                await firstCpuTask;
                await secondCpuTask;
                return secondCpu.PackegesSent;
            }
        }
    }
}
