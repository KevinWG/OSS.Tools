using System.Threading.Tasks;
using NUnit.Framework;
using OSS.Tools.DirConfig;

namespace OSS.Tools.Tests.DirConfigTests
{
    public class DirConfigTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task DirConfigTest()
        {
            var config = new ConfigTest() {Name = "ConfigTest"};
            await ListConfigHelper.SetItem("Test_Config","item", config);

            var rConfig = await ListConfigHelper.GetList<ConfigTest>("Test_Config");

            Assert.True(rConfig[0]?.value.Name == "ConfigTest");
            await DirConfigHelper.RemoveDirConfig("Test_Config");
        }
    }

    public class ConfigTest
    {
        public string Name { get; set; }
    }
}