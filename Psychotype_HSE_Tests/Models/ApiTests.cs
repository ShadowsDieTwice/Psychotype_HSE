using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychotype_HSE.Models;
using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace Psychotype.Models.Tests
{
    [TestClass()]
    public class ApiTests
    {
        [TestMethod()]
        public void GetTest()
        {
            var api = Api.Get();
            var get = api.Wall.Get(new WallGetParams { });

            Assert.IsTrue(true);
        }
    }
}