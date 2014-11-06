using System;
using System.Collections.Generic;

namespace Motley_Vis
{
    public class LruCache<K, V> : IDictionary<K, V>
    {
        private readonly Dictionary<K, V> cache;
        private readonly LinkedList<K> ordering;
        private readonly int capacity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity">Must be positive</param>
        public LruCache(int capacity)
        {
            this.capacity = capacity;
            ordering = new LinkedList<K>();
            cache = new Dictionary<K, V>(capacity);
        }

        /// <summary>
        /// Returns the value at key if in cache, default otherwise.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public V TryGet(K key, V def)
        {
            if (cache.ContainsKey(key))
            {
                var item = cache[key];

                // O(n) because of search
                ordering.Remove(key);
                ordering.AddLast(key);

                return item;
            }
            return def;
        }

        /// <summary>
        /// Add Key, Value pair to cache. Removing oldest if necessary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(K key, V value)
        {
            if (cache.Count >= capacity)
            {
                cache.Remove(ordering.First.Value);
                ordering.RemoveFirst();
            }
            cache[key] = value;
            ordering.AddLast(key);
        }


        public bool ContainsKey(K key)
        {
            return cache.ContainsKey(key);
        }

        public ICollection<K> Keys
        {
            get { return cache.Keys; }
        }

        public bool Remove(K key)
        {
            return cache.Remove(key) && ordering.Remove(key);
        }

        public bool TryGetValue(K key, out V value)
        {
            throw new NotImplementedException();
        }

        public ICollection<V> Values
        {
            get { return cache.Values; }
        }

        public V this[K key]
        {
            get { return cache[key]; }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            cache.Clear();
            ordering.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return cache.ContainsKey(item.Key) && cache[item.Key].Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return cache.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return cache.Remove(item.Key) && ordering.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
