using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Psychotype.Models.Components;
using System.Threading.Tasks;
using Psychotype_HSE.Models.Components;
using System.IO;

namespace Psychotype_HSE_Tests.Models.Components
{
    [TestClass()]
    public class UserTests
    {

        [TestMethod()]
        public void TestBot()
        {
            User user = new User("id_237641570");  //Dmitriy
            Assert.IsTrue(user.IsBot() < 0.4);
        }

        [TestMethod()]
        public void TestBot2()
        {
            User user = new User("id268930929");  //noname
            Assert.IsTrue(user.IsBot() > 0.8);
        }


        [TestMethod()]
        public void TestBot3()
        {
            User user = new User("maxim_rachinskiy");  //Maxim
            Assert.IsTrue(user.IsBot() < 0.4);
        }

        [TestMethod()]
        public void TestBot4()
        {
            User user = new User("id97513061");  //Bot
            Assert.IsTrue(user.IsBot() > 0.8);
        }

        [TestMethod()]
        public void TestBot5()
        {
            //User user = new User("maxim_rachinskiy");  //Maxim           
            //using (StreamWriter sw = new StreamWriter(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\testBot.txt", false, System.Text.Encoding.UTF8))
            //{
            //    sw.WriteLine(string.Format("{0}", user.IsBot()));
            //}
        }

        [TestMethod()]
        public void MyerBriggs1()
        {           
            User user = new User("larorr");
            Console.WriteLine(string.Format("{0}", user.GetMyerBriggsType()));
            //using (StreamWriter sw = new StreamWriter(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\testMB1.txt", false, System.Text.Encoding.UTF8))
            //{
            //    //sw.WriteLine(string.Format("{0}", user.GetMyerBriggsType()));
            //}
        }

        [TestMethod()]
        public void MyerBriggsTeam()
        {
            User[] users = new User[] { new User("larorr"), new User("maxim_rachinskiy"), new User("id_237641570"), new User("id182840340"), new User("n0ize34") };

            foreach (var user in users)
                Console.WriteLine("{0} : {1}", user.Link, user.GetMyerBriggsType());
            //using (StreamWriter sw = new StreamWriter(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\testMB1.txt", false, System.Text.Encoding.UTF8))
            //{
            //    foreach(var user in users)
            //        sw.WriteLine(string.Format("{0} : {1}", user.Link ,user.GetMyerBriggsType()));
            //}
        }

    }
}
