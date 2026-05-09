namespace Assets.Scripts.Curency
{
    public interface ICurrencyAccrualPolicy<T>
    {
        public int Execute();
    }
}
