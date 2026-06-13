using Assets.Scripts.Config;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieMover
    {
        private readonly NavMeshAgent _agent;
        private readonly Transform _agentTransform;
        private readonly Animator _animator;
        private readonly ZombieTarget _zombieTarget;

        private bool _hadPath = false;

        public ZombieMover(
            NavMeshAgent agent,
            Animator animator,
            ZombieTarget zombieTarget)
        {
            _agent = agent;
            _animator = animator;
            _zombieTarget = zombieTarget;
            _agentTransform = _agent.transform;
        }

        public event Action PathCompleted;

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
        }

        public void DisableAgent()
        {
            if (!_agent.enabled)
                return;

            _agent.ResetPath();
            _agent.enabled = false;
            _animator.enabled = false;
        }

        public void UpdatePath()
        {
            if (!_agent.enabled)
                return;

            StartMovement();

            if (_agent.pathPending)
                return;

            if (_agent.hasPath)
            {
                _hadPath = true;
                return;
            }

            if (_hadPath)
            {
                _hadPath = false;
                PathCompleted?.Invoke();
            }
        }

        public void UpdateMovementAnimation()
        {
            UpdateAnimations(_agent.velocity.z, _agent.desiredVelocity.z);
        }

        private void StartMovement()
        {
            if (!_zombieTarget.IsSuitable)
                return;

            var targetPosition = _zombieTarget.AttackPosition;
            _agent.SetDestination(targetPosition);
        }

        private void UpdateAnimations(float currentForwardSpeed, float maxForwardSpeed)
        {
            _animator.SetFloat(ZombieAnimatorParameters.ForwardSpeed,
                currentForwardSpeed / maxForwardSpeed);
        }
    }
}
