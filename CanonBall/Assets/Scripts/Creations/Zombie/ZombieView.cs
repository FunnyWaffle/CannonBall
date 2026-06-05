using Assets.Scripts.Combat;
using Assets.Scripts.Config;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieView : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;

        [SerializeField] private CapsuleCollider _collider;

        [SerializeField] private Rigidbody[] _rigidbodies;

        [SerializeField] private Transform _modelTransform;
        [SerializeField] private Transform _modelCenter;

        [SerializeField] private CollisionNotifier _rightHandCollisionNitifier;
        [SerializeField] private CollisionNotifier _leftHandCollisionNitifier;

        public NavMeshAgent Agent => _agent;
        public Animator Animator => _animator;

        public Transform Transform { get; private set; }
        public Transform ModelTransform => _modelTransform;

        public Rigidbody[] Rigidbodies => _rigidbodies;

        public Collider Collider => _collider;

        public Quaternion Rotation => _modelTransform.rotation;
        public Vector3 Position => _modelTransform.position;
        public Vector3 ModelCenterPosition => _modelCenter.position;
        public float AgentRadius => _agent.radius;
        public float RotationSpeed => _agent.angularSpeed;

        public CollisionNotifier RightHandCollisionNitifier => _rightHandCollisionNitifier;
        public CollisionNotifier LeftHandCollisionNitifier => _leftHandCollisionNitifier;

        private void Start()
        {
            Transform = _agent.transform;
        }

        public void Enable()
            => gameObject.SetActive(true);

        public void Place(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            transform.SetPositionAndRotation(position, rotation);
            transform.SetParent(parent);
        }

        public void Rotate(Quaternion rotation)
        {
            Transform.rotation = rotation;
        }

        public void EnableAttackAnimation()
        {
            _animator.SetTrigger(ZombieAnimatorParameters.Attack);
        }
    }
}
