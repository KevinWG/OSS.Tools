#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：Http请求 == 文件参数
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

using System;
using System.IO;

namespace OSS.Tools.Http.Mos
{
    [Obsolete("转移至OSS.Tools.Http 命名空间下")]
    public struct FileParameter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">参数的名称</param>
        /// <param name="fileStream">调用方会自动释放</param>
        /// <param name="filename"></param>
        /// <param name="contentType"></param>
        public FileParameter(string name, Stream fileStream, string filename, string contentType)
        {
            this.Writer = s =>
            {
                var buffer = new byte[1024];
                using (fileStream)
                {
                    int readCount;
                    while ((readCount = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        s.Write(buffer, 0, readCount);
                    }
                }
            };
            this.FileName = filename;
            this.ContentType = contentType;
            this.Name = name;
        }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 读写操作流
        ///  返回的是写入的字节流长度
        /// </summary>
        internal Action<Stream> Writer;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName;
        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType;
    }
}
