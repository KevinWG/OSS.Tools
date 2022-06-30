#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：Http请求 == 主请求实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSS.Tools.Http
{
    /// <summary>
    ///  请求基类
    /// </summary>
    public static class HttpClientExtension
    {
        private const string _lineBreak = "\r\n";

        /// <summary>
        ///   编码格式
        /// </summary>
        internal static Encoding Encoding { get; set; } = Encoding.UTF8;


        #region   扩展方法
        
        /// <summary>
        ///  执行请求方法
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> SendAsync(this HttpClient client, OssHttpRequest request)
        {
            return SendAsync(client, request, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
        }
        
        /// <summary>
        ///  执行请求方法
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <param name="completionOption"></param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> SendAsync(this HttpClient client, OssHttpRequest request,
            HttpCompletionOption completionOption)
        {
           return SendAsync(client, request, completionOption, CancellationToken.None);
        }


        /// <summary>
        ///  执行请求方法
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <param name="completionOption"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SendAsync(this HttpClient client, OssHttpRequest request,
            HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            request.PrepareSend();

            var reqMsg = new HttpRequestMessage
            {
                RequestUri = new Uri(request.address_url),
                Method     = request.http_method
            };
            
            PackageReqContent(reqMsg, request); //  配置内容
            request.OnSending(reqMsg);

            return await client.SendAsync(reqMsg, completionOption, cancellationToken);
        }

        /// <summary>
        ///  读取响应内容中的字符串
        /// </summary>
        /// <param name="taskResp"></param>
        /// <param name="disposeResponse"></param>
        /// <returns></returns>
        public static async Task<string> ReadStringAsync(this Task<HttpResponseMessage> taskResp, bool disposeResponse = true)
        {
            var resp   = await taskResp;
            return await ReadStringAsync(resp, disposeResponse);
        }

        /// <summary>
        ///  读取响应内容中的字符串
        /// </summary>
        /// <param name="resp"></param>
        /// <param name="disposeResponse"></param>
        /// <returns></returns>
        public static async Task<string> ReadStringAsync(this HttpResponseMessage resp, bool disposeResponse = true)
        {
            var resStr = string.Empty;

            if (resp.IsSuccessStatusCode && resp.StatusCode != HttpStatusCode.NoContent)
            {
                resStr = await resp.Content.ReadAsStringAsync();
            }

            if (disposeResponse)
            {
                resp.Dispose();
            }

            return resStr;
        }



        #endregion

        #region  配置 ReqMsg信息


        /// <summary>
        ///  配置使用的content
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private static void PackageReqContent(HttpRequestMessage reqMsg, OssHttpRequest req)
        {
            if (req.http_method == HttpMethod.Get)
                return;

            if (req.file_paras!=null&& req.file_paras.Any())
            {
                var boundary =GetBoundary();
                
                var memory=new MemoryStream();
                WriteMultipartFormData(memory, req, boundary);
                memory.Seek(0, SeekOrigin.Begin);//设置指针到起点

                reqMsg.Content = new StreamContent(memory);
                //req.RequestSet?.Invoke(reqMsg);

                reqMsg.Content.Headers.Remove("Content-Type");
                reqMsg.Content.Headers.TryAddWithoutValidation("Content-Type", $"multipart/form-data;boundary={boundary}");
            }
            else
            {
                var data = GetNormalFormData(req);
                if (!string.IsNullOrEmpty(data))
                {
                    // 默认表单提交，上层应用程序可以设置
                    reqMsg.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
            }
      
        }

        #endregion

        #region   请求数据的 内容 处理

        #region 处理带文件上传的数据处理
    
       
        /// <summary>
        /// 写入 Form 的内容值 【 非文件参数 + 文件头 + 文件参数（内部完成） + 请求结束符 】
        /// </summary> 
        /// <param name="memory"></param>
        /// <param name="request"></param>
        /// <param name="boundary"></param>
        private static void WriteMultipartFormData(Stream memory, OssHttpRequest request, string boundary)
        {
            if (request.form_paras != null)
            {
                foreach (var param in request.form_paras)
                {
                    WriteStringTo(memory, GetMultipartFormData(param, boundary));
                }
            }
            
            foreach (var file in request.file_paras)
            {
                //文件头
                WriteStringTo(memory, GetMultipartFileHeader(file, boundary));
                //文件内容
                file.Writer(memory);
                //文件结尾
                WriteStringTo(memory, _lineBreak);
            }
            //写入整个请求的底部信息
            WriteStringTo(memory, GetMultipartFooter(boundary));
        }

        /// <summary>
        /// 写入 Form 的内容值（文件头）
        /// </summary>
        /// <param name="file"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        private static string GetMultipartFileHeader(FileParameter file, string boundary)
        {
            var conType = file.ContentType ?? "application/octet-stream";
            return $"--{boundary}{_lineBreak}Content-Disposition: form-data; name=\"{file.Name}\"; filename=\"{file.FileName}\"{_lineBreak}Content-Type: {conType}{_lineBreak}{_lineBreak}";
        }
        /// <summary>
        /// 写入 Form 的内容值（非文件参数）
        /// </summary>
        /// <param name="param"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        private static string GetMultipartFormData(NameValuePair param, string boundary)
        {
            return
                $"--{boundary}{_lineBreak}Content-Disposition: form-data; name=\"{param.name}\"{_lineBreak}{_lineBreak}{param.value}{_lineBreak}";
        }

        /// <summary>
        /// 写入 Form 的内容值  （请求结束符）
        /// </summary>
        /// <param name="boundary"></param>
        /// <returns></returns>
        private static string GetMultipartFooter(string boundary)
        {
            return $"--{boundary}--{_lineBreak}";
        }

        #endregion

        #region 不包含文件的数据处理（正常 get/post 请求）
        /// <summary>
        /// 写入请求的内容信息 （非文件上传请求）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetNormalFormData(OssHttpRequest request)
        {
            var formstring = new StringBuilder();
            if (request.form_paras != null)
            {
                foreach (var p in request.form_paras)
                {
                    if (formstring.Length > 1)
                        formstring.Append("&");
                    formstring.AppendFormat(p.ToString());
                }
            }
         
            if (string.IsNullOrEmpty(request.custom_body))
                return formstring.ToString();

            if (formstring.Length > 1)
                formstring.Append("&");

            formstring.Append(request.custom_body);
            return formstring.ToString();
        }
        #endregion

        #endregion

        #region 请求辅助方法
        /// <summary>
        /// 写入数据方法（将数据写入  webrequest）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="toWrite"></param>
        /// <returns>写入的字节数量</returns>
        private static void WriteStringTo(Stream stream, string toWrite)
        {
            var bytes = Encoding.GetBytes(toWrite);
            stream.Write(bytes, 0, bytes.Length);
        }
        
        /// <summary>
        /// 创建 请求 分割界限
        /// </summary>
        /// <returns></returns>
        private static string GetBoundary()
        {
            const string pattern = "abcdefghijklmnopqrstuvwxyz0123456789";
            var boundaryBuilder = new StringBuilder();
            var rnd = new Random();
            for (var i = 0; i < 10; i++)
            {
                var index = rnd.Next(pattern.Length);
                boundaryBuilder.Append(pattern[index]);
            }
            return $"-------{boundaryBuilder}";
        }

        #endregion
    }
}
