
namespace OSS.Tools.DataStack
{
    public interface IDataSackProvider
    {
        /// <summary>
        /// 创建一个数据堆栈，需要实现push和pop（且调用poper回调函数）功能，并暴露push接口实现
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="poper"> 当前堆栈pop时的回调函数 </param>
        /// <param name="sourceName"></param>
        /// <returns> 返回当前堆栈的Push接口实现 </returns>
        IStackPusher<TData> CreateStack<TData>(IStackPoper<TData> poper, string sourceName = "default");
    }
}
