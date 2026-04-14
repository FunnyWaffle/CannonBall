using Assets.Scripts.Config;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector3 _targetPosition;

        private Transform _agentTransform;

        public Vector3 Position => _agentTransform.position;
        public bool IsAgentEnable => _agent.enabled;

        public void EnableAgent()
        {
            _agent.enabled = true;
            _animator.enabled = true;
        }

        public void DisableAgent()
        {
            if (!_agent.enabled)
                return;

            _agent.ResetPath();
            _agent.enabled = false;
            _animator.enabled = false;
        }

        private void Start()
        {
            _agentTransform = _agent.transform;

            _agent.SetDestination(_targetPosition);
            _agent.autoTraverseOffMeshLink = false;
        }

        private void Update()
        {
            if (_agent.isOnOffMeshLink)
            {
                CrossMeshLink();
                UpdateAnimations(_agent.speed, _agent.speed);
            }
            else
            {
                UpdateAnimations(_agent.velocity.z, _agent.desiredVelocity.z);
            }
        }

        private void CrossMeshLink()
        {
            var linkData = _agent.currentOffMeshLinkData;
            var linkEndPos = linkData.endPos;
            var direction = Vector3.Normalize(linkEndPos - linkData.startPos);

            _agent.Move(_agent.speed * Time.deltaTime * direction);

            if (Vector3.Distance(_agent.transform.position, linkEndPos) <= 0.1f)
                _agent.CompleteOffMeshLink();
        }

        private void UpdateAnimations(float currentForwardSpeed, float maxForwardSpeed)
        {
            _animator.SetFloat(ZombieAnimatorParameters.ForwardSpeed, currentForwardSpeed / maxForwardSpeed);
        }

        private void OnValidate()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
    }
}
