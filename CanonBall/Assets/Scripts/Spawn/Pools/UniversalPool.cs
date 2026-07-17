using Assets.Scripts.Creations;
using Assets.Scripts.Shop;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Spawn.Pools
{
    public class UniversalPool
    {
        private readonly Dictionary<ItemTypes, Queue<EntityComponents>> _disabledObjects = new();
        private readonly Dictionary<IPoolableObject, EntityComponents> _registeredObjects = new();

        public bool TryRegister(EntityComponents components)
        {
            if (!components.TryGet<IPoolableObject>(out var poolableObject))
                return false;

            _registeredObjects[poolableObject] = components;

            poolableObject.Disabled += OnObjectDisable;
            return true;
        }

        public bool TryGet(ItemTypes itemType, out EntityComponents components)
        {
            if (!_disabledObjects.TryGetValue(itemType, out var queue))
            {
                queue = new Queue<EntityComponents>();
                _disabledObjects[itemType] = queue;
            }

            if (queue.Count == 0)
            {
                components = default;
                return false;
            }

            components = queue.Dequeue();
            TryRegister(components);

            if (components.TryGet<IEnableable>(out var enableable))
                enableable.Enable();
            else
                Debug.Log($"Object {itemType} doesn't have IEnableable component, so in wont show up.");

            return true;
        }

        private void OnObjectDisable(object obj, ItemTypes itemType)
        {
            if (!_disabledObjects.TryGetValue(itemType, out var queue))
            {
                queue = new Queue<EntityComponents>();
                _disabledObjects[itemType] = queue;
            }

            var poolable = (IPoolableObject)obj;
            var components = _registeredObjects[poolable];
            _registeredObjects.Remove(poolable);

            queue.Enqueue(components);
            poolable.Disabled -= OnObjectDisable;
        }
    }
}
