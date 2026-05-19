using Assets.Scripts.Spawn;
using UnityEngine;

namespace Assets.Scripts.Guns.Projectile
{
    public interface IProjectile : ISpawnable, IPoolableObject
    {
        public void SetForce(float force);
        public void SetIgnoredCollider(Collider collider);
    }
}
