#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  配置插件接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion


using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSS.Tools.DirConfig
{
    /// <summary>
    /// 字典配置接口
    /// </summary>
    public interface IToolDirConfig
    {
        /// <summary>
        /// 添加字典配置
        /// </summary>
        /// <param name="key">配置关键字</param>
        /// <param name="dirConfig">配置具体信息</param>
        /// <param name="sourceName">来源名称</param>
        /// <typeparam name="TConfig">配置信息类型</typeparam>
        /// <returns></returns>
        Task<bool> SetDirConfig<TConfig>(string key, TConfig dirConfig, string sourceName);

        /// <summary>
        /// 获取字典配置
        /// </summary>
        /// <param name="key">配置关键字</param>
        /// <param name="sourceName">来源名称</param>
        /// <typeparam name="TConfig">配置信息类型</typeparam>
        /// <returns></returns>
        Task<TConfig> GetDirConfig<TConfig>(string key, string sourceName);

        /// <summary>
        /// 移除配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        Task RemoveDirConfig(string key, string sourceName);
    }

    public interface IToolListConfig
    {
        /// <summary>
        /// 添加列表配置项
        /// </summary>
        /// <param name="listKey">配置关键字</param>
        /// <param name="itemKey">项关键字</param>
        /// <param name="itemValue">配置具体信息</param>
        /// <param name="sourceName">来源名称</param>
        /// <typeparam name="TConfig">配置信息类型</typeparam>
        /// <returns></returns>
        Task<bool> SetItem<TConfig>(string listKey, string itemKey, TConfig itemValue, string sourceName);

        /// <summary>
        /// 获取列表配置
        /// </summary>
        /// <param name="listKey">配置关键字</param>
        /// <param name="sourceName">来源名称</param>
        /// <typeparam name="TConfig">配置信息类型</typeparam>
        /// <returns></returns>
        Task<List<ItemConfig<TConfig>>> GetList<TConfig>(string listKey, string sourceName);

        /// <summary>
        /// 获取列表数量
        /// </summary>
        /// <param name="listKey">配置关键字</param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        Task<int> GetCount(string listKey, string sourceName);

        /// <summary>
        /// 获取列表配置具体项
        /// </summary>
        /// <param name="listKey">配置关键字</param>
        /// <param name="itemKey">项Key</param>
        /// <param name="sourceName">来源名称</param>
        /// <typeparam name="TConfig">配置信息类型</typeparam>
        /// <returns></returns>
        Task<ItemConfig<TConfig>> GetItem<TConfig>(string listKey, string itemKey, string sourceName);

        /// <summary>
        /// 移除列表配置项
        /// </summary>
        /// <param name="listKey"></param>
        /// <param name="itemKey"></param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        Task RemoveItem(string listKey, string itemKey, string sourceName);
    }

    public class ItemConfig
    {
        /// <summary>
        ///  项Key
        /// </summary>
        public string key { get; set; }
    }

    public class ItemConfig<TConfig> : ItemConfig
    {
        /// <summary>
        /// 项值
        /// </summary>
        public TConfig value { get; set; }
    }
}