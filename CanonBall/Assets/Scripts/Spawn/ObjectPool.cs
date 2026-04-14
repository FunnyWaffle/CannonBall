using System;
using System.Collections.Generic;

namespace Assets.Scripts.Spawn
{
    public class ObjectPool
    {
        private readonly Dictionary<Type, Queue<IPoolableObject>> _objects = new();

        public void Register<T>(T @object)
        where T : class, IPoolableObject
        {
            @object.Disabled += OnObjectDisable<T>;
        }

        public bool TryGet<T>(out T @object)
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
                @object = default;
                return false;
            }

            @object = (T)queue.Dequeue();
            Register(@object);
            return true;
        }

        private void OnObjectDisable<T>(object @object, EventArgs e)
                    where T : class, IPoolableObject
        {
            var type = typeof(T);
            if (!_objects.TryGetValue(type, out var queue))
            {
                queue = new Queue<IPoolableObject>();
                _objects[type] = queue;
            }

            var typedObject = @object as T;
            queue.Enqueue(typedObject);
            typedObject.Disabled -= OnObjectDisable<T>;
        }
    }
}
