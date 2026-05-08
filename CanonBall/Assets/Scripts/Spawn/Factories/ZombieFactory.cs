using Assets.Scripts.Creations.Zombie;
using UnityEngine;

namespace Assets.Scripts.Spawn.Factories
{
    public class ZombieFactory : IFactory<ZombieController>
    {
        public ZombieController Create(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var view = GameObject.Instantiate(prefab, position, rotation, parent).GetComponent<ZombieView>();

            var mover = new ZombieMover(view.Agent, view.Animator, view.TargetPosition);
            var ragdoll = new ZombieRagdoll(view.Rigidbodies);
            var model = new ZombieModel(view.ModelTransform);
            var hitbox = new ZombieHitbox(view.Collider);

            var controller = new ZombieController(view, mover, ragdoll, model, hitbox);
            return controller;
        }
    }
}
