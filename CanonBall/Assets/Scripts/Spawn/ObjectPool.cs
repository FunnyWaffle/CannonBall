using Assets.Scripts.Shop;
using System.Collections.Generic;

namespace Assets.Scripts.Spawn
{
    public class ObjectPool<T>
                where T : IPoolableObject
    {
        private readonly Dictionary<ItemTypes, Queue<T>> _objects = new();

        public void Register(T obj)
        {
            obj.Disabled += OnObjectDisable;
        }

        public bool TryGet(ItemTypes itemType, out T obj)
        {
            if (!_objects.TryGetValue(itemType, out var queue))
            {
                queue = new Queue<T>();
                _objects[itemType] = queue;
            }

            if (queue.Count == 0)
            {
                obj = default;
                return false;
            }

            obj = queue.Dequeue();
            Register(obj);
            obj.Enable();
            return true;
        }

        private void OnObjectDisable(object obj, ItemTypes itemType)
        {
            if (!_objects.TryGetValue(itemType, out var queue))
            {
                queue = new Queue<T>();
                _objects[itemType] = queue;
            }

            var typedObject = (T)obj;
            queue.Enqueue(typedObject);
            typedObject.Disabled -= OnObjectDisable;
        }
    }
}
