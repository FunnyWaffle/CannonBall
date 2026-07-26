using System.Collections.Generic;

namespace Assets.Scripts.Shop
{
    public interface IItemStorage
    {
        public void ApplyPurchasedItems(IEnumerable<ItemType> items);
        public void RemoveItem(ItemType item);
    }
}
