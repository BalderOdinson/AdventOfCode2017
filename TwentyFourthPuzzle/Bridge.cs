using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwentyFourthPuzzle
{
    public class Bridge
    {
        private readonly Stack<Component> _components;

        public int TotalStrength => _components.Sum(c => c.Strength);

        public int RequiredConnector { get; private set; }

        public Bridge(Component firstComponent)
        {
            _components = new Stack<Component>();
            if (firstComponent.Ends.Contains(0))
            {
                _components = new Stack<Component>();
                _components.Push(firstComponent);
                RequiredConnector = firstComponent.Ends.FirstOrDefault(c => c != 0);
            }
        }

        public Bridge(IEnumerable<Component> components)
        {
            _components = new Stack<Component>();
            if (components.Any(component => !TryAddComponent(component)))
            {
                throw new ArgumentException("Components not sorted properly.");
            }
        }

        public Bridge(Bridge otherBridge)
        {
            _components = new Stack<Component>(otherBridge._components.Reverse());
            RequiredConnector = otherBridge.RequiredConnector;
        }

        public Bridge()
        {
            _components = new Stack<Component>();
        }

        public bool TryAddComponent(Component component)
        {
            if (_components.Count == 0)
            {
                if (component.Ends.Contains(0))
                {
                    _components.Push(component);
                    RequiredConnector = component.Ends.All(c => c == RequiredConnector) ? RequiredConnector :
                        component.Ends.FirstOrDefault(c => c != RequiredConnector);
                    return true;
                }
                else
                    return false;
            }

            if (!component.Ends.Contains(RequiredConnector)) return false;
            _components.Push(component);
            RequiredConnector = component.Ends.All(c => c == RequiredConnector) ? RequiredConnector :
                component.Ends.FirstOrDefault(c => c != RequiredConnector);
            return true;
        }

        public Component RemoveComponent()
        {            
            if(_components.Count == 0)
                throw new InvalidOperationException("The Bridge is empty.");
            var comp = _components.Pop();
            RequiredConnector = comp.Ends.All(c => c == RequiredConnector) ? RequiredConnector :
                comp.Ends.FirstOrDefault(c => c != RequiredConnector);
            return comp;
        }

        public IEnumerable<Component> Components => _components;


        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var component in _components)
            {
                sb.Append(component);
                sb.Append("--");
            }
            return sb.ToString().Substring(0,sb.Length - 2);
        }
    }
}
