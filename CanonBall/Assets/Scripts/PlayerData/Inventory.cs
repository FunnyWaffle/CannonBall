using Assets.Scripts.Shop;
using ObservableCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PlayerData
{
    public class Inventory
    {
        private readonly ObservableList<ItemTypes> _items = new();

        private int _money;
        private int _guns;

        public IObservableCollection<ItemTypes> Items => _items;

        public event Action<int> MoneyCountChanged;

        public void SpendMoney(int value)
        {
            _money = Mathf.Max(0, _money - value);
            MoneyCountChanged?.Invoke(_money);
        }

        public void AddMoney(int value)
        {
            _money += value;
            MoneyCountChanged?.Invoke(_money);
        }

        public void AddGun()
        {
            _guns++;
        }

        public void DecreaseGunCount()
        {
            _guns--;
        }

        public void AddItems(IEnumerable<ItemTypes> items)
        {
            _items.AddRange(items);
        }
    }
}
