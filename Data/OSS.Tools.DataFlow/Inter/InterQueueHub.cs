using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace OSS.Tools.DataFlow
{
    internal static class InterQueueHub
    {
        private static readonly ExecutionDataflowBlockOptions options = new ExecutionDataflowBlockOptions()
        {
            MaxDegreeOfParallelism = 32
        }; 
        private static readonly ActionBlock<InterData> _defaultDataQueue = new ActionBlock<InterData>(InterSubscriber, options);
       
        private static readonly ConcurrentDictionary<string, ActionBlock<InterData>> _sourceQueueMaps = new ConcurrentDictionary<string, ActionBlock<InterData>>();
       
        private static readonly ConcurrentDictionary<string, ISubscriberWrap> _keySubscriberMaps = new ConcurrentDictionary<string, ISubscriberWrap>();

        public static Task<bool> Publish(string msgFlowKey, object msg, string sourceName)
        {
            return Task.FromResult(GetQueue(sourceName).Post(new InterData()
            {
                flow_key = msgFlowKey, msg = msg
            }));
        }

        public static void InterSubscriber(InterData data)
        {
            if (_keySubscriberMaps.TryGetValue(data.flow_key, out var subscriber))
            {
                subscriber.Subscribe(data.msg);
            }
        }


        private static ActionBlock<InterData> GetQueue(string sourceName)
        {
            if (string.IsNullOrEmpty(sourceName))
            {
                return _defaultDataQueue;
            }

            return _sourceQueueMaps.TryGetValue(sourceName, out var q) ? q : _defaultDataQueue;
        }

        internal static void RegisterPublisher(string sourceName)
        {
            if (string.IsNullOrEmpty(sourceName) || _sourceQueueMaps.ContainsKey(sourceName))
                return;

            if (_sourceQueueMaps.TryAdd(sourceName, new ActionBlock<InterData>(InterSubscriber, options)))
                return;

        }

        internal static void RegisterSubscriber<TData>(string msgFlowKey, IDataSubscriber<TData> subscriber)
        {
            if (_keySubscriberMaps.ContainsKey(msgFlowKey))
            {
                throw new ArgumentException($"消息流key：{msgFlowKey} 已被注册");
            }

            if (_keySubscriberMaps.TryAdd(msgFlowKey, new InterDataSubscriberWrap<TData>(subscriber)))
                return;


            throw new ArgumentException($"未能注册（{msgFlowKey} ）对应的消息订阅处理!");
        }

    }


    internal class InterData
    {
        public string flow_key { get; set; }

        public object msg { get ; set; }
    }
}
