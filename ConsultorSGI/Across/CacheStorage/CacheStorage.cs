using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;

namespace Across.CacheStorage
{
    public class CacheStorage : ICacheStorage
    {
        public void Clear(string key)
        {
            HostingEnvironment.Cache.Remove(key);
        }

        public void ClearAll()
        {
            foreach (DictionaryEntry dictionaryEntry in HostingEnvironment.Cache)
                HostingEnvironment.Cache.Remove((string)dictionaryEntry.Key);
        }

        public void ClearAllByCacheKey(string cacheKey)
        {
            foreach (DictionaryEntry dictionaryEntry in HostingEnvironment.Cache)
            {
                string cacheKeyValue = (string)dictionaryEntry.Key;
                if (cacheKeyValue.Contains(cacheKey))
                {
                    HostingEnvironment.Cache.Remove((string)dictionaryEntry.Key);
                }
            }
        }

        public void ClearAllByContains(string cacheKey, string key)
        {
            foreach (DictionaryEntry dictionaryEntry in HostingEnvironment.Cache)
            {
                string cacheKeyValue = (string)dictionaryEntry.Key;
                if (cacheKeyValue.Contains(cacheKey) && cacheKeyValue.Contains(key))
                {
                    HostingEnvironment.Cache.Remove((string)dictionaryEntry.Key);
                }
            }
        }

        public T Get<T>(string key)
        {
            var obj = (T)HostingEnvironment.Cache.Get(key);
            if ((object)obj == null)
                obj = default(T);
            return obj;
        }

        public void Insert(string key, object data, TimeSpan TiempoEnExpirar)
        {
            HostingEnvironment.Cache.Insert(key, data, null, Cache.NoAbsoluteExpiration, TiempoEnExpirar, CacheItemPriority.Normal, null);
        }

        public void Add(string key, object data, TimeSpan TiempoEnExpirar)
        {
            HostingEnvironment.Cache.Add(key, data, null, Cache.NoAbsoluteExpiration, TiempoEnExpirar, CacheItemPriority.Normal, null);
        }
    }
}
