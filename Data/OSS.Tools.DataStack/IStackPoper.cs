using System;
using System.Threading.Tasks;

namespace OSS.Tools.DataStack
{
    public interface IStackPoper<TData>
    {
        /// <summary>
        /// 弹出数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns>是否弹出成功</returns>
        Task<bool> Pop(TData data);
    }

    internal class InterStackPoper<TData> : IStackPoper<TData>
    {
        private readonly Func<TData, Task<bool>> _poper;

        internal InterStackPoper(Func<TData, Task<bool>> popFunc)
        {
            if (popFunc == null)
            {
                throw new ArgumentNullException("Func<TData, Task<bool>> poper 方法不能为空！");
            }
            _poper = popFunc;
        }

        public Task<bool> Pop(TData data)
        {
            return _poper.Invoke(data);
        }
    }
}
