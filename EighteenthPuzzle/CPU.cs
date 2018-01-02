using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public class CPU
    {
        public IDictionary<char, long> Registers { get; private set; }

        private readonly ALU _alu;

        public int Id { get; }

        private NetworkCard _networkCard;

        public long PackegesSent => _networkCard.PackegesSent;

        public long PackegesRecieved => _networkCard.PackegesRecieved;

        private long _programCounter = 0;

        private long _newProgramCounter = 0;

        private List<Instruction> _instructions;

        private readonly CPUMode _cpuMode;

        private readonly Router _router;

        public CPU(int id, CPUMode cpuMode, Router router)
        {
            Id = id;
            _alu = new ALU();
            _cpuMode = cpuMode;
            _router = router;
        }

        public async Task Initialize(IEnumerable<char> registers)
        {
            await Task.Run(async () =>
            {
                Registers = new Dictionary<char, long>();
                foreach (var register in registers)
                {
                    Registers.Add(register, register == 'p' ? Id : 0);
                }

                _networkCard = new NetworkCard(Id, _router);
                await _networkCard.Connect();
                _programCounter = 0;
                _newProgramCounter = 0;
            });
        }

        public IEnumerable<Instruction> Instructions => _instructions;

        public void LoadProgram(IEnumerable<Instruction> instructions)
        {
            _instructions = instructions.ToList();
        }

        private async Task<CPUState> _send(Instruction instruction)
        {
            _newProgramCounter++;
            if (instruction.FirstRegister.HasValue)
            {
                await _networkCard.SendTo(Registers[instruction.FirstRegister.Value],
                    _cpuMode == CPUMode.Disconnected ? TargetConnectionOption.LocalSelf : TargetConnectionOption.LocalBroadcast);
                return new CPUState(Registers, instruction, _programCounter);
            }
            else if (instruction.FirstConstant.HasValue)
            {
                await _networkCard.SendTo(instruction.FirstConstant.Value,
                    _cpuMode == CPUMode.Disconnected ? TargetConnectionOption.LocalSelf : TargetConnectionOption.LocalBroadcast);

                return new CPUState(Registers, instruction, _programCounter);
            }
            else
                throw new InvalidOperationException("No corresponding number of operands given.");
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

        private CPUState _add(Instruction instruction)
        {
            _newProgramCounter++;
            if (instruction.SecondRegister.HasValue)
            {
                _alu.Add(Registers[instruction.FirstRegister.Value],
                    Registers[instruction.SecondRegister.Value]);
                Registers[instruction.FirstRegister.Value] =
                    _alu.AluResult;
                return new CPUState(Registers, instruction, _programCounter);
            }
            else if (instruction.FirstConstant.HasValue)
            {
                _alu.Add(Registers[instruction.FirstRegister.Value],
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

        private CPUState _mod(Instruction instruction)
        {
            _newProgramCounter++;
            if (instruction.SecondRegister.HasValue)
            {
                _alu.Modulo(Registers[instruction.FirstRegister.Value],
                    Registers[instruction.SecondRegister.Value]);
                Registers[instruction.FirstRegister.Value] =
                    _alu.AluResult;
                return new CPUState(Registers, instruction, _programCounter);
            }
            else if (instruction.FirstConstant.HasValue)
            {
                _alu.Modulo(Registers[instruction.FirstRegister.Value],
                    instruction.FirstConstant.Value);
                Registers[instruction.FirstRegister.Value] =
                    _alu.AluResult;
                return new CPUState(Registers, instruction, _programCounter);
            }
            else
                throw new InvalidOperationException("No corresponding number of operands given.");
        }

        private async Task<CPUState> _rcv(Instruction instruction)
        {
            _newProgramCounter++;
            if (!instruction.FirstRegister.HasValue)
                throw new InvalidOperationException("No corresponding number of operands given.");
            if (_cpuMode == CPUMode.Disconnected)
            {
                if (Registers[instruction.FirstRegister.Value] != 0)
                {
                    Registers[instruction.FirstRegister.Value] = _networkCard.LastMessage;
                    return new CPUState(Registers, instruction, _programCounter);
                }
                else
                {
                    return new CPUState(Registers, instruction, _programCounter, false);
                }
            }
            else
            {
                try
                {
                    Registers[instruction.FirstRegister.Value] = await _networkCard.Recieve();
                }
                catch (OperationCanceledException)
                {
                    _newProgramCounter = _instructions.Count;
                    return new CPUState(Registers, instruction, _programCounter, false);
                }
                return new CPUState(Registers, instruction, _programCounter);
            }
        }

        private CPUState _jgz(Instruction instruction)
        {
            if (instruction.FirstRegister.HasValue)
            {
                if (instruction.SecondRegister.HasValue)
                {
                    if (Registers[instruction.FirstRegister.Value] > 0)
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
                    if (Registers[instruction.FirstRegister.Value] > 0)
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
                    if (instruction.FirstConstant.Value > 0)
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
                    if (instruction.FirstConstant.Value > 0)
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
                    case InstructionId.Snd:
                        yield return _send(instruction).Result;
                        break;
                    case InstructionId.Set:
                        yield return _set(instruction);
                        break;
                    case InstructionId.Add:
                        yield return _add(instruction);
                        break;
                    case InstructionId.Mul:
                        yield return _mul(instruction);
                        break;
                    case InstructionId.Mod:
                        yield return _mod(instruction);
                        break;
                    case InstructionId.Rcv:
                        yield return _rcv(instruction).Result;
                        break;
                    case InstructionId.Jgz:
                        yield return _jgz(instruction);
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
            return await Task.Run(async () =>
            {
                var result = new List<CPUState> { new CPUState(Registers, new Instruction(InstructionId.Init), -1) };
                while (_programCounter < _instructions.Count)
                {
                    var instruction = _instructions[(int)_programCounter];
                    switch (instruction.InstructionId)
                    {
                        case InstructionId.Snd:
                            result.Add(await _send(instruction));
                            break;
                        case InstructionId.Set:
                            result.Add(_set(instruction));
                            break;
                        case InstructionId.Add:
                            result.Add(_add(instruction));
                            break;
                        case InstructionId.Mul:
                            result.Add(_mul(instruction));
                            break;
                        case InstructionId.Mod:
                            result.Add(_mod(instruction));
                            break;
                        case InstructionId.Rcv:
                            result.Add(await _rcv(instruction));
                            break;
                        case InstructionId.Jgz:
                            result.Add(_jgz(instruction));
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
