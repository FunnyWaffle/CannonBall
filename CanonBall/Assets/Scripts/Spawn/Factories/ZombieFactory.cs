using Assets.Scripts.Creations.Zombie;
using Assets.Scripts.Explosion;
using Assets.Scripts.Shop;
using Assets.Scripts.Space;
using Assets.Scripts.Systems;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class ZombieFactory : IFactory<ZombieController>
    {
        private readonly ExplosionHandler _explosionHandler;
        private readonly SpatialGrid _spatialGrid;
        private readonly SpatialSearchShape _spatialSearchShape;
        private readonly Updater _updater;

        public ZombieFactory(
            ExplosionHandler explosionHandler,
            SpatialGrid spatialGrid,
            SpatialSearchShape spatialSearchShape,
            Updater updater)
        {
            _explosionHandler = explosionHandler;
            _spatialGrid = spatialGrid;
            _spatialSearchShape = spatialSearchShape;
            _updater = updater;
        }

        public ItemTypes CreationType => ItemTypes.Zombie;

        public ZombieController Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var view = GameObject.Instantiate(prefab, position, rotation, parent).GetComponent<ZombieView>();

            var targetSearch = new ZombieTargetSearch(_spatialGrid,
                _spatialSearchShape, 50f);
            var mover = new ZombieMover(view.Agent, view.Animator, targetSearch);
            var ragdoll = new ZombieRagdoll(view.Rigidbodies);
            var model = new ZombieModel(view.ModelTransform);
            var hitbox = new ZombieHitbox(view.Collider);

            _explosionHandler.AddExplosionReceiver(view.Collider, hitbox);

            var controller = new ZombieController(view, mover, ragdoll, model, hitbox);

            _updater.SetUpdatable(controller);
            _updater.SetLateUpdatable(controller);

            return controller;
        }
    }
}
