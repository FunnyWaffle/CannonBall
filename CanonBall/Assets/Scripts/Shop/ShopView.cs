using Assets.Scripts.GameStateMachine.UIControl;
using Assets.Scripts.GameStateMachine.UIWindowsControl;
using Assets.Scripts.Interaction;
using Assets.Scripts.Spawn;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Shop
{
    public class ShopView : MonoBehaviour, IUIWindow, IPurchaseMaker
    {
        [SerializeField] private RectTransform _transform;
        [SerializeField] private Button _butButton;
        [SerializeField] private ShopElement[] _elements;

        private AssetLoader _assetLoader;

        private IItemSeller _seller;

        public UIWindowTypes Type => UIWindowTypes.Shop;

        public event Action<IItemSeller, IEnumerable<ItemType>> PurchasePerformed;

        [Inject]
        public void Initialize(AssetLoader assetLoader, InteractionObjectsRepositiory interactionObjectsRepositiory)
        {
            interactionObjectsRepositiory.AddUIWindow(InteractionableTypes.Vendor, this);
            _assetLoader = assetLoader;
            _transform.gameObject.SetActive(false);
        }

        public async Task SetItemSeller(IItemSeller itemSeller)
        {
            _seller = itemSeller;

            var index = 0;
            foreach (var item in itemSeller.GetItems())
            {
                var element = _elements[index];
                var sprite = await _assetLoader.LoadSprite(item);
                element.SetItem(item, sprite);

                index++;
            }
        }

        public void Open()
        {
            _transform.gameObject.SetActive(true);
        }

        public void Close()
        {
            _seller = null;
            _transform.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _butButton.onClick.AddListener(OnBuyButtonClick);
        }

        private void OnBuyButtonClick()
        {
            var selectedItems = new List<ItemType>();
            foreach (var element in _elements)
            {
                if (element.IsSelected)
                    selectedItems.Add(element.ItemType);
            }

            PurchasePerformed?.Invoke(_seller, selectedItems);

            foreach (var element in _elements)
            {
                element.Deselect();
            }
        }
    }
}
