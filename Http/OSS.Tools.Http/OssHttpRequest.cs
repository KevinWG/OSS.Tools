#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：Http请求 == 请求实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion


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
        
        /// <summary>
        ///  如果此值设置
        /// </summary>
        public string address_url { get; set; } = string.Empty;

        /// <summary>
        /// 请求方式
        /// </summary>
        public HttpMethod http_method { get; set; } = HttpMethod.Get;

        /// <summary>
        /// 自定义内容实体
        /// eg:当上传文件时，无法自定义内容
        /// </summary>
        public string? custom_body { get; set; }

        /// <summary>
        /// 准备发送执行
        /// </summary>
        [Obsolete("请使用 PrepareSendAsync() 方法代替, 3.0 版本之后不再支持")]
        protected internal virtual void PrepareSend() //(HttpRequestMessage httpRequestMessage)
        {
        }

        /// <summary>
        /// 发送执行
        /// </summary>
        [Obsolete("请使用 OnSendingAsync() 方法代替, 3.0 版本之后不再支持")]
        protected internal virtual void OnSending(HttpRequestMessage httpRequestMessage) //(HttpRequestMessage httpRequestMessage)
        {
        }


        /// <summary>
        /// 准备发送执行
        /// </summary>
        protected internal virtual Task PrepareSendAsync() //(HttpRequestMessage httpRequestMessage)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 发送执行
        /// </summary>
        protected internal virtual Task OnSendingAsync(HttpRequestMessage httpRequestMessage) //(HttpRequestMessage httpRequestMessage)
        {
            return Task.CompletedTask;
        }



        #region   请求的内容参数

        internal List<FileParameter>? FileParameters;
        internal List<NameValuePair>? FormParameters;
        
        /// <summary>
        /// 文件参数列表
        /// </summary>
        public IReadOnlyList<FileParameter>? file_paras => FileParameters;
        
        /// <summary>
        /// 非文件参数列表
        /// </summary>
        public IReadOnlyList<NameValuePair>? form_paras => FormParameters; // 兼容老版本，取值时默认赋值

        #endregion
    }

    public static class OssHttpRequestExtension
    {
        /// <summary>
        ///  添加文件
        /// </summary>
        /// <param name="req"></param>
        /// <param name="file"></param>
        public static OssHttpRequest AddFilePara(this OssHttpRequest req,FileParameter file)
        {
            req.FileParameters ??= new List<FileParameter>();
            req.FileParameters.Add(file);

            return req;
        }

        /// <summary>
        ///  添加表单参数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static OssHttpRequest AddFormPara(this OssHttpRequest req, string name, object value)
        {
            req.FormParameters ??= new List<NameValuePair>();
            req.FormParameters.Add(new NameValuePair(name, value));

            return req;
        }
    }
}
