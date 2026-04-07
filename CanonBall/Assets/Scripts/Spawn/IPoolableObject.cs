using System;

namespace Assets.Scripts.Spawn
{
    public interface IPoolableObject
    {
        public event EventHandler ObjectLifeEnded;

        public void Activate();
    }
}
