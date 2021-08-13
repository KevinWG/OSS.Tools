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

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OSS.Tools.Http.Mos.Extention
{
    /// <summary>
    /// http请求辅助类
    /// </summary>
    public static class RestExtention
    {
        #region   扩展方法

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <param name="sourceName"></param>
        /// <returns>自定义的Response结果</returns>
        public static Task<HttpResponseMessage> RestSend(this OssHttpRequest request, string sourceName=null)
        {
            return RestSend(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None, sourceName);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <param name="completionOption"></param>
        /// <param name="sourceName"></param>
        /// <returns>自定义的Response结果</returns>
        public static Task<HttpResponseMessage> RestSend(this OssHttpRequest request,
            HttpCompletionOption completionOption, string sourceName=null)
        {
            return RestSend(request, completionOption, CancellationToken.None, sourceName);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request">请求的参数</param>
        /// <param name="completionOption"></param>
        /// <param name="token"></param>
        /// <param name="sourceName"></param>
        /// <returns>自定义的Response结果</returns>
        public static Task<HttpResponseMessage> RestSend(this OssHttpRequest request, HttpCompletionOption completionOption,
            CancellationToken token, string sourceName=null)
        {
            return HttpClientHelper.CreateClient(sourceName).RestSend(request, completionOption, token);
        }

        #endregion

       
    }
}