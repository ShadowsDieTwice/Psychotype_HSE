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
       
        [TestMethod()]
        public void GetSubsFromOurGroup()
        {
            string link = "club179485371";
            Community cm = new Community(link);
            cm.GetSubsForBots(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\botSubs.csv");
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetFriendsFromMy()
        {
            string link = "larorr";
            User cm = new User(link);
            cm.GetFriendsForBots(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\myFriends.csv");
            Assert.IsTrue(true);
        }
    }
}