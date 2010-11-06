using System;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace urlme.Core.Web.Caching
{
    public class CacheItemManager
    {
        private static object lockObject = new object();
        private static Dictionary<string, int> shortStrawThreads = new Dictionary<string, int>(25);

        public static int UpdateWindowSeconds
        {
            get
            {
                return 60;
            }
        }

        public static void Store(string key, CacheItem value, CacheDependency dependency)
        {
            if (value != null && !value.IsExpired)
            {
                value.FlaggedForUpdate = false;
                HttpRuntime.Cache.Insert(key, value, dependency, value.Expires.AddSeconds(UpdateWindowSeconds), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                shortStrawThreads.Remove(key);
            }
        }

        public static void Store(string key, CacheItem value)
        {
            CacheItemManager.Store(key, value, null);
        }

        public static object Remove(string key)
        {
            object obj = HttpRuntime.Cache.Remove(key);
            if (obj != null && obj.GetType() == typeof(CacheItem))
                return ((CacheItem)obj).Value;

            return obj;
        }

        public static object GetValue(string key)
        {
            object obj = null;

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

            obj = HttpRuntime.Cache.Get(key);

            if (obj != null)
            {
                // check if object is a CacheItem (shortstraw only implemented for CacheItems)
                if (obj.GetType() == typeof(CacheItem))
                {
                    CacheItem ci = (CacheItem)obj;

                    // If Not expired, simply return the value
                    if (!ci.IsExpired)
                        return ci.Value;

                    // ** Expired ***

                    // If already flagged for update, then simply return the value
                    if (ci.FlaggedForUpdate)
                        return ci.Value;

                    // *** Enter Lock Zone ***
                    lock (lockObject)
                    {
                        obj = HttpRuntime.Cache.Get(key);
                        if (obj != null && obj.GetType() == typeof(CacheItem))
                        {
                            ci = (CacheItem)obj;

                            // another thread flagged it, simply return the item
                            if (ci.FlaggedForUpdate)
                                return ci.Value;

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
    }
}
