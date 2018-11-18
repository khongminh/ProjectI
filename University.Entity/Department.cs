using System;
using System.Collections.Generic;
using System.Text;

namespace University.Entity
{
	public class Department : BaseEntity
	{
		public Department()
		{
			Courses = new List<Course>();
			Students = new List<Student>();
			Teachers = new List<Teacher>();
		}

		public string Deptname { get; set; }
		public string DeptEgName { get; set; }
		public string Address { get; set; }
		public string Website { get; set; }

		public IList<Student> Students { get; set; }
		public IList<Teacher> Teachers { get; set; }
		public IList<Course> Courses { get; set; }

		public string AccountId { get; set; }
	}
}
