using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace OSS.Tools.DataFlow.Inter
{
    internal static class InterQueueHub
    {
        private static readonly object                     _syncObj   = new object();
        private static readonly ConcurrentQueue<InterData> _dataQueue = new ConcurrentQueue<InterData>();

        private static readonly ConcurrentDictionary<string, InterDataSubscriber<InterData>> _keySubscriberMaps =
            new ConcurrentDictionary<string, InterDataSubscriber<InterData>>();

        public static Task<bool> Publish(InterData data)
        {
            _dataQueue.Enqueue(data);
            CheckConsumer();
            return InterUtils.TrueTask;
        }
        
        private static void CheckConsumer()
        {
            GuardThread();
            autoEvent.Set();
        }

        private static Thread _consumerThread;
        private static void GuardThread()
        {
            if (_consumerThread == null || !_consumerThread.IsAlive)
            {
                lock (_syncObj)
                {
                    if (_consumerThread == null || !_consumerThread.IsAlive)
                    {
                        _consumerThread              = new Thread(ConsumerData);
                        _consumerThread.IsBackground = true;
                        _consumerThread.Start();
                    }
                }
            }
        }

        private static AutoResetEvent autoEvent = new AutoResetEvent(true);

        private static void ConsumerData()
        {
            while (true)
            {
                autoEvent.WaitOne();
                while (!_dataQueue.IsEmpty)
                {
                    NoticeSubscriber();
                }
            }

        }

        private static void NoticeSubscriber()
        {
            if (_dataQueue.TryDequeue(out InterData data))
            {
                if (_keySubscriberMaps.TryGetValue(data.data_flow_key, out var subscriber))
                {
                    subscriber.Subscribe(data);
                }
            }
        }
    }


    internal class InterData
    {
        public string data_flow_key { get; set; }
    }
}
