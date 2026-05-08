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
    public class Shop : MonoBehaviour, IUIWindow
    {
        [SerializeField] private RectTransform _transform;
        [SerializeField] private Button _butButton;
        [SerializeField] private ShopElement[] _elements;
        [SerializeField] private Collider _vendorCollider;

        public UIWindowTypes Type => UIWindowTypes.Shop;

        public event Action<IEnumerable<ItemTypes>> PurchaseCompleted;

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

        public void Open()
        {
            _transform.gameObject.SetActive(true);
        }

        public void Close()
        {
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

            PurchaseCompleted?.Invoke(itemTypes);

            foreach (var element in selectedElements)
            {
                element.Deselect();
            }
        }
    }
}
