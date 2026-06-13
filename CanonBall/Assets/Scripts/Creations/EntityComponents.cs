using System;
using System.Collections.Generic;

namespace Assets.Scripts.Creations
{
    public class EntityComponents
    {
        private readonly Dictionary<Type, IComponent> _components = new();

        public void Add<T>(T component)
            where T : IComponent
        {
            var type = component.GetType();
            var interfaces = type.GetInterfaces();

            if (interfaces.Length > 1)
                foreach (var @interface in interfaces)
                {
                    if (@interface == typeof(IComponent))
                        continue;

                    if (typeof(IComponent).IsAssignableFrom(@interface))
                        _components[@interface] = component;
                }
            else
                _components[type] = component;
        }

        public void Remove<T>()
            where T : IComponent
        {
            _components.Remove(typeof(T));
        }

        public T Get<T>()
            where T : IComponent
        {
            return (T)_components[typeof(T)];
        }

        public bool TryGet<T>(out T component)
            where T : IComponent
        {
            var type = typeof(T);
            if (_components.TryGetValue(type, out var value))
            {
                component = (T)value;
                return true;
            }

            component = default;
            return false;
        }
    }

    public interface IComponent { }
}
