using System.Threading.Tasks;

namespace OSS.Tools.DataStack
{
    public interface IStackPusher<TData>
    {
        /// <summary>
        /// 推进数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否推入成功</returns>
        Task<bool> Push(TData data);
    }

}
