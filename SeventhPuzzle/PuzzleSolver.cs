using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SeventhPuzzle
{
    class ProgramInfo
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public List<ProgramInfo> Childs { get; set; }
        public bool IsChild { get; set; }
        public int TotalChildSize { get; set; }
    }

    class Result
    {
        public ProgramInfo DisbalancedProgram { get; set; }
        public int? RequiredValue { get; set; }
        public bool IsFound { get; set; }
    }

    public class PuzzleSolver
    {
        private readonly SortedList<string, ProgramInfo> _dictionary;
        private ProgramInfo _rootElement;

        public PuzzleSolver(IEnumerable<string> input)
        {
            _dictionary = new SortedList<string, ProgramInfo>();
            foreach (var line in input)
            {
                var name = Regex.Match(line, "(.*)(?= \\()").Value;
                var weight = int.Parse(Regex.Match(line, "(?<=\\()(.*)(?=\\))").Value);
                var childs = Regex.Split(Regex.Match(line, "(?<=\\> ).*").Value, ", ").Where(s => !string.IsNullOrWhiteSpace(s));
                var childsPrograminfo = new List<ProgramInfo>();
                foreach (var child in childs)
                {
                    if (_dictionary.ContainsKey(child))
                    {
                        _dictionary[child].IsChild = true;
                        childsPrograminfo.Add(_dictionary[child]);
                    }
                    else
                    {
                        var progInfo = new ProgramInfo { Name = child, IsChild = true };
                        _dictionary.Add(child, progInfo);
                        childsPrograminfo.Add(progInfo);
                    }
                }

                if (_dictionary.ContainsKey(name))
                {
                    _dictionary[name].Childs = childsPrograminfo;
                    _dictionary[name].Weight = weight;
                }
                else
                {
                    _dictionary.Add(name, new ProgramInfo
                    {
                        Name = name,
                        Weight = weight,
                        Childs = childsPrograminfo
                    });
                }
            }
        }

        public string SolveFirst()
        {
            _rootElement = _dictionary.First(p => !p.Value.IsChild).Value;
            return _rootElement.Name;
        }

        public int SolveSecond()
        {
            CalculateTotalChildSize(_rootElement);
            var disbalancedElementResult = GetDisbalancedElement(_rootElement);
            if (disbalancedElementResult.IsFound)
                return disbalancedElementResult.RequiredValue.Value;
            return 0;
        }

        private int CalculateTotalChildSize(ProgramInfo element)
        {
            if (element.Childs.Count == 0)
                return element.Weight;
            var totalSum = 0;
            foreach (var child in element.Childs)
            {
                totalSum += CalculateTotalChildSize(child);
            }

            element.TotalChildSize = totalSum;

            return totalSum + element.Weight;
        }

        private Result GetDisbalancedElement(ProgramInfo root)
        {
            if (!root.Childs.Any())
                return new Result
                {
                    IsFound = false
                };
            if (root.Childs.All(p =>
                (p.Weight + p.TotalChildSize).Equals(root.Childs[0].Weight + root.Childs[0].TotalChildSize)))
            {
                return new Result
                {
                    DisbalancedProgram = root,
                    IsFound = true
                };
            }

            ProgramInfo difValue;
            if (root.Childs.Count == 1)
                difValue = root.Childs.First();
            else if (root.Childs.Count == 2)
            {
                foreach (var child in root.Childs)
                {
                    var result = GetDisbalancedElement(child);
                    if (result.IsFound)
                    {
                        result.DisbalancedProgram = child;
                        var other = root.Childs.Find(info => info.Name != result.DisbalancedProgram.Name);
                        result.RequiredValue = other.TotalChildSize + other.Weight -
                                               result.DisbalancedProgram.TotalChildSize;
                        return result;
                    }
                }
                return new Result
                {
                    DisbalancedProgram = root.Childs[0],
                    IsFound = true,
                    RequiredValue = root.Childs[1].TotalChildSize + root.Childs[1].Weight -
                                    root.Childs[0].TotalChildSize
                };
            }
            try
            {
                difValue = root.Childs.SingleOrDefault(p =>
                    !(p.Weight + p.TotalChildSize).Equals(root.Childs[0].Weight + root.Childs[0].TotalChildSize));
            }
            catch (InvalidOperationException)
            {
                difValue = root.Childs[0];
            }

            var resultDisbalancedElement = GetDisbalancedElement(difValue);
            if (resultDisbalancedElement.IsFound)
            {
                if ((resultDisbalancedElement.RequiredValue.HasValue))
                {
                    return resultDisbalancedElement;
                }
                var balancedElement = root.Childs.FirstOrDefault(info =>
                    info.Name != resultDisbalancedElement.DisbalancedProgram.Name);
                resultDisbalancedElement.RequiredValue = balancedElement.Weight + balancedElement.TotalChildSize -
                                                         resultDisbalancedElement.DisbalancedProgram.TotalChildSize;
                return resultDisbalancedElement;
            }
            return resultDisbalancedElement;
        }
    }
}
