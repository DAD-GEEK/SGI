using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Across.CacheStorage
{
    public interface ICacheStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void Clear(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        void Insert(string key, object data, TimeSpan TiempoEnExpirar);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        void Add(string key, object data, TimeSpan TiempoEnExpirar);

        /// <summary>
        /// 
        /// </summary>
        void ClearAll();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="key"></param>
        void ClearAllByContains(string cacheKey, string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        void ClearAllByCacheKey(string cacheKey);
    }
}
