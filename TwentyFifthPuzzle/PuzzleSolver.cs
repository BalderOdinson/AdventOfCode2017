using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwentyFifthPuzzle
{
    public class PuzzleSolver
    {
        private readonly IDictionary<char, State> _statesDictionary;
        private readonly char _initalState;
        private readonly int _diagnosticChecksumSteps;

        public PuzzleSolver(string input)
        {
            _statesDictionary = new Dictionary<char, State>();
            foreach (var match in Regex.Matches(input,
                    "In state (?<state>\\w)[\\S\\s]*?" +
                    "If the current value is (?<value0>\\d)[\\S\\s]*?" +
                    "Write.*?(?<conditionValue01>\\d)[\\S\\s]*?" +
                    "Move.*?(?<conditionValue02>((left)|(right)))[\\S\\s]*?" +
                    "Continue.*?(?<conditionValue03>[A-Z])[\\S\\s]*?" +
                    "If the current value is (?<value1>\\d)[\\S\\s]*?" +
                    "Write.*?(?<conditionValue11>\\d)[\\S\\s]*?" +
                    "Move.*?(?<conditionValue12>((left)|(right)))[\\S\\s]*?" +
                    "Continue.*?(?<conditionValue13>[A-Z])[\\S\\s]*?")
                .OfType<Match>())
            {
                var action0 = new TuringMachineAction(Convert.ToByte(match.Groups["value0"].Value),
                    match.Groups["conditionValue03"].Value[0],
                    match.Groups["conditionValue02"].Value.GetMoveDirection(),
                    Convert.ToByte(match.Groups["conditionValue01"].Value));
                var action1 = new TuringMachineAction(Convert.ToByte(match.Groups["value1"].Value),
                    match.Groups["conditionValue13"].Value[0],
                    match.Groups["conditionValue12"].Value.GetMoveDirection(),
                    Convert.ToByte(match.Groups["conditionValue11"].Value));
                var state = new State(match.Groups["state"].Value[0], action0, action1);
                _statesDictionary.Add(state.Value, state);
            }

            _initalState = Regex.Match(input, "(?<=Begin in state )[A-Z]").Value[0];
            _diagnosticChecksumSteps =
                Convert.ToInt32(Regex.Match(input, "(?<=Perform a diagnostic checksum after )\\d+").Value);
        }

        public int SolveFirst()
        {
            var tape = new LinkedList<byte>();
            var currentTapePosition = new LinkedListNode<byte>(0);
            var currentState = _statesDictionary[_initalState];
            tape.AddFirst(currentTapePosition);
            for (int i = 0; i < _diagnosticChecksumSteps; i++)
            {
                var nextState = _statesDictionary[currentState.NextState(currentTapePosition.Value)];
                var newTapeValue = currentState.Write(currentTapePosition.Value);
                switch (currentState.Move(currentTapePosition.Value))
                {
                    case MoveDirection.Left:
                        currentTapePosition.Value = newTapeValue;
                        currentTapePosition = currentTapePosition.Previous ?? tape.AddBefore(currentTapePosition, 0);
                        break;
                    case MoveDirection.Right:
                        currentTapePosition.Value = newTapeValue;
                        currentTapePosition = currentTapePosition.Next ?? tape.AddAfter(currentTapePosition, 0);
                        break;
                }
                currentState = nextState;
            }
            return tape.Sum(n => n);
        }
    }
}