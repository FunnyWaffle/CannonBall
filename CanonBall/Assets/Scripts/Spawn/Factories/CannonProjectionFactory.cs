using Assets.Scripts.Combat;
using Assets.Scripts.Creations;
using Assets.Scripts.Guns.Projections;
using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class CannonProjectionFactory : IUniversalFactory
    {
        public ItemTypes CreationType => ItemTypes.CannonProjection;

        public EntityComponents Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var obj = GameObject.Instantiate(prefab, position, rotation, parent);
            var view = obj.GetComponent<CannonProjectionView>();

            var hitBox = new HitBox(view, view.AttackCorners, view.Colliders);
            var components = new EntityComponents();

            components.Add(view);
            components.Add(hitBox);

            return components;
        }
    }
}
