#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：Http请求 == 参数实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

namespace OSS.Tools.Http
{
    /// <summary>
    /// 
    /// </summary>
    public struct FormParameter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public FormParameter(string name, object value)
        {
            Name = name;
            Value = value;
            //Type = type;
            //Domain = string.Empty;
        }

   

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value;

        ///// <summary>
        /////  cookie的域名   -- cookie 类型时需要
        ///// </summary>
        //public string Domain;

        /// <summary>
        /// 重写ToString返回   name=value编码后的格式
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
    
}
