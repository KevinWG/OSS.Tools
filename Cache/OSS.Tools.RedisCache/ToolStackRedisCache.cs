﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  Redis 缓存辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Newtonsoft.Json;
using OSS.Tools.Cache;
using System.Text;

namespace OSS.Tools.RedisCache
{
    /// <summary>
    ///   Redis  缓存工具实现
    /// </summary>
    public class ToolStackRedisCache : IToolCache
    {
        private readonly Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache _cache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="optionsAccessor"></param>
        public ToolStackRedisCache(RedisCacheOptions optionsAccessor)
        {
            _cache = new Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache(optionsAccessor);
        }

        /// <summary>
        ///  设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="cacheOpt"></param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T obj, CacheTimeOptions cacheOpt)
        {
            var text  = JsonConvert.SerializeObject(obj);
            var bytes = Encoding.UTF8.GetBytes(text);

            await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = cacheOpt.absolute_expiration_relative_to_now,
                SlidingExpiration               = cacheOpt.sliding_expiration,
                AbsoluteExpiration              = cacheOpt.absolute_expiration
            });
            return true;
        }

        /// <summary>
        ///  获取缓存实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T?> GetAsync<T>(string key)
        {
            var bytes = await _cache.GetAsync(key);
            var text  = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<T>(text);
        }

        /// <summary>
        ///  移除缓存实现
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(params string[] keys)
        {
            foreach (var key in keys)
            {
                await _cache.RemoveAsync(key);
            }
            return true;
        }
    }
}
