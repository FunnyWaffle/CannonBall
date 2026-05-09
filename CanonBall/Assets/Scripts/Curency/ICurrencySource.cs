using System;

namespace Assets.Scripts.Curency
{
    public interface ICurrencySource<T>
    {
        public event Action CurrencyAccrualPerformed;
    }
}
