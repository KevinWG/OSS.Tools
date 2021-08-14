#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：Http请求 == 请求实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
        ///   reqMessage 设置方法
        ///    如果当前的设置不能满足需求，可以通过这里再次设置
        /// </summary>
        public Action<HttpRequestMessage> RequestSet { get; set; }

        /// <summary>
        ///  如果此值设置，则忽略 Uri 值
        /// </summary>
        public string address_url { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public HttpMethod http_method { get; set; } = HttpMethod.Get;

  

        #region   请求的内容参数

        /// <summary>
        /// 是否存在文件
        /// </summary>
        public bool has_file => _fileParameters != null && _fileParameters.Any();

        private List<FileParameter> _fileParameters;
        /// <summary>
        /// 文件参数列表
        /// </summary>
        public IReadOnlyList<FileParameter> file_paras => _fileParameters ;
        
        /// <summary>
        ///  添加文件
        /// </summary>
        /// <param name="file"></param>
        public void AddFilePara(FileParameter file)
        {
            if (_fileParameters==null)
            {
                _fileParameters = new List<FileParameter>();
            }
            _fileParameters.Add(file);
        }
        
        private List<FormParameter> _formParameters;
        /// <summary>
        /// 非文件参数列表
        /// </summary>
        public IReadOnlyList<FormParameter> form_paras => _formParameters ;// 兼容老版本，取值时默认赋值

        /// <summary>
        ///  添加表单参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddFormPara(string name, object value)
        {
            if (_formParameters == null)
            {
                _formParameters = new List<FormParameter>();
            }
            _formParameters.Add(new FormParameter(name,value));
        }

        #endregion

        /// <summary>
        /// 自定义内容实体
        /// eg:当上传文件时，无法自定义内容
        /// </summary>
        public string custom_body { get; set; }

        /// <summary>
        /// 发送前准备
        /// </summary>
        protected internal virtual void PrepareSend()
        {

        }
    }
}
