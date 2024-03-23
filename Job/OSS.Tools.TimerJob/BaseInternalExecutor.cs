namespace OSS.Tools.TimerJob
{
    /// <summary>
    /// 任务基类
    ///       如果执行时间过长，重复触发时 当前任务还在进行中，则不做任何处理
    /// </summary>
    public abstract class Internal_BaseExecutor 
    {
        #region 开始结束异步方法

        private bool _isRunning;
        private bool _jobCommandStarted;

        /// <summary>
        ///  任务运行状态
        /// </summary>
        public StatusFlag StatusFlag
        {
            get
            {
                if (_jobCommandStarted)
                {
                    return _isRunning ? StatusFlag.Running : StatusFlag.Waiting;
                }

                return _isRunning ? StatusFlag.Stopping : StatusFlag.Stopped;
            }
        }

        /// <summary>
        ///   开始任务
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //  任务依然在执行中，不需要再次唤起
            if (_isRunning)
                return;

            _isRunning = _jobCommandStarted = true;

            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    await InternalStartJob(cancellationToken);
                }
            }
            catch
            {
                _isRunning = false;
                throw;
            }
            _isRunning = false;
        }

        /// <summary>
        /// 结束任务
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isRunning = _jobCommandStarted = false;
            return InternalStopJob(cancellationToken);
        }

        #endregion


        #region 扩展定义(内部)

        /// <summary>
        /// 启动任务方法（内部扩展）
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal abstract Task InternalStartJob(CancellationToken cancellationToken);

        /// <summary>
        /// 启动任务方法（内部扩展）
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal abstract Task InternalStopJob(CancellationToken cancellationToken);

        #endregion
    }
}
