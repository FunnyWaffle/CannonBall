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
        private readonly ZombieTargetSearch _zombieTargetSearch;

        private readonly float _findTargetDelay = 1f;
        private float _findTargetTimer;

        private Vector3 _target;

        public ZombieMover(
            NavMeshAgent agent,
            Animator animator,
            ZombieTargetSearch zombieTargetSearch)
        {
            _agent = agent;
            _animator = animator;
            _zombieTargetSearch = zombieTargetSearch;
            _agentTransform = _agent.transform;

            FindTarget();
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
            FindTarget();
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
            _findTargetTimer += Time.deltaTime;

            if (_findTargetTimer >= _findTargetDelay)
            {
                _findTargetTimer -= _findTargetDelay;
                FindTarget();
            }

            UpdateAnimations(_agent.velocity.z, _agent.desiredVelocity.z);
        }

        private void UpdateAnimations(float currentForwardSpeed, float maxForwardSpeed)
        {
            _animator.SetFloat(ZombieAnimatorParameters.ForwardSpeed,
                currentForwardSpeed / maxForwardSpeed);
        }

        private void FindTarget()
        {
            if (_zombieTargetSearch.HasPossibleTargets &&
                _zombieTargetSearch.TryGetTarget(Position, out var target))
            {
                if (_target == target)
                    return;

                _target = target;
                _agent.SetDestination(target);
            }
        }
    }
}
