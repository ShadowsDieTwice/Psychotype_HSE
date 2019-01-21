using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Psychotype_HSE.Models
{
	public class PsyhotypeDbInitializer : DropCreateDatabaseAlways<PsychotypeContext>
	{
		protected override void Seed(PsychotypeContext context)
		{
			context.RedundantWords.Add(new RedundantWord { Word = "на" });
			context.RedundantWords.Add(new RedundantWord { Word = "под" });

			context.SuicideWords.Add(new SuicideWord {Word = "синий"});
			context.SuicideWords.Add(new SuicideWord {Word = "кит"});

			base.Seed(context);
		}
	}
}