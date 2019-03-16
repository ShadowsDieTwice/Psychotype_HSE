using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychotype.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psychotype_HSE.Models.Components;

namespace Psychotype_HSE_Tests.Models
{
    [TestClass()]
    public class StemmerTest
    {
        [TestMethod()]
        public void GetTheBaseTest()
        {
            //User user = new User("n0ize34");
            //var mostPopularWords = user.GetMostPopularWordsOnWall(DateTime.MinValue, DateTime.MaxValue);
            //Assert.AreEqual(mostPopularWords[0], "тест");
            Dictionary<string, int> popularWords = new Dictionary<string, int>();
            string str = "вышка вышку вышке вышки тест тесты тесту test";
            string[] words = str.Split(' ');
            Console.WriteLine(str);
            foreach (string word in words)
            {
                string key = RussianStemmer.GetTheBase(word);
                if (popularWords.ContainsKey(key))
                    popularWords[key]++;
                else
                    popularWords.Add(key, 1);
            }
            Assert.AreEqual(3, popularWords.Count);
            Assert.AreEqual(4, popularWords["вышк"]);
            Assert.AreEqual(3, popularWords["тест"]);
	        Assert.AreEqual(1, popularWords["test"]);
        }

	    [TestMethod()]
	    public void GetGetMostPopularWordsOnWallTest()
	    {
		    var maxim = new User("maxim_rachinskiy");
		    Assert.AreEqual(maxim.GetMostPopularWordsOnWall(DateTime.MinValue, DateTime.MaxValue).Count(), 0);

		    var artem = new User("n0ize34");
		    //Assert.IsTrue(artem.GetMostPopularWordsOnWall(DateTime.MinValue, DateTime.MaxValue)
			//    .Any(group => group.Contains("вышка")));

		    foreach (var group in artem.GetMostPopularWordsOnWall(DateTime.MinValue, DateTime.MaxValue))
		    {
			    Console.WriteLine($"Count in group: {group.Count}");
			    foreach (var word in group)
			    {
				    Console.WriteLine("\t" + word);
			    }
		    }
	    }
	}
}
