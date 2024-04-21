#region Copyright (C) 2024 (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局辅助类 - 序列数字生成接口定义
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

namespace OSS.Tools.Num;

/// <summary>
/// 序列数字生成接口定义
/// </summary>
public interface ISequenceNumGenerator
{
    /// <summary>
    ///  获取新的序列数值（区间）
    /// </summary>
    /// <param name="sequenceKey">序列主键</param>
    /// <param name="count">获取个数</param>
    /// <returns></returns>
    Task<(long start,long end)> New(string sequenceKey,int count);
}