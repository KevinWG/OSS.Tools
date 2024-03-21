namespace OSS.Tools.TimerJob;

internal class InternalTimerExecutor : BaseJobExecutor
{
    private readonly Func<CancellationToken, Task>  _executingAction;
    private readonly Func<CancellationToken, Task>? _stopAction;

    /// <inheritdoc />
    public InternalTimerExecutor(string triggerName, 
        Func<CancellationToken, Task> _executingAction,
        Func<CancellationToken, Task>? stopAction)
        : base(triggerName)
    {
        this._executingAction = _executingAction ??
            throw new ArgumentNullException(nameof(_executingAction), $"未能设置定时器({triggerName})有效执行方法！");
        _stopAction = stopAction;
    }


    protected override Task OnStarting(CancellationToken cancellationToken)
    {
        return _executingAction?.Invoke(cancellationToken) ?? Task.CompletedTask;
    }

    protected override Task OnStopping(CancellationToken cancellationToken)
    {
        return _stopAction?.Invoke(cancellationToken) ?? Task.CompletedTask;
    }
}