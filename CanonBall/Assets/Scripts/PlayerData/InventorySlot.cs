using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerData
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _image;
        [SerializeField] private Button _placeButton;

        private AssetLoader _assetLoader;

        private ItemTypes _itemType;

        public event Action<ItemTypes> PlaceButtonPressed;

        public void Initialize(AssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

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

        public async Task SetItem(ItemTypes item)
        {
            _name.SetText(item.ToString());
            _itemType = item;

            _image.sprite = await _assetLoader.LoadSprite(item);
        }
    }
}
