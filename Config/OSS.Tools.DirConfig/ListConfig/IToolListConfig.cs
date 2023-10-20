namespace OSS.Tools.DirConfig;

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
    Task<ItemConfig<TConfig>?> GetItem<TConfig>(string listKey, string itemKey, string sourceName);

    /// <summary>
    /// 移除列表配置项
    /// </summary>
    /// <param name="listKey"></param>
    /// <param name="itemKey"></param>
    /// <param name="sourceName">来源名称</param>
    /// <returns></returns>
    Task RemoveItem(string listKey, string itemKey, string sourceName);
}