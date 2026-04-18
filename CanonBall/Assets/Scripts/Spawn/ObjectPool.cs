using System;
using System.Collections.Generic;

namespace Assets.Scripts.Spawn
{
    public class ObjectPool
    {
        private readonly Dictionary<Type, Queue<IPoolableObject>> _objects = new();

        public void Register<T>(T obj)
        where T : class, IPoolableObject
        {
            obj.Disabled += OnObjectDisable<T>;
        }

        public bool TryGet<T>(out T obj)
        where T : class, IPoolableObject
        {
            var type = typeof(T);
            if (!_objects.TryGetValue(type, out var queue))
            {
                queue = new Queue<IPoolableObject>();
                _objects[type] = queue;
            }

            if (queue.Count == 0)
            {
                obj = default;
                return false;
            }

            obj = (T)queue.Dequeue();
            Register(obj);
            return true;
        }

        private void OnObjectDisable<T>(object obj, EventArgs e)
                    where T : class, IPoolableObject
        {
            var type = typeof(T);
            if (!_objects.TryGetValue(type, out var queue))
            {
                queue = new Queue<IPoolableObject>();
                _objects[type] = queue;
            }

            var typedObject = obj as T;
            queue.Enqueue(typedObject);
            typedObject.Disabled -= OnObjectDisable<T>;
        }
    }
}
