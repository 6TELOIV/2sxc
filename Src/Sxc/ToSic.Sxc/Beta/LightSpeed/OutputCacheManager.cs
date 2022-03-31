﻿using System;
using System.Runtime.Caching;
using ToSic.Eav.Documentation;

namespace ToSic.Sxc.Beta.LightSpeed
{
    [PrivateApi]
    public class OutputCacheManager
    {
        internal const string GlobalCacheRoot = "2sxc.Lightspeed.Module.";

        internal string Id(int moduleId, int? userId, string suffix)
        {
            var id = GlobalCacheRoot + moduleId;
            if (userId.HasValue) id += ":" + userId.Value;
            if (suffix != null) id += ":" + suffix;
            return id;
        }

        public string Add(string cacheKey, OutputCacheItem data, int duration)
        {
            try
            {
                // Never store 0, that's like never-expire
                if (duration == 0) duration = 1;
                var expiration = new TimeSpan(0, 0, duration);
                var policy = new CacheItemPolicy { SlidingExpiration = expiration };
                Cache.Set(new CacheItem(cacheKey, data), policy);
                return cacheKey;
            }
            catch
            {
                /* ignore for now */
            }

            return "error";
        }

        public OutputCacheItem Get(string key) => Cache[key] as OutputCacheItem;

        private static MemoryCache Cache => MemoryCache.Default;

    }
}
