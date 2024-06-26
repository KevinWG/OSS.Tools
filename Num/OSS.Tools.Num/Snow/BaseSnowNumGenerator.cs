﻿namespace OSS.Tools.Num;

/// <summary>
///  数字编号生成基类
/// </summary>
public class BaseSnowNumGenerator
{
    //【sequence 部分】  随机序列 
    private long _maxSequence;       // 最大值
    private int  _sequenceBitLength; //长度

    // 【WorkId部分】 工作Id 
    private long _maxWorkerId;       // 最大值
    private int  _workerIdBitLength; // 长度

    /// <summary>
    ///  workerId需要偏移的位置
    /// </summary>
    protected int WorkerLeftShift { get; private set; }

    /// <summary>
    /// Timestamp 需要偏移的位置
    /// </summary>
    protected int TimestampLeftShift { get; private set; }

    private void InitialConfig(int seqBitLength, int worIdBitLength)
    {
        _sequenceBitLength = seqBitLength;
        _maxSequence       = -1L ^ (-1L << _sequenceBitLength);

        _workerIdBitLength = worIdBitLength;
        _maxWorkerId       = -1L ^ (-1L << _workerIdBitLength);

        WorkerLeftShift    = _sequenceBitLength;
        TimestampLeftShift = WorkerLeftShift + _workerIdBitLength;
    }

    /// <summary>
    ///  获取当前的工作ID
    /// </summary>
    public long WorkId { get; internal set; }


    private long _sequence;  //  时间戳下 序号值
    private long _timestamp; // 最后一次的时间戳值

    private readonly object obj = new object(); // 
    /// <summary>
    /// 构造函数
    /// </summary>
    protected BaseSnowNumGenerator(int workId, int seqBitLength, int worIdBitLength)
    {
        InitialConfig(seqBitLength, worIdBitLength);

        if (workId > _maxWorkerId || workId < 0)
        {
            throw new ArgumentException("workId", $"工作Id不能大于 {_maxWorkerId} 或 小于 0");
        }
        WorkId = workId;
    }


    /// <summary>
    ///  生成编号
    /// </summary>
    /// <returns></returns>
    public long NewNum()
    {
        long ts, seq;
        lock (obj)
        {
            SetTimestampAndSeq();
            ts  = _timestamp;
            seq = _sequence;
        }
        return CombineNum(ts, seq);
    }


    /// <summary>
    ///   组合数字ID
    /// </summary>
    /// <param name="timestamp"></param>
    /// <param name="sequence"></param>
    /// <returns></returns>
    private long CombineNum(long timestamp, long sequence)
    {
        return (timestamp << TimestampLeftShift) | (WorkId << WorkerLeftShift) | sequence;
    }

    private void SetTimestampAndSeq()
    {
        var newTimestamp = TimeMilliNum();
        if (newTimestamp < _timestamp)
        {
            //如果当前时间小于上一次ID生成的时间戳，说明系统时钟回退过这个时候应当抛出异常
            throw new ArgumentException(
                $"当前时间小于上次生成时间 {_timestamp - newTimestamp} 毫秒，注意系统时间是否发生变化！");
        }

        // 如果是同一时间生成的，则进行毫秒内序列
        if (_timestamp == newTimestamp)
        {
            _sequence = (_sequence + 1) & _maxSequence;

            //毫秒内序列溢出
            //阻塞到下一个毫秒,获得新的时间戳
            if (_sequence == 0)
                newTimestamp = WaitNextMillis(_timestamp);
        }
        //时间戳改变，毫秒内序列重置
        else
            _sequence = 0L;

        _timestamp = newTimestamp;
    }

    /// <summary>
    ///  当前毫秒内序列使用完，等待下一毫秒
    /// </summary>
    /// <returns></returns>
    protected long WaitNextMillis(long curTimeSpan)
    {
        long timeTicks;
        do
        {
            timeTicks = TimeMilliNum();
        } while (timeTicks <= curTimeSpan);
        return timeTicks;
    }
        
    private static readonly long _timeStartTicks = new DateTime(2020, 1, 1).ToUniversalTime().Ticks;

    /// <summary>
    ///  时间戳数字编号（精度 毫秒
    /// </summary>
    /// <returns></returns>
    private static long TimeMilliNum()
    {
        return (DateTime.UtcNow.Ticks - _timeStartTicks) / 10000;
    }
}