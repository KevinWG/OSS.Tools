#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  配置插件默认实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion


using System.Xml.Serialization;

namespace OSS.Tools.DirConfig
{
    /// <summary>
    /// 默认配置处理
    /// </summary>
    public class DefaultToolDirConfig : IToolDirConfig
    {
        /// <inheritdoc />
        public Task<bool> SetDirConfig<TConfig>(string key, TConfig dirConfig, string sourceName)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "配置键值不能为空！");

            if (dirConfig == null)
                throw new ArgumentNullException(nameof(dirConfig), "配置信息不能为空！");
            
            FileStream? fs = null;
            try
            {
                var filePath = Path.Combine(GetBasicPath(), $"{key}.config");
                fs = new FileStream(filePath, FileMode.Create,FileAccess.Write);

                var type   = typeof(TConfig);
                var xmlSer = new XmlSerializer(type);

                xmlSer.Serialize(fs, dirConfig);

                return Task.FromResult(true);
            }
            finally
            {
                fs?.Dispose();
            }

        }


        /// <inheritdoc />
        public Task<TConfig?> GetDirConfig<TConfig>(string key, string sourceName)
        {

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "配置键值不能为空！");

            FileStream? fs = null;
            try
            {
                var fileFullName = Path.Combine(GetBasicPath(), $"{key}.config");

                if (!File.Exists(fileFullName))
                    return Task.FromResult(default(TConfig));

                fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.Read);

                var type   = typeof(TConfig);
                var xmlSer = new XmlSerializer(type);

                return Task.FromResult((TConfig)xmlSer.Deserialize(fs));
            }
            finally
            {
                fs?.Dispose();
            }
        }


        /// <inheritdoc />
        public Task RemoveDirConfig(string key, string sourceName)
        {
            var fileName = Path.Combine(GetBasicPath(), $"{key}.config");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            return Task.CompletedTask;
        }


        private static string _defaultPath = string.Empty;

        private static string GetBasicPath()
        {
            if (!string.IsNullOrEmpty(_defaultPath))
            {
                return _defaultPath;
            }

            _defaultPath = Path.Combine(AppContext.BaseDirectory, "ConfigFiles");
            if (!Directory.Exists(_defaultPath))
            {
                Directory.CreateDirectory(_defaultPath);
            }
            return _defaultPath;
        }
    }
}
