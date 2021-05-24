#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  缓存插件接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;

namespace OSS.Tools.Cache
{
    /// <summary>
    /// 缓存插件接口
    /// </summary>
    public interface IToolCache
    {
        /// <summary>
        /// 添加固定过期时间缓存,如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="cacheOpt"></param>
        /// <returns>是否添加成功</returns>
        Task<bool> SetAsync<T>(string key, T obj, CacheTimeOptions cacheOpt);

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取缓存对象类型</typeparam>
        /// <param name="key">key</param>
        /// <returns>获取指定key对应的值 </returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否成功</returns>
        Task<bool> RemoveAsync(params string[] key);
    }
}