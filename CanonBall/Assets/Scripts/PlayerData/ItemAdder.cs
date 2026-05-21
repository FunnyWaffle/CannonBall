using Assets.Scripts.Placement;
using Assets.Scripts.Shop;
using System.Collections.Generic;

namespace Assets.Scripts.PlayerData
{
    public class ItemAdder
    {
        private readonly PlaceObjectSystem _placeObjectSystem;
        private readonly List<IItemStorage> _storages = new();

        public ItemAdder(PlaceObjectSystem placeObjectSystem, params IItemStorage[] storages)
        {
            _placeObjectSystem = placeObjectSystem;

            foreach (var storage in storages)
            {
                _placeObjectSystem.ObjectPlaced += storage.RemoveItem;
            }
        }
    }
}
