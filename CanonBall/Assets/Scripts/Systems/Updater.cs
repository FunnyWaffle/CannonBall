using Assets.Scripts.Creations;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Systems
{
    public class Updater : MonoBehaviour
    {
        private List<IUpdatable> _updatables = new();
        private List<IFixedUpdatable> _fixedUpdatables = new();
        private List<ILateUpdatable> _lateUpdatables = new();

        [Inject]
        public void SetUpdatables(List<IUpdatable> updatables)
        {
            _updatables = new(updatables);
        }

        public void SetUpdatable(IUpdatable updatable)
        {
            _updatables.Add(updatable);
        }

        [Inject]
        public void SetFixedUpdatables(List<IFixedUpdatable> updatables)
        {
            _fixedUpdatables = new(updatables);
        }

        public void SetFixedUpdatable(IFixedUpdatable updatable)
        {
            _fixedUpdatables.Add(updatable);
        }

        [Inject]
        public void SetLateUpdatables(List<ILateUpdatable> updatables)
        {
            _lateUpdatables = new(updatables);
        }

        public void SetLateUpdatable(ILateUpdatable updatable)
        {
            _lateUpdatables.Add(updatable);
        }

        private void Update()
        {
            foreach (var updatable in _updatables)
            {
                updatable.Update();
            }
        }

        private void FixedUpdate()
        {
            foreach (var updatable in _fixedUpdatables)
            {
                updatable.FixedUpdate();
            }
        }

        private void LateUpdate()
        {
            foreach (var updatable in _lateUpdatables)
            {
                updatable.LateUpdate();
            }
        }
    }
}
