using Assets.Scripts.Input;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interaction
{
    public class InteractionObjectsRepositiory
    {
        private readonly Dictionary<Collider, IGameplayController> _cannons = new();

        public void AddCannon(Collider collider, IGameplayController cannon)
        {
            _cannons[collider] = cannon;
        }

        public bool TryGetCannon(Collider collider, out IGameplayController cannon)
        {
            return _cannons.TryGetValue(collider, out cannon);
        }
    }
}
