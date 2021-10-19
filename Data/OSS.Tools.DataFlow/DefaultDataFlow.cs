using System.Threading.Tasks;
using OSS.Tools.DataFlow.Inter;

namespace OSS.Tools.DataFlow
{
    /// <summary>
    ///  默认数据流
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class DefaultDataFlow<TData> : IDataPublisher<TData>
    {
        private readonly string _msgKey;
        private readonly DataFlowOption _option;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="msgFlowKey"></param>
        /// <param name="subscriber"></param>
        /// <param name="option"></param>
        public DefaultDataFlow(string msgFlowKey,IDataSubscriber<TData> subscriber,DataFlowOption option)
        {
            _msgKey = msgFlowKey;
            _option = option;

            InterQueueHub.RegisterPublisher(option.SourceName);
            InterQueueHub.RegisterSubscriber(msgFlowKey, subscriber);
        }

        /// <summary>
        ///   发布数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<bool> Publish(TData data)
        {
            return InterQueueHub.Publish(_msgKey, data, _option?.SourceName);
        }
    }
}
