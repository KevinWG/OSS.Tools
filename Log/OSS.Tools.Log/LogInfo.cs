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
        /// 空构造函数
        /// </summary>
        public LogInfo()
        {
        }

        /// <summary>
        /// 日志构造函数
        /// </summary>
        /// <param name="loglevel"></param>
        /// <param name="logMsg"></param>
        /// <param name="msgKey"></param>
        /// <param name="sourceName"></param>
        internal LogInfo(LogLevelEnum loglevel, object logMsg, string msgKey = null, string sourceName = "default")
        {
            level = loglevel;
            source_name = sourceName;
            this.msg_body = logMsg;
            msg_key = msgKey;
        }

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevelEnum level { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string source_name { get; set; }

        /// <summary>
        ///   key值  可以是自定义的标识  
        ///   根据此字段可以处理当前module下不同复杂日志信息
        /// </summary>
        public string msg_key { get; set; }

        /// <summary>
        /// 日志信息  可以是复杂类型  如 具体实体类
        /// </summary>
        public object msg_body { get; set; }

        /// <summary>
        ///  日志跟踪编号
        /// </summary>
        public string trace_num { get; set; }
    }

}
