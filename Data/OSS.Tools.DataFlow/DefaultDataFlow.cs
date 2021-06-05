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
        private IDataSubscriber<TData> _subscriber;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="subscriber"></param>
        public DefaultDataFlow(IDataSubscriber<TData> subscriber)
        {
            _subscriber = subscriber;
        }

        /// <summary>
        ///   发布数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<bool> Publish(TData data)
        {
            Task.Factory.StartNew((obj) => { _subscriber?.Subscribe((TData) obj); }, data);
            return InterUtils.TrueTask;
        }
    }
}
