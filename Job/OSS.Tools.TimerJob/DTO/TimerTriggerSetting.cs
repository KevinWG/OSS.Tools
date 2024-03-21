namespace OSS.Tools.TimerJob;

/// <summary>
///  定时触发器
/// </summary>
public class TimerTriggerSetting
{
    /// <summary>
    ///  定时触发器
    /// </summary>
    /// <param name="dueTime">开始执行时间</param>
    /// <param name="periodTime">间隔时间</param>
    /// <param name="jobExecutor">工作执行者（StartAsync-定时执行方法，StopAsync-定时器停止执行方法）</param>
    public TimerTriggerSetting(TimeSpan dueTime, TimeSpan periodTime, IJobExecutor jobExecutor)
    {
        DueTime     = dueTime;
        PeriodTime  = periodTime;
        JobExecutor = jobExecutor;
        TriggerName = jobExecutor.JobName;
    }

    /// <summary>
    ///  定时触发器
    /// </summary>
    /// <param name="dueTime">开始执行时间</param>
    /// <param name="periodTime">间隔时间</param>
    /// <param name="executingAction">定时执行方法</param>
    /// <param name="stopAction">定时器停止执行方法</param>
    /// <param name="triggerName">触发器名称</param>
    public TimerTriggerSetting(TimeSpan dueTime, TimeSpan periodTime, Func<CancellationToken, Task> executingAction, Func<CancellationToken, Task>? stopAction, string triggerName = "")
        : this(dueTime, periodTime, new InternalTimerExecutor(triggerName, executingAction, stopAction))
    {
    }

    /// <summary>
    ///  定时触发器
    /// </summary>
    /// <param name="dueTime">开始执行时间</param>
    /// <param name="periodTime">间隔时间</param>
    /// <param name="executingAction">定时执行方法</param>
    /// <param name="triggerName">触发器名称</param>
    public TimerTriggerSetting(TimeSpan dueTime, TimeSpan periodTime, Func<CancellationToken, Task> executingAction, string triggerName = "")
        : this(dueTime, periodTime, executingAction, null, triggerName)
    {
    }


    /// <summary>到期开始执行时间</summary>
    public TimeSpan DueTime { get; init; }

    /// <summary>间隔时间</summary>
    public TimeSpan PeriodTime { get; init; }

    /// <summary>触发器名称</summary>
    public string TriggerName { get; init; }

    /// <summary>
    ///  工作执行者
    /// </summary>
    public IJobExecutor JobExecutor { get; }
}