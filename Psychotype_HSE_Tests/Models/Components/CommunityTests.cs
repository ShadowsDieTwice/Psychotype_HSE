using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychotype_HSE.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psychotype_HSE.Models.Components.Tests
{
    [TestClass()]
    public class CommunityTests
    {
        //[TestMethod()]
        //public void CommunityTest()
        //{
        //    Assert.Fail();
        //}

       
        [TestMethod()]
        public void SubsFromBK()
        {
            string link = "bluergh";
            Community cm = new Community(link);
            cm.getSubsForBots(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\botSubsBKtest.csv");
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetSubsFromOurGroup()
        {
            string link = "club179485371";
            Community cm = new Community(link);
            cm.getSubsForBots(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\botSubs.csv");
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetSubsFromOurGroup1()
        {
            string link = "club179485371";
            Community cm = new Community(link);
            cm.getSubsForBots(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\botSubs.csv");
            Assert.IsTrue(true);
        }
        //[TestMethod()]
        //public void GetSubsFromGroup()
        //{
        //    string link = "bluergh";
        //    Community cm = new Community(link);
        //    cm.getSubsForBots(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\botSubs.csv");
        //    Assert.IsTrue(true);
        //}
    }
}