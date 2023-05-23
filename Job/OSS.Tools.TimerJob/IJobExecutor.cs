namespace OSS.Tools.TimerJob
{
    /// <summary>
    ///  任务 提供者 接口
    /// </summary>
    public interface IJobExecutor
    {
        /// <summary>
        ///  运行状态
        /// </summary>
        StatusFlag StatusFlag { get; }

        /// <summary>
        ///  工作名称
        /// </summary>
        string JobName { get; }

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

    internal class InternalExecutor : BaseJobExecutor
    {
        private readonly Func<CancellationToken, Task> _startAction;
        private readonly Func<CancellationToken, Task> _stopAction;
           
        /// <inheritdoc />
        public InternalExecutor(string jobName, Func<CancellationToken, Task> startAction, Func<CancellationToken, Task> stopAction)
            :base(jobName)
        {
            _startAction = startAction;
            _stopAction = stopAction;
        }
            
        protected override Task OnStarting(CancellationToken cancellationToken)
        {
            return _startAction?.Invoke(cancellationToken) ?? Task.CompletedTask;
        }

        protected override Task OnStopping(CancellationToken cancellationToken)
        {
            return _stopAction?.Invoke(cancellationToken) ?? Task.CompletedTask;
        }
      
    }
}
