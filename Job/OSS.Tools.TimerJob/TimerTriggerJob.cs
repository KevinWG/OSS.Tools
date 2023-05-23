namespace OSS.Tools.TimerJob
{
    /// <summary>
    ///   定时器基础类
    /// </summary>
    public class TimerTriggerJob :BaseJobExecutor, IDisposable
    {
        private Timer _timer;

        private readonly TimeSpan _dueTime;
        private readonly TimeSpan _periodTime;       
 
        private CancellationToken _cancellationToken=CancellationToken.None;

        #region 构造函数

        /// <summary>
        ///  工作执行者
        /// </summary>
        public IJobExecutor JobExcutor { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="triggerJobName">触发器名称</param>
        /// <param name="dueTime">到期开始执行时间</param>
        /// <param name="periodTime">间隔时间</param>
        /// <param name="jobExcutor">任务执行者</param>
        protected TimerTriggerJob(string triggerJobName, TimeSpan dueTime, TimeSpan periodTime, IJobExecutor jobExcutor)
            : base(triggerJobName)
        {
            _dueTime = dueTime;
            _periodTime = periodTime;
            JobExcutor = jobExcutor;
        }

        /// <inheritdoc />
        protected TimerTriggerJob(string triggerJobName, TimeSpan dueTime, TimeSpan periodTime, string executeJobName, Func<CancellationToken, Task> startAction, Func<CancellationToken, Task> stopAction)
        : this(triggerJobName, dueTime, periodTime, new InternalExecutor(executeJobName, startAction, stopAction))
        {
        }

        /// <inheritdoc />
        protected TimerTriggerJob(string triggerJobName, TimeSpan dueTime, TimeSpan periodTime, string executeJobName, Func<CancellationToken, Task> startAction)
           : this(triggerJobName, dueTime, periodTime, executeJobName, startAction, null)
        {
        }
        #endregion


        #region 扩展方法

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

            var now = DateTime.Now;
            var startSeconds = now.Hour * 60 * 60 + now.Minute * 60 + now.Second;
            var endSeconds = (hour * 60 * 60 + minute * 60 + second * 60) % fullDaySeconds;//防止输入溢出一天的周期

            var spanSeconds = endSeconds - startSeconds;
            if (spanSeconds < 0)
                spanSeconds += fullDaySeconds;

            return TimeSpan.FromSeconds(spanSeconds);
        }

        #endregion
        
        protected override Task OnStarting(CancellationToken cancellationToken)
        {
            return StartTimerTrigger(cancellationToken);
        }

        protected override Task OnStopping(CancellationToken cancellationToken)
        {
            return StopTimerTrigger(cancellationToken);
        }

        #region  基础方法


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
                _timer = new Timer(ExcuteJob, null, _dueTime, _periodTime);
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

        private void ExcuteJob(object obj)
        {
            JobExcutor?.StartAsync(_cancellationToken).Wait(_cancellationToken);
        }


        /// <inheritdoc />
        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion


    }
}
