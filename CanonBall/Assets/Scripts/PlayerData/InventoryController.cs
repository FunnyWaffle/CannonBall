using Assets.Scripts.Curency;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using Assets.Scripts.Systems;
using ObservableCollections;
using R3;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PlayerData
{
    public class InventoryController : IUIWindow, ICurrencyReceiver<int>, IItemStorage, ICurrencyStorage
    {
        private readonly Inventory _core;
        private readonly InventoryView _view;

        private readonly AssetLoader _assetLoader;
        private readonly PlaceObjectSystem _placeObjectSystem;

        public InventoryController(InventoryView inventoryView,
            PlaceObjectSystem placeObjectSystem,
            AssetLoader assetLoader)
        {
            _view = inventoryView;
            _core = new Inventory();

            _placeObjectSystem = placeObjectSystem;
            _assetLoader = assetLoader;

            Initialize();

            _core.MoneyCountChanged += _view.SetMoneyValue;

            _placeObjectSystem.ObjectPlaced += _core.RemoveItem;
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

        public void Add(int count)
        {
            _core.AddMoney(count);
        }

        public void ApplyPurchasedItems(IEnumerable<ItemTypes> items)
        {
            _core.AddItems(items);
        }

        public bool TrySpend(int count)
        {
            if (_core.Money < count)
                return false;

            _core.SpendMoney(count);
            return true;
        }

        private void Initialize()
        {
            var slotViews = _core.Items.CreateView(item =>
            {
                var inventorySlot = GameObject.Instantiate(_view.SlotPrefab, _view.Grid.transform);
                inventorySlot.Initialize(_assetLoader);
                _ = inventorySlot.SetItem(item);
                inventorySlot.PlaceButtonPressed += _placeObjectSystem.ShowProjection;
                return inventorySlot;
            });
            slotViews.ObserveReplace().Subscribe(replace =>
            {
                _ = replace.OldValue.View.SetItem(replace.NewValue.Value);
            });
            slotViews.ObserveRemove().Subscribe(slot =>
            {
                slot.Value.View.Disable();
            });

            _view.Initialize(slotViews);
        }
    }
}
