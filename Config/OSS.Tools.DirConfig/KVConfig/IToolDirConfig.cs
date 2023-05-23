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
        Task<TConfig?> GetDirConfig<TConfig>(string key, string sourceName);

        /// <summary>
        /// 移除配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sourceName">来源名称</param>
        /// <returns></returns>
        Task RemoveDirConfig(string key, string sourceName);
    }

    public class ItemConfig
    {
        /// <summary>
        ///  项Key
        /// </summary>
        public string key { get; set; } = string.Empty;
    }

    public class ItemConfig<TConfig> : ItemConfig
    {
        /// <summary>
        /// 项值
        /// </summary>
        public TConfig? value { get; set; }
    }
}