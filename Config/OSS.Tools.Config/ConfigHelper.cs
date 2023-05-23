using Microsoft.Extensions.Configuration;

namespace OSS.Tools.Config
{
    /// <summary>
    ///  应用配置辅助类
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        ///  配置信息
        /// </summary>
        public static IConfiguration? Configuration
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets a configuration sub-section with the specified key.
        /// </summary>
        /// <param name="key">The key of the configuration section.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" />.</returns>
        /// <remarks>
        ///     This method will never return <c>null</c>. If no matching sub-section is found with the specified key,
        ///     an empty <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" /> will be returned.
        /// </remarks>
        public static IConfigurationSection? GetSection(string key)
        {
            return Configuration?.GetSection(key);
        }

        /// <summary>
        ///  获取数据库连接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConnectionString(string name)
        {
            return Configuration.GetConnectionString(name);
        }

        /// <summary>
        ///     Gets the immediate descendant configuration sub-sections.
        /// </summary>
        /// <returns>The configuration sub-sections.</returns>
        public static IEnumerable<IConfigurationSection>? GetChildren()
        {
            return Configuration?.GetChildren();
        }
    }
}
