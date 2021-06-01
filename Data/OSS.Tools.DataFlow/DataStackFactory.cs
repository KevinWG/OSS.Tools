using System;
using System.Threading.Tasks;

namespace OSS.Tools.DataStack
{
    /// <summary>
    ///  数据堆栈创建者
    /// </summary>
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
        /// <param name="stackKey"> 堆栈key ( 默认异步线程堆栈 Task.Factory.StartNew ) </param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static IStackPusher<TData> CreateStack<TData>(IStackPoper<TData> poper,string stackKey="default_task_stack", string sourceName="default") 
        {
            var pusher = StackProvider?.CreateStack(poper, stackKey, sourceName);
            return pusher ?? new DefaultDataStack<TData>(poper);
        }

        /// <summary>
        ///  创建数据堆栈
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="popFunc">数据的弹出处理委托方法</param>
        /// <param name="stackKey"> 堆栈key ( 默认异步线程堆栈 Task.Factory.StartNew ) </param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public static IStackPusher<TData> CreateStack<TData>(Func<TData, Task<bool>> popFunc, string stackKey = "default_task_stack", string sourceName = "default")
        {
            var poper = new InterStackPoper<TData>(popFunc);

            var pusher = StackProvider?.CreateStack(poper, stackKey, sourceName);
            return pusher ?? new DefaultDataStack<TData>(poper);
        }
    }


 
}
