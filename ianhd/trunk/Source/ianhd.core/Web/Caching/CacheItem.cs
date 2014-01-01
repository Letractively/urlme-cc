namespace ianhd.core.Web.Caching
{
    using System;

    /// <summary>
    /// Represents a single item in cache
    /// </summary>
    internal class CacheItem
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the CacheItem class
        /// </summary>
        /// <param name="value">the value to cache</param>
        /// <param name="expireSeconds">the time in seconds to when the item should expire</param>
        public CacheItem(object value, int expireSeconds)
            : this(value, DateTime.Now.AddSeconds(expireSeconds))
        {
        }

        /// <summary>
        /// Initializes a new instance of the CacheItem class
        /// </summary>
        /// <param name="value">the value to cache</param>
        /// <param name="expireTime">the time to expire the value</param>
        public CacheItem(object value, DateTime expireTime)
        {
            this.Expires = expireTime;
            this.Value = value;
            this.Created = DateTime.Now;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether this item has expired from cache
        /// </summary>
        public bool IsExpired
        {
            get
            {
                return this.Expires < DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the desired expire time
        /// </summary>
        public DateTime Expires
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the time cacheItem was created
        /// </summary>
        public DateTime Created
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the CacheItem is flagged for updating
        /// </summary>
        public bool FlaggedForUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the actual object being cached
        /// </summary>
        public object Value
        {
            get;
            set;
        }
        #endregion
    }
}
