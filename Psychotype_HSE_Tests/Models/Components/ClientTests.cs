using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychotype.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psychotype.Models.Components.Tests
{
    [TestClass()]
    public class ClientTests
    {
        [TestMethod()]
        public void GetIdFromLinkTest()
        {
            Client cl = new User("id1");
            Assert.AreEqual(cl.VkId, 1);
            cl = new User("maxim_rachinskiy");
            Assert.AreEqual(cl.VkId, 52372015);
          //  cl = new Community("team");
           
        }

        [TestMethod()]
        public void GetAllPostsTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void GetMostPopularWordsOnWallTest()
        {
            //Assert.Fail();
        }
    }
}