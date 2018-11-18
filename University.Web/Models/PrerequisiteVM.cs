using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Entity;

namespace University.Web.Models
{
	public class PrerequisiteVM
	{
		public PrerequisiteVM()
		{
			this.Prerequisites = new List<Prerequisite>();
		}

		public long Id { get; set; }
		public IEnumerable<Prerequisite> Prerequisites { get; set; }
	}
}
