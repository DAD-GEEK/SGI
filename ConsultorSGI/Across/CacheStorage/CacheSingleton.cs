namespace Across.CacheStorage
{
    public sealed class CacheSingleton
    {
        private static ICacheStorage cacheStorage;
        public static ICacheStorage InstanciarCache
        {
            get
            {
                if (cacheStorage == null)
                    cacheStorage = new CacheStorage();

                return cacheStorage;
            }
        }

    }
}
