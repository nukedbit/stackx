using System.Collections.Generic;

namespace StackX.Common
{
    public static class DictionaryExtensions
    {
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, object> dictionary, TKey key)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return (TValue)value;
            }

            return default(TValue);
        }
        
        
        public static TValue GetValueOrFail<TValue>(this IDictionary<string, object> dic, string key)
        {
            if (dic.TryGetValue(key, out var value))
            {
                return (TValue) value;
            }

            return default;
        }

        public static IDictionary<TKey, TValue> CopyOnlyKeys<TKey, TValue>(this IDictionary<TKey, TValue> src,
            params TKey[] keys)
        {
            return src.CopyOnlyKeys(new Dictionary<TKey, TValue>(), keys);
        }

        public static IDictionary<TKey, TValue> CopyOnlyKeys<TKey, TValue>(this IDictionary<TKey, TValue> src,
            IDictionary<TKey, TValue> dest, params TKey[] keys)
        {
            foreach (var key in keys)
            {
                if (dest.ContainsKey(key))
                {
                    dest[key] = src[key];
                }
                else
                {
                    dest.Add(key, src[key]);
                }
            }

            return dest;
        }

        public static IDictionary<TKey, TValue> Set<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key,
            TValue value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }

            return dic;
        }
    }
}