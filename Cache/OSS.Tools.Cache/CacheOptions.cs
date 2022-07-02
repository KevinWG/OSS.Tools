#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  缓存选项
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using System;

namespace OSS.Tools.Cache
{
    /// <summary>
    ///  缓存过期时间参数
    /// </summary>
    public class CacheTimeOptions
    {
        private DateTimeOffset? _absoluteExpiration;
        private TimeSpan?       _absoluteExpirationRelativeToNow;
        private TimeSpan?       _slidingExpiration;

        /// <summary>
        ///  绝对过期时间
        /// </summary>
        public DateTimeOffset? absolute_expiration
        {
            get => this._absoluteExpiration;
            set => this._absoluteExpiration = value;
        }

        /// <summary>
        ///  固定过期时间间隔
        /// </summary>
        public TimeSpan? absolute_expiration_relative_to_now
        {
            get => this._absoluteExpirationRelativeToNow;
            set
            {
                TimeSpan? nullable = value;
                TimeSpan  zero     = TimeSpan.Zero;
                if ((nullable.HasValue ? (nullable.GetValueOrDefault() <= zero ? 1 : 0) : 0) != 0)
                    throw new ArgumentOutOfRangeException(nameof(absolute_expiration_relative_to_now),  value,
                        "The relative expiration value must be positive.");
                this._absoluteExpirationRelativeToNow = value;
            }
        }

        /// <summary>
        ///  滚动过期时长，访问后自动延长，如果同时设置固定过期，则只能在固定时长范围内延长
        /// </summary>
        public TimeSpan? sliding_expiration
        {
            get => this._slidingExpiration;
            set
            {
                TimeSpan? nullable = value;
                TimeSpan  zero     = TimeSpan.Zero;
                if ((nullable.HasValue ? (nullable.GetValueOrDefault() <= zero ? 1 : 0) : 0) != 0)
                    throw new ArgumentOutOfRangeException(nameof(sliding_expiration), value, "The sliding expiration value must be positive.");
                this._slidingExpiration = value;
            }
        }
    }


    internal static class CacheTimeOptionsExtension
    {
        public static MemoryCacheEntryOptions ToMemCacheTimeOpt(this CacheTimeOptions cOpt)
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cOpt.absolute_expiration_relative_to_now,
                SlidingExpiration               = cOpt.sliding_expiration,
                AbsoluteExpiration              = cOpt.absolute_expiration
            };
        }
    }
}
