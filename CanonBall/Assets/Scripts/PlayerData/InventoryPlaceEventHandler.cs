using Assets.Scripts.Placement;
using Assets.Scripts.Shop;

namespace Assets.Scripts.PlayerData
{
    public class InventoryPlaceEventHandler
    {
        private readonly InventoryController _inventoryController;
        private readonly PlaceObjectSystem _placeObjectSystem;

        public InventoryPlaceEventHandler(InventoryController inventoryController, PlaceObjectSystem placeObjectSystem)
        {
            _inventoryController = inventoryController;
            _placeObjectSystem = placeObjectSystem;

            _inventoryController.PlaceButtonPressed += OnPlaceButtonPressed;
        }

        private void OnPlaceButtonPressed(ItemType type)
        {
            _placeObjectSystem.ShowProjection(type);
        }
    }
}
