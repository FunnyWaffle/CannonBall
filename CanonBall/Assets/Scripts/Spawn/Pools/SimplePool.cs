using System.Collections.Generic;

namespace Assets.Scripts.Spawn.Pools
{
    public class SimplePool<T> where T : new()
    {
        private readonly Queue<T> _pool = new();

        public T Get()
        {
            T value;

            if (_pool.Count == 0)
                value = new();
            else
                value = _pool.Dequeue();

            return value;
        }

        public void Return(T value)
        {
            _pool.Enqueue(value);
        }
    }
}
