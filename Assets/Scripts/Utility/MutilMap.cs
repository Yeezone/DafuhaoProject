using System.Collections.Generic;

namespace com.QH.QPGame.Utility
{
    /// <summary>
    /// 类似于c++的多key表
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// @Author: guofeng

    public class MutilMap<K, V>
    {
        private Dictionary<K, List<V>> _dictionary = new Dictionary<K, List<V>>();

        public void Add(K key, V value)
        {
            List<V> list;
            if (this._dictionary.TryGetValue(key, out list))
            {
                list.Add(value);
            }
            else
            {
                list = new List<V>();
                list.Add(value);
                this._dictionary[key] = list;
            }
        }

        public IEnumerable<K> Keys
        {
            get { return this._dictionary.Keys; }
        }

        public List<V> this[K key]
        {
            get
            {
                List<V> list;
                if (!this._dictionary.TryGetValue(key, out list))
                {
                    list = new List<V>();
                    this._dictionary[key] = list;
                }

                return list;
            }
        }

    }
}
