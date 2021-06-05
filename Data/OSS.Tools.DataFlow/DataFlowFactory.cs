using OSS.Tools.DataFlow.Inter;
using System;
using System.Threading.Tasks;

namespace OSS.Tools.DataFlow
{
    /// <summary>
    ///  数据流创建者
    /// </summary>
    public static class DataFlowFactory
    {
        /// <summary>
        /// 数据流的提供者
        /// </summary>
        public static IDataFlowProvider FlowProvider { get; set; }

        /// <summary>
        ///  创建数据流
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="subscriber">数据订阅者</param>
        /// <param name="flowKey"> 流key ( 默认对应实现是 Task.Factory.StartNew 传递数据实现 ) </param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static IDataPublisher<TData> CreateFlow<TData>(IDataSubscriber<TData> subscriber, string flowKey="task_flow", string sourceName="default") 
        {
            var pusher = FlowProvider?.CreateFlow(subscriber, flowKey, sourceName);
            return pusher ?? new DefaultDataFlow<TData>(subscriber);
        }

        /// <summary>
        ///  创建数据流
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="subscribeFunc"> 订阅数据流消息的委托方法</param>
        /// <param name="flowKey"> 流key ( 默认对应实现是 Task.Factory.StartNew 传递数据实现 ) </param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static IDataPublisher<TData> CreateFlow<TData>(Func<TData, Task<bool>> subscribeFunc, string flowKey = "task_flow", string sourceName = "default")
        {
            var poper = new InterDataSubscriber<TData>(subscribeFunc);

            var pusher = FlowProvider?.CreateFlow(poper, flowKey, sourceName);
            return pusher ?? new DefaultDataFlow<TData>(poper);
        }


        /// <summary>
        /// 数据流的提供者
        /// </summary>
        public static IDataFlowPublisherProvider PublisherProvider { get; set; }

        /// <summary>
        /// 创建单向数据发布者
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="flowKey"> 流key  </param>
        /// <param name="sourceName"></param>
        /// <returns> 返回当前流的发布接口实现 </returns>
        public static IDataPublisher<TData> CreatePublisher<TData>( string flowKey = "task_flow", string sourceName = "default")
        {
            var pusher = PublisherProvider?.CreatePublisher< TData>( flowKey, sourceName);
            if (pusher==null)
            {
                throw new NotImplementedException($"无法找到对应的{flowKey}值的 IDataFlowPublisherProvider 的具体实现");
            }

            return pusher;
        }


        /// <summary>
        /// 数据流订阅者的接收器
        /// </summary>
        public static IDataFlowSubscriberReceiver SubscriberReceiver { get; set; }

        /// <summary>
        /// 接收数据订阅者
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="subscriber"></param>
        /// <param name="flowKey"> 流key  </param>
        /// <param name="sourceName"></param>
        /// <returns> 是否接收成功 </returns>
        public static bool ReceiveSubscriber<TData>(IDataSubscriber<TData> subscriber, string flowKey,
            string sourceName = "default")
        {
            return SubscriberReceiver?.Receive(subscriber, flowKey, sourceName) ?? false;
        }
    }

    

}
