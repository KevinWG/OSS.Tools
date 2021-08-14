
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NUnit.Framework;
using OSS.Tools.Http;

namespace OSS.Tools.Tests.HttpTests
{
    public class HttpTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task FailProtectTest()
        {
            OssHttpRequest req = new OssHttpRequest("http://www.baidu.com");

            req.AddFormPara("test","test");
            req.http_method = HttpMethod.Post;


            req.RequestSet = (r) =>
            {
                r.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            };

            var resp=await req.SendAsync();
        }
     


    }

}