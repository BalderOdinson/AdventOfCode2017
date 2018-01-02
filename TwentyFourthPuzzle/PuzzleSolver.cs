using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyFourthPuzzle
{
    public class PuzzleSolver
    {
        private readonly IEnumerable<Component> _components;

        public PuzzleSolver(IEnumerable<string> input)
        {
            _components = input.Select(line => new Component(line.Split('/').Select(int.Parse))).ToList();
        }

        public int SolveFirst()
        {
            var components = _components.ToList();
            var bridge = GetStorngestBridge(components);
            Console.WriteLine(bridge.Result);
            return bridge.Result.TotalStrength;
        }

        public int SolveSecond()
        {
            var components = _components.ToList();
            var bridge = GetLongestBridge(components);
            Console.WriteLine(bridge.Result);
            return bridge.Result.TotalStrength;
        }


        private async Task<Bridge> GetStorngestBridge(IEnumerable<Component> components)
        {
            IEnumerable<Component> strongestBridge = new List<Component>();
            var nextLists = new List<Task<IEnumerable<Component>>>();
            foreach (var component in components)
            {
                var bridge = new Bridge();
                if (bridge.TryAddComponent(component))
                {
                    nextLists.Add(Task.Run(async () => await _getStrongestBridgeAsync(components.Where(c => !c.Equals(component)), bridge)));
                }
            }

            foreach (var nextList in nextLists)
            {
                if ((await nextList).Sum(c => c.Strength) > strongestBridge.Sum(c => c.Strength))
                    strongestBridge = await nextList;
            }

            return new Bridge(strongestBridge.Reverse());
        }

        private static async Task<IEnumerable<Component>> _getStrongestBridgeAsync(IEnumerable<Component> components, Bridge bridge)
        {
            var strongestBridge = new List<Component>();
            var nextLists = new List<Task<IEnumerable<Component>>>();
            foreach (var component in components)
            {
                var newBridge = new Bridge(bridge);
                if (newBridge.TryAddComponent(component))
                {
                    nextLists.Add(Task.Run(() => _getStrongestBridge(components.Where(c => !c.Equals(component)).ToList(), newBridge)));
                }
            }
            foreach (var nextList in nextLists)
            {
                if ((await nextList).Sum(c => c.Strength) > strongestBridge.Sum(c => c.Strength))
                    strongestBridge = (await nextList).ToList();
            }
            strongestBridge.Add(bridge.RemoveComponent());
            return strongestBridge;
        }

        private static IEnumerable<Component> _getStrongestBridge(IEnumerable<Component> components, Bridge bridge)
        {
            var strongestBridge = new List<Component>();
            foreach (var component in components)
            {
                if (bridge.TryAddComponent(component))
                {
                    var nextList = _getStrongestBridge(components.Where(c => !c.Equals(component)), bridge).ToList();
                    if (strongestBridge.Sum(c => c.Strength) < nextList.Sum(c => c.Strength))
                    {
                        strongestBridge = nextList;
                    }
                }
            }
            strongestBridge.Add(bridge.RemoveComponent());
            return strongestBridge;
        }

        private async Task<Bridge> GetLongestBridge(IEnumerable<Component> components)
        {
            IEnumerable<Component> longestBridge = new List<Component>();
            var nextLists = new List<Task<IEnumerable<Component>>>();
            foreach (var component in components)
            {
                var bridge = new Bridge();
                if (bridge.TryAddComponent(component))
                {
                    nextLists.Add(Task.Run(async () => await _getLongestBridgeAsync(components.Where(c => !c.Equals(component)), bridge)));
                }
            }

            foreach (var nextList in nextLists)
            {
                if ((await nextList).Count() > longestBridge.Count())
                    longestBridge = await nextList;
                else if ((await nextList).Count() == longestBridge.Count() &&
                         (await nextList).Sum(c => c.Strength) > longestBridge.Sum(c => c.Strength))
                {
                    longestBridge = await nextList;
                }
            }

            return new Bridge(longestBridge.Reverse());
        }

        private static async Task<IEnumerable<Component>> _getLongestBridgeAsync(IEnumerable<Component> components, Bridge bridge)
        {
            var longestBridge = new List<Component>();
            var nextLists = new List<Task<IEnumerable<Component>>>();
            foreach (var component in components)
            {
                var newBridge = new Bridge(bridge);
                if (newBridge.TryAddComponent(component))
                {
                    nextLists.Add(Task.Run(() => _getLongestBridge(components.Where(c => !c.Equals(component)).ToList(), newBridge)));
                }
            }
            foreach (var nextList in nextLists)
            {
                if ((await nextList).Count() > longestBridge.Count)
                    longestBridge = (await nextList).ToList();
                else if((await nextList).Count() == longestBridge.Count &&
                        (await nextList).Sum(c => c.Strength) > longestBridge.Sum(c => c.Strength))
                {
                    longestBridge = (await nextList).ToList();
                }
            }
            longestBridge.Add(bridge.RemoveComponent());
            return longestBridge;
        }

        private static IEnumerable<Component> _getLongestBridge(IEnumerable<Component> components, Bridge bridge)
        {
            var longestBridge = new List<Component>();
            foreach (var component in components)
            {
                if (bridge.TryAddComponent(component))
                {
                    var nextList = _getLongestBridge(components.Where(c => !c.Equals(component)), bridge).ToList();
                    if (longestBridge.Count < nextList.Count)
                    {
                        longestBridge = nextList;
                    }
                    else if(longestBridge.Count == nextList.Count && 
                            longestBridge.Sum(c => c.Strength) < nextList.Sum(c => c.Strength))
                    {
                        longestBridge = nextList;
                    }
                }
            }
            longestBridge.Add(bridge.RemoveComponent());
            return longestBridge;
        }
    }
}
