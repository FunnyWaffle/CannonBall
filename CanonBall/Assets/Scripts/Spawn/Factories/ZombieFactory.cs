using Assets.Scripts.Combat;
using Assets.Scripts.Creations.Zombie;
using Assets.Scripts.Explosion;
using Assets.Scripts.Shop;
using Assets.Scripts.Space;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class ZombieFactory : IFactory<ZombieController>
    {
        private readonly ExplosionHandler _explosionHandler;
        private readonly SpatialGrid _spatialGrid;
        private readonly SpatialSearchShape _spatialSearchShape;
        private readonly DamageSystem _damageSystem;
        private readonly SpatialObjectsMap _spatialObjectsMap;
        private readonly ZombieUpdater _updater;

        public ZombieFactory(
            ExplosionHandler explosionHandler,
            SpatialGrid spatialGrid,
            SpatialSearchShape spatialSearchShape,
            DamageSystem damageSystem,
            SpatialObjectsMap spatialObjectsMap,
            ZombieUpdater updater)
        {
            _explosionHandler = explosionHandler;
            _spatialGrid = spatialGrid;
            _spatialSearchShape = spatialSearchShape;
            _damageSystem = damageSystem;
            _spatialObjectsMap = spatialObjectsMap;
            _updater = updater;
        }

        public ItemTypes CreationType => ItemTypes.Zombie;

        public ZombieController Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var view = GameObject.Instantiate(prefab, position, rotation, parent).GetComponent<ZombieView>();

            var target = new ZombieTarget();
            var targetSearch = new ZombieTargetSearch(target, _spatialGrid,
                _spatialSearchShape, _spatialObjectsMap, 50f);
            var mover = new ZombieMover(view.Agent, view.Animator, target);
            var ragdoll = new ZombieRagdoll(view.Rigidbodies);
            var model = new ZombieModel(view.ModelTransform);
            var hitbox = new ZombieHitbox(view.Collider);
            var attacker = new ZombieAttacker(target, _damageSystem);

            _explosionHandler.AddExplosionReceiver(view.Collider, hitbox);

            var controller = new ZombieController(view, mover, ragdoll,
                model, hitbox, attacker, targetSearch);

            _updater.Add(controller);

            return controller;
        }
    }
}
