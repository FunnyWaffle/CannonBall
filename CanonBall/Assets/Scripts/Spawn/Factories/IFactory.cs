using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public interface IFactory<T>
    {
        public ItemType CreationType { get; }

        public T Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null);
    }
}
