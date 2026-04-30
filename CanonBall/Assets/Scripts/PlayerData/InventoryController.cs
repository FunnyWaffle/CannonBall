using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;
using Assets.Scripts.Spawn;
using ObservableCollections;
using R3;
using System;
using UnityEngine;

namespace Assets.Scripts.PlayerData
{
    public class InventoryController : IUIWindow
    {
        private readonly Inventory _core;
        private readonly InventoryView _view;

        private readonly WavesExecutor _wavesExecutor;
        private readonly Shop.Shop _shop;

        public UIWindowTypes Type => UIWindowTypes.Inventory;

        public event Action Closed;

        public InventoryController(InventoryView inventoryView, WavesExecutor wavesExecutor, Shop.Shop shop)
        {
            _view = inventoryView;
            _core = new Inventory();

            _wavesExecutor = wavesExecutor;
            _shop = shop;

            _core.MoneyCountChanged += _view.SetMoneyValue;
            _wavesExecutor.WaveEnded += AddCoinsToInventory;
            _shop.PurchaseCompleted += _core.AddItems;
        }

        public void Initialize()
        {
            var slotViews = _core.Items.CreateView(item =>
            {
                var inventorySlot = GameObject.Instantiate(_view.SlotPrefab, _view.Grid.transform);
                inventorySlot.SetItem(item);
                return inventorySlot;
            });
            slotViews.ObserveReplace().Subscribe(replace =>
            {
                replace.OldValue.View.SetItem(replace.NewValue.Value);
            });

            _view.Initialize(slotViews);
        }

        public void Open()
        {
            _view.Open();
        }

        public void Close()
        {
            _view.Close();
        }

        private void AddCoinsToInventory(int waveIndex)
        {
            const int coinPerWave = 5;
            _core.AddMoney(waveIndex * coinPerWave);
        }

        public void HandleInput(InputData input)
        {
            if (input.IsBackEventPerformed)
                Close();
        }
    }
}
