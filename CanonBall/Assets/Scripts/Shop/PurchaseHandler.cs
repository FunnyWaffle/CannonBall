using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Shop
{
    public class PurchaseHandler
    {
        private readonly IPurchaseMaker _purchaseMaker;
        private readonly IItemStorage _itemStorage;
        private readonly ICurrencyStorage _currencyStorage;

        public PurchaseHandler(IItemStorage purchaseSource, IPurchaseMaker purchaseMaker)
        {
            _itemStorage = purchaseSource;
            _purchaseMaker = purchaseMaker;

            _purchaseMaker.PurchasePerformed += OnPerformPurchase;
        }

        private void OnPerformPurchase(IItemSeller itemSeller, IEnumerable<ItemTypes> itemsToBuy)
        {
            var existingItems = itemSeller.GetItems();

            var itemsAvailableForPurchase = itemsToBuy.Where(item => existingItems.Contains(item));

            if (!_currencyStorage.TrySpend(itemsAvailableForPurchase.Count()))
                return;

            _itemStorage.ApplyPurchasedItems(itemsToBuy);
        }
    }
}
