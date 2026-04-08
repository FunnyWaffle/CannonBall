using System;
using System.Collections.Generic;

namespace Assets.Scripts.Spawn
{
    public class ObjectPool<T>
        where T : class, IPoolableObject
    {
        private readonly Queue<T> _objects = new();

        public void Register(T @object)
        {
            @object.ObjectLifeEnded += HandleObjectLifeEnd;
        }

        public bool TryGet(out T @object)
        {
            if (_objects.Count == 0)
            {
                @object = default;
                return false;
            }

            @object = _objects.Dequeue();
            Register(@object);
            return true;
        }

        private void HandleObjectLifeEnd(object @object, EventArgs e)
        {
            var typedObject = @object as T;
            _objects.Enqueue(typedObject);
            typedObject.ObjectLifeEnded -= HandleObjectLifeEnd;
        }
    }
}
