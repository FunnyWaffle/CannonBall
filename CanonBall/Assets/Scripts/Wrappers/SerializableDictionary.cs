using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Wrappers
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new();

        [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> _list = new();

        public Dictionary<TKey, TValue> Dictionary => _dictionary;

        public void OnAfterDeserialize()
        {
            _dictionary.Clear();
            foreach (var keyValuePair in _list)
            {
                if (keyValuePair.Key == null)
                    continue;

                _dictionary[keyValuePair.Key] = keyValuePair.Value;
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
