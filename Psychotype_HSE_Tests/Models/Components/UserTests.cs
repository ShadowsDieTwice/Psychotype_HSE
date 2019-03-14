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
            Assert.IsTrue(user.IsBot() < 0.5);
        }

        [TestMethod()]
        public void TestBot2()
        {
            User user = new User("id268930929");  //noname
            Assert.IsTrue(user.IsBot() > 0.5);
        }


        [TestMethod()]
        public void TestBot3()
        {
            User user = new User("maxim_rachinskiy");  //Maxim
            Assert.IsTrue(user.IsBot() < 0.5);
        }

        [TestMethod()]
        public void TestBot4()
        {
            User user = new User("krasnodar_arenda_kvartir");  //Maxim           
            using (StreamWriter sw = new StreamWriter(@"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\testBot.txt", false, System.Text.Encoding.UTF8))
            {           
                sw.WriteLine(user.IsBot());
            }
        }
    }
}
