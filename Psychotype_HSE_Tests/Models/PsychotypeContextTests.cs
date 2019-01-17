using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychotype_HSE.Models;

namespace Psychotype_HSE_Tests.Models
{
	[TestClass()]
	public class PsychotypeContextTests
	{
		private PsychotypeContext database = new PsychotypeContext();

		[TestMethod()]
		public void DatabaseAddTest()
		{
			database.RedundantWords.Add(new RedundantWord { Word = "Check" });
			database.SuicideWords.Add(new SuicideWord {Word = "This"});
			database.SaveChanges();

			Assert.IsTrue(database.RedundantWords.Any(word => word.Word == "Check"));
			Assert.IsTrue(database.SuicideWords.Any(word => word.Word == "This"));
		}
	}
}
