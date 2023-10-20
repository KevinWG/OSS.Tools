namespace OSS.Tools.DirConfig;

/// <summary>
/// 列表配置通用存储获取信息
/// </summary>
public static class ListConfigHelper
{
    public static IToolListConfig DefaultDirTool
    {
        get;
        set;
    } = new DefaultToolListConfig();

    /// <summary>
    ///   配置信息来源提供者
    /// </summary>
    public static Func<string, IToolListConfig>? ToolProvider { get; set; }

    /// <summary>
    /// 来源名称格式化
    /// </summary>
    public static Func<string, string>? SourceFormat { get; set; }

    /// <summary>
    /// 通过来源名称获取
    /// </summary>
    /// <param name="sourceName"></param>
    /// <returns></returns>
    private static IToolListConfig GetDirConfig(string sourceName)
    {
        if (SourceFormat != null)
            sourceName = SourceFormat.Invoke(sourceName);

        return ToolProvider?.Invoke(sourceName) ?? DefaultDirTool;
    }


    /// <summary>
    /// 添加列表配置项
    /// </summary>
    /// <param name="listKey">配置关键字</param>
    /// <param name="itemKey">项关键字</param>
    /// <param name="itemValue"></param>
    /// <param name="sourceName">来源名称</param>
    /// <typeparam name="TConfig">配置信息类型</typeparam>
    /// <returns></returns>
    public static Task<bool> SetItem<TConfig>(string listKey, string itemKey, TConfig itemValue, string sourceName = "") 
    {
        return GetDirConfig(sourceName).SetItem(listKey, itemKey, itemValue, sourceName);
    }


    /// <summary>
    /// 获取列表配置
    /// </summary>
    /// <param name="listKey">配置关键字</param>
    /// <param name="sourceName">来源名称</param>
    /// <typeparam name="TConfig">配置信息类型</typeparam>
    /// <returns></returns>
    public static Task<List<ItemConfig<TConfig>>> GetList<TConfig>(string listKey, string sourceName = "")
    {
        return GetDirConfig(sourceName).GetList<TConfig>(listKey, sourceName);
    }


    /// <summary>
    /// 获取列表数量
    /// </summary>
    /// <param name="listKey">配置关键字</param>
    /// <param name="sourceName">来源名称</param>
    /// <returns></returns>
    public static Task<int> GetCount(string listKey, string sourceName="")
    {
        return GetDirConfig(sourceName).GetCount(listKey, sourceName);
    }

    /// <summary>
    /// 获取列表配置具体项
    /// </summary>
    /// <param name="listKey">配置关键字</param>
    /// <param name="itemKey">项Key</param>
    /// <param name="sourceName">来源名称</param>
    /// <typeparam name="TConfig">配置信息类型</typeparam>
    /// <returns></returns>
    public static Task<ItemConfig<TConfig>?> GetItem<TConfig>(string listKey, string itemKey, string sourceName = "")
    {
        return GetDirConfig(sourceName).GetItem<TConfig>(listKey, itemKey,sourceName);
    }

    /// <summary>
    /// 移除列表配置项
    /// </summary>
    /// <param name="listKey"></param>
    /// <param name="itemKey"></param>
    /// <param name="sourceName">来源名称</param>
    /// <returns></returns>
    public static Task RemoveItem(string listKey, string itemKey, string sourceName = "")
    {
        return GetDirConfig(sourceName).RemoveItem(listKey, itemKey, sourceName);
    }


}