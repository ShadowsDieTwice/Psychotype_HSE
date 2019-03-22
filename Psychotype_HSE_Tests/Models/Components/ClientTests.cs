using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychotype.Models.Components;
using Psychotype_HSE.Models.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psychotype_HSE.Util;
using Psychotype_HSE.Models;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
        public void GetIdFromLinkTestNullException()
        {
            //Throws NullReferenceException
            Client cl = new Community("net_takogo_pablica_i_ya_nadeyus_ne_budet_123");
        }
        
        [TestMethod()]
        public void SuicideProbability()
        {
            //this test is correct with default suicide_result if he still has only 2 posts
            
            Client cl = new User("durov");
            string id = cl.Link;
            // calling script to process data and make prediction
            PythonRunner.RunScript(AppSettings.PythonScriptPath, AppSettings.PythonPath);
            Thread.Sleep(10000);
            //get prediction 
            double res = cl.SuicideProbability(DateTime.MinValue, DateTime.MaxValue, id);

            //disabling script
            IPEndPoint ipe = new IPEndPoint(AppSettings.LocalIP, AppSettings.ClientPort);
            Socket socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);
            Byte[] bytesSent = Encoding.UTF32.GetBytes("~exit~::");
            socket.Send(bytesSent, bytesSent.Length, 0);

            //+ Check correctness with your eyes
            Console.WriteLine(res);
            Assert.IsTrue(res < 0.999 && res >= 0.00);            
        }

        [TestMethod()]
        public void GetMostPopularWordsOnWallTest()
        {
            User user = new User("n0ize34");

            var mostPopularWords = user.GetMostPopularWordsOnWall(DateTime.MinValue, DateTime.MaxValue);

            using (StreamWriter sw = new StreamWriter(@"..\..\testTop.txt", false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(user.VkId);
                foreach (var word in mostPopularWords)
                {
                    sw.WriteLine(word[0]);
                }
            }
        }

        [TestMethod()]
        public void GetMostPopularWordsOnWallTest2()
        {
            User user = new User("larorr");

            var mostPopularWords = user.GetMostPopularWordsOnWall(DateTime.MinValue, DateTime.MaxValue);

            Assert.IsTrue(true);
        }
    }
}