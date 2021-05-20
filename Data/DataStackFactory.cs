using System;
using System.Threading.Tasks;

namespace OSS.Tools.DataStack
{
    public static class DataStackFactory
    {
        /// <summary>
        /// 数据堆栈的提供者
        /// </summary>

        public static IDataSackProvider StackProvider { get; set; }

        /// <summary>
        ///  创建数据堆栈
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="poper">数据的弹出处理对象</param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static IStackPusher<TData> CreateStack<TData>(IStackPoper<TData> poper, string sourceName="default") 
        {
            var pusher = StackProvider?.CreateStack(poper, sourceName);
            return pusher ?? new DefaultDataStack<TData>(poper);
        }

        /// <summary>
        ///  创建数据堆栈
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="poper">数据的弹出处理委托方法</param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static IStackPusher<TData> CreateStack<TData>(Func<TData, Task<bool>> popFunc, string sourceName = "default")
        {
            var poper = new InterStackPoper<TData>(popFunc);

            var pusher = StackProvider?.CreateStack(poper, sourceName);
            return pusher ?? new DefaultDataStack<TData>(poper);
        }
    }


 
}
