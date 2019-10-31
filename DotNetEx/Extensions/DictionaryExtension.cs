using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 如果找到 key 对应的value，则返回 value。否则返回 default(TValue)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            TValue value;
            if (!dic.TryGetValue(key, out value))
                value = default(TValue);

            return value;
        }

        /// <summary>
        /// 如果指定的键尚不存在，则将键/值对添加到字典中。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="valueGenerator"></param>
        /// <param name="syncRoot">同步锁对象。如果 syncRoot 不为 null，则使用双检锁方式向字典中添加键/值对。</param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TValue> valueGenerator, object syncRoot = null)
        {
            TValue value;
            if (!dic.TryGetValue(key, out value))
            {
                if (syncRoot != null)
                {
                    lock (syncRoot)
                    {
                        if (!dic.TryGetValue(key, out value))
                        {
                            value = valueGenerator();
                            dic[key] = value;
                        }
                    }
                }
                else
                {
                    value = valueGenerator();
                    dic[key] = value;
                }
            }

            return value;
        }
    }
}
