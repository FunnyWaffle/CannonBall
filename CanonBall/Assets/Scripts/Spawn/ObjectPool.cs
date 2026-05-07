using Assets.Scripts.Spawn.Factories;
using System;
using System.Collections.Generic;
using UnityEngine;

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
            obj.Enable();
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

    public class ObjectPool<T>
        where T : class, IPoolableObject
    {
        private readonly IFactory<T> _factory;
        private readonly Queue<T> _objects = new();

        public ObjectPool(IFactory<T> factory)
        {
            _factory = factory;
        }

        public void Register(T obj)
        {
            obj.Disabled += OnObjectDisable;
        }

        public T Get(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_objects.Count == 0)
                return _factory.Create(prefab, position, rotation, parent);

            var obj = _objects.Dequeue();
            Register(obj);
            obj.Enable();
            return obj;
        }

        private void OnObjectDisable(object obj, EventArgs e)
        {
            var typedObject = obj as T;
            _objects.Enqueue(typedObject);
            typedObject.Disabled -= OnObjectDisable;
        }
    }
}
