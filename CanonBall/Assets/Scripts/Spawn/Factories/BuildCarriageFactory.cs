using Assets.Scripts.Build.Carriage;
using Assets.Scripts.Creations;
using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class BuildCarriageFactory : IUniversalFactory
    {
        public ItemTypes CreationType => ItemTypes.BuildCarriage;

        public EntityComponents Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var obj = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = obj.GetComponent<BuildCarriage>();

            var components = new EntityComponents();
            components.Add(view);

            return components;
        }
    }
}
