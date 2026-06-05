using System;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class CollisionNotifier : MonoBehaviour
    {
        public event Action<Collider> TriggerEntered;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEntered?.Invoke(other);
        }
    }
}
