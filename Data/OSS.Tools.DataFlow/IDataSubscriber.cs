using System.Threading.Tasks;

namespace OSS.Tools.DataFlow
{
    /// <summary>
    ///  数据的订阅者
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IDataSubscriber<in TData>
    {
        /// <summary>
        /// 弹出数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否弹出成功</returns>
        Task<bool> Subscribe(TData data);
    }

  
}
