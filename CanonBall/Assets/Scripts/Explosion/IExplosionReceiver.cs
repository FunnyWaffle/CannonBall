using UnityEngine;

namespace Assets.Scripts.Explosion
{
    public interface IExplosionReceiver
    {
        public void OnExplosion(float force, Vector3 position, float radius);
    }
}
