using NUnit.Framework;

using System.Threading.Tasks;
using OSS.Tools.DataFlow;

namespace OSS.Tools.Tests.DataStack
{
    public class DataStackTests
    {
        [SetUp]
        public void Setup()
        {
        }
        private static readonly IDataPublisher<MsgData> _pusher = DataFlowFactory.CreateFlow(new MsgPoper());

        [Test]
        public async Task DataStackTest()
        {
            var pushRes = await _pusher.Publish(new MsgData() { name = "test" });
            Assert.True(pushRes);

            await Task.Delay(2000);
        }


        private static readonly IDataPublisher<MsgData> _fpusher = DataFlowFactory.CreateFlow<MsgData>(async (data)=>
        {
            await Task.Delay(1000);
            Assert.True(data.name == "test");
            return true;
        });

        [Test]
        public async Task DataStackFuncTest()
        {
            var pushRes = await _fpusher.Publish(new MsgData() { name = "test" });
            Assert.True(pushRes);
            await Task.Delay(2000);
        }
    }


    public class MsgData
    {
        public string name { get; set; }
    }


    public class MsgPoper : IDataSubscriber<MsgData>
    {
        public async Task<bool> Subscribe(MsgData data)
        {
            await Task.Delay(1000);
            Assert.True(data.name == "test");
            return true;
        }
    }
}
