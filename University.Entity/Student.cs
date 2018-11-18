using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class Student : BaseEntity
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Information { get; set; }

		public long DepartmentId { get; set; }

		public Department Department { get; set; }
		public string AccountId { get; set; }

		public IEnumerable<Enrollment> Enrollments { get; set; }
	}
}
