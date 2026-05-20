using Assets.Scripts.GameStateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interaction
{
    public class CannonColliderMap
    {
        private readonly Dictionary<Collider, ICannonController> _controllers = new();

        public void Register(Collider collider, ICannonController cannonController)
        {
            _controllers[collider] = cannonController;
        }
        public void Register(IEnumerable<Collider> colliders, ICannonController cannonController)
        {
            foreach (var collider in colliders)
            {
                Register(collider, cannonController);
            }
        }

        public bool TryGet(Collider collider, out ICannonController cannonController) =>
            _controllers.TryGetValue(collider, out cannonController);
    }
}
