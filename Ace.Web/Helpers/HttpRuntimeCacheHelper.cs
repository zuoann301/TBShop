//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Caching;

//namespace System.Web
//{
//    public static class HttpRuntimeCacheHelper
//    {
//        /*
//         * System.Web.Caching.Cache
//         * Add 和 Insert 方法的区别：当要加入的缓存项已经在Cache中存在时，Insert将会覆盖原有的缓存项目，而Add则不会修改原有缓存项。
//         */

//        public static int GetCount()
//        {
//            return HttpRuntime.Cache.Count;
//        }
//        public static T Get<T>(string key)
//        {
//            return (T)HttpRuntime.Cache.Get(key);
//        }
//        public static object Get(string key)
//        {
//            return HttpRuntime.Cache.Get(key);
//        }
//        /// <summary>
//        /// 如果 key 已经存在，则覆盖原来的值
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="value"></param>
//        public static void Set(string key, object value)
//        {
//            HttpRuntime.Cache.Insert(key, value);
//        }
//        public static void SetAbsoluteExpirationCache(string key, object value, double cacheMinutes, CacheItemPriority priority = CacheItemPriority.Default)
//        {
//            HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(cacheMinutes), Cache.NoSlidingExpiration/*关闭滑动过期*/, priority, null);
//        }
//        public static void SetSlidingExpirationCache(string key, object value, double slidingMinutes, CacheItemPriority priority = CacheItemPriority.Default)
//        {
//            HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(slidingMinutes), priority, null);
//        }

//        public static T GetOrSetAbsoluteExpirationCache<T>(string key, Func<T> valueCreator, double cacheMinutes, CacheItemPriority priority = CacheItemPriority.Default)
//        {
//            T value = Get<T>(key);

//            if (value != null)
//                return value;

//            value = valueCreator();

//            SetAbsoluteExpirationCache(key, value, cacheMinutes, priority);

//            return value;
//        }
//        public static T GetOrSetSlidingExpirationCache<T>(string key, Func<T> valueCreator, double slidingMinutes, CacheItemPriority priority = CacheItemPriority.Default)
//        {
//            T value = Get<T>(key);

//            if (value != null)
//                return value;

//            value = valueCreator();

//            SetSlidingExpirationCache(key, value, slidingMinutes, priority);

//            return value;
//        }

//        public static object Remove(string key)
//        {
//            return HttpRuntime.Cache.Remove(key);
//        }
//        public static List<KeyValuePair<string, object>> RemoveByKeyPrefix(string keyPrefix)
//        {
//            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();

//            List<KeyValuePair<string, object>> removedItems = new List<KeyValuePair<string, object>>();

//            while (enumerator.MoveNext())
//            {
//                string key = enumerator.Key.ToString();
//                if (key.StartsWith(keyPrefix))
//                {
//                    var kv = new KeyValuePair<string, object>(key, HttpRuntime.Cache.Remove(key));
//                    removedItems.Add(kv);
//                }
//            }

//            return removedItems;
//        }
//    }
//}
