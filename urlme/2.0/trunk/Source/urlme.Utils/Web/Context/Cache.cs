using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urlme.Utils.Web.Context
{
    public static class Cache
    {
        public static TEntity GetValue<TEntity>(this HttpContextBase context, string key, Func<TEntity> func) where TEntity : class
        {
            if (context == null)
            {
                return func.Invoke();
            }

            var obj = context.Items[key];

            // check to see if the item exists in cache
            if (obj == null)
            {
                // create lock on item
                lock (GetLockObject(key))
                {
                    obj = context.Items[key];

                    // make sure the item hasn't been recreated in cache since locking
                    if (obj == null)
                    {
                        // create new instance of object
                        obj = func.Invoke();

                        // store object in cache (dependency may be null)
                        context.Items.Add(key, obj);
                    }
                }
            }

            return (TEntity)obj;
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