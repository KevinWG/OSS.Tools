namespace OSS.Tools.TimerJob;

/// <summary>
///   任务定时器  基类
/// </summary>
public class TimerJobTrigger : Internal_BaseExecutor, IDisposable
{
    private Timer?            _timer;
    private CancellationToken _cancellationToken =CancellationToken.None;

    #region 构造函数
        
    /// <summary>
    /// 触发器配置信息
    /// </summary>
    public TimerTriggerSetting Setting { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="setting">触发器配置信息</param>
    protected TimerJobTrigger(TimerTriggerSetting setting)
    {
        Setting     = setting;
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
            _timer = new Timer(ExecuteJob, null, Setting.DueTime, Setting.PeriodTime);
        else
            _timer.Change(Setting.DueTime, Setting.PeriodTime);

        return Task.CompletedTask;
    }

    /// <summary>
    ///  停止定时器
    /// </summary>
    private Task StopTimerTrigger(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        return Setting.JobExecutor.StopAsync(cancellationToken);
    }

    private void ExecuteJob(object? obj)
    {
        Setting.JobExecutor?.StartAsync(_cancellationToken).Wait(_cancellationToken);
    }


    /// <inheritdoc />
    void IDisposable.Dispose() => _timer?.Dispose();

    #endregion


}