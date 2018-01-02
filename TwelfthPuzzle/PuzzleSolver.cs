using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwelfthPuzzle
{
    public class PuzzleSolver
    { 
        private readonly IDictionary<string, IEnumerable<string>> _pipesDictionary;

        public PuzzleSolver(IEnumerable<string> pipes)
        {
            _pipesDictionary = new Dictionary<string, IEnumerable<string>>();
            foreach (var pipe in pipes)
            {
                var pcId = Regex.Match(pipe, "(.*)(?=( \\<))").Value;
                var nextPcIds = Regex.Matches(pipe, "((?<=(\\> |, ))(.*?)(?=(,|$)))+").OfType<Match>().Select(m => m.Value);
                _pipesDictionary.Add(pcId,nextPcIds);
            }
        }

        public int SolveFirst()
        {
            var idsSet = new HashSet<string>();
            GetConnectedPCs("0", idsSet);
            return idsSet.Count;
        }

        public int SolveSecond()
        {
            var idsList = new List<string>(_pipesDictionary.Keys);
            var numOfGroups = 0;
            while (idsList.Any())
            {
                numOfGroups++;
                RemoveConnectedPCs(idsList.First(), idsList);
            }

            return numOfGroups;
        }

        private void GetConnectedPCs(string pcId, ISet<string> idsSet)
        {
            if(!idsSet.Add(pcId))
                return;
            foreach (var nextPcId in _pipesDictionary[pcId])
            {
                GetConnectedPCs(nextPcId, idsSet);
            }
        }

        private void RemoveConnectedPCs(string pcId, IList<string> idsList)
        {
            if(!idsList.Remove(pcId))
                return;
            foreach (var nextPcId in _pipesDictionary[pcId])
            {
                RemoveConnectedPCs(nextPcId, idsList);
            }
        }
    }
}
