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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OSS.Tools.DirConfig
{
    /// <summary>
    /// 默认配置处理
    /// </summary>
    /// 
    public class DefaultToolDirConfig : IToolDirConfig
    {
        private static string _defaultPath;

        static DefaultToolDirConfig()
        {
            InitialPath();
        }

        private static void InitialPath()
        {
            var basePat = Directory.GetCurrentDirectory();
            var sepChar = Path.DirectorySeparatorChar;
            var binIndex = basePat.IndexOf(string.Concat(sepChar, "bin", sepChar), StringComparison.OrdinalIgnoreCase);
            if (binIndex > 0)
            {
                basePat = basePat.Substring(0, binIndex);
            }

            _defaultPath = Path.Combine(basePat, _folderName);
            if (!Directory.Exists(_defaultPath))
            {
                Directory.CreateDirectory(_defaultPath);
            }
        }

        private const string _folderName = "Configs";

        /// <inheritdoc />
        public Task<bool> SetDirConfig<TConfig>(string key, TConfig dirConfig, string sourceName)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "配置键值不能为空！");

            if (dirConfig == null)
                throw new ArgumentNullException(nameof(dirConfig), "配置信息不能为空！");
            
            FileStream fs = null;
            try
            {
                var filePath = string.Concat(_defaultPath, Path.DirectorySeparatorChar, key, ".config");
                fs = new FileStream(filePath, FileMode.Create,FileAccess.Write);

                var type = typeof(TConfig);

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
        public Task<TConfig> GetDirConfig<TConfig>(string key, string sourceName)
        {

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "配置键值不能为空！");

            FileStream fs = null;
            try
            {
                var fileFullName = string.Concat(_defaultPath, Path.DirectorySeparatorChar, key, ".config");

                if (!File.Exists(fileFullName))
                    return Task.FromResult(default(TConfig));

                fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.Read);

                var type = typeof(TConfig);

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
            var fileName = string.Concat(_defaultPath, Path.DirectorySeparatorChar, key, ".config");
            
            if (File.Exists(fileName))
            {
                File.Delete(fileName); 
            }
            return Task.CompletedTask;
        }
    }


    public class DefaultToolListConfig : IToolListConfig
    {
        private static readonly DefaultToolDirConfig _dirConfig = new DefaultToolDirConfig();

        public async Task<bool> SetItem<TConfig>(string listKey, string itemKey, TConfig itemValue, string sourceName)
        {
            var configRes = await _dirConfig.GetDirConfig<List<ItemConfig<TConfig>>>(listKey, sourceName);

            ItemConfig<TConfig> item;
            if (configRes != null && (item = configRes.FirstOrDefault(c => c.key == itemKey)) != null)
            {
                item.value = itemValue;
            }
            else
            {
                if (configRes == null)
                    configRes = new List<ItemConfig<TConfig>>();

                configRes.Add(new ItemConfig<TConfig>() {key = itemKey, value = itemValue});
            }

            return await _dirConfig.SetDirConfig(listKey, configRes, sourceName);
        }

  
        public Task<List<ItemConfig<TConfig>>> GetList<TConfig>(string listKey, string sourceName)
        {
            return _dirConfig.GetDirConfig<List<ItemConfig<TConfig>>>(listKey, sourceName);
        }

        public async Task<int> GetCount(string listKey, string sourceName)
        {
            return (await _dirConfig.GetDirConfig<List<ItemConfig>>(listKey, sourceName))?.Count ?? 0;
        }

        public async Task<ItemConfig<TConfig>> GetItem<TConfig>(string listKey, string itemKey, string sourceName)
        {
            var configs =await _dirConfig.GetDirConfig<List<ItemConfig<TConfig>>>(listKey, sourceName);
            return configs?.FirstOrDefault(i => i.key == itemKey);
        }

        public async Task RemoveItem(string listKey, string itemKey, string sourceName)
        {
            var configRes = await _dirConfig.GetDirConfig<List<ItemConfig>>(listKey, sourceName);

            ItemConfig item;
            if (configRes != null && (item = configRes.FirstOrDefault(c => c.key == itemKey)) != null)
            {
                configRes.Remove(item); 
                await _dirConfig.SetDirConfig(listKey, configRes, sourceName);
            }
        }
    }
}
