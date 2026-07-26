using System;
using System.Collections.Generic;

namespace Assets.Scripts.Shop
{
    public interface IPurchaseMaker
    {
        public event Action<IItemSeller, IEnumerable<ItemType>> PurchasePerformed;
    }
}
