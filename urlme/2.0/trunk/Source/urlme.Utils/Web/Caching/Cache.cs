using System;
using System.Web;
using System.Data;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;
using urlme.Utils.Configuration;
using urlme.Utils.Extensions;

namespace urlme.Utils.Web.Caching
{
    public sealed class Cache
    {
        public static readonly int NoAbsoluteExpiration = 0;

        private static void Store(string key, object value, int expireSeconds, CacheDependency dependency)
        {
            if (expireSeconds == 0)
                expireSeconds = int.MaxValue;

            // wrap value within a CacheItem object
            CacheItem ci = new CacheItem(value, expireSeconds);
            CacheItemManager.Store(key, ci, dependency);
        }

        /// <summary>
        /// Gets the value for the given key from cache, may return null if item is up for refresh or is out of cache
        /// </summary>
        /// <param name="key">the unique key</param>
        /// <returns>the item from cache, if available</returns>
        private static object GetValue(string key)
        {
            return CacheItemManager.GetValue(key);
        }

        /// <summary>
        /// Grabs an item from cache, or calls the anonymous method to populate the cache
        /// </summary>
        /// <typeparam name="TEntity">the type of object to return from cache</typeparam>
        /// <param name="key">the unique key to the item in the cache</param>
        /// <param name="func">the function to call to create the item if no longer in cache</param>
        /// <returns>the item from cache</returns>
        /// <remarks>
        /// the purpose of this is to hide the population/retrieval from the cache 
        /// and to delay the execution of the creation of the cache object until needed
        /// </remarks>
        /// 
        public static TEntity GetValue<TEntity>(object[] key, Func<TEntity> func) where TEntity : class
        {
            return GetValue<TEntity>(key.FlattenToString(), func);
        }

        public static TEntity GetValue<TEntity>(string key, Func<TEntity> func) where TEntity : class
        {
            return Cache.GetValue<TEntity>(key, func, null, Cache.DefaultCacheSeconds);
        }

        /// <summary>
        /// Grabs an item from cache, or calls the anonymous method to populate the cache
        /// </summary>
        /// <typeparam name="TEntity">the type of object to return from cache</typeparam>
        /// <param name="key">the unique key to the item in the cache</param>
        /// <param name="func">the function to call to create the item if no longer in cache</param>
        /// <param name="dependency">the cache dependency to also trigger cache to need refreshing</param>
        /// <returns>the item from cache</returns>
        /// <remarks>
        /// the purpose of this is to hide the population/retrieval from the cache 
        /// and to delay the execution of the creation of the cache object until needed
        /// </remarks>
        public static TEntity GetValue<TEntity>(string key, Func<TEntity> func, CacheDependency dependency) where TEntity : class
        {
            return Cache.GetValue<TEntity>(key, func, dependency, Cache.DefaultCacheSeconds);
        }

        /// <summary>
        /// Grabs an item from cache, or calls the anonymous method to populate the cache
        /// </summary>
        /// <typeparam name="TEntity">the type of object to return from cache</typeparam>
        /// <param name="key">the unique key to the item in the cache</param>
        /// <param name="func">the function to call to create the item if no longer in cache</param>
        /// <param name="dependency">the cache dependency to also trigger cache to need refreshing</param>
        /// <param name="expireInSeconds">the time before the cache should expire, in seconds</param>
        /// <returns>the item from cache</returns>
        /// <remarks>
        /// the purpose of this is to hide the population/retrieval from the cache 
        /// and to delay the execution of the creation of the cache object until needed
        /// </remarks>
        public static TEntity GetValue<TEntity>(string key, Func<TEntity> func, CacheDependency dependency, int expireInSeconds)
        {
            var obj = Cache.GetValue(key); // use "as" so that we get null in case the type that comes back does not match

            // check to see if the item exists in cache
            if (obj == null)
            {
                // create lock on item
                lock (Utils.Web.Caching.Cache.GetLockObject(key))
                {
                    obj = Cache.GetValue(key);

                    // make sure the item hasn't been recreated in cache since locking
                    if (obj == null)
                    {
                        // create new instance of object
                        obj = func.Invoke();

                        // store object in cache (dependency may be null)
                        Cache.Store(key, obj, expireInSeconds, dependency);
                    }
                }
            }

            return (TEntity)obj;
        }

        public static int Count()
        {
            return HttpRuntime.Cache.Count;
        }

        public static void ClearAll()
        {
            List<string> keyList = new List<string>();
            IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();

            while (cacheEnum.MoveNext())
            {
                keyList.Add(cacheEnum.Key.ToString());
            }

            foreach (string key in keyList)
                Remove(key);
        }

        public static object Remove(string key)
        {
            return CacheItemManager.Remove(key);
        }

        public static int DefaultCacheSeconds
        {
            get
            {
                return int.Parse((ConfigurationManager.Instance.AppSettings["CacheSeconds"] ?? "300").ToString());
            }
        }

        private static Dictionary<string, object> lockObject = new Dictionary<string, object>();
        public static object GetLockObject(string key)
        {
            if (!lockObject.ContainsKey(key))
            {
                lock (lockObject)
                {
                    if (!lockObject.ContainsKey(key))
                    {
                        lockObject.Add(key, new object());
                    }
                }
            }
            return (object)lockObject[key];
        }

    }
}
