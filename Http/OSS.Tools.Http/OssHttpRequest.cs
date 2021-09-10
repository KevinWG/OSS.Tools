﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：Http请求 == 请求实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OSS.Tools.Http
{
    /// <summary>
    /// 请求实体
    /// </summary>
    public class OssHttpRequest
    {
        /// <summary>
        ///  请求构造函数
        /// </summary>
        public OssHttpRequest()
        {
        }
        
        /// <summary>
        /// 请求构造函数
        /// </summary>
        /// <param name="reqUrl">请求地址</param>
        public OssHttpRequest(string reqUrl)
        {
            address_url = reqUrl;
        }
        
        ///// <summary>
        /////   reqMessage 设置方法
        /////    如果当前的设置不能满足需求，可以通过这里再次设置
        ///// </summary>
        //public Action<HttpRequestMessage> RequestSet { get; set; }

        /// <summary>
        ///  如果此值设置
        /// </summary>
        public string address_url { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public HttpMethod http_method { get; set; } = HttpMethod.Get;

        /// <summary>
        /// 自定义内容实体
        /// eg:当上传文件时，无法自定义内容
        /// </summary>
        public string custom_body { get; set; }
        
        /// <summary>
        /// 准备发送执行
        /// </summary>
        protected internal virtual void PrepareSend() //(HttpRequestMessage httpRequestMessage)
        {
        }

        ///// <summary>
        ///// 准备发送执行
        ///// </summary>
        //protected virtual Task PrepareSendAsync()//(HttpRequestMessage httpRequestMessage)
        //{
        //    return Task.CompletedTask;
        //}
        
        ///// <summary>
        ///// 准备发送执行
        ///// </summary>
        //internal virtual Task InternalPrepareSendAsync()
        //{
        //    PrepareSend();
        //    return PrepareSendAsync();
        //}


        /// <summary>
        /// 发送执行
        /// </summary>
        protected internal virtual void OnSending(HttpRequestMessage httpRequestMessage) //(HttpRequestMessage httpRequestMessage)
        {
        }



        #region   请求的内容参数

        internal List<FileParameter> FileParameters;
        internal List<NameValuePair> FormParameters;

        #endregion
    }

    /// <summary>
    ///  表单请求
    /// </summary>
    public class OssHttpFormRequest : OssHttpRequest
    {
        /// <summary>
        ///  请求构造函数
        /// </summary>
        public OssHttpFormRequest()
        {
        }

        /// <summary>
        /// 请求构造函数
        /// </summary>
        /// <param name="reqUrl">请求地址</param>
        public OssHttpFormRequest(string reqUrl):base(reqUrl)
        {
        }

        /// <summary>
        /// 文件参数列表
        /// </summary>
        public IReadOnlyList<FileParameter> file_paras => FileParameters;

        /// <summary>
        ///  添加文件
        /// </summary>
        /// <param name="file"></param>
        public OssHttpFormRequest AddFilePara(FileParameter file)
        {
            if (FileParameters == null)
            {
                FileParameters = new List<FileParameter>();
            }
            FileParameters.Add(file);
            return this;
        }
        
        /// <summary>
        /// 非文件参数列表
        /// </summary>
        public IReadOnlyList<NameValuePair> form_paras => FormParameters; // 兼容老版本，取值时默认赋值

        /// <summary>
        ///  添加表单参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public OssHttpFormRequest AddFormPara(string name, object value)
        {
            if (FormParameters == null)
            {
                FormParameters = new List<NameValuePair>();
            }
            FormParameters.Add(new NameValuePair(name, value));
            return this;
        }

    }



}
