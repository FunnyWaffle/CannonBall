using Assets.Scripts.Creations.Placement;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creations.Builder.Components
{
    public class BuilderMover : IMover
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly IHasPosition _hasPosition;

        private bool _pavedPath;

        public BuilderMover(NavMeshAgent navMeshAgent, IHasPosition hasPosition)
        {
            _navMeshAgent = navMeshAgent;
            _hasPosition = hasPosition;
        }

        public event Action Arrived;

        public void SetDestination(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
        }

        public void Update()
        {
            _pavedPath = _navMeshAgent.hasPath;

            if (!_pavedPath)
                return;

            var target = _navMeshAgent.pathEndPosition;
            var distanceFromTarget = Vector3.Distance(target, _hasPosition.Position);

            if (distanceFromTarget <= _navMeshAgent.radius)
            {
                _pavedPath = false;
                Arrived?.Invoke();
            }
        }
    }
}
