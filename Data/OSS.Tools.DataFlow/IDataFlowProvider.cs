
namespace OSS.Tools.DataFlow
{
    /// <summary>
    ///  数据流提供器
    /// </summary>
    public interface IDataFlowProvider
    {
        /// <summary>
        /// 创建一个数据流，并暴露发布接口实现
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="subscriber"> 订阅者 </param>
        /// <param name="flowKey"> 流key  </param>
        /// <param name="option"></param>
        /// <returns> 返回当前流的发布接口实现 </returns>
        IDataPublisher<TData> CreateFlow<TData>(IDataSubscriber<TData> subscriber, string flowKey, DataFlowOption option=null);
    }


    /// <summary>
    ///  数据流发布者的提供器
    /// </summary>
    public interface IDataPublisherProvider
    {
        /// <summary>
        /// 创建单向数据发布者
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="flowKey"> 流key  </param>
        /// <param name="option"></param>
        /// <returns> 返回当前流的发布接口实现 </returns>
        IDataPublisher<TData> CreatePublisher<TData>(string flowKey, DataPublisherOption option = null);
    }
    
    /// <summary>
    ///  数据流订阅者的接收器
    /// </summary>
    public interface IDataSubscriberReceiver
    {
        /// <summary>
        /// 接收数据订阅者
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="subscriber"></param>
        /// <param name="flowKey"> 流key  </param>
        /// <param name="option"></param>
        /// <returns> 是否接收成功 </returns>
        bool Receive<TData>(IDataSubscriber<TData> subscriber, string flowKey, DataFlowOption option = null);
    }
}
