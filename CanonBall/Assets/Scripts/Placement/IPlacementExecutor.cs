using Assets.Scripts.Shop;
using System;

namespace Assets.Scripts.Placement
{
    public interface IPlacementExecutor
    {
        public event Action<ItemTypes> PlacementStarted;
    }
}
