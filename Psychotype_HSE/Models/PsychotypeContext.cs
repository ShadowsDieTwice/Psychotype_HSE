using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Psychotype_HSE.Models
{
	public class PsychotypeContext : DbContext
	{
		public DbSet<RedundantWord> RedundantWords { get; set; }
		public DbSet<SuicideWord> SuicideWords { get; set; }
	}
}