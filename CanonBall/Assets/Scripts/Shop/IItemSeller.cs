using System.Collections.Generic;

namespace Assets.Scripts.Shop
{
    public interface IItemSeller
    {
        public IEnumerable<ItemType> GetItems();
    }
}
