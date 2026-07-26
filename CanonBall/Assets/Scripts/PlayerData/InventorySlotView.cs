using Assets.Scripts.Shop;
using Assets.Scripts.Spawn;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerData
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _image;
        [SerializeField] private Button _placeButton;

        private AssetLoader _assetLoader;

        private ItemType _itemType;

        public event Action<ItemType> PlaceButtonPressed;

        public void Initialize(AssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public async Task SetItem(ItemType itemType)
        {
            if (!isActiveAndEnabled)
                gameObject.SetActive(true);

            _name.SetText(itemType.ToString());
            _itemType = itemType;
            _image.sprite = await _assetLoader.LoadSprite(itemType);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
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
    }
}
