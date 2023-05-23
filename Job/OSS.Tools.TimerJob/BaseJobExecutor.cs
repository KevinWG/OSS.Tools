namespace OSS.Tools.TimerJob
{
    /// <summary>
    /// 任务基类
    ///       如果执行时间过长，重复触发时 当前任务还在进行中，则不做任何处理
    /// </summary>
    public abstract class BaseJobExecutor : Internal_BaseExecutor
    {
        /// <summary>
        ///  列表任务执行者
        /// </summary>
        protected BaseJobExecutor(string jobName) :base(jobName)
        {
        }

        internal override Task InternalStartJob(CancellationToken cancellationToken)
        {
            return OnStarting(cancellationToken);
        }

        /// <summary>
        ///  任务执行
        /// </summary>
        protected abstract Task OnStarting(CancellationToken cancellationToken);

    }
}
