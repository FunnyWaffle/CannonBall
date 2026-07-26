using Assets.Scripts.Creations;
using Assets.Scripts.Navigation;
using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class NotForVehicleZoneFactiory : IUniversalFactory
    {
        public ItemType CreationType => ItemType.NotForVehicleZone;

        public EntityComponents Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var instance = GameObject.Instantiate(prefab, position, rotation, parent);
            var zone = instance.GetComponent<NotForVehicleZone>();

            var components = new EntityComponents();

            components.Add(zone);

            return components;
        }
    }
}
