//-----------------------------------------------------------------------
// <copyright file="Cache.cs" company="Salem Web Network">
//     Copyright (c) Salem Web Network. All rights reserved.
// </copyright>
// <author>Daniel Price</author>
//-----------------------------------------------------------------------
namespace movies.Core.Web.Caching
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Caching;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Used to cache items in the HttpRuntime
    /// </summary>
    public static class Cache
    {
        #region Variables
        /// <summary>
        /// the collection of objects locked
        /// </summary>
        private static readonly Dictionary<string, object> lockObject = new Dictionary<string, object>();

        /// <summary>
        /// the default number of cache seconds
        /// </summary>
        private static readonly int cacheSeconds =
            60; // 10800; // 3 hrs
            // int.Parse((ConfigurationManager.Instance.AppSettings["CacheSeconds"] ?? "10").ToString());
        #endregion

        #region Properties
        /// <summary>
        /// Gets the default cache time
        /// </summary>
        public static int DefaultCacheSeconds
        {
            get
            {
                return Cache.cacheSeconds;
            }
        }
        #endregion

        #region Methods
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
        /// <param name="expireInDays">the time before the cache should expire, in days</param>
        /// <returns>the item from cache</returns>
        /// <remarks>
        /// the purpose of this is to hide the population/retrieval from the cache 
        /// and to delay the execution of the creation of the cache object until needed
        /// </remarks>        
        public static TEntity GetValue<TEntity>(string key, Func<TEntity> func, int expireInDays) where TEntity : class
        {
            return Cache.GetValue<TEntity>(key, func, null, expireInDays * 86400);
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
                lock (Cache.GetLockObject(key))
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

        /// <summary>
        /// Returns the number of items in cache
        /// </summary>
        /// <returns>count of items in cache</returns>
        public static int Count()
        {
            return HttpRuntime.Cache.Count;
        }

        /// <summary>
        /// Removes all items from cache
        /// </summary>
        public static void ClearAll()
        {
            List<string> keyList = new List<string>();
            IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();

            while (cacheEnum.MoveNext())
            {
                keyList.Add(cacheEnum.Key.ToString());
            }

            foreach (string key in keyList)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// Removes the item from cache at the given key
        /// </summary>
        /// <param name="key">the unique key</param>
        /// <returns>the object removed from cache</returns>
        public static object Remove(string key)
        {
            return CacheItemManager.Remove(key);
        }

        /// <summary>
        /// Gets the lock object for the given key
        /// </summary>
        /// <param name="key">the unique key</param>
        /// <returns>the lock object</returns>
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

            return lockObject[key];
        }

        /// <summary>
        /// Stores an item in cache
        /// </summary>
        /// <param name="key">the unqiue key</param>
        /// <param name="value">the value to cache</param>
        public static void Store(string key, object value)
        {
            Cache.Store(key, value, Cache.DefaultCacheSeconds, null);
        }

        /// <summary>
        /// Stores an item in cache
        /// </summary>
        /// <param name="key">the unqiue key</param>
        /// <param name="value">the value to cache</param>
        /// <param name="expireSeconds">how long to cache</param>
        /// <param name="dependency">the dependency for expiring the cache earlier</param>
        public static void Store(string key, object value, int expireSeconds, CacheDependency dependency)
        {
            if (expireSeconds == 0)
            {
                expireSeconds = int.MaxValue;
            }

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

        public static bool KeyExists(string key)
        {
            return HttpRuntime.Cache.Get(key) != null;
        }

        //public static bool CacheKeys()
        //{
        //    List<String> ret = new List<String>();
            
        //}
        #endregion
    }
}