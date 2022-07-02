using System;
using System.Threading.Tasks;

namespace OSS.Tools.Cache
{
    /// <summary>
    ///    缓存委托方法扩展处理
    /// </summary>
    public static class FuncCacheExtension
    {
        #region 缓存获取

        /// <summary>
        /// 获取缓存数据，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">如果不存在，通过此方法获取原始数据添加缓存</param>
        /// <param name="slidingExpiration">滚动过期时长，访问后自动延长</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> GetOrSetAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey,
            TimeSpan slidingExpiration, string sourceName = "default")
        {
            return GetOrSetAsync(getFunc, cacheKey, new CacheTimeOptions() { sliding_expiration = slidingExpiration }
            , sourceName);
        }

        /// <summary>
        /// 获取缓存数据，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="absoluteExpiration">固定过期时长，设置后到时过期</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> GetOrSetAbsoluteAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey, 
            TimeSpan absoluteExpiration, string sourceName = "default")
        {
            return GetOrSetAsync(getFunc, cacheKey,  new CacheTimeOptions()
            {
                absolute_expiration_relative_to_now = absoluteExpiration
            }, sourceName);
        }

        /// <summary>
        /// 获取缓存数据，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="createFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="opt"></param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> GetOrSetAsync<RType>(this Func<Task<RType>> createFunc, string cacheKey, CacheTimeOptions opt, string sourceName = "default")
        {
            return CacheHelper.GetOrSetAsync(cacheKey, createFunc, opt, sourceName);
        }

        #endregion

        #region 缓存获取（击穿保护）

        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="slidingExpiration">滚动过期时长，访问后自动延长</param>
        /// <param name="hitProtectedCondition">缓存击穿保护触发条件</param>
        /// <param name="protectedSeconds">缓存击穿保护秒数</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> GetOrSetAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey, TimeSpan slidingExpiration,
            Func<RType, bool> hitProtectedCondition, int protectedSeconds = 10,
            string sourceName = "default")
        {
            return GetOrSetAsync(getFunc, cacheKey, new CacheTimeOptions() { sliding_expiration = slidingExpiration }, hitProtectedCondition, protectedSeconds, sourceName);
        }

        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">如果不存在，通过此方法获取原始数据添加缓存</param>
        /// <param name="absoluteExpiration">固定过期时长，设置后到时过期</param>
        /// <param name="hitProtectedCondition">缓存击穿保护触发条件</param>
        /// <param name="protectedSeconds">缓存击穿保护秒数</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> GetOrSetAbsoluteAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey,  TimeSpan absoluteExpiration,
            Func<RType, bool> hitProtectedCondition, int protectedSeconds = 10,
            string sourceName = "default")
        {
            return GetOrSetAsync(getFunc, cacheKey,  new CacheTimeOptions() { absolute_expiration_relative_to_now = absoluteExpiration }, hitProtectedCondition, protectedSeconds, sourceName);
        }

        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="getFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="cacheTimeOpt">滚动过期时长，访问后自动延长，如果同时设置固定过期，则只能在固定时长范围内延长</param>
        /// <param name="hitProtectedCondition">缓存击穿保护触发条件</param>
        /// <param name="protectedSeconds">缓存击穿保护秒数</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static  Task<RType> GetOrSetAsync<RType>(this Func<Task<RType>> getFunc ,string cacheKey
            , CacheTimeOptions cacheTimeOpt, Func<RType, bool> hitProtectedCondition, int protectedSeconds=10, string sourceName = "default")
        {
            return CacheHelper.GetOrSetAsync(cacheKey, getFunc, cacheTimeOpt, hitProtectedCondition, protectedSeconds, sourceName);
        }

        #endregion

    }
}
