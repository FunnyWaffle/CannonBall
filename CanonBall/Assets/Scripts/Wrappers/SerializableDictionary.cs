using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Wrappers
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new();

        [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> _list;

        public TValue this[TKey key] => _dictionary[key];

        public bool TryGetValue(TKey key, out TValue value)
            => _dictionary.TryGetValue(key, out value);

        public void OnAfterDeserialize()
        {
            _dictionary.Clear();
            foreach (var keyValuePair in _list)
            {
                if (keyValuePair.Key == null)
                    continue;

                if (_dictionary.ContainsKey(keyValuePair.Key))
                {
                    var clearKeyValuePair = new SerializableKeyValuePair<TKey, TValue>(default, default);
                    _dictionary[clearKeyValuePair.Key] = clearKeyValuePair.Value;
                }
                else
                {
                    _dictionary[keyValuePair.Key] = keyValuePair.Value;
                }
            }
        }

        public void OnBeforeSerialize()
        {
            _list.Clear();
            foreach (var (key, value) in _dictionary)
            {
                _list.Add(new SerializableKeyValuePair<TKey, TValue>(key, value));
            }
        }
    }
}
