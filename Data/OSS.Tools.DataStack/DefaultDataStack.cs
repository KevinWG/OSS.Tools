using System.Threading.Tasks;

namespace OSS.Tools.DataStack
{
    /// <summary>
    ///  默认数据堆栈
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class DefaultDataStack<TData> : IStackPusher<TData>
    {
        private IStackPoper<TData> _poper;
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="poper"></param>
        public DefaultDataStack(IStackPoper<TData> poper)
        {
            _poper = poper;
        }

        /// <summary>
        ///  推进数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<bool> Push(TData data)
        {
            Task.Factory.StartNew((obj) => { _poper?.Pop((TData) obj); }, data);
            return Task.FromResult(true);
        }
    }
}
