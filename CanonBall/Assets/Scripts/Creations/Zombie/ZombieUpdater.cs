using System.Collections.Generic;

namespace Assets.Scripts.Creations.Zombie
{
    public class ZombieUpdater : IUpdatable, ILateUpdatable
    {
        private readonly List<ZombieController> _controllers = new();

        private int _index;

        public void Update()
        {
            Research();

            foreach (var controller in _controllers)
            {
                controller.Update();
            }
        }

        public void LateUpdate()
        {
            foreach (var controller in _controllers)
            {
                controller.LateUpdate();
            }
        }

        public void Add(ZombieController zombieController)
        {
            _controllers.Add(zombieController);
        }

        public void Remove(ZombieController zombieController)
        {
            _controllers.Remove(zombieController);
        }

        private void Research()
        {
            int count = _controllers.Count;
            if (count == 0)
                return;

            if (_index >= count)
                _index = 0;

            var controller = _controllers[_index];
            controller.ResearchTarget();

            _index++;
        }
    }
}
