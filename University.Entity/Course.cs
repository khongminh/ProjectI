using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class Course : BaseEntity
	{
		public Course()
		{
			this.Prerequisites = new List<Prerequisite>();
		}
		public string Code { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Credits { get; set; }

		public long DepartmentId { get; set; }
		public Department Department { get; set; }

		public IEnumerable<Classroom> Classrooms { get; set; }

		public IList<Prerequisite> Prerequisites { get; set; }
	}
}
