using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieView : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector3 _targetPosition;

        [SerializeField] private CapsuleCollider _collider;

        [SerializeField] private Rigidbody[] _rigidbodies;

        [SerializeField] private Transform _modelTransform;

        public NavMeshAgent Agent => _agent;
        public Animator Animator => _animator;
        public Vector3 TargetPosition => _targetPosition;

        public Transform ModelTransform => _modelTransform;

        public Rigidbody[] Rigidbodies => _rigidbodies;

        public Collider Collider => Collider;

        public void Enable()
            => gameObject.SetActive(true);

        public void Place(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            transform.SetPositionAndRotation(position, rotation);
            transform.SetParent(parent);
        }
    }
}
