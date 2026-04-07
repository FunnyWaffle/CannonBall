using System;
using UnityEngine;

namespace Assets.Scripts.Wrappers
{
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        [SerializeField]
        private TKey _key;
        [SerializeField]
        private TValue _value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public TValue Value { get => _value; }
        public TKey Key { get => _key; }
    }
}
