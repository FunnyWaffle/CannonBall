using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Shop
{
    public class PurchaseHandler
    {
        private readonly IPurchaseMaker _purchaseMaker;
        private readonly IItemStorage _itemStorage;
        private readonly ICurrencyStorage _currencyStorage;

        public PurchaseHandler(IItemStorage purchaseSource, IPurchaseMaker purchaseMaker, ICurrencyStorage currencyStorage)
        {
            _itemStorage = purchaseSource;
            _purchaseMaker = purchaseMaker;
            _currencyStorage = currencyStorage;

            _purchaseMaker.PurchasePerformed += OnPerformPurchase;
        }

        private void OnPerformPurchase(IItemSeller itemSeller, IEnumerable<ItemType> itemsToBuy)
        {
            if (!_currencyStorage.TrySpend(itemsToBuy.Count()))
                return;

            _itemStorage.ApplyPurchasedItems(itemsToBuy);
        }
    }
}
