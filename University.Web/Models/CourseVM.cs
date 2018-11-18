using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Entity;

namespace University.Web.Models
{
	public class CourseVM
	{
		public long Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Credits { get; set; }

	}
}
