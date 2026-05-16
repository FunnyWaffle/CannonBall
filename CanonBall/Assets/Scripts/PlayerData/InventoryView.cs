using Assets.Scripts.Shop;
using ObservableCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PlayerData
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventorySlotView _slotPrefab;
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private TMP_Text _text;

        public bool IsActive => transform.gameObject.activeSelf;
        public InventorySlotView SlotPrefab => _slotPrefab;
        public GridLayoutGroup Grid => _grid;

        public ISynchronizedView<ItemTypes, InventorySlotView> SlotViews { get; private set; }

        public void Initialize(ISynchronizedView<ItemTypes, InventorySlotView> slotViews)
        {
            SlotViews = slotViews;
        }

        public void SetMoneyValue(int value)
        {
            _text.SetText(value.ToString());
        }

        public void Open()
        {
            transform.gameObject.SetActive(true);
        }

        public void Close()
        {
            transform.gameObject.SetActive(false);
        }
    }
}
