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

using System.Text;
using System.Threading.Tasks.Dataflow;

namespace OSS.Tools.Log
{
    /// <summary>
    /// 系统默认写日志来源
    /// </summary>
    public class DefaultToolLog : IToolLog
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultToolLog()
        {
        }
        
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="info"></param>
        public Task WriteLogAsync(LogInfo info)
        {
            _asyncBlock.Post(info);
            return Task.CompletedTask;
        }


        private static readonly object _obj = new();
        private static readonly ActionBlock<LogInfo> _asyncBlock = new((info) =>
        {
            try
            {
                var logTime = info.log_time > 0
                    ? DateTimeOffset.FromUnixTimeSeconds(info.log_time).LocalDateTime
                    : DateTime.Now;

                lock (_obj)
                {
                    var filePath = getLogFilePath(info.source_name, info.level, logTime);
                    using (var sw = new StreamWriter(new FileStream(filePath, FileMode.Append, FileAccess.Write),
                               Encoding.UTF8))
                    {
                        sw.WriteLine("{0:T}    TraceNo:{1}    Key:{2}   Detail:{3}\r\n",
                            logTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            info.trace_no, info.msg_key, info.msg_body);
                    }
                }
            }
            catch
            {
                //  写日志本身不能再报异常，这里特殊处理
            }
        });

        private static string getLogFilePath(string? sourceName, LogLevelEnum level, DateTime logTime)
        {
            var moduleName = string.IsNullOrEmpty(sourceName) ? "default" : sourceName;

            var dirPath = Path.Combine(GetBaseDirPath(), string.Concat(moduleName, "_", level), logTime.ToString("yyyyMM"));

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var fileName = string.Concat(logTime.ToString("yyyy-MM-dd-HH-"), Math.Floor(logTime.Minute / 10M), "0.txt");
            return Path.Combine(dirPath, fileName);
        }

        private static string _logBaseDirPath = string.Empty;

        private static string GetBaseDirPath()
        {
            if (!string.IsNullOrEmpty(_logBaseDirPath))
            {
                return _logBaseDirPath;
            }

            _logBaseDirPath = Path.Combine(AppContext.BaseDirectory, "logs");

            if (!Directory.Exists(_logBaseDirPath))
                Directory.CreateDirectory(_logBaseDirPath);

            return _logBaseDirPath;
        }

    }
}
