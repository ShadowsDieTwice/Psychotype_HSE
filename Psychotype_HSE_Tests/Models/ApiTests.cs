using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychotype.Models;
using System;

namespace Psychotype.Models.Tests
{
    [TestClass()]
    public class ApiTests
    {
        [TestMethod()]
        public void GetTest()
        {
            var api = Api.Get();

            Assert.Fail();
        }
    }
}