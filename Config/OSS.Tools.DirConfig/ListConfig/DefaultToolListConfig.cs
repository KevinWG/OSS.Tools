namespace OSS.Tools.DirConfig
{
    /// <summary>
    /// 默认list配置实现
    /// </summary>
    public class DefaultToolListConfig : IToolListConfig
    {
        private static readonly DefaultToolDirConfig _defaultTool = new DefaultToolDirConfig();

        /// <inheritdoc />
        public async Task<bool> SetItem<TConfig>(string listKey, string itemKey, TConfig itemValue, string sourceName)
        {
            var configRes = await _defaultTool.GetDirConfig<List<ItemConfig<TConfig>>>(listKey, sourceName);

            ItemConfig<TConfig>? item;
            if (configRes != null && (item = configRes.FirstOrDefault(c => c.key == itemKey)) != null)
            {
                item.value = itemValue;
            }
            else
            {
                configRes ??= new List<ItemConfig<TConfig>>();

                configRes.Add(new ItemConfig<TConfig>() {key = itemKey, value = itemValue});
            }

            return await _defaultTool.SetDirConfig(listKey, configRes, sourceName);
        }


        /// <inheritdoc />
        public async Task<List<ItemConfig<TConfig>>> GetList<TConfig>(string listKey, string sourceName)
        {
            return (await _defaultTool.GetDirConfig<List<ItemConfig<TConfig>>>(listKey, sourceName)) ??
                   new List<ItemConfig<TConfig>>();// new Task<List<ItemConfig<TConfig>>>();
        }

        /// <inheritdoc />
        public async Task<int> GetCount(string listKey, string sourceName)
        {
            return (await _defaultTool.GetDirConfig<List<ItemConfig>>(listKey, sourceName))?.Count ?? 0;
        }

        /// <inheritdoc />
        public async Task<ItemConfig<TConfig>?> GetItem<TConfig>(string listKey, string itemKey, string sourceName)
        {
            var configs =await _defaultTool.GetDirConfig<List<ItemConfig<TConfig>>>(listKey, sourceName);
            return configs?.FirstOrDefault(i => i.key == itemKey);
        }

        /// <inheritdoc />
        public async Task RemoveItem(string listKey, string itemKey, string sourceName)
        {
            var configRes = await _defaultTool.GetDirConfig<List<ItemConfig>>(listKey, sourceName);

            ItemConfig? item;
            if (configRes != null && (item = configRes.FirstOrDefault(c => c.key == itemKey)) != null)
            {
                configRes.Remove(item); 
                await _defaultTool.SetDirConfig(listKey, configRes, sourceName);
            }
        }
    }
}