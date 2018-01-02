using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using EighteenthPuzzle;

namespace TwentyThirdPuzzle
{
    public class CPU
    {
        public IDictionary<char, long> Registers { get; private set; }

        private readonly ALU _alu;

        public int Id { get; }

        private long _programCounter = 0;

        private long _newProgramCounter = 0;

        private List<Instruction> _instructions;

        public CPU(int id)
        {
            Id = id;
            _alu = new ALU();
        }

        public void Initialize(IEnumerable<char> registers)
        {
            Registers = new Dictionary<char, long>();
            foreach (var register in registers)
            {
                Registers.Add(register, register == 'a' ? Id : 0);
            }
            _programCounter = 0;
            _newProgramCounter = 0;
        }

        public async Task InitializeAsync(IEnumerable<char> registers)
        {
            await Task.Run(() =>
            {
                Registers = new Dictionary<char, long>();
                foreach (var register in registers)
                {
                    Registers.Add(register, register == 'a' ? Id : 0);
                }
                _programCounter = 0;
                _newProgramCounter = 0;
            });
        }

        public IEnumerable<Instruction> Instructions => _instructions;

        public void LoadProgram(IEnumerable<Instruction> instructions)
        {
            _instructions = instructions.ToList();
        }

        private CPUState _set(Instruction instruction)
        {
            _newProgramCounter++;
            if (instruction.SecondRegister.HasValue)
            {
                Registers[instruction.FirstRegister.Value] =
                    Registers[instruction.SecondRegister.Value];
                return new CPUState(Registers, instruction, _programCounter);
            }
            else if (instruction.FirstConstant.HasValue)
            {
                Registers[instruction.FirstRegister.Value] =
                    instruction.FirstConstant.Value;
                return new CPUState(Registers, instruction, _programCounter);
            }
            else
                throw new InvalidOperationException("No corresponding number of operands given.");
        }

        private CPUState _sub(Instruction instruction)
        {
            _newProgramCounter++;
            if (instruction.SecondRegister.HasValue)
            {
                _alu.Sub(Registers[instruction.FirstRegister.Value],
                    Registers[instruction.SecondRegister.Value]);
                Registers[instruction.FirstRegister.Value] =
                    _alu.AluResult;
                return new CPUState(Registers, instruction, _programCounter);
            }
            else if (instruction.FirstConstant.HasValue)
            {
                _alu.Sub(Registers[instruction.FirstRegister.Value],
                    instruction.FirstConstant.Value);
                Registers[instruction.FirstRegister.Value] =
                    _alu.AluResult;
                return new CPUState(Registers, instruction, _programCounter);
            }
            else
                throw new InvalidOperationException("No corresponding number of operands given.");
        }

        private CPUState _mul(Instruction instruction)
        {
            _newProgramCounter++;
            if (instruction.SecondRegister.HasValue)
            {
                _alu.Multiply(Registers[instruction.FirstRegister.Value],
                    Registers[instruction.SecondRegister.Value]);
                Registers[instruction.FirstRegister.Value] =
                    _alu.AluResult;
                return new CPUState(Registers, instruction, _programCounter);
            }
            else if (instruction.FirstConstant.HasValue)
            {
                _alu.Multiply(Registers[instruction.FirstRegister.Value],
                    instruction.FirstConstant.Value);
                Registers[instruction.FirstRegister.Value] =
                    _alu.AluResult;
                return new CPUState(Registers, instruction, _programCounter);
            }
            else
                throw new InvalidOperationException("No corresponding number of operands given.");
        }

        private CPUState _jnz(Instruction instruction)
        {
            if (instruction.FirstRegister.HasValue)
            {
                if (instruction.SecondRegister.HasValue)
                {
                    if (Registers[instruction.FirstRegister.Value] != 0)
                    {
                        _newProgramCounter += Registers[instruction.SecondRegister.Value];
                        return new CPUState(Registers, instruction, _programCounter);

                    }
                    else
                    {
                        _newProgramCounter++;
                        return new CPUState(Registers, instruction, _programCounter, false);
                    }
                }
                else if (instruction.FirstConstant.HasValue)
                {
                    if (Registers[instruction.FirstRegister.Value] != 0)
                    {
                        _newProgramCounter += instruction.FirstConstant.Value;
                        return new CPUState(Registers, instruction, _programCounter);
                    }
                    else
                    {
                        _newProgramCounter++;
                        return new CPUState(Registers, instruction, _programCounter, false);
                    }
                }
                else
                    throw new InvalidOperationException("No corresponding number of operands given.");
            }
            else if (instruction.FirstConstant.HasValue)
            {
                if (instruction.SecondRegister.HasValue)
                {
                    if (instruction.FirstConstant.Value != 0)
                    {
                        _newProgramCounter += Registers[instruction.SecondRegister.Value];
                        return new CPUState(Registers, instruction, _programCounter);
                    }
                    else
                    {
                        _newProgramCounter++;
                        return new CPUState(Registers, instruction, _programCounter, false);
                    }
                }
                else if (instruction.SecondConstant.HasValue)
                {
                    if (instruction.FirstConstant.Value != 0)
                    {
                        _newProgramCounter += instruction.SecondConstant.Value;
                        return new CPUState(Registers, instruction, _programCounter);
                    }
                    else
                    {
                        _newProgramCounter++;
                        return new CPUState(Registers, instruction, _programCounter, false);
                    }
                }
                else
                    throw new InvalidOperationException("No corresponding number of operands given.");
            }
            else
                throw new InvalidOperationException("No corresponding number of operands given.");

        }

        public IEnumerable<CPUState> Execute()
        {
            if (Registers == null)
                throw new InstanceNotFoundException("Registers not initialized.");
            yield return new CPUState(Registers, new Instruction(InstructionId.Init), -1);
            while (_programCounter < _instructions.Count)
            {
                var instruction = _instructions[(int)_programCounter];
                switch (instruction.InstructionId)
                {
                    case InstructionId.Set:
                        yield return _set(instruction);
                        break;
                    case InstructionId.Sub:
                        yield return _sub(instruction);
                        break;
                    case InstructionId.Mul:
                        yield return _mul(instruction);
                        break;
                    case InstructionId.Jnz:
                        yield return _jnz(instruction);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _programCounter = _newProgramCounter;
            }
            yield return new CPUState(Registers, new Instruction(InstructionId.Halt), _programCounter);
        }

        public async Task<IEnumerable<CPUState>> ExecuteAsync()
        {
            if (Registers == null)
                throw new InstanceNotFoundException("Registers not initialized.");
            return await Task.Run(() =>
            {
                var result = new List<CPUState> { new CPUState(Registers, new Instruction(InstructionId.Init), -1) };
                while (_programCounter < _instructions.Count)
                {
                    var instruction = _instructions[(int)_programCounter];
                    switch (instruction.InstructionId)
                    {
                        case InstructionId.Set:
                            result.Add(_set(instruction));
                            break;
                        case InstructionId.Sub:
                            result.Add(_sub(instruction));
                            break;
                        case InstructionId.Mul:
                            result.Add(_mul(instruction));
                            break;
                        case InstructionId.Jnz:
                            result.Add(_jnz(instruction));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    _programCounter = _newProgramCounter;
                }
                result.Add(new CPUState(Registers, new Instruction(InstructionId.Halt), _programCounter));
                return result;
            });
        }
    }
}
