namespace OSS.Tools.TimerJob
{
    /// <summary>
    /// 任务基类
    ///       如果执行时间过长，重复触发时 当前任务还在进行中，则不做任何处理
    /// </summary>
    public abstract class BaseJobExecutor : Internal_BaseExecutor, IJobExecutor
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; protected set; }

        /// <summary>
        ///  列表任务执行者
        /// </summary>
        protected BaseJobExecutor(string jobName)
        {
            JobName = jobName;
        }


        #region 重写父类方法

        /// <inheritdoc />
        internal override Task InternalStartJob(CancellationToken cancellationToken)
        {
            return OnStarting(cancellationToken);
        }

        /// <inheritdoc />
        internal override Task InternalStopJob(CancellationToken cancellationToken)
        {
            return OnStopping(cancellationToken);
        }

        #endregion

        #region 扩展方法

        /// <summary>
        ///  任务执行
        /// </summary>
        protected abstract Task OnStarting(CancellationToken cancellationToken);

        /// <summary>
        ///  任务结束
        /// </summary>
        protected virtual Task OnStopping(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
        #endregion
    }
}
