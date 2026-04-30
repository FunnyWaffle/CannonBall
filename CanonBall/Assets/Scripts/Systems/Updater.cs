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
        public void SetUpdatables(List<IFixedUpdatable> updatables)
        {
            _fixedUpdatables = new(updatables);
        }

        public void SetUpdatable(IFixedUpdatable updatable)
        {
            _fixedUpdatables.Add(updatable);
        }

        private void Update()
        {
            foreach (var updatable in _updatables)
            {
                updatable.Update();
            }
        }

        private void LateUpdate()
        {
            foreach (var updatable in _fixedUpdatables)
            {
                updatable.FixedUpdate();
            }
        }
    }
}
