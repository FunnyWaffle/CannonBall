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

        private bool _isMoving = false;
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

        public void StartMovement()
        {
            if (!_agent.enabled)
                return;

            if (_zombieTarget.Target == null)
                return;

            if (_agent.hasPath ||
                !_zombieTarget.HasMoved)
                return;

            var targetPosition = _zombieTarget.TargetPosition;
            _agent.SetDestination(targetPosition);
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

        private GameObject _reserved = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        private GameObject _target = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        public void UpdatePath()
        {
            if (!_agent.enabled)
                return;

            if (_agent.pathPending)
                return;

            if (_agent.hasPath)
            {
                _hadPath = true;
                Debug.Log(Vector3.Distance(_zombieTarget.TargetPosition, _agent.destination));

                var renderer = _reserved.GetComponent<MeshRenderer>();
                renderer.material.color = Color.green;
                _reserved.transform.localScale = Vector3.one * _agent.radius;
                _reserved.transform.position = _agent.destination;

                var renderer1 = _target.GetComponent<MeshRenderer>();
                renderer1.material.color = Color.red;
                _target.transform.localScale = Vector3.one * _agent.radius;
                Vector3 targetPosition = _zombieTarget.TargetPosition;
                targetPosition.y = _agent.destination.y;
                _target.transform.position = targetPosition;
                return;
            }

            if (_hadPath)
            {
                _hadPath = false;
                _isMoving = false;
                PathCompleted?.Invoke();
            }
        }

        public void UpdateMovementAnimation()
        {
            UpdateAnimations(_agent.velocity.z, _agent.desiredVelocity.z);
        }

        private void UpdateAnimations(float currentForwardSpeed, float maxForwardSpeed)
        {
            _animator.SetFloat(ZombieAnimatorParameters.ForwardSpeed,
                currentForwardSpeed / maxForwardSpeed);
        }
    }
}
