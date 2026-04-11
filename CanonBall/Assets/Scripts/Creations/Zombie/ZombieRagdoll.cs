using UnityEngine;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieRagdoll : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] _rigidbodies;

        private void Awake()
        {
            DisableRagdoll();
        }

        public void EnableRagdoll()
        {
            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = false;
            }
        }

        public void DisableRagdoll()
        {
            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = true;
            }
        }
    }
}
