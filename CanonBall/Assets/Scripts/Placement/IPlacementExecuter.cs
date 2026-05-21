using Assets.Scripts.Shop;
using System;

namespace Assets.Scripts.Placement
{
    public interface IPlacementExecuter
    {
        public event Action<ItemTypes> PlacementStarted;
    }
}
