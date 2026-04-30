using Assets.Scripts.PlayerData;

namespace Assets.Scripts.Systems
{
    public class Store
    {
        private readonly int _gunPrice = 10;

        public void BuyGun(Inventory inventory)
        {
            inventory.SpendMoney(_gunPrice);
            inventory.AddGun();
        }
    }
}
