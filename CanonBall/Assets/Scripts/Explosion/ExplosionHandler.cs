using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Explosion
{
    public class ExplosionHandler
    {
        private readonly Dictionary<Collider, IExplosionReceiver> _receivers = new();
        private readonly List<IExplosionMaker> _explosionMakers = new();

        public event Action<Vector3> Exploded;

        public void AddExplosionMaker(IExplosionMaker explosionMaker)
        {
            if (_explosionMakers.Contains(explosionMaker))
                return;

            explosionMaker.ExplosionPerformed += CauseExplosion;
            _explosionMakers.Add(explosionMaker);
        }

        public void AddExplosionReceiver(Collider collider, IExplosionReceiver explosionReceiver)
        {
            _receivers[collider] = explosionReceiver;
        }

        private void CauseExplosion(float force, Vector3 position, float radius)
        {
            var colliders = Physics.OverlapSphere(position, radius, ~0, QueryTriggerInteraction.Collide);

            foreach (var collider in colliders)
            {
                if (!_receivers.TryGetValue(collider, out var receiver))
                    continue;

                receiver.OnExplosion(force, position, radius);
            }

            Exploded?.Invoke(position);
        }
    }
}
