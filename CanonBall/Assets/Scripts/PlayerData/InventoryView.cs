using Assets.Scripts.Shop;
using ObservableCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerData
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventorySlot _slotPrefab;
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private Transform _transform;
        [SerializeField] private TMP_Text _text;

        private ISynchronizedView<ItemTypes, InventorySlot> _slotViews;

        public bool IsActive => _transform.gameObject.activeSelf;
        public InventorySlot SlotPrefab => _slotPrefab;
        public GridLayoutGroup Grid => _grid;

        public ISynchronizedView<ItemTypes, InventorySlot> SlotViews { get => _slotViews; private set => _slotViews = value; }

        public void Initialize(ISynchronizedView<ItemTypes, InventorySlot> slotViews)
        {
            SlotViews = slotViews;
        }

        public void SetMoneyValue(int value)
        {
            _text.SetText(value.ToString());
        }

        public void Open()
        {
            _transform.gameObject.SetActive(true);
        }

        public void Close()
        {
            _transform.gameObject.SetActive(false);
        }
    }
}
