using Assets.Scripts.Curency;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
using ObservableCollections;
using R3;
using UnityEngine;

namespace Assets.Scripts.PlayerData
{
    public class InventoryController : IUIWindow, ICurrencyReceiver<int>
    {
        private readonly Inventory _core;
        private readonly InventoryView _view;

        private readonly PlaceObjectSystem _placeObjectSystem;
        private readonly Shop.Shop _shop;
        private readonly AssetLoader _assetLoader;

        public InventoryController(InventoryView inventoryView,
            Shop.Shop shop,
            PlaceObjectSystem placeObjectSystem,
            AssetLoader assetLoader)
        {
            _view = inventoryView;
            _core = new Inventory();
            Initialize();

            _shop = shop;
            _placeObjectSystem = placeObjectSystem;
            _assetLoader = assetLoader;

            _core.MoneyCountChanged += _view.SetMoneyValue;
            _shop.PurchaseCompleted += _core.AddItems;
        }

        public UIWindowTypes Type => UIWindowTypes.Inventory;

        public void Open()
        {
            _view.Open();
        }

        public void Close()
        {
            _view.Close();
        }

        private void Initialize()
        {
            var slotViews = _core.Items.CreateView(item =>
            {
                var inventorySlot = GameObject.Instantiate(_view.SlotPrefab, _view.Grid.transform);
                inventorySlot.Initialize(_assetLoader);
                _ = inventorySlot.SetItem(item);
                inventorySlot.PlaceButtonPressed += _placeObjectSystem.Place;
                return inventorySlot;
            });
            slotViews.ObserveReplace().Subscribe(replace =>
            {
                _ = replace.OldValue.View.SetItem(replace.NewValue.Value);
            });

            _view.Initialize(slotViews);
        }

        public void Add(int count)
        {
            _core.AddMoney(count);
        }
    }
}
