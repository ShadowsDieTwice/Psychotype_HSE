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
        public void SaveTextsToCSV()
        {
            Client cl = new User("n0ize34");
            string filePath = @"../../TestFiles/suicide_predict.csv";
            cl.SaveTextsToCSV(DateTime.MinValue, DateTime.MaxValue, filePath);
            //+ Check correctness with your eyes
            string[] lines = File.ReadAllLines(filePath);
            //If that post still exists
            Assert.IsTrue(lines.Contains("тест тест тест тест тест тест вышка вышка вышка вышка привет пока это для проекта групповая динамика зачем это"));
        }


        [TestMethod()]
        public void SuisideProbability()
        {
            //this test is correct with default suicide_result if he still has only 2 posts
            
            Client cl = new User("durov");
            string id = cl.Link;
            string workingDir = AppSettings.WorkingDir;

            PythonRunner.RunScript(@"../../../Psychotype_HSE/Util/Scripts/suicideScript.py", AppSettings.PythonPath,
                workingDir);//, filePath1); // переписать тест
                
            double res = cl.SuicideProbability(DateTime.MinValue, DateTime.MaxValue, AppSettings.WorkingDir, id);
            //+ Check correctness with your eyes
            Console.WriteLine(res);
            Assert.IsTrue(res < 0.999 && res >= 0.00);            
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