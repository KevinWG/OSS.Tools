using System;
using System.Threading;

namespace OSS.Tools.DataFlow
{
    /// <summary>
    /// 数据流选项
    /// </summary>
    public class DataFlowOption: DataPublisherOption
    {
        /// <summary>
        /// 默认选项
        /// </summary>
        public static DataFlowOption Default { get; } = new DataFlowOption();
        


        /// <summary>默认最大并发数</summary>
        private int _maxDegreeOfParallelism = 1;

        /// <summary>
        ///  最大并发数
        ///   默认：1
        /// </summary>
        public int MaxDegreeOfParallelism
        {

            get => _maxDegreeOfParallelism;
            set
            {
                if (value < 1 && value != Unbounded) throw new ArgumentOutOfRangeException(nameof(value));
                _maxDegreeOfParallelism = value;
            }
        }

        /// <summary>
        /// 无限制的值
        /// </summary>
        public const int Unbounded = -1;


        /// <summary>
        /// 取消通知
        /// </summary>
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        private int _boundedCapacity = Unbounded;

        /// <summary>
        /// 边界容量
        /// 默认-1，无限制
        /// </summary>
        public int BoundedCapacity
        {
            get => _boundedCapacity;
            set
            {
                if (value < 1 && value != Unbounded) throw new ArgumentOutOfRangeException(nameof(value));
                _boundedCapacity = value;
            }
        }
    }
    /// <summary>
    ///  数据发布者选项
    /// </summary>
    public class DataPublisherOption
    {
        /// <summary>
        ///  系统调用来源
        /// </summary>
        public string SourceName { get; set; } = "default";
    }
}
