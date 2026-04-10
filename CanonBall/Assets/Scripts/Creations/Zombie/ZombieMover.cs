using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _model;
        [SerializeField] private Vector3 _targetPosition;

        private void Start()
        {
            _navMeshAgent.SetDestination(_targetPosition);
        }

        private void LateUpdate()
        {
            if (!_navMeshAgent.enabled) return;

            if (Physics.Raycast(_model.position + Vector3.up, Vector3.down, out var hit, 5f))
            {
                Vector3 pos = _model.position;
                pos.y = hit.point.y;

                _model.position = pos;
            }
        }
    }
}
