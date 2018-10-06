using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class Course : BaseEntity
	{
		public string Code { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Credits { get; set; }

		public int DepartmentId { get; set; }
		public Department Department { get; set; }
	}
}
