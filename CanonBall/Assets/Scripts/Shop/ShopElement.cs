using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    public class ShopElement : MonoBehaviour
    {
        [SerializeField] private ItemTypes _itemType;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Toggle _checkMark;

        public bool IsSelected
        {
            get => _checkMark.isOn;
            set
            {
                _checkMark.isOn = value;
            }
        }
        public ItemTypes ItemType => _itemType;

        public void Initialize(ItemTypes itemType)
        {
            _itemType = itemType;
            _name.SetText(_itemType.ToString());
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
