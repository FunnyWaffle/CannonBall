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

        private readonly PlaceObjectSystem _placeObjectSystem;
        private readonly AssetLoader _assetLoader;

        public InventoryController(InventoryView inventoryView,
            PlaceObjectSystem placeObjectSystem,
            AssetLoader assetLoader)
        {
            _view = inventoryView;
            _core = new Inventory();
            Initialize();

            _placeObjectSystem = placeObjectSystem;
            _assetLoader = assetLoader;

            _core.MoneyCountChanged += _view.SetMoneyValue;
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
                inventorySlot.PlaceButtonPressed += _placeObjectSystem.Place;
                return inventorySlot;
            });
            slotViews.ObserveReplace().Subscribe(replace =>
            {
                _ = replace.OldValue.View.SetItem(replace.NewValue.Value);
            });

            _view.Initialize(slotViews);
        }
    }
}
