using System.Threading.Tasks;

namespace OSS.Tools.DataFlow
{
    /// <summary>
    ///  数据的发布者
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IDataPublisher<TData>
    {
        /// <summary>
        /// 推进数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否推入成功</returns>
        Task<bool> Publish(TData data);
    }

}
