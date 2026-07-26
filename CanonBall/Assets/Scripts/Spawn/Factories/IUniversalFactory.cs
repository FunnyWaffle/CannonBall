using Assets.Scripts.Creations;
using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public interface IUniversalFactory
    {
        public ItemType CreationType { get; }

        public EntityComponents Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null);
    }
}
