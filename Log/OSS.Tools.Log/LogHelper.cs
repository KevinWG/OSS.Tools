#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  日志插件实体及辅助类
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
    /// 日志写来源
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        ///  默认日志工具
        /// </summary>
        private static readonly IToolLog _defaultLogTool = new DefaultToolLog();

        /// <summary>
        ///   日志工具提供者
        /// </summary>
        public static Func<string?, IToolLog>? LogToolProvider { get; set; }

        /// <summary>
        ///  对日志内容格式化
        ///     例如可以 初始化日志Id，加工日志内容 等
        /// </summary>
        public static Action<LogInfo>? LogFormat { get; set; }

        /// <summary>
        /// 通过来源名称获取日志来源实例
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static IToolLog GetLogWriter(string? sourceName)
        {
            return LogToolProvider?.Invoke(sourceName) ?? _defaultLogTool;
        }


        /// <summary>
        /// 记录信息
        /// </summary>
        /// <param name="msg"> 日志信息  </param>
        /// <param name="msgKey">  关键值  </param>
        /// <param name="sourceName"> 来源名称 </param>
        public static Task<string?> Info(object msg, string? msgKey = null, string? sourceName = "")
        {
            return Log(new LogInfo(LogLevelEnum.Info, msg, msgKey, sourceName));
        }

        /// <summary>
        /// 记录警告，用于未处理异常的捕获
        /// </summary>
        /// <param name="msg"> 日志信息  </param>
        /// <param name="msgKey">  关键值  </param>
        /// <param name="sourceName">来源名称</param>
        public static Task<string?> Warning(object msg, string? msgKey = null, string? sourceName = "")
        {
            return Log(new LogInfo(LogLevelEnum.Warning, msg, msgKey, sourceName));
        }

        /// <summary>
        /// 记录错误，用于捕获到的异常信息记录
        /// </summary>
        /// <param name="msg"> 日志信息  </param>
        /// <param name="msgKey">  关键值  </param>
        /// <param name="sourceName">来源名称</param>
        public static Task<string?> Error(object msg, string? msgKey = null, string? sourceName = "")
        {
            return Log(new LogInfo(LogLevelEnum.Error, msg, msgKey, sourceName));
        }

        /// <summary>
        /// 记录错误，用于捕获到的异常信息记录
        /// </summary>
        /// <param name="msg"> 日志信息  </param>
        /// <param name="msgKey">  关键值  </param>
        /// <param name="sourceName">来源名称</param>
        public static Task<string?> Trace(object msg, string? msgKey = null, string? sourceName = "")
        {
            return Log(new LogInfo(LogLevelEnum.Trace, msg, msgKey, sourceName));
        }
        
        /// <summary>
        ///   记录日志
        /// </summary>
        /// <param name="info"></param>
        private static async Task<string?> Log(LogInfo info)
        {
            try
            {
                LogFormat?.Invoke(info);
                
                await GetLogWriter(info.source_name).WriteLogAsync(info);
            }
            catch
            {
                // 写日志本身不能再出错误，这里做特殊处理
            }
            return info.trace_no;
        }

    }
}
