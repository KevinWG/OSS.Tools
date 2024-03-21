// See https://aka.ms/new-console-template for more information

using OSS.Tools.TimerJob;

Console.WriteLine("Hello, World!");

var timerJob = new TimerTriggerJob(new TimerTriggerSetting(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10),
    (c) =>
    {
        Console.WriteLine(DateTime.Now.Second + "测试" + Thread.CurrentThread.ManagedThreadId);

        return Task.CompletedTask;
    })
);

var t = new  CancellationTokenSource().Token;
await timerJob.StartAsync(t);

Console.ReadLine();