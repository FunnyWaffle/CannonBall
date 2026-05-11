using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Interaction;
using Assets.Scripts.Spawn;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Collider _vendorCollider;

        private IItemSeller _seller;

        public UIWindowTypes Type => UIWindowTypes.Shop;

        public event Action<IItemSeller, IEnumerable<ItemTypes>> PurchasePerformed;

        [Inject]
        public void Initialize(InteractionObjectsRepositiory interactionObjectsRepositiory,
            AssetLoader assetLoader)
        {
            interactionObjectsRepositiory.AddInteactionable(_vendorCollider, InteractionableTypes.Vendor);
            interactionObjectsRepositiory.AddUIWindow(InteractionableTypes.Vendor, this);
            _transform.gameObject.SetActive(false);

            foreach (var element in _elements)
            {
                _ = element.InitializeAsync(assetLoader);
            }
        }

        public void SetItemSeller(IItemSeller itemSeller)
        {
            _seller = itemSeller;
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
            var selectedElements = _elements.Where(element => element.IsSelected);
            var itemTypes = selectedElements.Where(element => element.ItemType != ItemTypes.None).Select(element => element.ItemType);

            PurchasePerformed?.Invoke(_seller, itemTypes);

            foreach (var element in selectedElements)
            {
                element.Deselect();
            }
        }
    }
}
