#region Copyright (C) 2019 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局辅助类 - 唯一数字编号生成类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion


namespace OSS.Tools.Num;

/// <summary>
///  唯一编码生成类
/// </summary>
public class SnowNumGenerator : BaseSnowNumGenerator
{
    // 符号位(1位) + Timestamp(41位 最长70年) + WorkId(10) + sequence(12) = 编号Id (64位)

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workId">当前的工作id 最大值不能超过（2^10 - 1）</param>
    public SnowNumGenerator(int workId) : base(workId, 12, 10)
    {
    }
}