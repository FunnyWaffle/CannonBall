using Assets.Scripts.Shop;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.PlayerData
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;

        public void SetItem(ItemTypes item)
        {
            _name.SetText(item.ToString());
        }
    }
}
