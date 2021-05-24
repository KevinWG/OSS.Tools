using System;
using System.Threading.Tasks;
using NUnit.Framework;
using OSS.Common.BasicMos.Resp;
using OSS.Tools.Cache;

namespace OSS.Tools.Tests.CacheTests
{
    public class CacheTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task FailProtectTest()
        {
            var testRes = await CacheHelper.GetOrSetAsync("TesttttttKey", 
                () => Task.FromResult(new Resp()),TimeSpan.FromSeconds(100),res=>!res.IsSuccess());
           
            var secRes = await CacheHelper.GetOrSetAsync("TesttttttKey",
                () => Task.FromResult(new Resp()),TimeSpan.FromSeconds(10),res=>!res.IsSuccess());

            Assert.IsTrue(testRes.IsSuccess());
        }
        [Test]
        public async Task GetListTest()
        {
            var listRes =await CacheHelper.GetOrSetAbsoluteAsync("test_userkey", () 
                    => Task.FromResult(new ListResp<string>(new[]{ "test"}) ), 
                TimeSpan.FromHours(2),
                res => !res.IsSuccess());
            Assert.IsTrue(listRes.IsSuccess());
        }

        [Test]
        public async Task GetTest()
        {
            var r = await CacheHelper.GetOrSetAbsoluteAsync("test_userkey"
                , () => {
                    return Task.FromResult(true);
                }                
                ,TimeSpan.FromHours(2));
            
            // CacheHelper
            Assert.IsTrue(r);
        }


    }

}