using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Motley_Vis
{
    public class LruCache<K, V>
    {
        private readonly Dictionary<K, V> cache;
        private readonly LinkedList<K> ordering;
        private readonly int capacity;

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
    }
}
