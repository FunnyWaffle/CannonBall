using Assets.Scripts.Build;
using Assets.Scripts.Creations;
using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class CannonConstructionFactory : IUniversalFactory
    {
        public ItemType CreationType => ItemType.CannonToBuild;

        public EntityComponents Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var gameObject = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = gameObject.GetComponent<Construction>();

            var components = new EntityComponents();

            components.Add(view);

            return components;
        }
    }
}
