using Assets.Scripts.Guns.Projections;
using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class CannonProjectionFactory : IFactory<CannonProjection>
    {
        public ItemTypes CreationType => ItemTypes.CannonProjection;

        public CannonProjection Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var obj = GameObject.Instantiate(prefab, position, rotation, parent);
            return obj.GetComponent<CannonProjection>();
        }
    }
}
