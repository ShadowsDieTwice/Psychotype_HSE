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

            cl = new Community("team");
            Assert.AreEqual(cl.VkId, 22822305);
            cl = new Community("public74284053");
            Assert.AreEqual(cl.VkId, 74284053);           
        }

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException),
        "There is no page with link: net_takogo_pablica_i_ya_nadeyus_ne_budet_123")]
        public void GetIdFromLinkTestNullExeption()
        {
            //Throws NullReferenceException
            Client cl = new Community("net_takogo_pablica_i_ya_nadeyus_ne_budet_123");
        }

        [TestMethod()]
        public void GetAllPostsTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetMostPopularWordsOnWallTest()
        {
            User user = new User("n0ize34");

            var mostPopularWords = user.GetMostPopularWordsOnWall(DateTime.MinValue, DateTime.MaxValue);

            Assert.IsTrue(mostPopularWords[0].Contains("тест"));
        }
    }
}