using Assets.Scripts.GameStateMachine;
using Assets.Scripts.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    public class Shop : MonoBehaviour, IUIWindow
    {
        [SerializeField] private RectTransform _transform;
        [SerializeField] private Button _butButton;
        [SerializeField] private ShopElement[] _elements;

        public UIWindowTypes Type => UIWindowTypes.Shop;

        public event Action Closed;
        public event Action<IEnumerable<ItemTypes>> PurchaseCompleted;

        public void Initialize()
        {
            _transform.gameObject.SetActive(false);
        }

        public void Open()
        {
            _transform.gameObject.SetActive(true);
        }

        public void Close()
        {
            _transform.gameObject.SetActive(false);
            Closed?.Invoke();
        }

        private void OnEnable()
        {
            _butButton.onClick.AddListener(OnBuyButtonClick);
        }

        private void OnBuyButtonClick()
        {
            var selectedElements = _elements.Where(element => element.IsSelected);
            var itemTypes = selectedElements.Select(element => element.ItemType);

            PurchaseCompleted?.Invoke(itemTypes);

            foreach (var element in selectedElements)
            {
                element.Deselect();
            }
        }

        public void HandleInput(InputData input)
        {
            if (input.IsBackEventPerformed)
                Close();
        }
    }
}
