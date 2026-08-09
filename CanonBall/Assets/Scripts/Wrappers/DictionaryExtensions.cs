using System.Collections.Generic;

namespace Assets.Scripts.Wrappers
{
    public static class DictionaryExtenions
    {
        public static TValue GetOrCreate<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            if (!dictionary.TryGetValue(key, out var collection))
            {
                collection = new TValue();
                dictionary[key] = collection;
            }

            return collection;
        }

        public static void Add<TKey, TCollection, TValue>(
            this Dictionary<TKey, TCollection> dictionary,
            TKey key, TValue value)
            where TCollection : ICollection<TValue>, new()
        {
            if (!dictionary.TryGetValue(key, out var collection))
            {
                collection = new TCollection();
                dictionary[key] = collection;
            }

            collection.Add(value);
        }
    }
}
