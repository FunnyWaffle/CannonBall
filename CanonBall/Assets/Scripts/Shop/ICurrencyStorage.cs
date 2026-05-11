namespace Assets.Scripts.Shop
{
    public interface ICurrencyStorage
    {
        public bool TrySpend(int count);
    }
}
