using Assets.Scripts.Config;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieMover
    {
        private readonly NavMeshAgent _agent;
        private readonly Transform _agentTransform;
        private readonly Animator _animator;
        private readonly Vector3 _targetPosition;

        public ZombieMover(NavMeshAgent agent, Animator animator, Vector3 targetPosition)
        {
            _agent = agent;
            _animator = animator;
            _targetPosition = targetPosition;

            _agentTransform = _agent.transform;

            _agent.SetDestination(_targetPosition);
        }

        public Vector3 Position => _agentTransform.position;
        public bool IsAgentEnable => _agent.enabled;

        public void SetPosition(Vector3 position)
        {
            _agentTransform.position = position;
        }

        public void EnableAgent()
        {
            _agent.enabled = true;
            _animator.enabled = true;
            _agent.SetDestination(_targetPosition);
        }

        public void DisableAgent()
        {
            if (!_agent.enabled)
                return;

            _agent.ResetPath();
            _agent.enabled = false;
            _animator.enabled = false;
        }

        public void Update()
        {
            UpdateAnimations(_agent.velocity.z, _agent.desiredVelocity.z);
        }

        private void UpdateAnimations(float currentForwardSpeed, float maxForwardSpeed)
        {
            _animator.SetFloat(ZombieAnimatorParameters.ForwardSpeed, currentForwardSpeed / maxForwardSpeed);
        }
    }
}
