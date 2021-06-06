using System.Threading.Tasks;
using OSS.Tools.DataFlow.Inter;

namespace OSS.Tools.DataFlow
{
    /// <summary>
    ///  默认数据流
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class DefaultDataPublisher<TData> : IDataPublisher<TData>
    {
        private readonly string              _msgKey;
        private readonly DataPublisherOption _option;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="msgFlowKey"></param>
        /// <param name="option"></param>
        public DefaultDataPublisher(string msgFlowKey,DataPublisherOption option)
        {
            _msgKey = msgFlowKey;
            _option = option;
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
