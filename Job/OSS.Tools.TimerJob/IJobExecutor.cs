namespace OSS.Tools.TimerJob
{
    /// <summary>
    ///  任务 提供者 接口
    /// </summary>
    public interface IJobExecutor
    {
        /// <summary>
        ///  任务名称
        /// </summary>
        string JobName { get; }

        /// <summary>
        ///  运行状态
        /// </summary>
        StatusFlag StatusFlag { get; }


        /// <summary>
        /// 开始任务
        /// </summary>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        ///  结束任务
        /// </summary>
        Task StopAsync(CancellationToken cancellationToken);
    }


    /// <summary>
    ///  状态标识
    /// </summary>
    public enum StatusFlag
    {
        /// <summary>
        ///  等待执行
        /// </summary>
        Waiting,

        /// <summary>
        ///  执行中
        /// </summary>
        Running,

        /// <summary>
        ///  正在结束
        /// </summary>
        Stopping,

        /// <summary>
        ///  已结束
        /// </summary>
        Stopped
    }
}
