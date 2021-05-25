#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  缓存插件默认实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace OSS.Tools.Cache
{
    /// <summary>
    /// 默认缓存实现
    /// </summary>
    public class DefaultToolCache : IToolCache
    {
        private static readonly MemoryCache _cache=new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        ///  添加缓存,如果存在则更新为新值
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="cacheOpt"></param>
        /// <returns>是否添加成功</returns>
        public Task<bool> SetAsync<T>(string key, T obj, CacheTimeOptions cacheOpt)
        {
            if (!cacheOpt.AbsoluteExpirationRelativeToNow.HasValue && !cacheOpt.SlidingExpiration.HasValue)
                throw new ArgumentNullException("cacheOpt", "缓存过期时间不正确,需要设置固定过期时间或者相对过期时间");

            var opt = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheOpt.AbsoluteExpirationRelativeToNow,
                SlidingExpiration               = cacheOpt.SlidingExpiration
            };

            _cache.Set(key, obj, opt);
            return Task.FromResult(true);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取缓存对象类型</typeparam>
        /// <param name="key">key</param>
        /// <returns>获取指定key对应的值 </returns>
        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(_cache.Get<T>(key));
        }
        
        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否成功</returns>
        public Task<bool> RemoveAsync(params string[] keys)
        {
            foreach (var key in keys)
            {
                _cache.Remove(key);
            }
            return Task.FromResult(true);
        }
    }
}
