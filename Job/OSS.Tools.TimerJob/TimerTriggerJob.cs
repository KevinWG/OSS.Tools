namespace OSS.Tools.TimerJob
{
    /// <inheritdoc />
    public class TimerTriggerJob : TimerJobTrigger
    {
        /// <inheritdoc />
        protected TimerTriggerJob(string triggerName, TimeSpan dueTime, TimeSpan periodTime, IJobExecutor jobExcutor) :
            base(triggerName, dueTime, periodTime, jobExcutor)
        {
        }

        /// <inheritdoc />
        protected TimerTriggerJob(string triggerName, TimeSpan dueTime, TimeSpan periodTime, string executeJobName,
            Func<CancellationToken, Task> startAction, Func<CancellationToken, Task>? stopAction)
            : base(triggerName,
                dueTime, periodTime, executeJobName, startAction, stopAction)
        {
        }

        /// <inheritdoc />
        protected TimerTriggerJob(string triggerName, TimeSpan dueTime, TimeSpan periodTime, string executeJobName,
            Func<CancellationToken, Task> startAction)
            : base(triggerName, dueTime, periodTime, executeJobName,
                startAction)
        {
        }
    }

    /// <summary>
    ///   任务定时器  基类
    /// </summary>
    public class TimerJobTrigger : Internal_BaseExecutor, IDisposable
    {
        private Timer? _timer;
        
        // 首次启动时间
        private readonly TimeSpan _dueTime;
        private readonly TimeSpan _periodTime;       
 
        private CancellationToken _cancellationToken=CancellationToken.None;

        #region 构造函数
        
        /// <summary>
        /// 触发器名称
        /// </summary>
        public readonly string TriggerName;

        /// <summary>
        ///  工作执行者
        /// </summary>
        public IJobExecutor JobExcutor { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="triggerName">触发器名称</param>
        /// <param name="dueTime">到期开始执行时间</param>
        /// <param name="periodTime">间隔时间</param>
        /// <param name="jobExcutor">任务执行者</param>
        protected TimerJobTrigger(string triggerName, TimeSpan dueTime, TimeSpan periodTime, IJobExecutor jobExcutor)
        {
            TriggerName = triggerName;
            _dueTime    = dueTime;
            _periodTime = periodTime;
            JobExcutor  = jobExcutor;
        }

        /// <inheritdoc />
        protected TimerJobTrigger(string triggerName, TimeSpan dueTime, TimeSpan periodTime, string executeJobName,
            Func<CancellationToken, Task> startAction, Func<CancellationToken, Task>? stopAction)
            : this(triggerName, dueTime, periodTime, new InternalExecutor(executeJobName, startAction, stopAction))
        {
        }

        /// <inheritdoc />
        protected TimerJobTrigger(string triggerName, TimeSpan dueTime, TimeSpan periodTime, string executeJobName,
            Func<CancellationToken, Task> startAction)
            : this(triggerName, dueTime, periodTime, executeJobName, startAction, null)
        {
        }

        #endregion


        #region 重写内部方法

        internal override Task InternalStartJob(CancellationToken cancellationToken)
        {
            return StartTimerTrigger(cancellationToken);
        }

        internal override Task InternalStopJob(CancellationToken cancellationToken)
        {
            return StopTimerTrigger(cancellationToken);
        }

        #endregion


        #region  定时器 开始 结束方法

        /// <summary>
        ///   配置并触发定时器    
        /// </summary>
        /// <returns></returns>
        private Task StartTimerTrigger(CancellationToken cancellationToken)
        {
            if (cancellationToken != CancellationToken.None)
            {
                _cancellationToken = cancellationToken;
            }

            if (_timer == null)
                _timer = new Timer(ExecuteJob, null, _dueTime, _periodTime);
            else
                _timer.Change(_dueTime, _periodTime);

            return Task.CompletedTask;
        }

        /// <summary>
        ///  停止定时器
        /// </summary>
        private Task StopTimerTrigger(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return JobExcutor.StopAsync(cancellationToken);
        }

        private void ExecuteJob(object obj)
        {
            JobExcutor?.StartAsync(_cancellationToken).Wait(_cancellationToken);
        }


        /// <inheritdoc />
        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion


        #region 辅助方法

        /// <summary>
        /// 指定时分秒和当前的时间差
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        protected static TimeSpan PointTimeSpan(int hour, int minute, int second)
        {
            const int fullDaySeconds = 24 * 60 * 60;

            var now          = DateTime.Now;
            var startSeconds = now.Hour * 60 * 60 + now.Minute * 60 + now.Second;
            var endSeconds   = (hour * 60 * 60 + minute * 60 + second * 60) % fullDaySeconds; //防止输入溢出一天的周期

            var spanSeconds = endSeconds - startSeconds;
            if (spanSeconds < 0)
                spanSeconds += fullDaySeconds;

            return TimeSpan.FromSeconds(spanSeconds);
        }



        #endregion
    }
}
