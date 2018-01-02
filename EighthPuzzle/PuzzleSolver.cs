using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EighthPuzzle
{
    enum Operator
    {
        LT = '<',
        LE = '<' + '=',
        GT = '>',
        GE = '>' + '=',
        EQ = '=' + '=',
        NE = '!' + '=',
        INC = 'i' + 'n' + 'c',
        DEC = 'd' + 'e' + 'c'
    }



    public class PuzzleSolver
    {
        private readonly IEnumerable<string> _input;
        private readonly Dictionary<string, int> _registers = new Dictionary<string, int>();
        public PuzzleSolver(IEnumerable<string> input)
        {
            _input = input;
        }

        public int SolveFirst()
        {
            foreach (var line in _input)
            {
                var splitedLine = Regex.Split(line, " ");
                var operand = splitedLine[0];
                var operation = (Operator)splitedLine[1].Sum(Convert.ToInt32);
                var constant = int.Parse(splitedLine[2]);
                var ifOperand = splitedLine[4];
                var ifOperation = (Operator)splitedLine[5].Sum(Convert.ToInt32);
                var ifConstant = int.Parse(splitedLine[6]);
                if(!_registers.ContainsKey(operand))
                    _registers.Add(operand, 0);
                if(!_registers.ContainsKey(ifOperand))
                    _registers.Add(ifOperand, 0);
                switch (ifOperation)
                {
                    case Operator.LT:
                        if (_registers[ifOperand] < ifConstant)
                            DoOperation(operand, constant, operation);
                        break;
                    case Operator.LE:
                        if (_registers[ifOperand] <= ifConstant)
                            DoOperation(operand, constant, operation);
                        break;
                    case Operator.GT:
                        if (_registers[ifOperand] > ifConstant)
                            DoOperation(operand, constant, operation);
                        break;
                    case Operator.GE:
                        if (_registers[ifOperand] >= ifConstant)
                            DoOperation(operand, constant, operation);
                        break;
                    case Operator.EQ:
                        if (_registers[ifOperand] == ifConstant)
                            DoOperation(operand, constant, operation);
                        break;
                    case Operator.NE:
                        if (_registers[ifOperand] != ifConstant)
                            DoOperation(operand, constant, operation);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return _registers.Max(pair => pair.Value);
        }

        public int SolveSecond()
        {
            ResetRegisters();
            var maxValue = 0;
            foreach (var line in _input)
            {
                var splitedLine = Regex.Split(line, " ");
                var operand = splitedLine[0];
                var operation = (Operator)splitedLine[1].Sum(Convert.ToInt32);
                var constant = int.Parse(splitedLine[2]);
                var ifOperand = splitedLine[4];
                var ifOperation = (Operator)splitedLine[5].Sum(Convert.ToInt32);
                var ifConstant = int.Parse(splitedLine[6]);
                if (!_registers.ContainsKey(operand))
                    _registers.Add(operand, 0);
                if (!_registers.ContainsKey(ifOperand))
                    _registers.Add(ifOperand, 0);
                switch (ifOperation)
                {
                    case Operator.LT:
                        if (_registers[ifOperand] < ifConstant)
                        {
                            var newValue = DoOperation(operand, constant, operation);
                            maxValue = maxValue < newValue ? newValue : maxValue;
                        }
                        break;
                    case Operator.LE:
                        if (_registers[ifOperand] <= ifConstant)
                        {
                            var newValue = DoOperation(operand, constant, operation);
                            maxValue = maxValue < newValue ? newValue : maxValue;
                        }
                        break;
                    case Operator.GT:
                        if (_registers[ifOperand] > ifConstant)
                        {
                            var newValue = DoOperation(operand, constant, operation);
                            maxValue = maxValue < newValue ? newValue : maxValue;
                        }
                        break;
                    case Operator.GE:
                        if (_registers[ifOperand] >= ifConstant)
                        {
                            var newValue = DoOperation(operand, constant, operation);
                            maxValue = maxValue < newValue ? newValue : maxValue;
                        }
                        break;
                    case Operator.EQ:
                        if (_registers[ifOperand] == ifConstant)
                        {
                            var newValue = DoOperation(operand, constant, operation);
                            maxValue = maxValue < newValue ? newValue : maxValue;
                        }
                        break;
                    case Operator.NE:
                        if (_registers[ifOperand] != ifConstant)
                        {
                            var newValue = DoOperation(operand, constant, operation);
                            maxValue = maxValue < newValue ? newValue : maxValue;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return maxValue;
        }

        private int DoOperation(string operand, int constant, Operator operation)
        {
            switch (operation)
            { 
                case Operator.INC:
                    _registers[operand] += constant;
                    return _registers[operand];
                case Operator.DEC:
                    _registers[operand] -= constant; 
                    return _registers[operand];
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }

        private void ResetRegisters()
        {
            _registers.Clear();
        }
    }
}
