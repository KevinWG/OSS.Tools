#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：Http请求辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*    	修改日期：2017-2-12
*    	修改内容：迁移至HttpClient框架下
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OSS.Tools.Http
{
    /// <summary>
    /// http请求辅助类
    /// </summary>
    public static class RestExtension
    {
        #region   扩展方法

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <param name="clientSourceName">如果设置 HttpClientHelper.HttpClientFactory,会在 CreateClient 时传入</param>
        /// <returns>自定义的Response结果</returns>
        public static Task<HttpResponseMessage> SendAsync(this OssHttpRequest request, string clientSourceName=null)
        {
            return SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None, clientSourceName);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <param name="completionOption"></param>
        /// <param name="clientSourceName">如果设置 HttpClientHelper.HttpClientFactory,会在 CreateClient 时传入</param>
        /// <returns>自定义的Response结果</returns>
        public static Task<HttpResponseMessage> SendAsync(this OssHttpRequest request,
            HttpCompletionOption completionOption, string clientSourceName = null)
        {
            return SendAsync(request, completionOption, CancellationToken.None, clientSourceName);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <param name="completionOption"></param>
        /// <param name="token"></param>
        /// <param name="clientSourceName">如果设置 HttpClientHelper.HttpClientFactory,会在 CreateClient 时传入</param>
        /// <returns>自定义的Response结果</returns>
        public static Task<HttpResponseMessage> SendAsync(this OssHttpRequest request, HttpCompletionOption completionOption,
            CancellationToken token, string clientSourceName = null)
        {
            return HttpClientHelper.CreateClient(clientSourceName).SendAsync(request, completionOption, token);
        }

        #endregion

        /// <summary>
        ///  Post 请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="body"></param>
        /// <param name="clientSourceName">如果设置 HttpClientHelper.HttpClientFactory,会在 CreateClient 时传入</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> PostAsync(this OssHttpRequest request,string body, string clientSourceName = null)
        { 
            request.http_method = HttpMethod.Post;
            request.custom_body = body;

            return request.SendAsync(clientSourceName);
        }


        /// <summary>
        ///  Get 请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="clientSourceName">如果设置 HttpClientHelper.HttpClientFactory,会在 CreateClient 时传入</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> GetAsync(this OssHttpRequest request, string clientSourceName = null)
        {
            request.http_method = HttpMethod.Get;

            return request.SendAsync(clientSourceName);
        }


    }
}