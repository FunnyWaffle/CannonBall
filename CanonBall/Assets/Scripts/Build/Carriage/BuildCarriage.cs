using Assets.Scripts.Creations;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Build.Carriage
{
    public class BuildCarriage : MonoBehaviour, IPoolableObject, IDisabler, IEnableable, IMover, IUpdatable, IBuildCarriage
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _trunkExit;

        private bool _pavedPath;

        public float TrunkOffset => Vector3.Distance(transform.position, _trunkExit.position);
        public Vector3 TrunkExit => _trunkExit.position;
        public Vector3 TrunkExitForward => _trunkExit.forward;

        public event EventHandler<ItemTypes> Disabled;
        public event Action Arrived;

        public void SetDestination(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            Disabled?.Invoke(this, ItemTypes.BuildCarriage);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        void IUpdatable.Update()
        {
            _pavedPath = _navMeshAgent.hasPath;

            if (!_pavedPath)
                return;

            var target = _navMeshAgent.pathEndPosition;
            var distanceFromTarget = Vector3.Distance(target, transform.position);

            if (distanceFromTarget <= _navMeshAgent.radius)
            {
                _pavedPath = false;
                Arrived?.Invoke();
            }
        }
    }
}
