using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    public class ShopElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Toggle _checkMark;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _price;

        private ItemTypes _itemType;

        public bool IsSelected
        {
            get => _checkMark.isOn;
            set
            {
                _checkMark.isOn = value;
            }
        }
        public ItemTypes ItemType => _itemType;

        public void SetItem(ItemTypes itemType, Sprite sprite)
        {
            _itemType = itemType;

            _name.SetText(_itemType.ToString());
            _image.sprite = sprite;
        }

        public void Deselect()
        {
            IsSelected = false;
        }

        private void OnValidate()
        {
            _name.SetText(_itemType.ToString());
        }
    }
}
