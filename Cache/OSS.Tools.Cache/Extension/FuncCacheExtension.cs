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
        /// <param name="getFunc">获取原始数据方法</param>
        /// <param name="slidingExpiration">滚动过期时长，访问后自动延长</param>
        /// <param name="hitProtectedSeconds">缓存击穿保护秒数，默认值10。</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> WithCacheAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey,
            TimeSpan slidingExpiration, int hitProtectedSeconds = 10, string sourceName = "")
        {
            return CacheHelper.GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions() { sliding_expiration = slidingExpiration }, null, hitProtectedSeconds, sourceName);
        }

        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">获取原始数据方法</param>
        /// <param name="slidingExpiration">滚动过期时长，访问后自动延长</param>
        /// <param name="beforeSettingChecker">设置缓存前验证原始数据，false - 不满足设置缓存条件（会触发验证缓存击穿保护）， true - 满足设置缓存条件</param>
        /// <param name="hitProtectedSeconds">缓存击穿保护秒数，默认值10。</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> WithCacheAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey, TimeSpan slidingExpiration,
            Func<RType, bool> beforeSettingChecker, int hitProtectedSeconds = 10,
            string sourceName = "")
        {
            return CacheHelper.GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions() { sliding_expiration = slidingExpiration }, beforeSettingChecker, hitProtectedSeconds, sourceName);
        }


        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">获取原始数据方法</param>
        /// <param name="absoluteExpiration">固定过期时长，设置后到时过期</param>
        /// <param name="hitProtectedSeconds">缓存击穿保护秒数，默认值10。</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> WithAbsoluteCacheAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey, TimeSpan absoluteExpiration, int hitProtectedSeconds = 10, string sourceName = "")
        {
            return CacheHelper.GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions() { absolute_expiration_relative_to_now = absoluteExpiration }, null
                , hitProtectedSeconds, sourceName);
        }


        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">获取原始数据方法</param>
        /// <param name="absoluteExpiration">固定过期时长，设置后到时过期</param>
        /// <param name="beforeSettingChecker">设置缓存前验证原始数据，false - 不满足设置缓存条件（会触发验证缓存击穿保护）， true - 满足设置缓存条件</param>
        /// <param name="hitProtectedSeconds">缓存击穿保护秒数，默认值10。</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> WithAbsoluteCacheAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey, TimeSpan absoluteExpiration,
            Func<RType, bool> beforeSettingChecker, int hitProtectedSeconds = 10, string sourceName = "")
        {
            return CacheHelper.GetOrSetAsync(cacheKey, getFunc,
                new CacheTimeOptions()
                {
                    absolute_expiration_relative_to_now = absoluteExpiration
                },
                beforeSettingChecker, hitProtectedSeconds, sourceName);
        }



        /// <summary>
        /// 获取缓存数据，如果没有则添加
        /// </summary>
        /// <typeparam name="RType">数据类型</typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="getFunc">获取原始数据方法</param>
        /// <param name="cacheTimeOpt">缓存时间信息</param>
        /// <param name="beforeSettingChecker">设置缓存前验证原始数据，false - 不满足设置缓存条件（会触发验证缓存击穿保护）， true - 满足设置缓存条件</param>
        /// <param name="hitProtectedSeconds">缓存击穿保护秒数，默认值10。</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        public static Task<RType> WithCacheAsync<RType>(this Func<Task<RType>> getFunc, string cacheKey, CacheTimeOptions cacheTimeOpt,
            Func<RType, bool> beforeSettingChecker = null, int hitProtectedSeconds = 10, string sourceName = "")
        {
            return CacheHelper.GetOrSetAsync(cacheKey, getFunc, cacheTimeOpt, beforeSettingChecker, hitProtectedSeconds, sourceName);
        }

        #endregion

    }
}
