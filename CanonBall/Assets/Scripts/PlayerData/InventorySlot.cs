using Assets.Scripts.Shop;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerData
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Button _placeButton;

        private ItemTypes _itemType;

        public event Action<ItemTypes> PlaceButtonPressed;

        private void OnEnable()
        {
            _placeButton.onClick.AddListener(OnPlaceButtonClick);
        }

        private void OnDisable()
        {
            _placeButton.onClick.RemoveListener(OnPlaceButtonClick);
        }

        private void OnPlaceButtonClick()
        {
            PlaceButtonPressed?.Invoke(_itemType);
        }

        public void SetItem(ItemTypes item)
        {
            _name.SetText(item.ToString());
            _itemType = item;
        }
    }
}
