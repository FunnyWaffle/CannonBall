using Assets.Scripts.Config;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _model;
        [SerializeField] private Vector3 _targetPosition;

        public void Stop()
        {
            _agent.ResetPath();
        }

        private void Start()
        {
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

        private void LateUpdate()
        {
            PlaceModelOnGround();
        }

        private void PlaceModelOnGround()
        {
            if (!_agent.enabled) return;

            if (Physics.Raycast(_model.position + Vector3.up, Vector3.down, out var hit, 5f))
            {
                Vector3 pos = _model.position;
                pos.y = hit.point.y;

                _model.position = pos;
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
    }
}
