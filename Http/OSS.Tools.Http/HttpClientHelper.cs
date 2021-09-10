using System;
using System.Net.Http;

namespace OSS.Tools.Http
{
    /// <summary>
    ///  HttpClient 辅助类
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        ///  HttpClient 的工厂实例
        /// </summary>
        public static IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// 获取HttpClient请求
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns>
        /// 返回 HttpClientFactory 创建的对象， 
        /// 如果 HttpClientFactory 未初始化，则在固定时间间隔（五分钟）返回固定的HttpClient
        /// </returns>
        internal static HttpClient CreateClient(string sourceName = null)
        {
            if (HttpClientFactory != null)
            {
                return string.IsNullOrEmpty(sourceName) ?
                    HttpClientFactory.CreateClient() :
                    HttpClientFactory.CreateClient(sourceName);
            }
            return GetDefaultClient();
        }

        private static HttpClient _client = null;
        private static DateTime _lastTime = DateTime.Now;
   
        /// <summary>
        /// 配置请求处理类
        /// </summary>
        /// <returns></returns>
        private static HttpClient GetDefaultClient()
        {
            if (_client != null)
            {
                if ((DateTime.Now - _lastTime).TotalMinutes < 5) {
                    return _client;
                }
            }
            _lastTime = DateTime.Now;

            var handler = new HttpClientHandler { UseProxy = false };        
            return _client = new HttpClient(handler);
        }
    }
}
