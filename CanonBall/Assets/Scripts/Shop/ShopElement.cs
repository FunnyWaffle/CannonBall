using Assets.Scripts.Spawn;
using System.Threading.Tasks;
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
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _price;

        public bool IsSelected
        {
            get => _checkMark.isOn;
            set
            {
                _checkMark.isOn = value;
            }
        }
        public ItemTypes ItemType => _itemType;

        public async Task InitializeAsync(AssetLoader assetLoader)
        {
            _name.SetText(_itemType.ToString());
            _image.sprite = await assetLoader.LoadSprite(_itemType);
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
