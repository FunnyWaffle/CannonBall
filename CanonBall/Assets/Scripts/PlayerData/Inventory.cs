using Assets.Scripts.Shop;
using ObservableCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PlayerData
{
    public class Inventory
    {
        private readonly ObservableList<ItemType> _items = new();

        private int _money;

        public Inventory()
        {
            _items.Add(ItemType.Cannon);
        }

        public int Money
        {
            get => _money;
            private set
            {
                _money = value;
                MoneyCountChanged?.Invoke(value);
            }
        }
        public IObservableCollection<ItemType> Items => _items;

        public event Action<int> MoneyCountChanged;

        public void SpendMoney(int value)
        {
            Money = Mathf.Max(0, _money - value);
        }

        public void AddMoney(int value)
        {
            Money += value;
        }

        public void AddItems(IEnumerable<ItemType> items)
        {
            _items.AddRange(items);
        }

        public void RemoveItem(ItemType item)
        {
            _items.Remove(item);
        }
    }
}
