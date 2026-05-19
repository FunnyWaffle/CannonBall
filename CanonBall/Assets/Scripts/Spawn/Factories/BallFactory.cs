using Assets.Scripts.Explosion;
using Assets.Scripts.Guns.Projectile;
using Assets.Scripts.Shop;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class BallFactory : IFactory<IProjectile>
    {
        private readonly ExplosionHandler _explosionHandler;

        public BallFactory(ExplosionHandler explosionHandler)
        {
            _explosionHandler = explosionHandler;
        }

        public ItemTypes CreationType => ItemTypes.Ball;

        public IProjectile Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var instance = GameObject.Instantiate(prefab, position, rotation, parent);
            var ball = instance.GetComponent<Ball>();

            _explosionHandler.AddExplosionMaker(ball);
            return ball;
        }
    }
}
