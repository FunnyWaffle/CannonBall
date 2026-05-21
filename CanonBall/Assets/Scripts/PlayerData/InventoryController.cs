using Assets.Scripts.Curency;
using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Placement;
using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PlayerData
{
    public class InventoryController : IUIWindow, ICurrencyReceiver<int>, IItemStorage, ICurrencyStorage, IPlacementExecuter
    {
        private readonly Inventory _core;
        private readonly InventoryView _view;

        private readonly AssetLoader _assetLoader;

        public InventoryController(InventoryView inventoryView,
            AssetLoader assetLoader)
        {
            _view = inventoryView;
            _core = new Inventory();

            _assetLoader = assetLoader;

            Initialize();

            _core.MoneyCountChanged += _view.SetMoneyValue;
        }

        public UIWindowTypes Type => UIWindowTypes.Inventory;

        public event Action<ItemTypes> PlacementStarted;

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

        public void RemoveItem(ItemTypes item)
        {
            _core.RemoveItem(item);
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
                inventorySlot.PlaceButtonPressed += OnPlaceButtonPress;
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

        private void OnPlaceButtonPress(ItemTypes type)
        {
            PlacementStarted?.Invoke(type);
        }
    }
}
