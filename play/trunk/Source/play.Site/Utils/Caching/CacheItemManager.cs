//-----------------------------------------------------------------------
// <copyright file="CacheItemManager.cs" company="Salem Web Network">
//     Copyright (c) Salem Web Network. All rights reserved.
// </copyright>
// <author>Daniel Price</author>
//-----------------------------------------------------------------------
namespace play.Site.Utils.Caching
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Web;
    using System.Web.Caching;

    /// <summary>
    /// Represents the manager that interfaces with the cache runtime
    /// </summary>
    internal class CacheItemManager
    {
        #region Variables
        /// <summary>
        /// the object used for locking
        /// </summary>
        private static readonly object lockObject = new object();

        /// <summary>
        /// the dictionary of threads to manage who gets stuck waiting for the cache to break
        /// </summary>
        private static readonly Dictionary<string, int> shortStrawThreads = new Dictionary<string, int>(25);
        #endregion

        #region Properties
        /// <summary>
        /// Gets the update window, in seconds
        /// </summary>
        public static int UpdateWindowSeconds
        {
            get
            {
                return 60;
            }
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Stores the item in cache
        /// </summary>
        /// <param name="key">the unique key to retrieve the item from cache</param>
        /// <param name="value">the value to cache</param>
        /// <param name="dependency">the dependency for the CacheItem that will let us know when the cache needs to be busted</param>
        public static void Store(string key, CacheItem value, CacheDependency dependency)
        {
            if (value != null && !value.IsExpired)
            {
                value.FlaggedForUpdate = false;
                HttpRuntime.Cache.Insert(key, value, dependency, value.Expires.AddSeconds(UpdateWindowSeconds), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                shortStrawThreads.Remove(key);
            }
        }

        /// <summary>
        /// Stores an item in the cache
        /// </summary>
        /// <param name="key">the unique key to retrieve the item from cache</param>
        /// <param name="value">the value to cache</param>
        public static void Store(string key, CacheItem value)
        {
            CacheItemManager.Store(key, value, null);
        }

        /// <summary>
        /// Removes an item from cache
        /// </summary>
        /// <param name="key">the key of the item to remove</param>
        /// <returns>the object in cache that was removed</returns>
        public static object Remove(string key)
        {
            object obj = HttpRuntime.Cache.Remove(key);
            if (obj != null && obj.GetType() == typeof(CacheItem))
            {
                return ((CacheItem)obj).Value;
            }

            return obj;
        }

        /// <summary>
        /// Gets the value at the specified key, in cache
        /// </summary>
        /// <param name="key">the unique key</param>
        /// <returns>the object at the key</returns>
        public static object GetValue(string key)
        {
            // check if current thread is already supposed to be updating for this key.  If so, return nothing so it does it's job
            try
            {
                if (shortStrawThreads.ContainsKey(key) && (shortStrawThreads[key] == Thread.CurrentThread.ManagedThreadId))
                {
                    shortStrawThreads.Remove(key);
                    return null;
                }
            }
            catch
            {
            }

            object obj = HttpRuntime.Cache.Get(key);

            if (obj != null)
            {
                // check if object is a CacheItem (shortstraw only implemented for CacheItems)
                if (obj.GetType() == typeof(CacheItem))
                {
                    CacheItem ci = (CacheItem)obj;

                    // If Not expired, simply return the value
                    if (!ci.IsExpired)
                    {
                        return ci.Value;
                    }

                    // ** Expired ***

                    // If already flagged for update, then simply return the value
                    if (ci.FlaggedForUpdate)
                    {
                        return ci.Value;
                    }

                    // *** Enter Lock Zone ***
                    lock (lockObject)
                    {
                        obj = HttpRuntime.Cache.Get(key);
                        if (obj != null && obj.GetType() == typeof(CacheItem))
                        {
                            ci = (CacheItem)obj;

                            // another thread flagged it, simply return the item
                            if (ci.FlaggedForUpdate)
                            {
                                return ci.Value;
                            }

                            // this thread elected to update the cache, so set flag and return nothing
                            ci.FlaggedForUpdate = true;
                            shortStrawThreads[key] = Thread.CurrentThread.ManagedThreadId;
                            return null;
                        }
                    }
                    //// *** Exit Lock Zone ***
                }

                return obj;
            }

            return null;
        } 
        #endregion
    }
}
