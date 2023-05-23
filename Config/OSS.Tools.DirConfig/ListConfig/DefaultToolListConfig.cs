namespace OSS.Tools.DirConfig
{
    /// <summary>
    /// 默认list配置实现
    /// </summary>
    public class DefaultToolListConfig : IToolListConfig
    {
        private static readonly DefaultToolDirConfig DirConfig = new DefaultToolDirConfig();

        /// <inheritdoc />
        public async Task<bool> SetItem<TConfig>(string listKey, string itemKey, TConfig itemValue, string sourceName)
        {
            var configRes = await DirConfig.GetDirConfig<List<ItemConfig<TConfig>>>(listKey, sourceName);

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

            return await DirConfig.SetDirConfig(listKey, configRes, sourceName);
        }


        /// <inheritdoc />
        public Task<List<ItemConfig<TConfig>>?> GetList<TConfig>(string listKey, string sourceName)
        {
            return DirConfig.GetDirConfig<List<ItemConfig<TConfig>>>(listKey, sourceName);
        }

        /// <inheritdoc />
        public async Task<int> GetCount(string listKey, string sourceName)
        {
            return (await DirConfig.GetDirConfig<List<ItemConfig>>(listKey, sourceName))?.Count ?? 0;
        }

        /// <inheritdoc />
        public async Task<ItemConfig<TConfig>?> GetItem<TConfig>(string listKey, string itemKey, string sourceName)
        {
            var configs =await DirConfig.GetDirConfig<List<ItemConfig<TConfig>>>(listKey, sourceName);
            return configs?.FirstOrDefault(i => i.key == itemKey);
        }

        /// <inheritdoc />
        public async Task RemoveItem(string listKey, string itemKey, string sourceName)
        {
            var configRes = await DirConfig.GetDirConfig<List<ItemConfig>>(listKey, sourceName);

            ItemConfig? item;
            if (configRes != null && (item = configRes.FirstOrDefault(c => c.key == itemKey)) != null)
            {
                configRes.Remove(item); 
                await DirConfig.SetDirConfig(listKey, configRes, sourceName);
            }
        }
    }
}