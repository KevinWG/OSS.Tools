namespace OSS.Tools.Num;

/// <summary>
/// 生成兼容js的编号（53bit）
/// </summary>
public class JSSafeSnowNumGenerator : BaseSnowNumGenerator
{
    // 符号位(1位) + Timestamp(41位 最长70年) + WorkId( 3 位) + sequence （9 位）  = 编号Id (53位)

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workId">当前的工作id 最大值不能超过（7   2^3-1）</param>
    public JSSafeSnowNumGenerator(int workId) : base(workId, 9, 3)
    {
    }
}