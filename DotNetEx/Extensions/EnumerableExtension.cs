using DotNet.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static Dictionary<TKey, List<T>> Group<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            var g = source.GroupBy(keySelector);
            Dictionary<TKey, List<T>> ret = new Dictionary<TKey, List<T>>(g.Count());

            foreach (var kv in g)
            {
                var eles = kv.ToList();
                ret.Add(kv.Key, eles);
            }

            return ret;
        }

        public static List<TResult> CastList<TResult>(this IEnumerable source)
        {
            return source.Cast<TResult>().ToList();
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            dnUtils.NotNull(source);
            dnUtils.NotNull(keySelector);

            Dictionary<TKey, T> dic = new Dictionary<TKey, T>();

            foreach (var item in source)
            {
                TKey key = keySelector(item);
                if (key == null)
                    throw new ArgumentException("source 集合存在 key 为 null 的元素");

                if (dic.ContainsKey(key) == false)
                {
                    dic.Add(key, item);
                }
            }

            return dic.Values;
        }
        public static bool In<T>(this T obj, IEnumerable<T> source)
        {
            return source.Contains(obj);
        }
    }
}
