#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  日志插件默认实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

namespace OSS.Tools.Log
{

    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevelEnum
    {
        /// <summary>
        /// 跟踪查看
        /// </summary>
        Trace,

        /// <summary>
        /// 信息
        /// </summary>
        Info,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,
    }

    /// <summary>
    /// 日志实体
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        /// 日志构造函数
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="logMsg"></param>
        /// <param name="msgKey"></param>
        /// <param name="sourceName"></param>
        internal LogInfo(LogLevelEnum logLevel, object logMsg, string? msgKey = null, string? sourceName = null)
        {
            level    = logLevel;
            msg_body = logMsg;
            msg_key  = msgKey;

            source_name = sourceName;

            log_time = InternalLogHelper.GetUtcSeconds();
        }

        /// <summary>
        /// 日志信息  可以是复杂类型  如 具体实体类
        /// </summary>
        public object msg_body { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevelEnum level { get; set; }

        /// <summary>
        ///  日志记录时间 (UTC Seconds)
        /// </summary>
        public long log_time { get; set; }


        /// <summary>
        /// 日志来源名称
        /// </summary>
        public string? source_name { get; set; }

        /// <summary>
        ///   key值  可以是自定义的标识  
        ///   根据此字段可以处理当前module下不同复杂日志信息
        /// </summary>
        public string? msg_key { get; set; }

        /// <summary>
        ///  日志跟踪编号
        /// </summary>
        public string? trace_no { get; set; }
    }


    internal static class InternalLogHelper
    {
        private static readonly long startTicks = new DateTime(1970, 1, 1).Ticks;

        /// <summary>获取距离 1970-01-01（格林威治时间）的秒数</summary>
        /// <returns></returns>
        public static long GetUtcSeconds() => (DateTime.Now.ToUniversalTime().Ticks - startTicks) / 10000000L;

    }

}
