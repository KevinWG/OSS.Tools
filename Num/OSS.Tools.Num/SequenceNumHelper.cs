#region Copyright (C) 2019 (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局辅助类 - 序列数字编码生成辅助
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

namespace OSS.Tools.Num;

public static class SequenceNumHelper
{
    /// <summary>
    ///   序列实现来源提供者
    /// </summary>
    public static Func<string, ISequenceNumGenerator>? SequenceProvider { get; set; }

    /// <summary>
    /// 通过来源名称获取 序列编码 实例
    /// </summary>
    /// <param name="sourceName"></param>
    /// <returns></returns>
    public static ISequenceNumGenerator GetSequenceNumGenerator(string sourceName)
    {
        var sImpl = SequenceProvider?.Invoke(sourceName);
        return sImpl ?? throw new NotImplementedException("并未发现有效的  ISequenceNumGenerate 接口实现，请检查SequenceProvider是否有正确配置或返回是否正确!");
    }

    /// <summary>
    ///  获取新的序列数（区间）
    /// </summary>
    /// <param name="sequenceKey">序列主键</param>
    /// <param name="sourceName">来源</param>
    /// <returns></returns>
    public static async Task<long> New(string sequenceKey, string sourceName = "")
    {
        return (await GetSequenceNumGenerator(sourceName).New(sequenceKey, 1)).start;
    }

    /// <summary>
    ///  获取新的序列数（区间）
    /// </summary>
    /// <param name="sequenceKey">序列主键</param>
    /// <param name="count">获取个数</param>
    /// <param name="sourceName">来源</param>
    /// <returns></returns>
    public static Task<(long start, long end)> New(string sequenceKey, int count, string sourceName = "")
    {
        return GetSequenceNumGenerator(sourceName).New(sequenceKey, count);
    }
}