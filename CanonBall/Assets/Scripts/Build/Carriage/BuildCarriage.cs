using Assets.Scripts.Creations;
using Assets.Scripts.Creations.Placement;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Build.Carriage
{
    public class BuildCarriage : MonoBehaviour, IPoolableObject, IDisabler, IEnableable, IMover, IRotateable, IUpdatable, IBuildCarriage
    {
        [SerializeField] private Transform _trunkExit;
        [SerializeField] private Vector3 _size;

        [Inject] private CustomPathFinder _customPathFinder;

        private readonly List<Vector3> _path = new();

        private readonly float _movementSpeed = 1f;

        private bool _hasPath;
        private int _currentCornerIndex = 0;

        public float TrunkOffset => Vector3.Distance(transform.position, _trunkExit.position);
        public Vector3 TrunkExit => _trunkExit.position;
        public Vector3 TrunkExitForward => _trunkExit.forward;

        public event EventHandler<ItemType> Disabled;
        public event Action Arrived;

        public async void SetDestination(Vector3 destination)
        {
            var found = await _customPathFinder.TryFindAlloc(transform.position, _size / 2, destination, 0.2f, _path);

            _hasPath = found;
        }

        public void Rotate(Quaternion rotation)
        {

        }

        public void Disable()
        {
            gameObject.SetActive(false);
            Disabled?.Invoke(this, ItemType.BuildCarriage);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        void IUpdatable.Update()
        {
            if (!_hasPath)
                return;

            Move();

            var target = _path[^1];
            if (IsReached(target))
            {
                _currentCornerIndex = 0;
                _hasPath = false;
                Arrived?.Invoke();
            }
        }

        private void Move()
        {
            var currentCorner = _path[_currentCornerIndex];
            transform.position = Vector3.MoveTowards(transform.position, currentCorner, _movementSpeed * Time.deltaTime);

            if (IsReached(currentCorner))
                _currentCornerIndex++;
        }

        private bool IsReached(Vector3 position)
        {
            var distanceFromTarget = Vector3.Distance(position, transform.position);

            if (distanceFromTarget <= _size.z / 2)
                return true;

            return false;
        }
    }
}
