#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  缓存插件辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;

namespace OSS.Tools.Cache
{
    /// <summary>
    /// 缓存的辅助类
    /// </summary>
    public static class CacheHelper
    {
        private static readonly DefaultToolCache defaultCache = new DefaultToolCache();

        /// <summary>
        /// 缓存来源提供者
        /// </summary>
        public static Func<string, IToolCache> CacheProvider { get; set; }

        /// <summary>
        /// 来源名称格式化
        /// </summary>
        public static Func<string, string> SourceFormat { get; set; }

        /// <summary>
        /// 通过来源名称获取
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static IToolCache GetCache(string sourceName= "default")
        {
            if (string.IsNullOrEmpty(sourceName))
                sourceName = "default";

            if (SourceFormat != null)
                sourceName = SourceFormat.Invoke(sourceName);

            return CacheProvider?.Invoke(sourceName) ?? defaultCache;
        }


        #region 缓存添加


        /// <summary> 
        /// 添加滚动过期缓存，如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="slidingExpiration">滚动过期时长，访问后自动延长</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns>是否添加成功</returns>
        public static Task<bool> SetAsync<T>(string key, T obj, TimeSpan slidingExpiration, string sourceName = "default")
        {
            return SetAsync(key, obj,new CacheTimeOptions(){sliding_expiration = slidingExpiration },  sourceName);
        }

        /// <summary>
        /// 添加固定过期时间缓存，如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="absoluteExpiration"> 固定过期时长，设置后到时过期 </param>
        /// <param name="sourceName">来源名称</param>
        /// <returns>是否添加成功</returns>
        public static Task<bool> SetAbsoluteAsync<T>(string key, T obj, TimeSpan absoluteExpiration, string sourceName = "default")
        {
            return SetAsync(key, obj, new CacheTimeOptions(){absolute_expiration_relative_to_now = absoluteExpiration}, sourceName);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="opt">缓存过期时间选项</param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static Task<bool> SetAsync<T>(string key, T obj, CacheTimeOptions opt, string sourceName = "default")
        {
            return GetCache(sourceName).SetAsync(key, obj, opt);
        }

        #endregion

        #region 缓存获取（简单）

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取缓存对象类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns>获取指定key对应的值 </returns>
        public static Task<T> GetAsync<T>(string key, string sourceName = "default")
        {
            return GetCache(sourceName).GetAsync<T>(key);
        }

        #endregion

     
        #region 缓存获取（击穿保护）

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
        public static Task<RType> GetOrSetAsync<RType>(string cacheKey, Func<Task<RType>> getFunc,
            TimeSpan slidingExpiration, int hitProtectedSeconds = 10, string sourceName = "default")
        {
            return GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions() { sliding_expiration = slidingExpiration }, null, hitProtectedSeconds, sourceName);
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
        public static Task<RType> GetOrSetAsync<RType>(string cacheKey, Func<Task<RType>> getFunc,TimeSpan slidingExpiration,
            Func<RType, bool> beforeSettingChecker, int hitProtectedSeconds = 10, 
            string sourceName = "default")
        {
            return GetOrSetAsync(cacheKey, getFunc,new CacheTimeOptions(){sliding_expiration = slidingExpiration}, beforeSettingChecker, hitProtectedSeconds, sourceName);
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
        public static Task<RType> GetOrSetAbsoluteAsync<RType>(string cacheKey, Func<Task<RType>> getFunc, TimeSpan absoluteExpiration, int hitProtectedSeconds = 10, string sourceName = "default")
        {
            return GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions() { absolute_expiration_relative_to_now = absoluteExpiration }, null
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
        public static Task<RType> GetOrSetAbsoluteAsync<RType>(string cacheKey, Func<Task<RType>> getFunc,TimeSpan absoluteExpiration,
            Func<RType, bool> beforeSettingChecker, int hitProtectedSeconds = 10,string sourceName = "default")
        {
            return GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions(){absolute_expiration_relative_to_now = absoluteExpiration}, beforeSettingChecker, hitProtectedSeconds, sourceName);
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
        public static async Task<RType> GetOrSetAsync<RType>(string cacheKey, Func<Task<RType>> getFunc, CacheTimeOptions cacheTimeOpt,
            Func<RType, bool> beforeSettingChecker = null, int hitProtectedSeconds = 10, string sourceName = "default")
        {
            if (getFunc == null)
                throw new ArgumentNullException("获取原始数据方法(getFunc)不能为空!");

            var isProtected = hitProtectedSeconds > 0;
            if (isProtected)
            {
                var obj = await GetAsync<ProtectCacheData<RType>>(cacheKey, sourceName);
                if (obj != null)
                    return obj.data;
            }
            else
            {
                var obj = await GetAsync<RType>(cacheKey, sourceName);
                if (obj != null && !obj.Equals(default(RType)))
                    return obj;
            }

            var data = await getFunc();
            var checkRes = beforeSettingChecker == null ? (data != null && !data.Equals(default)) : beforeSettingChecker.Invoke(data);

            if (isProtected)
            {
                if (!checkRes)
                {
                    cacheTimeOpt = new CacheTimeOptions()
                    {
                        absolute_expiration_relative_to_now = TimeSpan.FromSeconds(hitProtectedSeconds)
                    };
                }
                var cacheData = new ProtectCacheData<RType>(data);
                await SetAsync(cacheKey, cacheData, cacheTimeOpt, sourceName);
                return data;
            }

            if (checkRes)
            {
                await SetAsync(cacheKey, data, cacheTimeOpt, sourceName);
            }
            return data;
        }


        #endregion

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sourceName">来源名称</param>
        /// <returns>是否成功</returns>
        public static Task<bool> RemoveAsync(string key, string sourceName = "default")
        {
            return GetCache(sourceName).RemoveAsync(key);
        }
        
        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="sourceName">来源名称</param>
        /// <returns>是否成功</returns>
        public static Task<bool> RemoveAsync(string[] keys, string sourceName = "default")
        {
            return GetCache(sourceName).RemoveAsync(keys);
        }



        #region 过时方法

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取缓存对象类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns>获取指定key对应的值 </returns>
        [Obsolete("请使用 GetAsync")]
        public static T Get<T>(string key, string sourceName = "default")
        {
            return GetAsync<T>(key, sourceName).Result;
        }


        [Obsolete("请使用 RemoveAsync")]
        public static bool Remove(string key, string sourceName = "default")
        {
            return RemoveAsync(key,sourceName).Result;
        }
        
        /// <summary> 
        /// 添加滚动过期缓存，如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="slidingExpiration">滚动过期时长，访问后自动延长</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns>是否添加成功</returns>
        [Obsolete("请使用 SetAsync")]
        public static bool Set<T>(string key, T obj, TimeSpan slidingExpiration, string sourceName = "default")
        {
            return SetAsync(key, obj, slidingExpiration, sourceName).Result;
        }

        /// <summary>
        /// 添加固定过期时间缓存，如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="absoluteExpiration"> 固定过期时长，设置后到时过期 </param>
        /// <param name="sourceName">来源名称</param>
        /// <returns>是否添加成功</returns>
        [Obsolete("请使用 SetAbsoluteAsync")]
        public static bool Set<T>(string key, T obj, DateTime absoluteExpiration, string sourceName = "default")
        {
            return SetAbsoluteAsync(key, obj, TimeSpan.FromTicks((absoluteExpiration - DateTime.Now).Ticks), sourceName).Result;
        }



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
        [Obsolete]
        public static Task<RType> GetOrSetAsync<RType>(string cacheKey, Func<Task<RType>> getFunc,
            TimeSpan slidingExpiration, string sourceName)
        {
            return GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions() { sliding_expiration = slidingExpiration }
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
        [Obsolete]
        public static Task<RType> GetOrSetAbsoluteAsync<RType>(string cacheKey, Func<Task<RType>> getFunc,
            TimeSpan absoluteExpiration, string sourceName)
        {
            return GetOrSetAsync(cacheKey, getFunc, new CacheTimeOptions()
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
        [Obsolete]
        public static async Task<RType> GetOrSetAsync<RType>(string cacheKey, Func<Task<RType>> createFunc, CacheTimeOptions opt, string sourceName )
        {
            var obj = await GetAsync<RType>(cacheKey, sourceName);
            if (obj != null && !obj.Equals(default(RType)))
                return obj;

            if (createFunc == null)
                return default;

            var data = await createFunc.Invoke();
            if (data == null || data.Equals(default(RType)))
                return data;

            await SetAsync(cacheKey, data, opt, sourceName);
            return data;
        }

        #endregion

        #endregion
    }


    internal class ProtectCacheData<TT>
    {
        public ProtectCacheData(TT data)
        {
            this.data = data;
        }
        public TT data { get;  }
    }
}
