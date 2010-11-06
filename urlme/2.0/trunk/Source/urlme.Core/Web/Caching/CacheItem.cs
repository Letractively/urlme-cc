using System;
using System.Collections.Generic;
using System.Text;

namespace urlme.Core.Web.Caching
{
	public class CacheItem
	{
		public DateTime Expires = DateTime.MaxValue; // desired expire time
		public readonly DateTime Created = DateTime.Now; // time cacheItem was created
		public bool FlaggedForUpdate = false;  // Gets/Sets whether the CacheItem is flagged for updating
		public object Value; // the actual object being cached.

		public CacheItem(object value, int expireSeconds)
		{
			this.Expires = DateTime.Now.AddSeconds(expireSeconds);
			this.Value = value;
		}

		public CacheItem(object value, DateTime expireTime)
		{
			this.Expires = expireTime;
			this.Value = value;
		}

		public bool IsExpired
		{
			get
			{
				return this.Expires < DateTime.Now;
			}
		}

	}
}
