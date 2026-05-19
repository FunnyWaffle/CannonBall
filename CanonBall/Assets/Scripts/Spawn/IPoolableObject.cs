using Assets.Scripts.Shop;
using System;

namespace Assets.Scripts.Spawn
{
    public interface IPoolableObject
    {
        public event EventHandler<ItemTypes> Disabled;

        public void Enable();
    }
}
