#region Copyright (C) 2024 (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局辅助类 - 随机数字编码生成辅助
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System.Text;

namespace OSS.Tools.Num;

/// <summary>
///  随机动态数字串 辅助类
/// </summary>
public static class RandomNumHelper
{
    private static readonly Random _rnd = new(DateTime.Now.Millisecond);

    /// <summary>
    /// 随机数字
    /// </summary>
    /// <returns></returns>
    public static string RandomNum(int length = 4)
    {
        var num = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            num.Append(_rnd.Next(0, 9));
        }

        return num.ToString();
    }

}

