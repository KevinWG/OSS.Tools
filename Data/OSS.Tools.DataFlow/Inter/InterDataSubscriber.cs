using System;
using System.Threading.Tasks;

namespace OSS.Tools.DataFlow
{
    internal class InterDataSubscriber<TData> : IDataSubscriber<TData>
    {
        private readonly Func<TData, Task<bool>> _subscriber;

        internal InterDataSubscriber(Func<TData, Task<bool>> subscribeFunc)
        {
            _subscriber = subscribeFunc ?? throw new ArgumentNullException(nameof(subscribeFunc), "订阅者方法不能为空！");
        }

        public Task<bool> Subscribe(TData data)
        {
            return _subscriber.Invoke(data);
        }

   
    }
}
