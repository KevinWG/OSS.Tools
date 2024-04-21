#region Copyright (C) 2019 (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局辅助类 - 编码生成辅助
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion
namespace OSS.Tools.Num;

/// <summary>
///  雪花算法唯一数字编码生成辅助类
/// </summary>
public static class SnowNumHelper
{
    private static readonly SnowNumGenerator      _generator      = new SnowNumGenerator(0);
    private static readonly JSSafeSnowNumGenerator _smallGenerator = new JSSafeSnowNumGenerator(0);

    /// <summary>
    ///  获取 twitter 的snowflake唯一Id算法实例(排除机器位)
    /// </summary>
    /// <returns></returns>
    public static SnowNumGenerator GetSnowNumGenerator(int workId)
    {
        return new SnowNumGenerator(workId);
    }

    /// <summary>
    ///  获取 twitter 的snowflake唯一Id算法实例(排除机器位)
    ///   id大小不超过 2^52次方-1
    /// </summary>
    /// <returns></returns>
    public static JSSafeSnowNumGenerator GetSmallSnowNumGenerator(int workId)
    {
        return new JSSafeSnowNumGenerator(workId);
    }


    /// <summary>
    /// twitter 的snowflake算法 workid=0 的算法实例：
    /// 生成的Id(排除机器位)
    /// </summary>
    /// <returns></returns>
    public static long New()
    {
        return _generator.NewNum();
    }
    
    /// <summary>
    /// twitter 的snowflake算法 workid=0 的算法实例：
    /// 生成的大小不超过 2^52次方-1 的Id(排除机器位)
    /// </summary>
    /// <returns></returns>
    public static long JSSafeNew()
    {
        return _smallGenerator.NewNum();
    }
}